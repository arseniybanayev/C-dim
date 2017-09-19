using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpDim.Messages
{
	public class Message
	{
		public Message() {
			Identifiers = new List<byte[]>();
			UUID = string.Empty;
			HMac = string.Empty;
			Metadata = new Dictionary<string, object>();
			Content = string.Empty;
		}

		[JsonProperty("identifiers")]
		public List<byte[]> Identifiers { get; set; }

		[JsonProperty("uuid")]
		public string UUID { get; set; }

		[JsonProperty("hmac")]
		public string HMac { get; set; }

		/// <summary>
		/// The message header contains a pair of unique identifiers for the
		/// originating session and the actual message id, in addition to the
		/// username for the process that generated the message. This is useful in
		/// collaborative settings where multiple users may be interacting with the
		/// same kernel simultaneously, so that frontends can label the various
		/// messages in a meaningful way.
		/// </summary>
		[JsonProperty("header")]
		public Header Header { get; set; }

		/// <summary>
		/// In a chain of messages, the header from the prent is copied so that
		/// clients can track where messsages come from.
		/// </summary>
		[JsonProperty("parent_header")]
		public Header ParentHeader { get; set; }

		/// <summary>
		/// Any metadata associated with the message.
		/// </summary>
		[JsonProperty("metadata")]
		public Dictionary<string, object> Metadata { get; set; }

		/// <summary>
		/// The actual content of the message must be a dict, whose structure
		/// depends on the message type.
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }
	}
}