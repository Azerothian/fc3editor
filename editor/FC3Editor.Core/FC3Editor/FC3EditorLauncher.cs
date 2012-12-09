using FC3Editor.Nomad;
using System;
namespace FC3Editor
{
	public class FC3EditorLauncher : IFC3EditorLauncher
	{
		public bool Run(bool engineRunning, string initMapPath)
		{
			Program.Run(engineRunning, initMapPath);
			return Engine.Reloading;
		}
	}
}
