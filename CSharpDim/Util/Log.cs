using System;
using System.Collections.Generic;
using Common.Logging;
using Common.Logging.Simple;

namespace CSharpDim.Util {
	public static class Log {
		private static readonly Lazy<IList<ILog>> Loggers = new Lazy<IList<ILog>>(() => {
			return new List<ILog> {
				new ConsoleOutLogger("kernel.log", LogLevel.All, true, true, false, "yyyy/MM/dd HH:mm:ss:fff")
			};
		});

		public static void AddLogger(ILog logger) {
			Loggers.Value.Add(logger);
		}

		public static void Info(string message) {
			foreach (var log in Loggers.Value)
				log.Info(message);
		}

		public static void Error(string message) {
			foreach (var log in Loggers.Value)
				log.Error(message);
		}
	}
}