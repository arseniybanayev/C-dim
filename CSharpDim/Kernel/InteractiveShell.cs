using System;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace CSharpDim.Kernel {
	internal class InteractiveShell {
		private static ScriptState<object> _state;

		public static (string, ConsoleColor) ExecuteCode(string code) {
			try {
				_state = _state?.ContinueWithAsync(code).Result ?? CSharpScript.RunAsync(code).Result;
				return !string.IsNullOrEmpty(_state.ReturnValue?.ToString())
					? (_state.ReturnValue.ToString(), ConsoleColor.Black)
					: ("Expression has been evaluated and has no value", ConsoleColor.Gray);
			}
			catch (CompilationErrorException e) {
				return (e.Message, ConsoleColor.Red);
			}
			catch (Exception e) {
				return (e.Message, ConsoleColor.DarkRed);
			}
		}
	}
}