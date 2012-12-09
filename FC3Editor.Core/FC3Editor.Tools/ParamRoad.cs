using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamRoad : Parameter
	{
		protected int m_value = -1;
		public event EventHandler ValueChanged;
		public int Value
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
		public ParamRoad(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ParamSplineSlotList paramSplineSlotList = new ParamSplineSlotList();
			paramSplineSlotList.Value = this.m_value;
			paramSplineSlotList.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.Value = ((ParamSplineSlotList)sender).Value;
			}
			;
			return paramSplineSlotList;
		}
		protected override void UpdateUIControl(Control control)
		{
			((ParamSplineSlotList)control).UpdateUI();
		}
	}
}
