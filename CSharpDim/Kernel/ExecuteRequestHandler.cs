using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using CSharpDim.Messages;
using CSharpDim.Util;
using Microsoft.CodeAnalysis.Scripting;
using NetMQ.Sockets;

namespace CSharpDim.Kernel {
	public class ExecuteRequestHandler : IShellMessageHandler {
		public ExecuteRequestHandler() { }

		private int _executionCount = 1;

		private static ScriptState<object> _state = null;

		public void HandleMessage(Message message, RouterSocket serverSocket, PublisherSocket ioPub) {
			Log.Info($"Message Content {message.Content}");
			var executeRequest = JsonSerializer.Deserialize<ExecuteRequest>(message.Content);

			Log.Info($"Execute Request received with code {executeRequest.Code}");

			// 1: Send Busy status on IOPub
			SendMessageToIoPub(message, ioPub, StatusValues.Busy);

			// 2: Send execute input on IOPub
			SendInputMessageToIoPub(message, ioPub, executeRequest.Code);

			// 3: Evaluate the C# code
			var result = InteractiveShell.ExecuteCode(executeRequest.Code);

			var displayData = new DisplayData {
				Data = new Dictionary<string, object> {
					{"text/plain", result.Item1},
					{"text/html", $"<font style=\"color:{result.Item2}\">{HttpUtility.HtmlEncode(result.Item1)}</font>"}
				}
			};
			
			// 4: Send execute reply to shell socket
			SendExecuteReplyMessage(message, serverSocket);

			// 5: Send execute result message to IOPub
			SendOutputMessageToIoPub(message, ioPub, displayData);
			
			// 6: Send IDLE status message to IOPub
			SendMessageToIoPub(message, ioPub, StatusValues.Idle);

			_executionCount += 1;
		}

		public void SendMessageToIoPub(Message message, PublisherSocket ioPub, string statusValue) {
			var content = new Dictionary<string, string>();
			content.Add("execution_state", statusValue);
			var ioPubMessage = MessageBuilder.CreateMessage(MessageTypeValues.Status,
				JsonSerializer.Serialize(content), message.Header);

			Log.Info($"Sending message to IOPub {JsonSerializer.Serialize(ioPubMessage)}");
			MessageSender.Send(ioPubMessage, ioPub);
			Log.Info("Message Sent");
		}

		public void SendOutputMessageToIoPub(Message message, PublisherSocket ioPub, DisplayData data) {
			var content = new Dictionary<string, object>();
			content.Add("execution_count", _executionCount);
			content.Add("data", data.Data);
			content.Add("metadata", data.MetaData);

			var outputMessage = MessageBuilder.CreateMessage(MessageTypeValues.Output,
				JsonSerializer.Serialize(content), message.Header);

			Log.Info($"Sending message to IOPub {JsonSerializer.Serialize(outputMessage)}");
			MessageSender.Send(outputMessage, ioPub);
		}

		public void SendInputMessageToIoPub(Message message, PublisherSocket ioPub, string code) {
			var content = new Dictionary<string, object>();
			content.Add("execution_count", 1);
			content.Add("code", code);

			var executeInputMessage = MessageBuilder.CreateMessage(MessageTypeValues.Input, JsonSerializer.Serialize(content),
				message.Header);

			Log.Info($"Sending message to IOPub {JsonSerializer.Serialize(executeInputMessage)}");
			MessageSender.Send(executeInputMessage, ioPub);
		}

		public void SendExecuteReplyMessage(Message message, RouterSocket shellSocket) {
			var executeReply = new ExecuteReplyOk {
				ExecutionCount = _executionCount,
				Payload = new List<Dictionary<string, string>>(),
				UserExpressions = new Dictionary<string, string>()
			};

			var executeReplyMessage = MessageBuilder.CreateMessage(MessageTypeValues.ExecuteReply,
				JsonSerializer.Serialize(executeReply), message.Header);

			// Stick the original identifiers on the message so they'll be sent first
			// Necessary since the shell socket is a ROUTER socket
			executeReplyMessage.Identifiers = message.Identifiers;

			Log.Info($"Sending message to Shell {JsonSerializer.Serialize(executeReplyMessage)}");
			MessageSender.Send(executeReplyMessage, shellSocket);
		}
	}
}