using System;
using System.Collections.Generic;
using CSharpDim.Messages;
using Newtonsoft.Json;

namespace CSharpDim.Execution
{
	public class ExecuteReply : Reply
	{
		public ExecuteReply(int executionCount, Exception e = null) : base(e) {
			ExecutionCount = executionCount;
			UserExpressions = new Dictionary<string, string>(); // TODO: Support user expressions
		}

		/// <summary>
		/// The global kernel counter that increases by one with each request that
		/// stores history. This will typically be used by clients to display
		/// prompt numbers to the user. If the request did not store history, this will
		/// be the current value of the counter in the kernel.
		/// </summary>
		[JsonProperty("execution_count")]
		public int ExecutionCount { get; set; }

		/// <summary>
		/// Results for the user_expressions.
		/// </summary>
		[JsonProperty("user_expressions")]
		public Dictionary<string, string> UserExpressions { get; set; }
	}
}