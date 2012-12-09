using FC3Editor.UI;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
namespace FC3Editor.Nomad
{
	internal static class Editor
	{
		public enum MouseEvent
		{
			MouseDown,
			MouseUp,
			MouseMove,
			MouseMoveDelta,
			MouseWheel,
			MouseEnter,
			MouseLeave
		}
		public enum KeyEvent
		{
			KeyDown,
			KeyChar,
			KeyUp
		}
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
				return Win32.GetActiveWindow() != IntPtr.Zero && MainForm.Instance != null && Win32.IsWindowEnabled(MainForm.Instance.Handle);
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
		public static ViewportControl Viewport
		{
			get
			{
				return MainForm.Instance.Viewport;
			}
		}
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
			Editor.OnEditorEvent(eventType, eventPtr);
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
			MainForm.Instance.EnableUI(enable);
		}
		public static bool GetScreenPointFromWorldPos(Vec3 worldPos, out Vec2 screenPoint)
		{
			return Editor.GetScreenPointFromWorldPos(worldPos, out screenPoint, false);
		}
		public static bool GetScreenPointFromWorldPos(Vec3 worldPos, out Vec2 screenPoint, bool clipped)
		{
			bool flag = Binding.FCE_Editor_GetScreenPointFromWorldPos(worldPos.X, worldPos.Y, worldPos.Z, out screenPoint.X, out screenPoint.Y);
			if (flag && clipped)
			{
				screenPoint.X = Math.Min(Math.Max(0f, screenPoint.X), 1f);
				screenPoint.Y = Math.Min(Math.Max(0f, screenPoint.Y), 1f);
			}
			return flag;
		}
		public static void GetWorldRayFromScreenPoint(Vec2 screenPoint, out Vec3 raySrc, out Vec3 rayDir)
		{
			Binding.FCE_Editor_GetWorldRayFromScreenPoint(screenPoint.X, screenPoint.Y, out raySrc.X, out raySrc.Y, out raySrc.Z, out rayDir.X, out rayDir.Y, out rayDir.Z);
		}
		public static bool RayCastTerrain(Vec3 raySrc, Vec3 rayDir, out Vec3 hitPos, out float hitDist)
		{
			return Binding.FCE_Editor_RayCastTerrain(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObject ignore, out Vec3 hitPos, out float hitDist)
		{
			Vec3 vec;
			return Editor.RayCastPhysics(raySrc, rayDir, ignore, out hitPos, out hitDist, out vec);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObject ignore, out Vec3 hitPos, out float hitDist, out Vec3 hitNormal)
		{
			return Binding.FCE_Editor_RayCastPhysics(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, ignore.Pointer, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist, out hitNormal.X, out hitNormal.Y, out hitNormal.Z);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObjectSelection ignore, out Vec3 hitPos, out float hitDist)
		{
			Vec3 vec;
			return Editor.RayCastPhysics(raySrc, rayDir, ignore, out hitPos, out hitDist, out vec);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObjectSelection ignore, out Vec3 hitPos, out float hitDist, out Vec3 hitNormal)
		{
			return Binding.FCE_Editor_RayCastPhysics2(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, ignore.Pointer, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist, out hitNormal.X, out hitNormal.Y, out hitNormal.Z);
		}
		public static void EnterIngame(string gameMode)
		{
			if (!Binding.FCE_Editor_ValidateIngame())
			{
				LocalizedMessageBox.Show(MainForm.Instance, Localizer.LocalizeCommon("MSG_DESC_INGAME_INVALID_OBJECTS"), Localizer.Localize("WARNING"), Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}
			MainForm.Instance.EnterIngame();
			Binding.FCE_Editor_EnterIngame(gameMode);
		}
		public static void ExitIngame()
		{
			Binding.FCE_Editor_ExitIngame();
			MainForm.Instance.ExitIngame();
		}
		public static bool RayCastTerrainFromScreenPoint(Vec2 screenPoint, out Vec3 hitPos)
		{
			Vec3 raySrc;
			Vec3 rayDir;
			Editor.GetWorldRayFromScreenPoint(screenPoint, out raySrc, out rayDir);
			float num;
			return Editor.RayCastTerrain(raySrc, rayDir, out hitPos, out num);
		}
		public static bool RayCastTerrainFromMouse(out Vec3 hitPos)
		{
			return Editor.RayCastTerrainFromScreenPoint(Editor.Viewport.NormalizedMousePos, out hitPos);
		}
		public static bool RayCastPhysicsFromScreenPoint(Vec2 screenPoint, out Vec3 hitPos)
		{
			Vec3 raySrc;
			Vec3 rayDir;
			Editor.GetWorldRayFromScreenPoint(screenPoint, out raySrc, out rayDir);
			float num;
			return Editor.RayCastPhysics(raySrc, rayDir, EditorObject.Null, out hitPos, out num);
		}
		public static bool RayCastPhysicsFromMouse(out Vec3 hitPos)
		{
			return Editor.RayCastPhysicsFromScreenPoint(Editor.Viewport.NormalizedMousePos, out hitPos);
		}
		public static void ApplyScreenDeltaToWorldPos(Vec2 screenDelta, ref Vec3 worldPos)
		{
			Vec3 vec = Camera.FrontVector;
			if ((double)Math.Abs(vec.X) < 0.001 && (double)Math.Abs(vec.Y) < 0.001)
			{
				vec = Camera.UpVector;
			}
			Vec2 vec2 = -vec.XY;
			vec2.Normalize();
			Vec2 vec3 = new Vec2(-vec2.Y, vec2.X);
			float num = (float)((double)Vec3.Dot(worldPos - Camera.Position, Camera.FrontVector) * Math.Tan((double)Camera.HalfFOV) * 2.0);
			worldPos.X += num * screenDelta.X * vec3.X + num * screenDelta.Y * vec2.X;
			worldPos.Y += num * screenDelta.X * vec3.Y + num * screenDelta.Y * vec2.Y;
		}
		public static void OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			foreach (IInputSink current in Editor.GetInputs())
			{
				if (current.OnMouseEvent(mouseEvent, mouseEventArgs))
				{
					break;
				}
			}
		}
		public static void OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			if (Editor.IsIngame)
			{
				if (keyEvent == Editor.KeyEvent.KeyUp && keyEventArgs.KeyCode == Keys.Escape)
				{
					Editor.ExitIngame();
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
		public static void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			foreach (IInputSink current in Editor.GetInputs())
			{
				current.OnEditorEvent(eventType, eventPtr);
			}
		}
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
		public static RegistryKey GetRegistrySettings()
		{
			return Registry.CurrentUser.CreateSubKey("Software\\Ubisoft\\Far Cry 3\\Editor");
		}
		public static int GetRegistryInt(string name, int defaultValue)
		{
			int registryInt;
			using (RegistryKey registrySettings = Editor.GetRegistrySettings())
			{
				registryInt = Editor.GetRegistryInt(registrySettings, name, defaultValue);
			}
			return registryInt;
		}
		public static int GetRegistryInt(RegistryKey key, string name, int defaultValue)
		{
			object value = key.GetValue(name);
			if (value is int)
			{
				return (int)value;
			}
			return defaultValue;
		}
		public static string GetRegistryString(RegistryKey key, string name, string defaultValue)
		{
			object value = key.GetValue(name);
			if (value is string)
			{
				return (string)value;
			}
			return defaultValue;
		}
		public static void SetRegistryInt(string name, int value)
		{
			using (RegistryKey registrySettings = Editor.GetRegistrySettings())
			{
				Editor.SetRegistryInt(registrySettings, name, value);
			}
		}
		public static void SetRegistryInt(RegistryKey key, string name, int value)
		{
			key.SetValue(name, value);
		}
	}
}
