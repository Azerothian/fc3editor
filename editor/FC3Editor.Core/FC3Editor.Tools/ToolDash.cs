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
	internal class ToolDash : ITool, IToolBase, IParameterProvider, IInputSink
	{
		private ParamEnumBase<int> m_paramNode = new ParamEnumBase<int>("Dash Node", 0, ParamEnumUIType.Buttons);
		private ParamFloat m_paramAssaultTime = new ParamFloat("Assault time", 0f, 0f, 300f, 1f);
		private ParamButton m_paramTestDash = new ParamButton("Test Dash", null);
		private GameModeDashNodeParams m_nodeParams;
		public ToolDash()
		{
			this.m_paramNode.Names = new string[]
			{
				"A",
				"B",
				"C",
				"D",
				"E",
				"F"
			};
			this.m_paramNode.Values = new int[]
			{
				0,
				1,
				2,
				3,
				4,
				5
			};
			this.m_paramNode.ValueChanged += new EventHandler(this.paramNode_ValueChanged);
			this.m_paramAssaultTime.ValueChanged += new EventHandler(this.paramAssaultTime_ValueChanged);
			this.m_paramTestDash.Callback = new ParamButton.ButtonDelegate(this.action_TestDash);
		}
		public string GetToolName()
		{
			return "Dash Setup";
		}
		public Image GetToolImage()
		{
			return Resources.Dash;
		}
		public IEnumerable<IParameter> GetParameters()
		{
			yield return this.m_paramNode;
			yield return this.m_paramAssaultTime;
			yield return this.m_paramTestDash;
			yield break;
		}
		public IParameter GetMainParameter()
		{
			return null;
		}
		public string GetContextHelp()
		{
			return null;
		}
		public void Activate()
		{
			this.UpdateNodeUI(this.m_paramNode.Value);
		}
		public void Deactivate()
		{
			this.UpdateNodeUI(-1);
		}
		private void paramNode_ValueChanged(object sender, EventArgs e)
		{
			this.UpdateNodeUI(this.m_paramNode.Value);
		}
		private void paramAssaultTime_ValueChanged(object sender, EventArgs e)
		{
			if (this.m_nodeParams == null)
			{
				return;
			}
			this.m_nodeParams.AssaultTime = this.m_paramAssaultTime.Value;
		}
		private void action_TestDash()
		{
			if (!GameModeDash.ValidateIngame())
			{
				if (LocalizedMessageBox.Show(MainForm.Instance, Localizer.LocalizeCommon("MSG_DESC_DASH_TEST_INVALID"), Localizer.Localize("ERROR"), Localizer.Localize("Generic", "GENERIC_YES"), Localizer.Localize("Generic", "GENERIC_NO"), null, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
				{
					MainForm.Instance.CurrentTool = ToolValidation.Instance;
				}
				return;
			}
			Editor.EnterIngame("FCXDash");
		}
		private void EnableNodeUI(bool enabled)
		{
			this.m_paramAssaultTime.Enabled = enabled;
		}
		private void UpdateNodeUI(int nodeIndex)
		{
			GameModeDash.SetDebugMode(nodeIndex);
			if (nodeIndex == -1)
			{
				this.m_nodeParams = null;
				this.EnableNodeUI(false);
				return;
			}
			this.m_nodeParams = GameModeDash.GetNodeParams(nodeIndex);
			this.m_paramAssaultTime.Value = this.m_nodeParams.AssaultTime;
			this.EnableNodeUI(true);
		}
		public void OnInputAcquire()
		{
		}
		public void OnInputRelease()
		{
		}
		public bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{
			return false;
		}
		public bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}
		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		public void Update(float dt)
		{
		}
	}
}
