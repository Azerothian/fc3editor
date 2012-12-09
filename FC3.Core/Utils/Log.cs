using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FC3.Core.Utils
{
	public static class Log
	{
		public static void log(string msg, params object[] param)
		{
			writeMessage("log", msg, param);
		}
		private static void writeMessage(string type, string msg, params object[] param)
		{

			writeMessage("[{0}][{1}] {2}",type, DateTime.Now, String.Format(msg, param));

			Console.WriteLine(msg, param);
		}

		internal static void error(string msg, params object[] param)
		{
			writeMessage("error", msg, param);
		}
	}
}
