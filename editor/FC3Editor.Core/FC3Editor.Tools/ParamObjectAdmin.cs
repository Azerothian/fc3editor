using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ParamObjectAdmin : Parameter
	{
		public ParamObjectAdmin(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			ObjectAdmin objectAdmin = new ObjectAdmin();
			this.UpdateUIControl(objectAdmin);
			return objectAdmin;
		}
		protected override void UpdateUIControl(Control control)
		{
		}
	}
}
