using System.Collections.Generic;
using Newtonsoft.Json;

namespace CSharpDim.Execution
{
	/// <summary>
	/// This message is used by frontends to ask the kernel to execute code
	/// on behalf ot he user, in a namespace reserved to the user's variables
	/// (and thus separate from the kernel's own internal code and variables).
	/// </summary>
	public class ExecuteRequest
	{
		/// <summary>
		/// Source code to be executed by the kernel, one or more lines.
		/// </summary>
		[JsonProperty("code")]
		public string Code { get; set; }

		/// <summary>
		/// A boolean flag which, if True, signals the kernel to execute this
		/// code as quietly as possible. If true, forces store_history to be false,
		/// and will not broadcast output on the IOPUB channel or have an
		/// execute_result. The default is false.
		/// </summary>
		[JsonProperty("silent")]
		public bool ShouldExecuteSilently { get; set; }

		/// <summary>
		/// A boolean flag which, if true, signals the kernel to populate history.
		/// The default is true if silent is false. If silent is true, store_history
		/// is forced to be false.
		/// </summary>
		[JsonProperty("store_history")]
		public bool ShouldStoreHistory { get; set; }

		/// <summary>
		/// A dict mapping names to expressions to be evaluated in the user's dict.
		/// The rich display-data representation of each will be evaluated after
		/// execution. See the display_data content for the structure of the
		/// representation data.
		/// </summary>
		[JsonProperty("user_expressions")]
		public Dictionary<string, string> UserExpressions { get; set; }

		/// <summary>
		/// Some frontends do not support stdin requests. If this is true, code
		/// running in the kernel can prompt the user for input with an input_request
		/// message. If it is false, the kernel should not send these messages.
		/// </summary>
		[JsonProperty("allow_stdin")]
		public bool ShouldAllowStandardInput { get; set; }
	}
}