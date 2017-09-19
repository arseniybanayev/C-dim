using Newtonsoft.Json;

namespace CSharpDim.Completion
{
	public class CompleteRequest
	{
		/// <summary>
		/// The code context in which completion is requested. This may be up to an
		/// entire multiline cell, such as "foo = a.isal".
		/// </summary>
		[JsonProperty("code")]
		public string Code { get; set; }

		/// <summary>
		/// The cursor position within "code" (in Unicode characters) where completion is requested.
		/// </summary>
		[JsonProperty("cursor_pos")]
		public int CursorPosition { get; set; }
	}
}