using Newtonsoft.Json;

namespace CSharpDim.Messages
{
	public class Header
	{
		/// <summary>
		/// Typically UUID, must be unique per message
		/// </summary>
		[JsonProperty("msg_id")]
		public string MessageId { get; set; }

		[JsonProperty("username")]
		public string Username { get; set; }

		/// <summary>
		/// Typically UUID, should be unique per session
		/// </summary>
		[JsonProperty("session")]
		public string Session { get; set; }

		/// <summary>
		/// ISO 8601 timestamp for when the message is created
		/// </summary>
		[JsonProperty("date")]
		public string Date { get; set; }

		[JsonProperty("msg_type")]
		public string MessageType { get; set; }

		/// <summary>
		/// The message protocol version
		/// </summary>
		[JsonProperty("version")]
		public string Version { get; set; }
	}
}