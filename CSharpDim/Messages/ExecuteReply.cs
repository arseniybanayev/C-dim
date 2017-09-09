using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpDim.Messages {
	public class ExecuteReply {
		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("execution_count")]
		public int ExecutionCount { get; set; }
	}

	public class ExecuteReplyError : ExecuteReply {
		public ExecuteReplyError() {
			Status = StatusValues.Error;
		}

		[JsonProperty("ename")]
		public string EName { get; set; }

		[JsonProperty("evalue")]
		public string EValue { get; set; }

		[JsonProperty("traceback")]
		public List<string> Traceback { get; set; }
	}

	public class ExecuteReplyOk : ExecuteReply {
		public ExecuteReplyOk() {
			Status = StatusValues.Ok;
		}

		[JsonProperty("payload")]
		public List<Dictionary<string, string>> Payload { get; set; }

		[JsonProperty("user_expressions")]
		public Dictionary<string, string> UserExpressions { get; set; }
	}
}