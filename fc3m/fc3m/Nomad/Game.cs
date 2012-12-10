using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Nomad.Editor;
using Nomad.Interfaces;
using Nomad.Utils;

namespace Nomad
{
	public abstract class Game : ApplicationContext, IGame
	{

		#region delegates
		private Binding.EditorUpdateCallback m_delegateUpdateCallback;
		private Binding.EditorEventCallback m_delegateEventCallback;
		private Binding.EditorLoadCompletedCallback m_delegateLoadCompletedCallback;
		private Binding.EditorSaveCompletedCallback m_delegateSaveCompletedCallback;
		private Binding.EditorEnableUICallback m_delegateEnableUICallback;
		public event EventHandler OnPostLoad;

		#endregion
		#region fields
		private static List<IInputSink> m_inputStack = new List<IInputSink>();

		public static string programGuid = "9de9f6ee-6db7-41bf-a0b4-112e45dd3693";
		#endregion
		#region properties
		public bool IsLoadPending
		{
			get
			{
				return Binding.FCE_Editor_IsLoadPending();
			}
		}
		public float FrameTime
		{
			get
			{
				return Binding.FCE_Editor_GetFrameTime();
			}
		}
		public bool IsInGame
		{
			get { return Binding.FCE_Editor_IsIngame(); }
		}


		public bool IsActive { get; set; }

		public abstract NomadForm MainForm { get; }
		public abstract ViewportControl Viewport { get; }
		#endregion
		public void Initialise()
		{
			IsActive = true;
			m_delegateUpdateCallback = new Binding.EditorUpdateCallback(UpdateCallback);
			Binding.FCE_Editor_Update_Callback(m_delegateUpdateCallback);
			m_delegateEventCallback = new Binding.EditorEventCallback(EventCallback);
			Binding.FCE_Editor_Event_Callback(m_delegateEventCallback);
			m_delegateLoadCompletedCallback = new Binding.EditorLoadCompletedCallback(LoadCompletedCallback);
			Binding.FCE_Editor_LoadCompleted_Callback(m_delegateLoadCompletedCallback);
			m_delegateSaveCompletedCallback = new Binding.EditorSaveCompletedCallback(SaveCompletedCallback);
			Binding.FCE_Editor_SaveCompleted_Callback(m_delegateSaveCompletedCallback);
			m_delegateEnableUICallback = new Binding.EditorEnableUICallback(EnableUICallback);
			Binding.FCE_Editor_EnableUI_Callback(m_delegateEnableUICallback);
			while (!Binding.FCE_Editor_IsInitialized())
			{
				Binding.TickDuniaEngine();
			}
		}

		public virtual void OnMouseEvent(Enums.MouseEvent me, System.Windows.Forms.MouseEventArgs args)
		{
			
		}



		public virtual void OnKeyEvent(Enums.KeyEvent ke, System.Windows.Forms.KeyEventArgs args)
		{
			if (ke == Enums.KeyEvent.KeyUp && args.KeyValue == 80)
			{
				EngineUtils.EnterIngame("FCXEditor");
			}
		}

		public virtual void UpdateCallback(float dt)
		{
			foreach (IInputSink current in GetInputs())
			{
				current.Update(dt);
			}
			//Editor.OnUpdate(dt);
		}
		private static void EventCallback(uint eventType, IntPtr eventPtr)
		{
			//Editor.OnEditorEvent(eventType, eventPtr);
		}
		public virtual void LoadCompletedCallback(bool success)
		{
			//EditorDocument.OnLoadCompleted(success);
		}
		public virtual void SaveCompletedCallback(bool success)
		{
			//EditorDocument.OnSaveCompleted(success);
		}
		public virtual void EnableUICallback(bool enable)
		{

			//TODO: Need to fix this MainForm EnabledUI
			//MainForm.Instance.EnableUI(enable);
		}
		#region Inputs
		public void PushInput(IInputSink input)
		{
			Trace.Assert(!m_inputStack.Contains(input));
			if (m_inputStack.Count > 0)
			{
				m_inputStack[m_inputStack.Count - 1].OnInputRelease();
			}
			m_inputStack.Add(input);
			input.OnInputAcquire();
		}
		public void PopInput(IInputSink input)
		{
			int num = m_inputStack.LastIndexOf(input);
			if (num == -1)
			{
				return;
			}
			m_inputStack[m_inputStack.Count - 1].OnInputRelease();
			m_inputStack.RemoveRange(num, m_inputStack.Count - num);
			if (m_inputStack.Count > 0)
			{
				m_inputStack[m_inputStack.Count - 1].OnInputAcquire();
			}
		}
		private static IEnumerable<IInputSink> GetInputs()
		{
			for (int i = m_inputStack.Count - 1; i >= 0; i--)
			{
				yield return m_inputStack[i];
			}
			yield break;
		}
		#endregion




		public void Run(bool engineRunning, string initMapPath)
		{
			bool flag;
			using (new Mutex(true, programGuid, out flag))
			{
				if (!flag)
				{
					Win32.EnumWindows(new Win32.EnumWindowsProc(OpenExistingAppCallback), IntPtr.Zero);
				}
				else
				{
					//MainForm mainForm = new MainForm();
					if (!engineRunning)
					{
						//	SplashForm.Start();
						bool flag2 = Engine.Instance.Init(this);
						//	SplashForm.Stop();
						if (!flag2)
						{

							return;
						}
					}
					else
					{
						Engine.Instance.Reset(MainForm, Viewport);
					}
					
					if (OnPostLoad != null)
					{
						OnPostLoad(this, null);
					}
					//this.LoadMapInternal(initMapPath, null);
					//EngineUtils.EnterIngame("FCXEditor");
					Engine.Instance.Run(true);

					if (!Engine.Instance.Reloading)
					{
						Engine.Instance.Close();
					}
				}
			}
		}
		private void LoadMapInternal(string fileName, EditorDocument.LoadCompletedCallback callback)
		{
			EditorDocument.Load(fileName, callback);
		}
		public static bool OpenExistingAppCallback(IntPtr hWnd, IntPtr lParam)
		{
			if (Win32.GetProp(hWnd, Game.programGuid) != IntPtr.Zero)
			{
				Win32.SetForegroundWindow(hWnd);
				IntPtr intPtr = IntPtr.Zero;
				int cbData = 0;
				string mapArgument = EngineUtils.GetMapArgument();
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
	}
}
