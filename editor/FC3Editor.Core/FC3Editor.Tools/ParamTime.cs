using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamTime : Parameter
	{
		private TimeSpan m_value;
		public event EventHandler ValueChanged;
		public TimeSpan Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				if (this.ValueChanged != null)
				{
					this.ValueChanged(this, new EventArgs());
				}
			}
		}
		public ParamTime(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ParamTimePicker paramTimePicker = new ParamTimePicker();
			paramTimePicker.ParameterName = base.DisplayName;
			paramTimePicker.Value = this.m_value;
			paramTimePicker.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.Value = ((ParamTimePicker)sender).Value;
			}
			;
			return paramTimePicker;
		}
	}
}
