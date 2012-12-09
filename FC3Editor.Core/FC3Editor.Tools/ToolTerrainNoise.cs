using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolTerrainNoise : ToolPaint
	{
		private ParamFloat m_strength = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 0.5f, 0f, 1f, 0.01f);
		private ParamFloat m_roughness = new ParamFloat(Localizer.Localize("PARAM_ROUGHNESS"), 0.5f, 0f, 1f, 0.01f);
		private ParamEnum<TerrainManipulator.NoiseType> m_noiseType = new ParamEnum<TerrainManipulator.NoiseType>(Localizer.Localize("PARAM_NOISE_TYPE"), TerrainManipulator.NoiseType.Absolute, ParamEnumUIType.ComboBox);
		public ToolTerrainNoise()
		{
			this.m_noiseType.Names = new string[]
			{
				Localizer.Localize("PARAM_NOISE_RAISE"),
				Localizer.Localize("PARAM_NOISE_LOWER"),
				Localizer.Localize("PARAM_NOISE_RAISE_LOWER")
			};
			ParamEnumBase<TerrainManipulator.NoiseType> arg_BF_0 = this.m_noiseType;
			TerrainManipulator.NoiseType[] array = new TerrainManipulator.NoiseType[3];
			array[0] = TerrainManipulator.NoiseType.Absolute;
			array[1] = TerrainManipulator.NoiseType.InverseAbsolute;
			arg_BF_0.Values = array;
			this.m_hardness.Value = 0.3f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TERRAIN_NOISE");
		}
		public override Image GetToolImage()
		{
			return Resources.TerrainEdit_Noise;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_strength;
			yield return this.m_roughness;
			yield return this.m_noiseType;
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
				Localizer.LocalizeCommon("HELP_TOOL_NOISE")
			});
		}
		protected override void OnBeginPaint()
		{
			base.OnBeginPaint();
			TerrainManipulator.Noise_Begin(8, 128f, this.m_roughness.Value, this.m_noiseType.Value);
		}
		protected override void OnPaintGrab(float x, float y)
		{
			base.OnPaintGrab(x, y);
			float amount = -y * this.m_strength.Value * 0.3f;
			TerrainManipulator.Noise(this.m_cursorPos.XY, amount, this.m_brush);
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			float num = this.m_strength.Value * 40f * dt;
			TerrainManipulator.Noise(pos, (this.m_painting == ToolPaint.PaintingMode.Plus) ? num : (-num), this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			TerrainManipulator.Noise_End();
		}
	}
}
