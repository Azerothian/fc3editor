using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamCollection : Parameter
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
		public ParamCollection(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ParamCollectionSlotList paramCollectionSlotList = new ParamCollectionSlotList();
			paramCollectionSlotList.Value = this.m_value;
			paramCollectionSlotList.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.Value = ((ParamCollectionSlotList)sender).Value;
			}
			;
			return paramCollectionSlotList;
		}
		protected override void UpdateUIControl(Control control)
		{
			((ParamCollectionSlotList)control).UpdateUI();
		}
	}
}
