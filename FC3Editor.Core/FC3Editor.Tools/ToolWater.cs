using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolWater : ITool, IToolBase, IParameterProvider, IInputSink
	{
		private ParamFloat m_paramWaterLevel = new ParamFloat(Localizer.Localize("PARAM_WATER_LEVEL"), 0f, 0f, 255f, 0.1f);
		private ParamWaterMaterial m_paramWaterMaterial = new ParamWaterMaterial(null);
		private bool m_painting;
		private bool m_cursorValid;
		private Vec3 m_cursorPos;
		private int m_cursorSX;
		private int m_cursorSY;
		public ToolWater()
		{
			this.m_paramWaterMaterial.Value = WaterInventory.Instance.GetFromId("MM-WaterStandard");
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_WATER");
		}
		public Image GetToolImage()
		{
			return Resources.Water;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramWaterLevel;
			yield return this.m_paramWaterMaterial;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		public string GetContextHelp()
		{
			return Localizer.Localize("HELP_CONTROLS_WATER") + "\r\n\r\n" + Localizer.LocalizeCommon("HELP_TOOL_WATER");
		}
		public void Activate()
		{
		}
		public void Deactivate()
		{
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		private void OnMouseMove(MouseEventArgs mouseEventArgs)
		{
			this.m_cursorValid = Editor.RayCastTerrainFromMouse(out this.m_cursorPos);
			if (this.m_cursorValid)
			{
				this.m_cursorSX = (int)(this.m_cursorPos.X / 64f);
				this.m_cursorSY = (int)(this.m_cursorPos.Y / 64f);
				if (this.m_painting)
				{
					TerrainManager.SetWaterLevelSector(this.m_cursorSX, this.m_cursorSY, ((Control.ModifierKeys & Keys.Control) == Keys.None) ? this.m_paramWaterLevel.Value : 0f, this.m_paramWaterMaterial.Value);
					TerrainManager.UpdateWaterLevel();
				}
			}
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			switch (mouseEvent)
			{
			case Editor.MouseEvent.MouseDown:
				if (mouseEventArgs.Button == MouseButtons.Left)
				{
					this.m_painting = true;
					UndoManager.RecordUndo();
					this.OnMouseMove(mouseEventArgs);
				}
				break;

			case Editor.MouseEvent.MouseUp:
				if (this.m_painting)
				{
					this.m_painting = false;
					UndoManager.CommitUndo();
				}
				break;

			case Editor.MouseEvent.MouseMove:
				this.OnMouseMove(mouseEventArgs);
				break;
			}
			return false;
		}
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}
		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		private void DrawSource(Vec3 cursorCenter, Vec3 cursorPos)
		{
			Color color = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.White : Color.Black;
			Color borderColor = ((Control.ModifierKeys & Keys.Control) == Keys.None) ? Color.Black : Color.White;
			float length = (Camera.Position - cursorCenter).Length;
			Render.DrawTerrainSquare(cursorCenter.XY, 32f, length * 0.02f, color, 0.01f, 0.31f, borderColor);
			Render.DrawTerrainCircle(cursorPos.XY, length * 0.0075f, length * 0.015f, color, 0f, 0f, borderColor);
		}
		private void DrawTarget(Vec3 cursorTarget)
		{
			float length = (Camera.Position - cursorTarget).Length;
			if (!this.m_painting)
			{
				Render.DrawQuad(cursorTarget, 64f, 64f, Color.FromArgb(24, Color.Turquoise));
			}
			Render.DrawSquare(cursorTarget, 32f, length * 0.02f, Color.DarkGreen, 0f, Color.Black);
		}
		public void Update(float dt)
		{
			if (this.m_cursorValid)
			{
				Vec3 vec = new Vec3((float)(this.m_cursorSX * 64 + 32), (float)(this.m_cursorSY * 64 + 32), 0f);
				vec.Z = TerrainManager.GetHeightAtWithWater(vec.XY);
				Vec3 vec2 = new Vec3(vec.X, vec.Y, this.m_paramWaterLevel.Value);
				float num = Vec3.Dot(vec - Camera.Position, Camera.FrontVector);
				float num2 = Vec3.Dot(vec2 - Camera.Position, Camera.FrontVector);
				if (num > num2)
				{
					this.DrawSource(vec, this.m_cursorPos);
					if ((Control.ModifierKeys & Keys.Control) == Keys.None && !this.m_painting)
					{
						this.DrawTarget(vec2);
						return;
					}
				}
				else
				{
					if ((Control.ModifierKeys & Keys.Control) == Keys.None && !this.m_painting)
					{
						this.DrawTarget(vec2);
					}
					this.DrawSource(vec, this.m_cursorPos);
				}
			}
		}
	}
}
