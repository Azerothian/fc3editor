using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolTexture : ToolPaint
	{
		private ParamFloat m_paramStrength = new ParamFloat(Localizer.Localize("PARAM_SPEED"), 0.5f, 0f, 1f, 0.01f);
		private ParamTexture m_paramTexture = new ParamTexture("");
		private ParamBool m_paramConstraints = new ParamBool(Localizer.Localize("PARAM_CONSTRAINTS"), false);
		private ParamFloat m_paramMinHeight = new ParamFloat(Localizer.Localize("PARAM_ALTITUDE_MIN"), 0f, 0f, 255f, 0.01f);
		private ParamFloat m_paramMaxHeight = new ParamFloat(Localizer.Localize("PARAM_ALTITUDE_MAX"), 255f, 0f, 255f, 0.01f);
		private ParamFloat m_paramHeightFuzziness = new ParamFloat(Localizer.Localize("PARAM_ALTITUDE_FUZZINESS"), 0f, 0f, 32f, 0.01f);
		private ParamFloat m_paramMinSlope = new ParamFloat(Localizer.Localize("PARAM_SLOPE_MIN"), 0f, 0f, 90f, 0.01f);
		private ParamFloat m_paramMaxSlope = new ParamFloat(Localizer.Localize("PARAM_SLOPE_MAX"), 90f, 0f, 90f, 0.01f);
		public ToolTexture()
		{
			this.m_paramMinHeight.Enabled = false;
			this.m_paramMaxHeight.Enabled = false;
			this.m_paramHeightFuzziness.Enabled = false;
			this.m_paramMinSlope.Enabled = false;
			this.m_paramMaxSlope.Enabled = false;
			this.m_paramConstraints.ValueChanged += new EventHandler(this.constraints_ValueChanged);
			this.m_hardness.Value = 0.85f;
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_TEXTURE");
		}
		public override Image GetToolImage()
		{
			return Resources.Texture;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			foreach (IParameter current in base._GetParameters())
			{
				yield return current;
			}
			yield return this.m_paramStrength;
			yield return this.m_paramTexture;
			yield return this.m_paramConstraints;
			yield return this.m_paramMinHeight;
			yield return this.m_paramMaxHeight;
			yield return this.m_paramHeightFuzziness;
			yield return this.m_paramMinSlope;
			yield return this.m_paramMaxSlope;
			yield break;
		}
		public override IParameter GetMainParameter()
		{
			return this.m_paramTexture;
		}
		public override string GetContextHelp()
		{
			return string.Concat(new string[]
			{
				base.GetPaintContextHelp(),
				"\r\n",
				base.GetShortcutContextHelp(),
				"\r\n\r\n",
				Localizer.LocalizeCommon("HELP_TOOL_TEXTURE")
			});
		}
		private void constraints_ValueChanged(object sender, EventArgs e)
		{
			this.m_paramMinHeight.Enabled = this.m_paramConstraints.Value;
			this.m_paramMaxHeight.Enabled = this.m_paramConstraints.Value;
			this.m_paramHeightFuzziness.Enabled = this.m_paramConstraints.Value;
			this.m_paramMinSlope.Enabled = this.m_paramConstraints.Value;
			this.m_paramMaxSlope.Enabled = this.m_paramConstraints.Value;
		}
		protected override void OnBeginPaint()
		{
			base.OnBeginPaint();
			if (this.m_paramConstraints.Value)
			{
				TextureManipulator.PaintConstraints_Begin(this.m_paramMinHeight.Value, this.m_paramMaxHeight.Value, this.m_paramHeightFuzziness.Value, this.m_paramMinSlope.Value, this.m_paramMaxSlope.Value);
			}
		}
		protected override void OnPaint(float dt, Vec2 pos)
		{
			base.OnPaint(dt, pos);
			int num = (Control.ModifierKeys != Keys.Control) ? this.m_paramTexture.Value : 0;
			if (num == -1)
			{
				return;
			}
			TextureInventory.Entry textureEntryFromId = TerrainManager.GetTextureEntryFromId(num);
			if (!textureEntryFromId.IsValid)
			{
				return;
			}
			if (!this.m_paramConstraints.Value)
			{
				TextureManipulator.Paint(pos, this.m_paramStrength.Value * 512f * dt, num, this.m_brush);
				return;
			}
			TextureManipulator.PaintConstraints(pos, this.m_paramStrength.Value * 512f * dt, num, this.m_brush);
		}
		protected override void OnEndPaint()
		{
			base.OnEndPaint();
			if (!this.m_paramConstraints.Value)
			{
				TextureManipulator.Paint_End();
				return;
			}
			TextureManipulator.PaintConstraints_End();
		}
		public override void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			if (eventType == EditorEventUndo.TypeId)
			{
				this.m_paramTexture.UpdateUIControls();
			}
		}
	}
}
