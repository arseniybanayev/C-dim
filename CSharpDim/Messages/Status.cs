using Newtonsoft.Json;

namespace CSharpDim.Messages
{
	public class Status
	{
		[JsonProperty("execution_state")]
		public string ExecutionState { get; set; }
	}
}