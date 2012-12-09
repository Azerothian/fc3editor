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
	internal class ToolNavmesh : ITool, IToolBase, IParameterProvider, IInputSink
	{
		private ParamEnumBase<Navmesh.Layer> m_paramLayer = new ParamEnumBase<Navmesh.Layer>(Localizer.Localize("PARAM_NAVMESH_LAYER"), Navmesh.Layer.Character, ParamEnumUIType.Buttons);
		private ParamBool m_displayNavmesh = new ParamBool(Localizer.Localize("PARAM_DISPLAY_NAVMESH"), false);
		private ParamBool m_displayAP = new ParamBool(Localizer.Localize("PARAM_DISPLAY_ACTIONPOINTS"), false);
		private ParamFloat m_displayAlpha = new ParamFloat(Localizer.Localize("PARAM_TRANSPARENCY"), 0.7f, 0f, 1f, 0.01f);
		private ParamBool m_alwaysDisplay = new ParamBool(Localizer.Localize("PARAM_ALWAYS_DISPLAY"), true);
		private ParamCheckButton m_regenerateTile = new ParamCheckButton("Regenerate tile");
		private bool m_regenerating;
		private bool m_cursorValid;
		private Vec3 m_cursorPos;
		public ToolNavmesh()
		{
			this.m_paramLayer.Names = new string[]
			{
				Localizer.Localize("PARAM_CHARACTER"),
				Localizer.Localize("PARAM_VEHICLE")
			};
			this.m_paramLayer.Values = new Navmesh.Layer[]
			{
				Navmesh.Layer.Character,
				Navmesh.Layer.Vehicle
			};
			this.m_paramLayer.Images = new Image[]
			{
				Resources.Character,
				Resources.Vehicle
			};
			this.m_paramLayer.ValueChanged += new EventHandler(this.paramLayer_ValueChanged);
			this.m_displayNavmesh.ValueChanged += new EventHandler(this.paramDisplayNavmesh_ValueChanged);
			this.m_displayAP.ValueChanged += new EventHandler(this.paramDisplayAP_ValueChanged);
			this.m_displayAlpha.ValueChanged += new EventHandler(this.displayAlpha_ValueChanged);
			ParamCheckButton expr_161 = this.m_regenerateTile;
			expr_161.ActivateCallback = (ParamCheckButton.ButtonDelegate)Delegate.Combine(expr_161.ActivateCallback, new ParamCheckButton.ButtonDelegate(this.regenerateTile_Activate));
			ParamCheckButton expr_188 = this.m_regenerateTile;
			expr_188.DeactivateCallback = (ParamCheckButton.ButtonDelegate)Delegate.Combine(expr_188.DeactivateCallback, new ParamCheckButton.ButtonDelegate(this.regenerateTile_Deactivate));
		}
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_NAVMESH");
		}
		public Image GetToolImage()
		{
			return Resources.Navmesh;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramLayer;
			yield return this.m_displayNavmesh;
			yield return this.m_displayAP;
			yield return this.m_displayAlpha;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		public string GetContextHelp()
		{
			return Localizer.LocalizeCommon("HELP_SETTINGS_SHOW_NAVMESH") + "\r\n\r\n" + Localizer.LocalizeCommon("HELP_SETTINGS_SHOW_COVERS");
		}
		private void UpdateDisplay()
		{
			if (this.m_displayNavmesh.Value)
			{
				EditorSettings.ShowNavmesh(this.m_paramLayer.Value);
			}
			else
			{
				EditorSettings.HideNavmesh();
			}
			EditorSettings.ShowCovers = this.m_displayAP.Value;
		}
		public void Activate()
		{
			if (!EditorDocument.NavmeshEnabled)
			{
				if (LocalizedMessageBox.Show(MainForm.Instance, Localizer.LocalizeCommon("EDITOR_NAVMESH_PROMPT"), Localizer.Localize("EDITOR_CONFIRMATION"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), null, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
				{
					MainForm.Instance.CurrentTool = null;
					return;
				}
				EditorDocument.NavmeshEnabled = true;
			}
			this.m_displayAlpha.Value = 1f - Navmesh.DebugAlpha;
			this.m_displayNavmesh.Value = EditorSettings.IsNavmeshVisible;
			this.m_paramLayer.Value = EditorSettings.NavmeshLayer;
			this.m_displayAP.Value = EditorSettings.ShowCovers;
			this.UpdateDisplay();
		}
		public void Deactivate()
		{
		}
		private void paramLayer_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateDisplay();
		}
		private void paramDisplayNavmesh_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateDisplay();
		}
		private void paramDisplayAP_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateDisplay();
		}
		private void displayAlpha_ValueChanged(object sender, EventArgs e)
		{
			Navmesh.DebugAlpha = 1f - this.m_displayAlpha.Value;
		}
		private void regenerateTile_Activate()
		{
			this.m_regenerating = true;
		}
		private void regenerateTile_Deactivate()
		{
			this.m_regenerating = false;
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			switch (mouseEvent)
			{
			case Editor.MouseEvent.MouseUp:
				if (mouseEventArgs.Button == MouseButtons.Left && this.m_cursorValid)
				{
					Navmesh.RegenerateTileAt(this.m_cursorPos.XY, true);
					this.m_regenerateTile.Checked = false;
				}
				break;

			case Editor.MouseEvent.MouseMove:
				this.m_cursorValid = Editor.RayCastTerrainFromMouse(out this.m_cursorPos);
				break;
			}
			return false;
		}
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}
		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		private Vec3 GetTileCenter(Vec3 cursorPos)
		{
			return new Vec3
			{
				X = cursorPos.X + 4f - cursorPos.X % 8f,
				Y = cursorPos.Y + 4f - cursorPos.Y % 8f,
				Z = cursorPos.Z
			};
		}
		public void Update(float dt)
		{
			if (this.m_regenerating && this.m_cursorValid)
			{
				Render.DrawTerrainSquare(this.GetTileCenter(this.m_cursorPos).XY, 4f, 0.5f, Color.DarkGreen, 0.01f, 0.31f);
			}
		}
	}
}
