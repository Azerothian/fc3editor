using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamButton : Parameter
	{
		public delegate void ButtonDelegate();
		private bool m_enabled = true;
		private ParamButton.ButtonDelegate m_callback;
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
		public ParamButton.ButtonDelegate Callback
		{
			get
			{
				return this.m_callback;
			}
			set
			{
				this.m_callback = value;
			}
		}
		public ParamButton(string display, ParamButton.ButtonDelegate callback) : base(display)
		{
			this.m_callback = callback;
		}
		protected override Control CreateUIControl()
		{
			NomadButton nomadButton = new NomadButton();
			nomadButton.Text = base.DisplayName;
			nomadButton.Click += delegate(object sender, EventArgs e)
			{
				this.m_callback();
			}
			;
			return nomadButton;
		}
		protected override void UpdateUIControl(Control control)
		{
			NomadButton nomadButton = (NomadButton)control;
			nomadButton.Enabled = this.m_enabled;
		}
	}
}
