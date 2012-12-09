using FC3Editor.Nomad;
using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamWaterMaterial : Parameter
	{
		protected WaterInventory.Entry m_value;
		public event EventHandler ValueChanged;
		public WaterInventory.Entry Value
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
		public ParamWaterMaterial(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ParamWaterPicker paramWaterPicker = new ParamWaterPicker();
			paramWaterPicker.Entry = this.m_value;
			paramWaterPicker.EntryChanged += delegate(object sender, EventArgs e)
			{
				this.Value = ((ParamWaterPicker)sender).Entry;
			}
			;
			return paramWaterPicker;
		}
		protected override void UpdateUIControl(Control control)
		{
			((ParamWaterPicker)control).UpdateUI();
		}
	}
}
