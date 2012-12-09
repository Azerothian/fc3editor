using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolTerrainRamp : ToolPaint
	{
		private Vec3 m_rampStart;
		private bool m_rampStarted;
		public ToolTerrainRamp()
		{
			this.m_square.Enabled = false;
			this.m_distortion.Enabled = false;
			this.m_hardness.Value = 0.25f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_RAMP");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Ramp;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				Localizer.Localize("HELP_CONTROLS_RAMP"),
				"\r\n",
				base.GetShortcutContextHelp(),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_RAMP")
			});
		}
		public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			if (mouseEvent == Editor.MouseEvent.MouseUp && this.m_painting == ToolPaint.PaintingMode.None)
			{
				if (!this.m_rampStarted)
				{
					this.m_rampStarted = Editor.RayCastTerrainFromMouse(out this.m_rampStart);
				}
				else
				{
					Vec3 vec;
					if (Editor.RayCastTerrainFromMouse(out vec))
					{
						UndoManager.RecordUndo();
						TerrainManipulator.Ramp(this.m_rampStart.XY, vec.XY, this.m_radius.Value, this.m_hardness.Value);
						UndoManager.CommitUndo();
						this.m_rampStarted = false;
					}
				}
			}
			return base.OnMouseEvent(mouseEvent, mouseEventArgs);
		}
		protected override void OnBeginPaint()
		{
		}
		public override void Update(float dt)
		{
			if (this.m_rampStarted)
			{
				float length = (Camera.Position - this.m_rampStart).Length;
				Render.DrawTerrainCircle(this.m_rampStart.XY, this.m_radius.Value, length * 0.01f, Color.OrangeRed, -0.001f, 0f);
				Render.DrawTerrainCircle(this.m_rampStart.XY, length * 0.00375f, length * 0.0075f, Color.OrangeRed, -0.001f, 0f);
			}
			base.Update(dt);
		}
	}
}
