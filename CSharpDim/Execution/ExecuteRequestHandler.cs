using System;
using System.Collections.Generic;
using System.Web;
using CSharpDim.Kernel;
using CSharpDim.Messages;
using CSharpDim.Util;
using NetMQ.Sockets;

namespace CSharpDim.Execution
{
	internal class ExecuteRequestHandler : IShellMessageHandler
	{
		private int _executionCount = 1;

		public void HandleMessage(Message message, RouterSocket shellSocket, PublisherSocket ioPubSocket) {
			var executeRequest = JsonSerializer.Deserialize<ExecuteRequest>(message.Content);

			Log.Info($"Execute Request received with code {executeRequest.Code}");

			// Kernel sends a "status: busy" message on IOPub
			SendMessageToIoPub(message, ioPubSocket, StatusValues.Busy);

			// Kernel 
			SendInputMessageToIoPub(message, ioPubSocket, executeRequest.Code);

			// 3: Evaluate the C# code
			var result = InteractiveShell.ExecuteCode(executeRequest.Code);

			// 4: Send execute reply to shell socket
			SendExecuteReplyMessage(message, shellSocket);

			// 5: Send execute result message to IOPub
			SendOutputMessageToIoPub(message, ioPubSocket, result);

			// 6: Send IDLE status message to IOPub
			SendMessageToIoPub(message, ioPubSocket, StatusValues.Idle);

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

		public void SendOutputMessageToIoPub(Message message, PublisherSocket ioPub, (string, ConsoleColor) executionResult) {
			var data = new DisplayData {
				Data = new Dictionary<string, object> {
					{"text/plain", executionResult.Item1},
					{"text/html", $"<font style=\"color:{executionResult.Item2}\">{HttpUtility.HtmlEncode(executionResult.Item1)}</font>"}
				}
			};

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
			var executeReply = new ExecuteReply(_executionCount);

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