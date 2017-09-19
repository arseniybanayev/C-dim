using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace CSharpDim.Messages
{
	public abstract class Reply
	{
		protected Reply(Exception e = null) {
			if (e == null) {
				Status = StatusValues.Ok;
			} else {
				Status = StatusValues.Error;
				ExceptionName = e.GetType().Name;
				ExceptionValue = e.Message;
				StackTrace = e.StackTrace.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();
			}
		}

		/// <summary>
		/// One of "ok" OR "error".
		/// See <see cref="StatusValues.Ok"/> and <see cref="StatusValues.Error"/>.
		/// </summary>
		[JsonProperty("status")]
		public string Status { get; set; }

		/// <summary>
		/// Exception name, as a string. Null if status is "ok".
		/// </summary>
		[JsonProperty("ename")]
		public string ExceptionName { get; set; }

		/// <summary>
		/// Exception value, as a string. Null if status is "ok".
		/// </summary>
		[JsonProperty("evalue")]
		public string ExceptionValue { get; set; }

		/// <summary>
		/// Stack trace frames as strings. Null if status is "ok".
		/// </summary>
		[JsonProperty("traceback")]
		public List<string> StackTrace { get; set; }
	}
}