using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fc3m;

namespace FC3Editor.Core
{
	public static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		public static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new fc3mGame());
		}
		public static string programGuid = "9de9f6ee-6db7-41bf-a0b4-112e45dd3693";
	}
}
