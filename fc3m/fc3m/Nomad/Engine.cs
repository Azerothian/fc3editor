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
using Nomad.Utils;
using Nomad.Enums;
namespace Nomad
{
	public class Engine
	{
		public static Game Game;
		public static Engine _instance;
		public delegate void InvokeDelegate();

		public bool TickAlways = false;
		private List<Engine.InvokeDelegate> m_delayedCallbacks = new List<Engine.InvokeDelegate>();
		private Binding.MessagePumpCallback m_delegateMessagePumpCallback;
		private bool m_initialized = false;

		#region Properties

		public static Engine Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new Engine();
				}
				return _instance;
			}
		}

		public string PersonalPath
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Engine_GetPersonalPath());
			}
		}
		public string GenericDataPath
		{
			get
			{
				return Marshal.PtrToStringAnsi(Binding.FCE_Engine_GetGenericDataPath());
			}
		}
		public bool Initialized
		{
			get
			{
				return m_initialized;
			}
		}
		public bool ConsoleOpened
		{
			get
			{
				return Nomad.Binding.FCE_Engine_IsConsoleOpen();
			}
		}
		public TimeSpan TimeOfDay
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
		public int CloudTypeCount
		{
			get
			{
				return Binding.FCE_Engine_GetCloudTypeCount();
			}
		}
		public int CloudType
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
		public float StormFactor
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
		public string BinFile
		{
			get;
			private set;
		}
		public string BinDir
		{
			get;
			private set;
		}
		public bool Reloading
		{
			get;
			set;
		}
		#endregion

		private void InitInternal()
		{
			Log.log("InitInternal");
			BinFile = Assembly.GetExecutingAssembly().Location;
			BinDir = Path.GetDirectoryName(BinFile) + "\\";
			Log.log("Binding Initialise");
			Binding.Initialise();
			Log.log("LoadDll");
			Binding.LoadDll();
			m_delegateMessagePumpCallback = new Binding.MessagePumpCallback(MessagePumpCallback);
		}
		private string GetLanguage()
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
		public bool Init(Game game)
		{
			Log.log("Init Game");

			InitInternal();
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			string text = " -editorpc -language=";
			string language = GetLanguage();
			if (language.Length == 0)
			{
				ResourceManager resourceManager = new ResourceManager("FC3.Properties.Resources", Assembly.GetExecutingAssembly());
				string @string = resourceManager.GetString("IDS_ERR_MISSING_REGISTRY_KEY");
				Log.error(@string, "Far Cry 3 Map Editor");
				return false;
			}
			text += language;
			int num = 1;
			if (EngineUtils.GetMapArgument() != null)
			{
				num = 2;
			}

			Log.log("Init DuniaEngine");
			if (!Binding.InitDuniaEngine(Process.GetCurrentProcess().MainModule.BaseAddress, game.MainForm.Handle, game.MainForm.Viewport.Handle, string.Join(" ", commandLineArgs, num, commandLineArgs.Length - num) + text, true, true, m_delegateMessagePumpCallback))
			{
				return false;
			}
			// TODO:Pretty Slow do we need?
			//foreach (ProcessModule processModule in Process.GetCurrentProcess().Modules)
			//{
			//	if (processModule.ModuleName.StartsWith("Nomad") || processModule.ModuleName.StartsWith("InGameEditor"))
			//	{
			//		Binding.FCE_Hack_Init(processModule.BaseAddress);
			//	}
			//}

			Log.log("Acquiring Input");
			Binding.FCE_Engine_AutoAcquireInput(true);
			//Editor.Init(); // TODO:Editor Init ? Hooks?
			game.Initialise();

			Binding.FCE_Engine_Reset(game.MainForm.Handle, game.MainForm.Viewport.Handle, m_delegateMessagePumpCallback);
			if (!Directory.Exists(PersonalPath))
			{
				Directory.CreateDirectory(PersonalPath);
			}
			m_initialized = true;
			return true;
		}
		public void Reset(System.Windows.Forms.Form mainWindow, System.Windows.Forms.Control viewport)
		{
			Log.log("Reset");
			InitInternal();
			//Editor.Init();// TODO:Editor Init ? Hooks?
			Binding.FCE_Engine_Reset(mainWindow.Handle, viewport.Handle, m_delegateMessagePumpCallback);
			m_initialized = true;
		}
		public void Close()
		{
			Log.log("Closing System");
			Binding.UnloadDll();
			Binding.CloseDuniaEngine();
		}

		public bool Running { get; set; }
		public void Run(bool tickAlways)
		{
			Log.log("Running");

			TickAlways |= tickAlways;
			Running = true;
			while (Running)
			{
				bool flag = Game.IsActive || TickAlways;
				if (m_delayedCallbacks.Count > 0)
				{
					flag = true;
					List<Engine.InvokeDelegate> delayedCallbacks;
					Monitor.Enter(delayedCallbacks = m_delayedCallbacks);
					try
					{
						foreach (Engine.InvokeDelegate current in m_delayedCallbacks)
						{
							current();
						}
						m_delayedCallbacks.Clear();
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
				System.Windows.Forms.Application.DoEvents();
			}
		}
		public void Invoke(Engine.InvokeDelegate callback)
		{
			Log.log("Invoking");
			List<Engine.InvokeDelegate> delayedCallbacks;
			Monitor.Enter(delayedCallbacks = m_delayedCallbacks);
			try
			{
				m_delayedCallbacks.Add(callback);
			}
			finally
			{
				Monitor.Exit(delayedCallbacks);
			}
		}
		private void MessagePumpCallback(bool deferQuit, bool blockRenderer)
		{
		}
		public void UpdateResolution(Size size)
		{

			if (Initialized)
			{
				Log.log("UpdateResolution");
				Binding.FCE_Engine_UpdateViewport(size.Width, size.Height);
			}
		}
		public ReloadState EvaluateReloadState()
		{
			Log.log("EvaluateReloadState");
			FileInfo fileInfo = new FileInfo(BinDir + "output\\" + Binding.gameDll);
			FileInfo fileInfo2 = new FileInfo(BinDir + Binding.gameDll);
			if (fileInfo.LastWriteTime > fileInfo2.LastWriteTime)
			{
				return ReloadState.Native;
			}
			FileInfo fileInfo3 = new FileInfo(BinDir + "output\\Nomad.dll");
			FileInfo fileInfo4 = new FileInfo(BinDir + Binding.managedDll);
			if (fileInfo3.LastWriteTime > fileInfo4.LastWriteTime)
			{
				return ReloadState.Managed;
			}
			return ReloadState.None;
		}
		public void Reload(ReloadState reloadState)
		{
			Log.log("Reload {0}", reloadState.ToString());
			if (reloadState == ReloadState.None)
			{
				return;
			}
			if (reloadState == ReloadState.Managed)
			{
				Reloading = true;
				// TODO:Editor Reload ? Hooks?
				//MainForm.Instance.CloseSaveConfirmed = true;
				//MainForm.Instance.Close();
				Binding.UnloadDll();
				return;
			}
			if (reloadState == ReloadState.Native)
			{
				Reloading = true;
				// TODO:Editor Reload Native ? Hooks?
				//MainForm.Instance.CloseSaveConfirmed = true;
				//MainForm.Instance.Close();
				Binding.FCE_Editor_Destroy();
				Binding.UnloadDll();
				Binding.UnloadIGEDll();
				File.Copy(BinDir + "output\\" + Binding.gameDll, BinDir + Binding.gameDll, true);
				string text = Path.ChangeExtension(Binding.gameDll, ".pdb");
				File.Copy(BinDir + "output\\" + text, BinDir + text, true);
				Binding.LoadIGEDll();
				Binding.LoadDll();
				Binding.FCE_Editor_Create(true);
				Binding.UnloadDll();
			}
		}
	}
}
