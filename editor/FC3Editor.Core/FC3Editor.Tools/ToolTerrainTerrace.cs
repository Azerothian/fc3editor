using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainTerrace : ToolPaint
	{
		private ParamFloat m_strength = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 0.5f, 0f, 1f, 0.01f);
		private ParamFloat m_height = new ParamFloat(Localizer.Localize("PARAM_HEIGHT"), 2f, 0f, 32f, 0.01f);
		public ToolTerrainTerrace()
		{
			this.m_hardness.Value = 0.125f;
		}
		public override string GetToolName()
		{
			return "Terrain Terrace";
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Terrace;
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
			return "This tool levels out the terrain in multiple steps of equal height.\r\n\r\n" + base.GetPaintContextHelp() + "\r\n\r\n" + base.GetShortcutContextHelp();
		}
		protected override void OnBeginPaint()
		{
			base.OnBeginPaint();
			this.m_opacity.Value = this.m_strength.Value * 0.04f;
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			float value = this.m_height.Value;
			TerrainManipulator.Terrace(pos, value, 1.5f, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.Terrace_End();
		}
	}
}
