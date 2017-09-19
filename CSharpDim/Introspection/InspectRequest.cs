using Newtonsoft.Json;

namespace CSharpDim.Introspection
{
	/// <summary>
	/// Code can be inspected to show useful information to the user. It is up to the kernel to
	/// decide what information should be displayed, and its formatting.
	/// </summary>
	public class InspectRequest
	{
		/// <summary>
		/// The code context in which introspection is requested, maybe up to an entire multiline cell.
		/// </summary>
		[JsonProperty("code")]
		public string Code { get; set; }

		/// <summary>
		/// The cursor position within "code" (in Unicode characters) where inspection is requested.
		/// </summary>
		[JsonProperty("cursor_pos")]
		public int CursorPosition { get; set; }

		/// <summary>
		/// The level of detail desired. In IPython, the default (0) is equivalent to typing
		/// 'x?' at the prompt, 1 is equivalent to 'x??'. The difference is up to kernels,
		/// but in IPython level 1 includes the source code if available.
		/// </summary>
		[JsonProperty("detail_level")]
		public byte DetailLevel { get; set; }
	}
}