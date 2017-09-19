using System;
using System.Collections.Generic;
using System.Linq;
using CSharpDim.Messages;
using Newtonsoft.Json;

namespace CSharpDim.Completion
{
	public class CompleteReply : Reply
	{
		public CompleteReply(IEnumerable<string> matches, (int, int) cursorRange) {
			Matches = matches.ToList();
			(CursorStart, CursorEnd) = cursorRange;
			Metadata = new Dictionary<string, object>(); // TODO: Use 'metadata'
		}

		public CompleteReply(Exception e) : base(e) {
			Metadata = new Dictionary<string, object>(); // TODO: Use 'metadata'
		}

		/// <summary>
		/// The list of all matches to the completion request, such as
		/// ["a.isalnum", "a.isalpha"] for the example in <see cref="CompleteRequest.Code"/>.
		/// </summary>
		[JsonProperty("matches")]
		public List<string> Matches { get; set; }

		/// <summary>
		/// The start of the range of text that should be replaced one of the 
		/// <see cref="Matches"/> when a completion is accepted. Typically this
		/// is the same as <see cref="CompleteRequest.CursorPosition"/> in the request.
		/// </summary>
		[JsonProperty("cursor_start")]
		public int CursorStart { get; set; }

		/// <summary>
		/// The end of the range of text that should be replaced by one of the
		/// <see cref="Matches"/> when a completion is accepted. 
		/// </summary>
		[JsonProperty("cursor_end")]
		public int CursorEnd { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, object> Metadata { get; set; }
	}
}