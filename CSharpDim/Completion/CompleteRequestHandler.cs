using CSharpDim.Kernel;
using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ.Sockets;

namespace CSharpDim.Completion
{
	public class CompleteRequestHandler : IShellMessageHandler
	{
		public void HandleMessage(Message message, RouterSocket shellSocket, PublisherSocket ioPubSocket) {
			var completeRequest = JsonSerializer.Deserialize<CompleteRequest>(message.Content);

			// TODO: Send reply
		}
	}
}