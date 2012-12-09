using System;
using System.Collections.Generic;
using System.Windows.Forms;
using fc3m;

namespace FC3Editor
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
	}
}
