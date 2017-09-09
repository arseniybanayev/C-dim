using System;
using CSharpDim.Util;

namespace CSharpDim.Messages {
	public class MessageBuilder {
		public static Header CreateHeader(string messageType, string session) {
			var newHeader = new Header {
				Username = Constants.Username,
				Session = session,
				MessageId = Guid.NewGuid().ToString(),
				MessageType = messageType,
				Version = "4.0"
			};

			return newHeader;
		}

		public static Message CreateMessage(string messageType, string content, Header parentHeader) {
			var session = parentHeader.Session;

			var message = new Message {
				UUID = session,
				ParentHeader = parentHeader,
				Header = CreateHeader(messageType, session),
				Content = content
			};

			return message;
		}
	}
}