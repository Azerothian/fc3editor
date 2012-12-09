using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainRaiseLower : ToolPaint
	{
		private ParamFloat m_height = new ParamFloat(Localizer.Localize("PARAM_HEIGHT"), 5f, -32f, 32f, 0.01f);
		public ToolTerrainRaiseLower()
		{
			this.m_hardness.Value = 0.125f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_RAISE_LOWER");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_RaiseLower;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_height;
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
				Localizer.LocalizeCommon("HELP_TOOL_RAISELOWER")
			});
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			float value = this.m_height.Value;
			TerrainManipulator.RaiseLower(pos, (this.m_painting == ToolPaint.PaintingMode.Plus) ? value : (-value), this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.RaiseLower_End();
		}
	}
}
