using FC3Editor.Nomad;
using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace FC3Editor
{
	internal static class Program
	{
		public static string programGuid = "9de9f6ee-6db7-41bf-a0b4-112e45dd3693";
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
			if (commandLineArgs.Length >= 2 && !commandLineArgs[1].StartsWith("-") && commandLineArgs[1].EndsWith(".fc3map"))
			{
				return commandLineArgs[1];
			}
			return null;
		}
		private static bool OpenExistingAppCallback(IntPtr hWnd, IntPtr lParam)
		{
			if (Win32.GetProp(hWnd, Program.programGuid) != IntPtr.Zero)
			{
				Win32.SetForegroundWindow(hWnd);
				IntPtr intPtr = IntPtr.Zero;
				int cbData = 0;
				string mapArgument = Program.GetMapArgument();
				if (mapArgument != null)
				{
					intPtr = Marshal.StringToCoTaskMemUni(mapArgument);
					cbData = (mapArgument.Length + 1) * 2;
				}
				if (intPtr != IntPtr.Zero)
				{
					Win32.COPYDATASTRUCT cOPYDATASTRUCT = default(Win32.COPYDATASTRUCT);
					cOPYDATASTRUCT.dwData = IntPtr.Zero;
					cOPYDATASTRUCT.lpData = intPtr;
					cOPYDATASTRUCT.cbData = cbData;
					Win32.SendMessage(hWnd, 74, 0, ref cOPYDATASTRUCT);
				}
				Marshal.FreeCoTaskMem(intPtr);
				return false;
			}
			return true;
		}
		public static void Run(bool engineRunning, string initMapPath)
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.CurrentCulture = CultureInfo.InvariantCulture;
			bool flag;
			using (new Mutex(true, Program.programGuid, out flag))
			{
				if (!flag)
				{
					Win32.EnumWindows(new Win32.EnumWindowsProc(Program.OpenExistingAppCallback), IntPtr.Zero);
				}
				else
				{
					MainForm mainForm = new MainForm();
					if (!engineRunning)
					{
						SplashForm.Start();
						bool flag2 = Engine.Init(mainForm, mainForm.Viewport);
						SplashForm.Stop();
						if (!flag2)
						{
							return;
						}
					}
					else
					{
						Engine.Reset(mainForm, mainForm.Viewport);
					}
					mainForm.Show();
					mainForm.InitMapPath = initMapPath;
					mainForm.PostLoad();
					Engine.Run();
					if (!Engine.Reloading)
					{
						Engine.Close();
					}
				}
			}
		}
	}
}
