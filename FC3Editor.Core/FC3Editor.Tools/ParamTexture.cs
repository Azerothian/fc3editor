using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamTexture : Parameter
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
		public ParamTexture(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ParamTextureSlotList paramTextureSlotList = new ParamTextureSlotList();
			paramTextureSlotList.Value = this.m_value;
			paramTextureSlotList.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.Value = ((ParamTextureSlotList)sender).Value;
			}
			;
			return paramTextureSlotList;
		}
		protected override void UpdateUIControl(Control control)
		{
			((ParamTextureSlotList)control).UpdateUI();
		}
	}
}
