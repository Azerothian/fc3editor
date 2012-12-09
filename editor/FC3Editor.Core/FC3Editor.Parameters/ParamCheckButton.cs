using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamCheckButton : Parameter
	{
		public delegate void ButtonDelegate();
		private bool m_enabled = true;
		private bool m_checked;
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
				base.UpdateUIControls();
			}
		}
		public bool Checked
		{
			get
			{
				return this.m_checked;
			}
			set
			{
				this.m_checked = value;
				base.UpdateUIControls();
			}
		}
		public ParamCheckButton.ButtonDelegate ActivateCallback
		{
			get;
			set;
		}
		public ParamCheckButton.ButtonDelegate DeactivateCallback
		{
			get;
			set;
		}
		public ParamCheckButton(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			NomadCheckButton button = new NomadCheckButton();
			button.Text = base.DisplayName;
			button.CheckedChanged += delegate(object sender, EventArgs e)
			{
				this.m_checked = button.Checked;
				if (button.Checked)
				{
					if (this.ActivateCallback != null)
					{
						this.ActivateCallback();
						return;
					}
				}
				else
				{
					if (this.DeactivateCallback != null)
					{
						this.DeactivateCallback();
					}
				}
			}
			;
			return button;
		}
		protected override void UpdateUIControl(Control control)
		{
			NomadCheckButton nomadCheckButton = (NomadCheckButton)control;
			nomadCheckButton.Enabled = this.m_enabled;
			nomadCheckButton.Checked = this.m_checked;
		}
	}
}
