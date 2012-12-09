using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolPlayableZone : ToolSpline
	{
		private ParamButton m_actionReset = new ParamButton(Localizer.Localize("PARAM_RESET"), null);
		public ToolPlayableZone()
		{
			this.m_actionReset.Callback = new ParamButton.ButtonDelegate(this.action_Reset);
		}
		public override string GetToolName()
		{
			return Localizer.Localize("TOOL_PLAYABLE_ZONE");
		}
		public override Image GetToolImage()
		{
			return Resources.PlayableZone;
		}
		public override IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramEditTool;
			yield return this.m_actionReset;
			yield break;
		}
		public override string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_TOOL_PLAYABLEZONE") + "\r\n\r\n" + base.GetSplineHelp();
		}
		private void action_Reset()
		{
			UndoManager.RecordUndo();
			SplineManager.GetPlayableZone().Reset();
			UndoManager.CommitUndo();
		}
		public override void Activate()
		{
			base.Activate();
			base.SetSpline(SplineManager.GetPlayableZone());
		}
	}
}
