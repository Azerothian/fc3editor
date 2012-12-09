using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamText : Parameter
	{
		public ParamText(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			return new Label
			{
				AutoSize = true,
				Padding = new Padding(0, 2, 0, 2),
				Text = base.DisplayName
			};
		}
		protected override void UpdateUIControl(Control control)
		{
			control.Text = base.DisplayName;
		}
		protected override void OnDisplayChanged()
		{
			base.UpdateUIControls();
		}
	}
}
