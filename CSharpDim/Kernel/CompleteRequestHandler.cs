using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ.Sockets;

namespace CSharpDim.Kernel {
	public class CompleteRequestHandler : IShellMessageHandler {
		public void HandleMessage(Message message, RouterSocket serverSocket, PublisherSocket ioPub) {
			var completeRequest = JsonSerializer.Deserialize<CompleteRequest>(message.Content);

			// TODO: Send reply
		}
	}
}