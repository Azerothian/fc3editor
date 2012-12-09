using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamBool : Parameter
	{
		protected bool m_value;
		private bool m_enabled = true;
		public event EventHandler ValueChanged;
		public bool Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				base.UpdateUIControls();
			}
		}
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
		public ParamBool(string display, bool value) : base(display)
		{
			this.Value = value;
		}
		protected override Control CreateUIControl()
		{
			CheckBox control = new CheckBox();
			control.Text = base.DisplayName;
			control.AutoSize = true;
			control.Padding = new Padding(2, 2, 0, 2);
			control.Click += delegate(object sender, EventArgs e)
			{
				this.OnUIValueChanged(control.Checked);
			}
			;
			return control;
		}
		protected override void UpdateUIControl(Control control)
		{
			control.Enabled = this.m_enabled;
			((CheckBox)control).Checked = this.Value;
		}
		protected void OnUIValueChanged(bool value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, null);
			}
		}
	}
}
