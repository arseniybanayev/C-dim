using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpDim.Kernel {
	public class ExecutionResult {
		public IEnumerable<string> OutputResults {
			get { return OutputResultWithColorInformation.Select(tuple => tuple.Item1).ToArray(); }
		}

		public IEnumerable<Tuple<string, ConsoleColor>> OutputResultWithColorInformation { get; set; }
	}
}