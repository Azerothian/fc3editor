using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainHole : ToolPaintStrict
	{
		private ParamBool m_hole = new ParamBool(Localizer.Localize("PARAM_HOLE"), true);
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_HOLE");
		}
		public override Image GetToolImage()
		{
			return Resources.Hole;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_hole;
			yield break;
		}
		public override IParameter GetMainParameter()
		{
			return null;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				Localizer.Localize("HELP_PAINT"),
				"\r\n",
				Localizer.Localize("HELP_SHORTCUT"),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_HOLE")
			});
		}
		protected override void OnPaint()
		{
			base.OnPaint();
			TerrainManipulator.Hole(this.m_snappedRect, (this.m_painting == ToolPaintStrict.PaintingMode.Plus) ? this.m_hole.Value : (!this.m_hole.Value));
		}
	}
}
