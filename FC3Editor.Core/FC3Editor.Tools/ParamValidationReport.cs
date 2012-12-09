using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamValidationReport : Parameter
	{
		private ValidationReport m_value;
		public event EventHandler ValueChanged;
		public ValidationReport Value
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
		public string ValidationName
		{
			get;
			set;
		}
		public ParamValidationReport(string display) : base(display)
		{
		}
		protected void OnValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
		protected override Control CreateUIControl()
		{
			ParamReport paramReport = new ParamReport();
			paramReport.ParameterName = base.DisplayName;
			this.UpdateUIControl(paramReport);
			return paramReport;
		}
		protected override void UpdateUIControl(Control control)
		{
			ParamReport paramReport = (ParamReport)control;
			if (!string.IsNullOrEmpty(this.ValidationName))
			{
				paramReport.ParameterName = base.DisplayName + ": " + this.ValidationName;
			}
			paramReport.UpdateUI(this.m_value);
		}
	}
}
