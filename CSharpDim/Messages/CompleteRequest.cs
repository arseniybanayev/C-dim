using Newtonsoft.Json;

namespace CSharpDim.Messages {
	public class CompleteRequest {
		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("cursor_pos")]
		public int CursorPosition { get; set; }
	}
}