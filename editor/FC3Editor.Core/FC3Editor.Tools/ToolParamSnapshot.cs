using FC3Editor.Parameters;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Tools
{
	internal class ToolParamSnapshot : Parameter
	{
		public ToolParamSnapshot() : base(null)
		{
		}
		protected override Control CreateUIControl()
		{
			return new ParamSnapshot();
		}
	}
}
