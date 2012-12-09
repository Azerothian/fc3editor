using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainBump : ToolPaint
	{
		private ParamFloat m_strength = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 0.5f, 0f, 1f, 0.01f);
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_BUMP");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Bump;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_strength;
			yield return this.m_grabMode;
			yield break;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				base.GetPaintContextHelp(),
				"\r\n",
				base.GetShortcutContextHelp(),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_BUMP")
			});
		}
		protected override void OnPaintGrab(float x, float y)
		{
			base.OnPaintGrab(x, y);
			float amount = -y * this.m_strength.Value * 0.3f;
			TerrainManipulator.Bump(this.m_cursorPos.XY, amount, this.m_brush);
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			float num = this.m_strength.Value * 32f * dt;
			TerrainManipulator.Bump(pos, (this.m_painting == ToolPaint.PaintingMode.Plus) ? num : (-num), this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.Bump_End();
		}
	}
}
