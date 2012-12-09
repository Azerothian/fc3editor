using System;
using System.Drawing;
namespace FC3Editor.Tools
{
	internal interface IToolBase
	{
		string GetToolName();
		Image GetToolImage();
	}
}
