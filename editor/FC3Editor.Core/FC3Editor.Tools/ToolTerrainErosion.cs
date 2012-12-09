using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainErosion : ToolPaint
	{
		private ParamFloat m_density = new ParamFloat(Localizer.Localize("PARAM_DENSITY"), 0.5f, 0f, 1f, 0.01f);
		private ParamFloat m_deformation = new ParamFloat(Localizer.Localize("PARAM_DEFORMATION"), 0.5f, 0f, 1f, 0.01f);
		private ParamFloat m_channelDepth = new ParamFloat(Localizer.Localize("PARAM_LAND_SLIDE"), 0.5f, 0f, 1f, 0.01f);
		private ParamFloat m_randomness = new ParamFloat("Randomness", 0f, 0f, 1f, 0.01f);
		public ToolTerrainErosion()
		{
			this.m_square.Value = true;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_EROSION");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Erosion;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				if (current != this.m_square)
				{
					yield return current;
				}
			}
			yield return this.m_density;
			yield return this.m_deformation;
			yield return this.m_channelDepth;
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
				Localizer.LocalizeCommon("HELP_TOOL_EROSION")
			});
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			TerrainManipulator.Erosion(pos, this.m_radius.Value, this.m_density.Value, this.m_deformation.Value, this.m_channelDepth.Value, this.m_randomness.Value);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.Erosion_End();
		}
	}
}
