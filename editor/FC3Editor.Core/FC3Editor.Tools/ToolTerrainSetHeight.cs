using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolTerrainSetHeight : ToolPaint
	{
		private bool m_picking;
		private ParamFloat m_height = new ParamFloat(Localizer.Localize("PARAM_HEIGHT"), 32f, 0f, 256f, 0.01f);
		private ParamFloat m_strength = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 0.75f, 0f, 1f, 0.01f);
		public ToolTerrainSetHeight()
		{
			this.m_hardness.Value = 0.25f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_SET_HEIGHT");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_SetHeight;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_strength;
			yield return this.m_height;
			yield break;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				Localizer.Localize("HELP_PICK_HEIGHT"),
				"\r\n",
				base.GetShortcutContextHelp(),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_SETHEIGHT")
			});
		}
		public override bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			if ((Control.ModifierKeys & Keys.Control) == Keys.None)
			{
				this.m_picking = false;
				return base.OnMouseEvent(mouseEvent, mouseEventArgs);
			}
			switch (mouseEvent)
			{
			case Editor.MouseEvent.MouseDown:
				if (!this.m_picking)
				{
					this.m_picking = true;
					this.UpdatePicking();
				}
				break;

			case Editor.MouseEvent.MouseUp:
				this.m_picking = false;
				break;

			case Editor.MouseEvent.MouseMove:
				if (this.m_picking)
				{
					this.UpdatePicking();
				}
				break;
			}
			return false;
		}
		protected override void OnBeginPaint()
		{
			base.OnBeginPaint();
			this.m_opacity.Value = this.m_strength.Value;
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			float value = this.m_height.Value;
			TerrainManipulator.SetHeight(pos, value, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.SetHeight_End();
		}
		public override void Update(float dt)
		{
			if ((Control.ModifierKeys & Keys.Control) == Keys.None)
			{
				base.Update(dt);
			}
		}
		private void UpdatePicking()
		{
			this.m_height.Value = this.m_cursorPos.Z;
		}
	}
}
