using System;
namespace FC3Editor
{
	public interface IFC3EditorLauncher
	{
		bool Run(bool engineRunning, string initMapPath);
	}
}
