using CSharpDim.Messages;
using NetMQ.Sockets;

namespace CSharpDim.Kernel {
	public interface IShellMessageHandler {
		void HandleMessage(Message message, RouterSocket serverSocket, PublisherSocket ioPub);
	}
}