
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpDim.Messages {
	public class DisplayData {
		public DisplayData() {
			Source = string.Empty;
			Data = new Dictionary<string, object>();
			MetaData = new Dictionary<string, string>();
		}

		[JsonProperty("source")]
		public string Source { get; set; }

		[JsonProperty("data")]
		public Dictionary<string, object> Data { get; set; }

		[JsonProperty("metadata")]
		public Dictionary<string, string> MetaData { get; set; }
	}
}