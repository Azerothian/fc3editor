using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolRoad : ToolSpline
	{
		private ParamRoad m_paramRoad = new ParamRoad(null);
		private ParamFloat m_paramRoadWidth = new ParamFloat(Localizer.Localize("PARAM_ROAD_WIDTH"), 4f, 1f, 8f, 0.1f);
		private SplineRoad m_splineRoad;
		public ToolRoad()
		{
			this.m_paramRoad.ValueChanged += new EventHandler(this.spline_ValueChanged);
			this.m_paramRoadWidth.ValueChanged += new EventHandler(this.roadWidth_ValueChanged);
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_ROADS");
		}
		public override Image GetToolImage()
		{
			return Resources.Spline;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramEditTool;
			yield return this.m_paramRoadWidth;
			yield return this.m_paramRoad;
			yield break;
		}
		public override IParameter GetMainParameter()
		{
			return this.m_paramRoad;
		}
		public override string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_TOOL_ROAD") + "\r\n\r\n" + base.GetSplineHelp();
		}
		private void spline_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateSelectedSpline();
		}
		private void roadWidth_ValueChanged(object sender, EventArgs e)
		{
			if (this.m_splineRoad.IsValid)
			{
				this.m_splineRoad.Width = this.m_paramRoadWidth.Value;
				this.m_splineRoad.UpdateSpline();
			}
		}
		private void UpdateSelectedSpline()
		{
			this.SetSplineRoad((this.m_paramRoad.Value != -1) ? SplineManager.GetRoadFromId(this.m_paramRoad.Value) : SplineRoad.Null);
		}
		private void SetSplineRoad(SplineRoad splineRoad)
		{
			this.m_splineRoad = splineRoad;
			base.SetSpline(splineRoad);
			this.m_paramRoadWidth.Enabled = this.m_splineRoad.IsValid;
			if (this.m_splineRoad.IsValid)
			{
				this.m_paramRoadWidth.Value = this.m_splineRoad.Width;
			}
		}
		public override void Activate()
		{
			base.Activate();
			this.UpdateSelectedSpline();
		}
		public override void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
			base.OnEditorEvent(eventType, eventPtr);
			if (eventType == EditorEventUndo.TypeId)
			{
				this.m_paramRoad.UpdateUIControls();
			}
		}
	}
}
