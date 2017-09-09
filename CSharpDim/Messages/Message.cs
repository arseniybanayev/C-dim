using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpDim.Messages {
	public class Message {
		public Message() {
			Identifiers = new List<byte[]>();
			UUID = string.Empty;
			HMac = string.Empty;
			MetaData = new Dictionary<string, object>();
			Content = string.Empty;
		}

		[JsonProperty("identifiers")]
		public List<byte[]> Identifiers { get; set; }

		[JsonProperty("uuid")]
		public string UUID { get; set; }

		[JsonProperty("hmac")]
		public string HMac { get; set; }

		[JsonProperty("header")]
		public Header Header { get; set; }

		[JsonProperty("parent_header")]
		public Header ParentHeader { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, object> MetaData { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }
	}
}