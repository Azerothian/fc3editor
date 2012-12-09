using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainFlatten : ToolPaint
	{
		private float m_height;
		public ToolTerrainFlatten()
		{
			this.m_hardness.Value = 0.25f;
			this.m_opacity.Value = 0.75f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_FLATTEN");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Flatten;
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
				Localizer.LocalizeCommon("HELP_TOOL_FLATTEN")
			});
		}
		protected override void OnBeginPaint()
		{
			base.OnBeginPaint();
			this.m_height = TerrainManipulator.GetAverageHeight(this.m_cursorPos.XY, this.m_brush);
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			if (this.m_painting == ToolPaint.PaintingMode.Plus)
			{
				TerrainManipulator.SetHeight(pos, this.m_height, this.m_brush);
				return;
			}
			TerrainManipulator.Average(pos, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			if (this.m_painting == ToolPaint.PaintingMode.Plus)
			{
				TerrainManipulator.SetHeight_End();
				return;
			}
			TerrainManipulator.Average_End();
		}
	}
}
