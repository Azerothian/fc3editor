using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
namespace FC3Editor.Nomad
{
	internal static class Engine
	{
		public delegate void InvokeDelegate();
		public enum ReloadState
		{
			None,
			Managed,
			Native
		}
		public static bool TickAlways = false;
		private static List<Engine.InvokeDelegate> m_delayedCallbacks = new List<Engine.InvokeDelegate>();
		private static Binding.MessagePumpCallback m_delegateMessagePumpCallback;
		private static bool m_initialized = false;
		public static string PersonalPath
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Engine_GetPersonalPath());
			}
		}
		public static string GenericDataPath
		{
			get
			{
				return Marshal.PtrToStringAnsi(Binding.FCE_Engine_GetGenericDataPath());
			}
		}
		public static bool Initialized
		{
			get
			{
				return Engine.m_initialized;
			}
		}
		public static bool ConsoleOpened
		{
			get
			{
				return Binding.FCE_Engine_IsConsoleOpen();
			}
		}
		public static TimeSpan TimeOfDay
		{
			get
			{
				int hours;
				int minutes;
				int seconds;
				Binding.FCE_Engine_GetTimeOfDay(out hours, out minutes, out seconds);
				return new TimeSpan(hours, minutes, seconds);
			}
			set
			{
				Binding.FCE_Engine_SetTimeOfDay(value.Hours, value.Minutes, value.Seconds);
			}
		}
		public static int CloudTypeCount
		{
			get
			{
				return Binding.FCE_Engine_GetCloudTypeCount();
			}
		}
		public static int CloudType
		{
			get
			{
				return Binding.FCE_Engine_GetCloudType();
			}
			set
			{
				Binding.FCE_Engine_SetCloudType(value);
			}
		}
		public static float StormFactor
		{
			get
			{
				return Binding.FCE_Engine_GetStormFactor();
			}
			set
			{
				Binding.FCE_Engine_SetStormFactor(value);
			}
		}
		public static string BinFile
		{
			get;
			private set;
		}
		public static string BinDir
		{
			get;
			private set;
		}
		public static bool Reloading
		{
			get;
			set;
		}
		private static void InitInternal()
		{
			Engine.BinFile = Assembly.GetExecutingAssembly().Location;
			Engine.BinDir = Path.GetDirectoryName(Engine.BinFile) + "\\";
			Binding.LoadDll();
			Engine.m_delegateMessagePumpCallback = new Binding.MessagePumpCallback(Engine.MessagePumpCallback);
		}
		private static string GetLanguage()
		{
			string text = "";
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE");
			if (registryKey != null)
			{
				registryKey = registryKey.OpenSubKey("Ubisoft");
			}
			if (registryKey != null)
			{
				registryKey = registryKey.OpenSubKey("Far Cry 3");
			}
			if (registryKey != null)
			{
				text += registryKey.GetValue("Language");
			}
			return text;
		}
		public static bool Init(Form mainWindow, Control viewport)
		{
			Engine.InitInternal();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string text = " -editorpc -language=";
			string language = Engine.GetLanguage();
			if (language.Length == 0)
			{
				ResourceManager resourceManager = new ResourceManager("FC3Editor.Properties.Resources", Assembly.GetExecutingAssembly());
				string @string = resourceManager.GetString("IDS_ERR_MISSING_REGISTRY_KEY");
				MessageBox.Show(@string, "Far Cry 3 Map Editor");
				return false;
			}
			text += language;
			int num = 1;
			if (Program.GetMapArgument() != null)
			{
				num = 2;
			}
			if (!Binding.InitDuniaEngine(Process.GetCurrentProcess().MainModule.BaseAddress, mainWindow.Handle, viewport.Handle, string.Join(" ", commandLineArgs, num, commandLineArgs.Length - num) + text, true, true, Engine.m_delegateMessagePumpCallback))
			{
				return false;
			}
			foreach (ProcessModule processModule in Process.GetCurrentProcess().Modules)
			{
				if (processModule.ModuleName.StartsWith("FC3Editor.Core") || processModule.ModuleName.StartsWith("InGameEditor"))
				{
					Binding.FCE_Hack_Init(processModule.BaseAddress);
				}
			}
			Binding.FCE_Engine_AutoAcquireInput(true);
			Editor.Init();
			Binding.FCE_Engine_Reset(mainWindow.Handle, viewport.Handle, Engine.m_delegateMessagePumpCallback);
			if (!Directory.Exists(Engine.PersonalPath))
			{
				Directory.CreateDirectory(Engine.PersonalPath);
			}
			Engine.m_initialized = true;
			return true;
		}
		public static void Reset(Form mainWindow, Control viewport)
		{
			Engine.InitInternal();
			Editor.Init();
			Binding.FCE_Engine_Reset(mainWindow.Handle, viewport.Handle, Engine.m_delegateMessagePumpCallback);
			Engine.m_initialized = true;
		}
		public static void Close()
		{
			Binding.UnloadDll();
			Binding.CloseDuniaEngine();
		}
		public static void Run()
		{
			Engine.TickAlways |= Program.HasArgument("-alwaysTick");
			while (!MainForm.Instance.IsDisposed)
			{
				bool flag = Editor.IsActive || Engine.TickAlways;
				if (Engine.m_delayedCallbacks.Count > 0)
				{
					flag = true;
					List<Engine.InvokeDelegate> delayedCallbacks;
					Monitor.Enter(delayedCallbacks = Engine.m_delayedCallbacks);
					try
					{
						foreach (Engine.InvokeDelegate current in Engine.m_delayedCallbacks)
						{
							current();
						}
						Engine.m_delayedCallbacks.Clear();
					}
					finally
					{
						Monitor.Exit(delayedCallbacks);
					}
				}
				if (flag)
				{
					Binding.TickDuniaEngine();
				}
				else
				{
					Thread.Sleep(50);
				}
				Application.DoEvents();
			}
		}
		public static void Invoke(Engine.InvokeDelegate callback)
		{
			List<Engine.InvokeDelegate> delayedCallbacks;
			Monitor.Enter(delayedCallbacks = Engine.m_delayedCallbacks);
			try
			{
				Engine.m_delayedCallbacks.Add(callback);
			}
			finally
			{
				Monitor.Exit(delayedCallbacks);
			}
		}
		private static void MessagePumpCallback(bool deferQuit, bool blockRenderer)
		{
		}
		public static void UpdateResolution(Size size)
		{
			if (Engine.Initialized)
			{
				Binding.FCE_Engine_UpdateViewport(size.Width, size.Height);
			}
		}
		public static Engine.ReloadState EvaluateReloadState()
		{
			FileInfo fileInfo = new FileInfo(Engine.BinDir + "output\\" + Binding.gameDll);
			FileInfo fileInfo2 = new FileInfo(Engine.BinDir + Binding.gameDll);
			if (fileInfo.LastWriteTime > fileInfo2.LastWriteTime)
			{
				return Engine.ReloadState.Native;
			}
			FileInfo fileInfo3 = new FileInfo(Engine.BinDir + "output\\FC3Editor.core.dll");
			FileInfo fileInfo4 = new FileInfo(Engine.BinDir + Binding.managedDll);
			if (fileInfo3.LastWriteTime > fileInfo4.LastWriteTime)
			{
				return Engine.ReloadState.Managed;
			}
			return Engine.ReloadState.None;
		}
		public static void Reload(Engine.ReloadState reloadState)
		{
			if (reloadState == Engine.ReloadState.None)
			{
				return;
			}
			if (reloadState == Engine.ReloadState.Managed)
			{
				Engine.Reloading = true;
				MainForm.Instance.CloseSaveConfirmed = true;
				MainForm.Instance.Close();
				Binding.UnloadDll();
				return;
			}
			if (reloadState == Engine.ReloadState.Native)
			{
				Engine.Reloading = true;
				MainForm.Instance.CloseSaveConfirmed = true;
				MainForm.Instance.Close();
				Binding.FCE_Editor_Destroy();
				Binding.UnloadDll();
				Binding.UnloadIGEDll();
				File.Copy(Engine.BinDir + "output\\" + Binding.gameDll, Engine.BinDir + Binding.gameDll, true);
				string text = Path.ChangeExtension(Binding.gameDll, ".pdb");
				File.Copy(Engine.BinDir + "output\\" + text, Engine.BinDir + text, true);
				Binding.LoadIGEDll();
				Binding.LoadDll();
				Binding.FCE_Editor_Create(true);
				Binding.UnloadDll();
			}
		}
	}
}
