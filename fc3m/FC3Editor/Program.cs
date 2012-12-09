using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
namespace FC3Editor
{
	internal static class Program
	{
		public static string BinDir
		{
			get;
			set;
		}
		public static string BinFile
		{
			get;
			set;
		}
		public static bool HasArgument(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				string a = commandLineArgs[i];
				if (a == key)
				{
					return true;
				}
			}
			return false;
		}
		public static string GetMapArgument()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				string text = commandLineArgs[i];
				if (!text.StartsWith("-") && text.EndsWith(".fc3map"))
				{
					return text;
				}
			}
			return null;
		}
		[STAThread]
		private static void Main()
		{
			bool engineRunning = false;
			while (true)
			{
				Program.BinFile = Assembly.GetEntryAssembly().Location;
				Program.BinDir = Path.GetDirectoryName(Program.BinFile) + "\\";
				bool flag = Debugger.IsAttached && Program.HasArgument("-usereload");
				if (!flag)
				{
					goto IL_7D;
				}
				AppDomain appDomain = AppDomain.CreateDomain("FC3EditorAppDomain");
				FC3EditorLoader fC3EditorLoader = (FC3EditorLoader)appDomain.CreateInstanceFromAndUnwrap(Program.BinFile, "FC3Editor.FC3EditorLoader");
				bool flag2 = fC3EditorLoader.Load(Program.BinDir, engineRunning, null);
				AppDomain.Unload(appDomain);
				if (!flag2)
				{
					break;
				}
				engineRunning = true;
			}
			return;
			IL_7D:
			string mapArgument = Program.GetMapArgument();
			FC3EditorLoader fC3EditorLoader2 = new FC3EditorLoader();
			fC3EditorLoader2.Load(Program.BinDir, engineRunning, mapArgument);
		}
	}
}
