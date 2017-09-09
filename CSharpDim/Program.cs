using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using CSharpDim.Kernel;
using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using JsonSerializer = CSharpDim.Util.JsonSerializer;

namespace CSharpDim {
	public class Program {
		public static void Main(string[] args) {
			if (args == null || args.Length != 1)
				throw new ArgumentException("Expected 1 argument with zmq connection information file");
			var conn = GetConnectionInformation(args[0]);

			var signatureAlgorithm = conn.SignatureScheme.Replace("-", "").ToUpperInvariant();
			var signatureValidator = new SignatureValidator(conn.Key, signatureAlgorithm);
			MessageSender.Initialize(signatureValidator);

			// Start the shell
			new Thread(() => StartShellLoop(conn.ShellAddress, conn.IoPubAddress)).Start();
			Log.Info("Shell started");

			// Start the heartbeat
			new Thread(() => StartHeartbeatLoop(conn.HeartbeatAddress)).Start();
			Log.Info("Heartbeat started");

			StopEvent.Wait();
			Thread.Sleep(60000);
		}

		private static ConnectionInformation GetConnectionInformation(string filename) {
			Log.Info($"Opening file '{filename}'");
			var fileContent = File.ReadAllText(filename);
			Log.Info($"Connection information: {fileContent}");

			var connectionInformation = JsonConvert.DeserializeObject<ConnectionInformation>(fileContent);
			return connectionInformation;
		}

		private static readonly IReadOnlyDictionary<string, IShellMessageHandler> MessageHandlers =
			new Dictionary<string, IShellMessageHandler> {
				{MessageTypeValues.KernelInfoRequest, new KernelInfoRequestHandler()},
				{MessageTypeValues.CompleteRequest, new CompleteRequestHandler()},
				{MessageTypeValues.ExecuteRequest, new ExecuteRequestHandler()}
			};

		private static readonly RouterSocket ShellRouterSocket = new RouterSocket();
		private static readonly PublisherSocket ShellPublisherSocket = new PublisherSocket();
		private static readonly ResponseSocket HeartbeatSocket = new ResponseSocket();

		private static readonly ManualResetEventSlim StopEvent = new ManualResetEventSlim();

		private static void StartShellLoop(string shellAddress, string ioPubAddress) {
			ShellRouterSocket.Bind(shellAddress);
			Log.Info($"Bound shell server to address {shellAddress}");

			ShellPublisherSocket.Bind(ioPubAddress);
			Log.Info($"Bound IO pub to address {ioPubAddress}");

			while (!StopEvent.Wait(0)) {
				var message = GetNextMessage();

				Log.Info($"Received message {JsonSerializer.Serialize(message)}");

				IShellMessageHandler handler;
				if (MessageHandlers.TryGetValue(message.Header.MessageType, out handler)) {
					Log.Info($"Sending message to {message.Header.MessageType} handler");
					handler.HandleMessage(message, ShellRouterSocket, ShellPublisherSocket);
					Log.Info("Message handling complete");
				}
				else {
					Log.Error($"No message handler found for message type {message.Header.MessageType}");
				}
			}
		}

		private static void StartHeartbeatLoop(string heartbeatAddress) {
			HeartbeatSocket.Bind(heartbeatAddress);

			while (!StopEvent.Wait(0)) {
				var data = HeartbeatSocket.ReceiveFrameBytes();

				Log.Info(Encoding.Default.GetString(data));
				// Echoing back whatever was received
				HeartbeatSocket.TrySendFrame(data);
			}
		}

		private static Message GetNextMessage() {
			var message = new Message();

			// There may be additional ZMQ identities attached; read until the delimiter <IDS|MSG>"
			// and store them in message.identifiers
			// http://ipython.org/ipython-doc/dev/development/messaging.html#the-wire-protocol
			var delimAsBytes = Encoding.ASCII.GetBytes(Constants.Delimiter);
			while (true) {
				var delim = ShellRouterSocket.ReceiveFrameBytes();
				if (delim.SequenceEqual(delimAsBytes)) break;

				message.Identifiers.Add(delim);
			}

			// Getting Hmac
			message.HMac = ShellRouterSocket.ReceiveFrameString();
			Log.Info(message.HMac);

			// Getting Header
			var header = ShellRouterSocket.ReceiveFrameString();
			Log.Info(header);

			message.Header = JsonSerializer.Deserialize<Header>(header);

			// Getting parent header
			var parentHeader = ShellRouterSocket.ReceiveFrameString();
			Log.Info(parentHeader);

			message.ParentHeader = JsonSerializer.Deserialize<Header>(parentHeader);

			// Getting metadata
			var metadata = ShellRouterSocket.ReceiveFrameString();
			Log.Info(metadata);

			message.MetaData = JsonSerializer.Deserialize<Dictionary<string, object>>(metadata);

			// Getting content
			var content = ShellRouterSocket.ReceiveFrameString();
			Log.Info(content);

			message.Content = content;

			return message;
		}
	}
}