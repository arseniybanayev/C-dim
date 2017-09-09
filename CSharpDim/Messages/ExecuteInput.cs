using Newtonsoft.Json;

namespace CSharpDim.Messages {
	public class ExecuteInput {
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("execution_count")]
		public int ExecutionCount { get; set; }
	}
}