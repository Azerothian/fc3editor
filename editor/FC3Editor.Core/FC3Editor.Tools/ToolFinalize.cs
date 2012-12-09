using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal class ToolFinalize : IToolAction, IToolBase
	{
		public string GetToolName()
		{
			return Localizer.Localize("TOOL_MAKE_BEAUTIFUL");
		}
		public Image GetToolImage()
		{
			return Resources.Finalize;
		}
		public void Fire()
		{
			EditorDocument.FinalizeMap();
		}
	}
}
