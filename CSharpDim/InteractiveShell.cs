using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpDim
{
	internal class InteractiveShell
	{
		private static ScriptState<object> _state;

		private const ConsoleColor ReturnValueColor = ConsoleColor.Black;
		private const ConsoleColor CompilationErrorColor = ConsoleColor.Red;
		private const ConsoleColor ErrorColor = ConsoleColor.DarkRed;
		private const ConsoleColor MetaCommandColor = ConsoleColor.Blue;

		public static (string, ConsoleColor) ExecuteCode(string code) {
			try {
				// 1. Handle meta commands
				if (code.StartsWith("%%"))
					return ExecuteMetaCommand(code.Substring(2));

				// 2. Let Roslyn handle regular code
				_state = _state?.ContinueWithAsync(code).Result ?? CSharpScript.RunAsync(code).Result;
				return !string.IsNullOrEmpty(_state.ReturnValue?.ToString())
					? (_state.ReturnValue.ToString(), ConsoleColor.Black)
					: ("Expression has been evaluated and has no value", ConsoleColor.Gray);
			} catch (CompilationErrorException e) {

				return (e.Message, ConsoleColor.Red);
			} catch (Exception e) {
				return (e.Message, ConsoleColor.DarkRed);
			}
		}

		private static (string, ConsoleColor) ExecuteMetaCommand(string commandWithoutPcts) {
			try {
				if (string.Equals(commandWithoutPcts, "reset", StringComparison.InvariantCultureIgnoreCase))
					return (ResetState(), MetaCommandColor);
				return ($"Unrecognized command '{commandWithoutPcts}'", CompilationErrorColor);
			} catch (Exception e) {
				return (e.Message, ErrorColor);
			}
		}

		public static string ResetState() {
			_state = null;
			return "State has been reset. All types, variables, tasks and results have been erased.";
		}
	}
}