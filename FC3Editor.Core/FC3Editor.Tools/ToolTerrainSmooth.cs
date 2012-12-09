using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainSmooth : ToolPaint
	{
		public ToolTerrainSmooth()
		{
			this.m_hardness.Value = 0.125f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_SMOOTH");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Smooth;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_opacity;
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
				Localizer.LocalizeCommon("HELP_TOOL_SMOOTH")
			});
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			TerrainManipulator.Smooth(pos, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.Smooth_End();
		}
	}
}
