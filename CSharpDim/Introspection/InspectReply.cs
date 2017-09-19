using System;
using System.Collections.Generic;
using System.Linq;
using CSharpDim.Messages;
using Newtonsoft.Json;

namespace CSharpDim.Introspection
{
	public class InspectReply : Reply
	{
		/// <summary>
		/// Found nothing and didn't throw an exception.
		/// </summary>
		public InspectReply() : this(null, null) { }

		/// <summary>
		/// Found something and didn't throw an exception.
		/// </summary>
		/// <param name="data">MIME-bundle</param>
		public InspectReply(Dictionary<string, string> data) : this(data, null) { }

		/// <summary>
		/// Threw an exception.
		/// </summary>
		public InspectReply(Exception e) : this(null, e) { }

		private InspectReply(Dictionary<string, string> data, Exception e) : base(e) {
			Data = data ?? new Dictionary<string, string>();
			Found = (data?.Any() ?? false) && e == null;
			Metadata = new Dictionary<string, string>(); // TODO: Use 'metadata'
		}

		[JsonProperty("found")]
		public bool Found { get; set; }

		/// <summary>
		/// A MIME-bundle, like a display_data message, which should be a formatted
		/// representation of information about the context. In the notebook, this
		/// is used to show tooltips over function calls, etc.
		/// </summary>
		[JsonProperty("data")]
		public Dictionary<string, string> Data { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> Metadata { get; set; }
	}
}