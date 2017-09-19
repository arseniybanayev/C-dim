using Newtonsoft.Json;

namespace CSharpDim
{
	/// <summary>
	/// http://jupyter-client.readthedocs.io/en/latest/kernels.html#connection-files
	/// </summary>
	public class ConnectionInformation
	{
		private string GetAddress(int port) => $"{Transport}://{IpAddress}:{port}";

		[JsonProperty("stdin_port")]
		public int StandardInPort { get; set; }

		public string StandardInAddreses => GetAddress(StandardInPort);

		[JsonProperty("ip")]
		public string IpAddress { get; set; }

		[JsonProperty("control_port")]
		public int ControlPort { get; set; }

		[JsonProperty("hb_port")]
		public int HeartbeatPort { get; set; }

		public string HeartbeatAddress => GetAddress(HeartbeatPort);

		[JsonProperty("signature_scheme")]
		public string SignatureScheme { get; set; }

		[JsonProperty("key")]
		public string Key { get; set; }

		[JsonProperty("shell_port")]
		public int ShellPort { get; set; }

		public string ShellAddress => GetAddress(ShellPort);

		[JsonProperty("transport")]
		public string Transport { get; set; }

		[JsonProperty("iopub_port")]
		public int IoPubPort { get; set; }

		public string IoPubAddress => GetAddress(IoPubPort);
	}
}