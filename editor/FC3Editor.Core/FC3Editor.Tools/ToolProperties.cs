using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.Properties;
using FC3Editor.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolProperties : ITool, IToolBase, IParameterProvider
	{
		private ParamString m_paramMapName = new ParamString(Localizer.Localize("PARAM_MAP_NAME"), null);
		private ParamText m_paramCreatorName = new ParamText("");
		private ParamString m_paramAuthorName = new ParamString(Localizer.Localize("PARAM_MAP_AUTHOR"), null);
		private ParamEnum<EditorDocument.BattlefieldSizes> m_paramBattlefield = new ParamEnum<EditorDocument.BattlefieldSizes>(Localizer.Localize("PARAM_MAP_SIZE"), EditorDocument.BattlefieldSizes.Medium, ParamEnumUIType.ComboBox);
		private ParamEnum<EditorDocument.PlayerSizes> m_paramPlayers = new ParamEnum<EditorDocument.PlayerSizes>(Localizer.Localize("PARAM_MAP_PLAYERS"), EditorDocument.PlayerSizes.Medium, ParamEnumUIType.ComboBox);
		private ToolParamSnapshot m_paramSnapshot = new ToolParamSnapshot();
		private ParamBool m_paramNavmeshEnabled = new ParamBool(Localizer.Localize("PARAM_NAVMESH_ENABLED"), false);
		private ParamString m_paramMapId = new ParamString(Localizer.Localize("PARAM_MAP_MAPID"), null);
		private ParamButton m_actionGenerateMapId = new ParamButton(Localizer.Localize("PARAM_MAP_GENERATEMAPID"), null);
		private ParamText m_paramVersionId = new ParamText("");
		public ToolProperties()
		{
			this.m_paramBattlefield.Names = new string[]
			{
				Localizer.LocalizeCommon("PROPERTIES_BATTLEZONE_SMALL"),
				Localizer.LocalizeCommon("PROPERTIES_BATTLEZONE_MEDIUM"),
				Localizer.LocalizeCommon("PROPERTIES_BATTLEZONE_LARGE")
			};
			this.m_paramPlayers.Names = new string[]
			{
				"2-4",
				"4-8",
				"8-12",
				"12-16"
			};
			this.m_paramMapName.ValueChanged += new EventHandler(this.mapName_ValueChanged);
			this.m_paramAuthorName.ValueChanged += new EventHandler(this.authorName_ValueChanged);
			this.m_paramBattlefield.ValueChanged += new EventHandler(this.battlefield_ValueChanged);
			this.m_paramPlayers.ValueChanged += new EventHandler(this.players_ValueChanged);
			this.m_paramNavmeshEnabled.ValueChanged += new EventHandler(this.paramNavmeshEnabled_ValueChanged);
			this.m_paramMapId.ValueChanged += new EventHandler(this.mapId_ValueChanged);
			this.m_actionGenerateMapId.Callback = new ParamButton.ButtonDelegate(this.action_GenerateMapId);
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_PROPERTIES");
		}
		public Image GetToolImage()
		{
			return Resources.Properties;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramMapName;
			yield return this.m_paramCreatorName;
			yield return this.m_paramAuthorName;
			yield return this.m_paramBattlefield;
			yield return this.m_paramPlayers;
			yield return this.m_paramSnapshot;
			yield return this.m_paramNavmeshEnabled;
			yield return this.m_paramMapId;
			yield return this.m_actionGenerateMapId;
			yield return this.m_paramVersionId;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		public string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_TOPIC_PROPERTIES") + "\r\n\r\n" + Localizer.LocalizeCommon("HELP_TOOL_SNAPSHOT");
		}
		private void mapName_ValueChanged(object sender, EventArgs e)
		{
			EditorDocument.MapName = this.m_paramMapName.Value;
		}
		private void authorName_ValueChanged(object sender, EventArgs e)
		{
			EditorDocument.AuthorName = this.m_paramAuthorName.Value;
		}
		private void battlefield_ValueChanged(object sender, EventArgs e)
		{
			EditorDocument.BattlefieldSize = this.m_paramBattlefield.Value;
		}
		private void players_ValueChanged(object sender, EventArgs e)
		{
			EditorDocument.PlayerSize = this.m_paramPlayers.Value;
		}
		private void paramNavmeshEnabled_ValueChanged(object sender, EventArgs e)
		{
			EditorDocument.NavmeshEnabled = this.m_paramNavmeshEnabled.Value;
		}
		private bool ConfirmMapIdChange()
		{
			return LocalizedMessageBox.Show(MainForm.Instance, Localizer.Localize("MSG_WARNING_MAPID_CHANGE"), Localizer.Localize("WARNING"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), null, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes;
		}
		private void mapId_ValueChanged(object sender, EventArgs e)
		{
			if (this.ConfirmMapIdChange())
			{
				Guid guid = Guid.Empty;
				try
				{
					guid = new Guid(this.m_paramMapId.Value);
				}
				catch (Exception)
				{
				}
				if (guid != Guid.Empty)
				{
					EditorDocument.MapId = guid;
				}
			}
			this.m_paramMapId.Value = EditorDocument.MapId.ToString();
		}
		private void action_GenerateMapId()
		{
			if (!this.ConfirmMapIdChange())
			{
				return;
			}
			Guid mapId = Guid.NewGuid();
			EditorDocument.MapId = mapId;
			this.m_paramMapId.Value = EditorDocument.MapId.ToString();
		}
		public void Activate()
		{
			this.m_paramMapName.MaxLength = 20;
			this.m_paramAuthorName.MaxLength = 20;
			this.m_paramMapName.Value = EditorDocument.MapName;
			string creatorName = EditorDocument.CreatorName;
			this.m_paramCreatorName.DisplayName = Localizer.Localize("PARAM_MAP_CREATOR") + ": " + (string.IsNullOrEmpty(creatorName) ? Localizer.Localize("PARAM_UNDEFINED") : creatorName);
			this.m_paramAuthorName.Value = EditorDocument.AuthorName;
			this.m_paramBattlefield.Value = EditorDocument.BattlefieldSize;
			this.m_paramPlayers.Value = EditorDocument.PlayerSize;
			this.m_paramNavmeshEnabled.Value = EditorDocument.NavmeshEnabled;
			this.m_paramMapId.Value = EditorDocument.MapId.ToString();
			this.m_paramVersionId.DisplayName = Localizer.Localize("PARAM_MAP_VERSIONID") + ": " + EditorDocument.VersionId.ToString();
		}
		public void Deactivate()
		{
		}
	}
}
