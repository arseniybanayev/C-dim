using CSharpDim.Kernel;
using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ.Sockets;

namespace CSharpDim.Introspection
{
	internal class InspectRequestHandler : IShellMessageHandler
	{
		public void HandleMessage(Message message, RouterSocket shellSocket, PublisherSocket ioPubSocket) {
			var inspectRequest = JsonSerializer.Deserialize<InspectRequest>(message.Content);

			// TODO: Send reply
		}
	}
}