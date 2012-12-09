using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal interface IParameter
	{
		string DisplayName
		{
			get;
		}
		string ToolTip
		{
			get;
		}
		Control AcquireUIControl();
		void ReleaseUIControl(Control control);
	}
}
