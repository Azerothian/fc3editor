using System;
using System.Collections.Generic;
using System.Diagnostics;
using Nomad;
using Nomad.Utils;
using Nomad.Interfaces;
using Nomad.Editor;
using Nomad.Maths;
using Nomad.Logic;
using Nomad.Enums;
using Nomad.Components;
using Microsoft.Win32;

namespace Nomad.Editor
{
	internal static class Editor
	{

		private static Binding.EditorUpdateCallback m_delegateUpdateCallback;
		private static Binding.EditorEventCallback m_delegateEventCallback;
		private static Binding.EditorLoadCompletedCallback m_delegateLoadCompletedCallback;
		private static Binding.EditorSaveCompletedCallback m_delegateSaveCompletedCallback;
		private static Binding.EditorEnableUICallback m_delegateEnableUICallback;
		private static List<IInputSink> m_inputStack = new List<IInputSink>();
		public static bool IsActive
		{
			get
			{
				//TODO: Fix isActive Check
				return true;
				//return Win32.GetActiveWindow() != IntPtr.Zero && MainForm.Instance != null && Win32.IsWindowEnabled(MainForm.Instance.Handle);
			}
		}
		public static bool IsLoadPending
		{
			get
			{
				return Binding.FCE_Editor_IsLoadPending();
			}
		}
		public static float FrameTime
		{
			get
			{
				return Binding.FCE_Editor_GetFrameTime();
			}
		}
		public static bool IsIngame
		{
			get
			{
				return Binding.FCE_Editor_IsIngame();
			}
		}
		// TODO: Need to shift this out

		//public static ViewportControl Viewport
		//{
		//	get
		//	{
		//		return MainForm.Instance.Viewport;
		//	}
		//}
		public static void Init()
		{
			Editor.m_delegateUpdateCallback = new Binding.EditorUpdateCallback(Editor.UpdateCallback);
			Binding.FCE_Editor_Update_Callback(Editor.m_delegateUpdateCallback);
			Editor.m_delegateEventCallback = new Binding.EditorEventCallback(Editor.EventCallback);
			Binding.FCE_Editor_Event_Callback(Editor.m_delegateEventCallback);
			Editor.m_delegateLoadCompletedCallback = new Binding.EditorLoadCompletedCallback(Editor.LoadCompletedCallback);
			Binding.FCE_Editor_LoadCompleted_Callback(Editor.m_delegateLoadCompletedCallback);
			Editor.m_delegateSaveCompletedCallback = new Binding.EditorSaveCompletedCallback(Editor.SaveCompletedCallback);
			Binding.FCE_Editor_SaveCompleted_Callback(Editor.m_delegateSaveCompletedCallback);
			Editor.m_delegateEnableUICallback = new Binding.EditorEnableUICallback(Editor.EnableUICallback);
			Binding.FCE_Editor_EnableUI_Callback(Editor.m_delegateEnableUICallback);
			while (!Binding.FCE_Editor_IsInitialized())
			{
				Binding.TickDuniaEngine();
			}
		}
		private static void UpdateCallback(float dt)
		{
			Editor.OnUpdate(dt);
		}
		private static void EventCallback(uint eventType, IntPtr eventPtr)
		{
			//Editor.OnEditorEvent(eventType, eventPtr);
		}
		private static void LoadCompletedCallback(bool success)
		{
			EditorDocument.OnLoadCompleted(success);
		}
		private static void SaveCompletedCallback(bool success)
		{
			EditorDocument.OnSaveCompleted(success);
		}
		private static void EnableUICallback(bool enable)
		{

			//TODO: Need to fix this MainForm EnabledUI
			//MainForm.Instance.EnableUI(enable);
		}

		public static void OnMouseEvent(MouseEvent mouseEvent, System.Windows.Forms.MouseEventArgs mouseEventArgs)
		{
			foreach (IInputSink current in Editor.GetInputs())
			{
				if (current.OnMouseEvent(mouseEvent, mouseEventArgs))
				{
					break;
				}
			}
		}
		public static void OnKeyEvent(KeyEvent keyEvent, System.Windows.Forms.KeyEventArgs keyEventArgs)
		{
			if (Editor.IsIngame)
			{
				if (keyEvent == KeyEvent.KeyUp && keyEventArgs.KeyCode == System.Windows.Forms.Keys.Escape)
				{
				//	Editor.ExitIngame();
					return;
				}
			}
			else
			{
				foreach (IInputSink current in Editor.GetInputs())
				{
					if (current.OnKeyEvent(keyEvent, keyEventArgs))
					{
						break;
					}
				}
			}
		}

		//TODO: Do we need this? OnEditorEvent
		//public static void OnEditorEvent(uint eventType, IntPtr eventPtr)
		//{
		//	foreach (IInputSink current in Editor.GetInputs())
		//	{
		//		current.OnEditorEvent(eventType, eventPtr);
		//	}
		//}
		public static void OnUpdate(float dt)
		{
			foreach (IInputSink current in Editor.GetInputs())
			{
				current.Update(dt);
			}
		}
		public static void PushInput(IInputSink input)
		{
			Trace.Assert(!Editor.m_inputStack.Contains(input));
			if (Editor.m_inputStack.Count > 0)
			{
				Editor.m_inputStack[Editor.m_inputStack.Count - 1].OnInputRelease();
			}
			Editor.m_inputStack.Add(input);
			input.OnInputAcquire();
		}
		public static void PopInput(IInputSink input)
		{
			int num = Editor.m_inputStack.LastIndexOf(input);
			if (num == -1)
			{
				return;
			}
			Editor.m_inputStack[Editor.m_inputStack.Count - 1].OnInputRelease();
			Editor.m_inputStack.RemoveRange(num, Editor.m_inputStack.Count - num);
			if (Editor.m_inputStack.Count > 0)
			{
				Editor.m_inputStack[Editor.m_inputStack.Count - 1].OnInputAcquire();
			}
		}
		private static IEnumerable<IInputSink> GetInputs()
		{
			for (int i = Editor.m_inputStack.Count - 1; i >= 0; i--)
			{
				yield return Editor.m_inputStack[i];
			}
			yield break;
		}
		
	}
}
