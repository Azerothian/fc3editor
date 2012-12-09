using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolEnvironment : ITool, IToolBase, IParameterProvider
	{
		private ParamTime m_paramTime = new ParamTime(Localizer.Localize("PARAM_TIME"));
		private ParamFloat m_paramCloudType = new ParamFloat(Localizer.Localize("PARAM_CLOUD_TYPE"), 0f, 0f, (float)(Engine.CloudTypeCount - 1), 1f);
		private ParamFloat m_paramStormFactor = new ParamFloat(Localizer.Localize("PARAM_STORM_FACTOR"), 0f, 0f, 1f, 0.01f);
		private ParamFloat m_paramWaterLevel = new ParamFloat(Localizer.Localize("PARAM_WATER_LEVEL"), 0f, 0f, 255f, 0.1f);
		public ToolEnvironment()
		{
			this.m_paramTime.ValueChanged += new EventHandler(this.time_ValueChanged);
			this.m_paramCloudType.ValueChanged += new EventHandler(this.cloudType_ValueChanged);
			this.m_paramStormFactor.ValueChanged += new EventHandler(this.stormFactor_ValueChanged);
			this.m_paramWaterLevel.ValueChanged += new EventHandler(this.waterLevel_ValueChanged);
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_ENVIRONMENT");
		}
		public Image GetToolImage()
		{
			return Resources.Environment;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramTime;
			yield return this.m_paramCloudType;
			yield return this.m_paramStormFactor;
			yield return this.m_paramWaterLevel;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		public string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_TOOL_ENVIRONMENT");
		}
		private void time_ValueChanged(object sender, EventArgs e)
		{
			Engine.TimeOfDay = this.m_paramTime.Value;
		}
		private void cloudType_ValueChanged(object sender, EventArgs e)
		{
			Engine.CloudType = (int)Math.Round((double)this.m_paramCloudType.Value);
		}
		private void stormFactor_ValueChanged(object sender, EventArgs e)
		{
			Engine.StormFactor = this.m_paramStormFactor.Value;
		}
		private void waterLevel_ValueChanged(object sender, EventArgs e)
		{
			TerrainManager.GlobalWaterLevel = this.m_paramWaterLevel.Value;
		}
		public void Activate()
		{
			this.m_paramTime.Value = Engine.TimeOfDay;
			this.m_paramCloudType.Value = (float)Engine.CloudType;
			this.m_paramStormFactor.Value = Engine.StormFactor;
			this.m_paramWaterLevel.Value = TerrainManager.GlobalWaterLevel;
		}
		public void Deactivate()
		{
		}
	}
}
