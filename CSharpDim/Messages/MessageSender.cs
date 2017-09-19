using System;
using CSharpDim.Kernel;
using CSharpDim.Util;
using NetMQ;

namespace CSharpDim.Messages
{
	public static class MessageSender
	{
		private static SignatureValidator _signatureValidator;

		public static void Initialize(SignatureValidator signatureValidator) {
			_signatureValidator = signatureValidator;
		}

		public static bool Send(Message message, NetMQSocket socket) {
			if (_signatureValidator == null)
				throw new InvalidOperationException($"{nameof(MessageSender)} has not been initialized with a {nameof(SignatureValidator)}");

			var hmac = _signatureValidator.CreateSignature(message);

			if (message.Identifiers.Count > 0) {
				// Send ZMQ identifiers from the message we're responding to.
				// This is important when we're dealing with ROUTER sockets, like the shell socket,
				// because the message won't be sent unless we manually include these.
				foreach (var ident in message.Identifiers) {
					socket.TrySendFrame(ident, true);
				}
			} else {
				// This is just a normal message so send the UUID
				Send(message.UUID, socket);
			}

			Send(Constants.Delimiter, socket);
			Send(hmac, socket);
			Send(JsonSerializer.Serialize(message.Header), socket);
			Send(JsonSerializer.Serialize(message.ParentHeader), socket);
			Send(JsonSerializer.Serialize(message.Metadata), socket);
			Send(message.Content, socket, false);

			return true;
		}

		private static void Send(string message, IOutgoingSocket socket, bool sendMore = true) {
			socket.SendFrame(message, sendMore);
		}
	}
}