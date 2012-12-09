using FC3Editor.Nomad;
using FC3Editor.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class EditorSettingsDock : UserDockableWindow, IParameterProvider
	{
		private ParamBool m_paramShowFog = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_FOG"), true);
		private ParamBool m_paramShowExposure = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_EXPOSURE"), true);
		private ParamBool m_paramShowShadow = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_SHADOWS"), true);
		private ParamBool m_paramShowWater = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_WATER"), true);
		private ParamBool m_paramShowCollections = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_COLLECTIONS"), true);
		private ParamBool m_paramShowIcons = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_ICONS"), true);
		private ParamBool m_paramEnableSound = new ParamBool(Localizer.LocalizeCommon("SETTINGS_ENABLE_SOUND"), false);
		private ParamBool m_paramShowGrid = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SHOW_GRID"), false);
		private ParamEnumBase<float> m_paramGridResolution = new ParamEnumBase<float>(Localizer.LocalizeCommon("SETTINGS_SHOW_GRID_RESOLUTION"), 64f, ParamEnumUIType.Buttons);
		private ParamBool m_paramInvincible = new ParamBool(Localizer.LocalizeCommon("SETTINGS_INVINCIBILITY"), false);
		private ParamBool m_paramInvisible = new ParamBool(Localizer.LocalizeCommon("SETTINGS_INVISIBILITY"), false);
		private ParamBool m_paramSnapObjects = new ParamBool(Localizer.LocalizeCommon("SETTINGS_SNAP_TO_TERRAIN"), true);
		private ParamBool m_paramAutoSnappingObjects = new ParamBool(Localizer.LocalizeCommon("SETTINGS_AUTO_SNAP_OBJECTS"), true);
		private ParamBool m_paramAutoSnappingObjectsRotation = new ParamBool(Localizer.LocalizeCommon("SETTINGS_AUTO_SNAP_OBJECTS_ROTATION"), true);
		private ParamBool m_paramAutoSnappingObjectsTerrain = new ParamBool(Localizer.LocalizeCommon("SETTINGS_AUTO_SNAP_OBJECTS_TERRAIN"), true);
		private ParamBool m_paramInvertMouseView = new ParamBool(Localizer.Localize("SETTINGS_INVERT_MOUSE_VIEW"), false);
		private ParamBool m_paramInvertMousePan = new ParamBool(Localizer.Localize("SETTINGS_INVERT_MOUSE_PAN"), false);
		private ParamEnumBase<EditorSettings.QualityLevel> m_paramEngineQuality = new ParamEnumBase<EditorSettings.QualityLevel>(Localizer.Localize("SETTINGS_ENGINE_QUALITY"), EditorSettings.QualityLevel.UltraHigh, ParamEnumUIType.ComboBox);
		private ParamEnumBase<float> m_paramViewportQuality = new ParamEnumBase<float>(Localizer.Localize("SETTINGS_VIEWPORT_QUALITY"), 1f, ParamEnumUIType.ComboBox);
		private ParamBool m_paramKillDistanceOverride = new ParamBool(Localizer.Localize("SETTINGS_KILL_DISTANCE_OVERRIDE"), false);
		private IContainer components;
		private ParametersList parametersList;
		public EditorSettingsDock()
		{
			this.InitializeComponent();
			this.m_paramShowFog.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_FOG");
			this.m_paramShowExposure.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_SHOW_EXPOSURE");
			this.m_paramShowShadow.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_SHADOW");
			this.m_paramShowWater.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_WATER");
			this.m_paramShowCollections.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_COLLECTION");
			this.m_paramShowIcons.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_ICONS");
			this.m_paramEnableSound.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_SOUND");
			this.m_paramShowGrid.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_GRID");
			this.m_paramInvincible.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_INVINCIBILITY");
			this.m_paramInvisible.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_INVISIBILITY");
			this.m_paramSnapObjects.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_KEEPOBJTERRAIN");
			this.m_paramAutoSnappingObjects.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_AUTOSNAP_OBJ");
			this.m_paramAutoSnappingObjectsRotation.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_AUTOSNAP_OBJ_ROT");
			this.m_paramAutoSnappingObjectsTerrain.ToolTip = Localizer.LocalizeCommon("HELP_SETTINGS_AUTOSNAP_OBJ_TER");
			this.m_paramInvertMouseView.ToolTip = Localizer.Localize("HELP_SETTINGS_INVERT_MOUSE_VIEW");
			this.m_paramInvertMousePan.ToolTip = Localizer.Localize("HELP_SETTINGS_INVERT_MOUSE_PAN");
			this.m_paramEngineQuality.ToolTip = Localizer.Localize("HELP_SETTINGS_ENGINE_QUALITY");
			this.m_paramViewportQuality.ToolTip = Localizer.Localize("HELP_SETTINGS_VIEWPORT_QUALITY");
			this.m_paramKillDistanceOverride.ToolTip = Localizer.Localize("HELP_SETTINGS_KILL_DISTANCE");
			this.m_paramShowFog.ValueChanged += new EventHandler(this.paramShowFog_ValueChanged);
			this.m_paramShowExposure.ValueChanged += new EventHandler(this.paramShowExposure_ValueChanged);
			this.m_paramShowShadow.ValueChanged += new EventHandler(this.paramShowShadow_ValueChanged);
			this.m_paramShowWater.ValueChanged += new EventHandler(this.paramShowWater_ValueChanged);
			this.m_paramShowCollections.ValueChanged += new EventHandler(this.paramShowCollections_ValueChanged);
			this.m_paramShowIcons.ValueChanged += new EventHandler(this.paramShowIcons_ValueChanged);
			this.m_paramEnableSound.ValueChanged += new EventHandler(this.paramEnableSound_ValueChanged);
			this.m_paramShowGrid.ValueChanged += new EventHandler(this.paramShowGrid_ValueChanged);
			this.m_paramGridResolution.ValueChanged += new EventHandler(this.paramGridResolution_ValueChanged);
			this.m_paramInvincible.ValueChanged += new EventHandler(this.paramInvincible_ValueChanged);
			this.m_paramInvisible.ValueChanged += new EventHandler(this.paramInvisible_ValueChanged);
			this.m_paramSnapObjects.ValueChanged += new EventHandler(this.paramSnapObjects_ValueChanged);
			this.m_paramAutoSnappingObjects.ValueChanged += new EventHandler(this.paramAutoSnappingObjects_ValueChanged);
			this.m_paramAutoSnappingObjectsRotation.ValueChanged += new EventHandler(this.paramAutoSnappingObjectsRotation_ValueChanged);
			this.m_paramAutoSnappingObjectsTerrain.ValueChanged += new EventHandler(this.paramAutoSnappingObjectsTerrain_ValueChanged);
			this.m_paramInvertMouseView.ValueChanged += new EventHandler(this.paramInvertMouseView_ValueChanged);
			this.m_paramInvertMousePan.ValueChanged += new EventHandler(this.paramInvertMousePan_ValueChanged);
			this.m_paramEngineQuality.ValueChanged += new EventHandler(this.paramEngineQuality_ValueChanged);
			this.m_paramViewportQuality.ValueChanged += new EventHandler(this.paramViewportQuality_ValueChanged);
			this.m_paramKillDistanceOverride.ValueChanged += new EventHandler(this.paramKillDistanceOverride_ValueChanged);
			this.m_paramGridResolution.Names = new string[]
			{
				"16",
				"32",
				"64",
				"128"
			};
			this.m_paramGridResolution.Values = new float[]
			{
				16f,
				32f,
				64f,
				128f
			};
			this.m_paramEngineQuality.Names = new string[]
			{
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_LOW"),
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_MEDIUM"),
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_HIGH"),
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_VERY_HIGH"),
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_ULTRA"),
				Localizer.Localize("Video", "OPTIONS_CONTROLS_PC_OPTIMAL")
			};
			this.m_paramEngineQuality.Values = new EditorSettings.QualityLevel[]
			{
				EditorSettings.QualityLevel.Low,
				EditorSettings.QualityLevel.Medium,
				EditorSettings.QualityLevel.High,
				EditorSettings.QualityLevel.VeryHigh,
				EditorSettings.QualityLevel.UltraHigh,
				EditorSettings.QualityLevel.Optimal
			};
			this.m_paramViewportQuality.Names = new string[]
			{
				"100%",
				"90%",
				"80%",
				"70%",
				"60%",
				"50%"
			};
			this.m_paramViewportQuality.Values = new float[]
			{
				1f,
				0.9f,
				0.8f,
				0.7f,
				0.6f,
				0.5f
			};
			this.Text = Localizer.Localize(this.Text);
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramShowFog;
			yield return this.m_paramShowExposure;
			yield return this.m_paramShowWater;
			yield return this.m_paramShowCollections;
			yield return this.m_paramShowIcons;
			yield return this.m_paramEnableSound;
			yield return this.m_paramShowGrid;
			yield return this.m_paramGridResolution;
			yield return this.m_paramInvincible;
			yield return this.m_paramInvisible;
			yield return this.m_paramSnapObjects;
			yield return this.m_paramAutoSnappingObjectsTerrain;
			yield return this.m_paramInvertMouseView;
			yield return this.m_paramInvertMousePan;
			yield return this.m_paramEngineQuality;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		private void paramShowFog_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowFog = this.m_paramShowFog.Value;
		}
		private void paramShowExposure_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowExposure = this.m_paramShowExposure.Value;
		}
		private void paramShowShadow_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowShadow = this.m_paramShowShadow.Value;
		}
		private void paramShowWater_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowWater = this.m_paramShowWater.Value;
		}
		private void paramShowCollections_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowCollections = this.m_paramShowCollections.Value;
		}
		private void paramShowIcons_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowIcons = this.m_paramShowIcons.Value;
		}
		private void paramEnableSound_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.SoundEnabled = this.m_paramEnableSound.Value;
		}
		private void paramShowGrid_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ShowGrid = this.m_paramShowGrid.Value;
			this.m_paramGridResolution.Enabled = this.m_paramShowGrid.Value;
		}
		private void paramGridResolution_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.GridResolution = (int)this.m_paramGridResolution.Value;
		}
		private void paramInvincible_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.Invincible = this.m_paramInvincible.Value;
		}
		private void paramInvisible_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.Invisible = this.m_paramInvisible.Value;
		}
		private void paramSnapObjects_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.SnapObjectsToTerrain = this.m_paramSnapObjects.Value;
		}
		private void paramAutoSnappingObjects_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.AutoSnappingObjects = this.m_paramAutoSnappingObjects.Value;
			this.m_paramAutoSnappingObjectsRotation.Enabled = this.m_paramAutoSnappingObjects.Value;
		}
		private void paramAutoSnappingObjectsRotation_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.AutoSnappingObjectsRotation = this.m_paramAutoSnappingObjectsRotation.Value;
		}
		private void paramAutoSnappingObjectsTerrain_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.AutoSnappingObjectsTerrain = this.m_paramAutoSnappingObjectsTerrain.Value;
		}
		private void paramInvertMouseView_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.InvertMouseView = this.m_paramInvertMouseView.Value;
		}
		private void paramInvertMousePan_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.InvertMousePan = this.m_paramInvertMousePan.Value;
		}
		private void paramEngineQuality_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.EngineQuality = this.m_paramEngineQuality.Value;
		}
		private void paramViewportQuality_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.ViewportQuality = this.m_paramViewportQuality.Value;
			Editor.Viewport.UpdateSize();
		}
		private void paramKillDistanceOverride_ValueChanged(object sender, EventArgs e)
		{
			EditorSettings.KillDistanceOverride = this.m_paramKillDistanceOverride.Value;
		}
		public void RefreshSettings()
		{
			if (Engine.Initialized)
			{
				this.m_paramShowFog.Value = EditorSettings.ShowFog;
				this.m_paramShowExposure.Value = EditorSettings.ShowExposure;
				this.m_paramShowShadow.Value = EditorSettings.ShowShadow;
				this.m_paramShowWater.Value = EditorSettings.ShowWater;
				this.m_paramShowCollections.Value = EditorSettings.ShowCollections;
				this.m_paramShowIcons.Value = EditorSettings.ShowIcons;
				this.m_paramEnableSound.Value = EditorSettings.SoundEnabled;
				this.m_paramShowGrid.Value = EditorSettings.ShowGrid;
				this.m_paramGridResolution.Value = (float)EditorSettings.GridResolution;
				this.m_paramGridResolution.Enabled = this.m_paramShowGrid.Value;
				this.m_paramInvincible.Value = EditorSettings.Invincible;
				this.m_paramInvisible.Value = EditorSettings.Invisible;
				this.m_paramSnapObjects.Value = EditorSettings.SnapObjectsToTerrain;
				this.m_paramAutoSnappingObjects.Value = EditorSettings.AutoSnappingObjects;
				this.m_paramAutoSnappingObjectsRotation.Value = EditorSettings.AutoSnappingObjectsRotation;
				this.m_paramAutoSnappingObjectsTerrain.Value = EditorSettings.AutoSnappingObjectsTerrain;
				this.m_paramInvertMouseView.Value = EditorSettings.InvertMouseView;
				this.m_paramInvertMousePan.Value = EditorSettings.InvertMousePan;
				this.m_paramEngineQuality.Value = EditorSettings.EngineQuality;
				this.m_paramViewportQuality.Value = EditorSettings.ViewportQuality;
				this.m_paramKillDistanceOverride.Value = EditorSettings.KillDistanceOverride;
			}
		}
		private void EditorSettingsDock_Load(object sender, EventArgs e)
		{
			this.parametersList.Parameters = this;
			this.RefreshSettings();
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.parametersList = new ParametersList();
			base.SuspendLayout();
			this.parametersList.AutoScroll = true;
			this.parametersList.Dock = DockStyle.Fill;
			this.parametersList.Location = new Point(0, 0);
			this.parametersList.Name = "parametersList";
			this.parametersList.Parameters = null;
			this.parametersList.Size = new Size(250, 400);
			this.parametersList.TabIndex = 1;
			base.Controls.Add(this.parametersList);
			base.Name = "EditorSettingsDock";
			base.ShowOptions = false;
			this.Text = "DOCK_EDITOR_SETTINGS";
			base.Load += new EventHandler(this.EditorSettingsDock_Load);
			base.ResumeLayout(false);
		}
	}
}
