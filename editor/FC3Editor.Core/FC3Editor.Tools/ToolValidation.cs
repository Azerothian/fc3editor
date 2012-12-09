using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolValidation : ITool, IToolBase, IParameterProvider
	{
		private static ToolValidation s_instance;
		private ParamEnum<GameModes> m_paramGameModes = new ParamEnum<GameModes>(Localizer.Localize("PARAM_GAMEMODE"), GameModes.Domination, ParamEnumUIType.List);
		private ParamValidationReport m_paramGameModeReport = new ParamValidationReport(Localizer.Localize("PARAM_GAMEMODE_CRITERIAS"));
		private ParamValidationReport m_paramGameReport = new ParamValidationReport(Localizer.Localize("PARAM_MAP_REPORT"));
		private ValidationReport m_gameModeReport;
		private ValidationReport m_gameReport;
		public static ToolValidation Instance
		{
			get
			{
				return ToolValidation.s_instance;
			}
		}
		public GameModes GameMode
		{
			get
			{
				return this.m_paramGameModes.Value;
			}
			set
			{
				this.m_paramGameModes.Value = value;
			}
		}
		public ToolValidation()
		{
			ToolValidation.s_instance = this;
			this.m_paramGameModes.Names = new string[]
			{
				Localizer.LocalizeCommon("VALIDATION_MODE_TDM"),
				Localizer.LocalizeCommon("VALIDATION_MODE_DOMINATION"),
				Localizer.LocalizeCommon("VALIDATION_MODE_TRANSMISSION"),
				Localizer.LocalizeCommon("VALIDATION_MODE_FIRESTORM")
			};
			this.m_paramGameModes.ValueChanged += new EventHandler(this.gameModes_ValueChanged);
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_VALIDATION");
		}
		public Image GetToolImage()
		{
			return Resources.Validation;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramGameModes;
			yield return this.m_paramGameModeReport;
			yield return this.m_paramGameReport;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return this.m_paramGameReport;
		}
		public string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_TOPIC_VALIDATION");
		}
		private void gameModes_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateReports();
		}
		public void Activate()
		{
			this.UpdateGameModesValidation();
			this.UpdateReports();
		}
		public void Deactivate()
		{
			if (this.m_gameModeReport.IsValid)
			{
				this.m_gameModeReport.Destroy();
			}
			if (this.m_gameReport.IsValid)
			{
				this.m_gameReport.Destroy();
			}
			this.RefreshReports();
		}
		private void ClearReports()
		{
			if (this.m_gameModeReport.IsValid)
			{
				this.m_gameModeReport.Destroy();
			}
			if (this.m_gameReport.IsValid)
			{
				this.m_gameReport.Destroy();
			}
		}
		private void RefreshReports()
		{
			this.m_paramGameModeReport.ValidationName = this.m_paramGameModes.Names[(int)this.m_paramGameModes.Value];
			this.m_paramGameModeReport.Value = this.m_gameModeReport;
			this.m_paramGameReport.Value = this.m_gameReport;
		}
		private void UpdateGameModesValidation()
		{
			Image[] array = new Image[this.m_paramGameModes.Names.Length];
			for (int i = 0; i < this.m_paramGameModes.Values.Length; i++)
			{
				bool flag = false;
				bool flag2 = false;
				ValidationReport validationReport = Validation.ValidateGameMode(this.m_paramGameModes.Values[i]);
				for (int j = 0; j < validationReport.Count; j++)
				{
					if (validationReport[j].Severity == ValidationRecord.Severities.Error)
					{
						flag = true;
						break;
					}
					if (validationReport[j].Severity == ValidationRecord.Severities.Warning)
					{
						flag2 = true;
					}
				}
				if (flag)
				{
					array[i] = Resources.error16;
				}
				else
				{
					if (flag2)
					{
						array[i] = Resources.warning16;
					}
					else
					{
						array[i] = Resources.valid16;
					}
				}
			}
			this.m_paramGameModes.Images = array;
			this.m_paramGameModes.UpdateUIControls();
		}
		private void UpdateReports()
		{
			this.ClearReports();
			this.m_gameModeReport = Validation.ValidateGameMode(this.m_paramGameModes.Value);
			this.m_gameReport = Validation.ValidateGame();
			this.RefreshReports();
		}
	}
}
