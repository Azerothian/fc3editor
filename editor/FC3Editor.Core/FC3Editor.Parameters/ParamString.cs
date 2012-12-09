using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamString : Parameter
	{
		protected string m_value;
		protected int m_maxLength;
		public event EventHandler ValueChanged;
		public string Value
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
		public int MaxLength
		{
			get
			{
				return this.m_maxLength;
			}
			set
			{
				this.m_maxLength = value;
				base.UpdateUIControls();
			}
		}
		public ParamString(string display, string value) : base(display)
		{
			this.m_maxLength = 128;
			this.Value = value;
		}
		protected override Control CreateUIControl()
		{
			ParamStringField control = new ParamStringField();
			control.ParameterName = base.DisplayName;
			control.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnUIValueChanged(control.Value);
			}
			;
			return control;
		}
		protected override void UpdateUIControl(Control control)
		{
			((ParamStringField)control).Value = this.Value;
			((ParamStringField)control).MaxLength = this.MaxLength;
		}
		protected void OnUIValueChanged(string value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, null);
			}
		}
	}
}
