using System;
using System.Collections.Generic;
using System.Text;
using Nomad;
using Nomad.Interfaces;

namespace FC3Editor.Core
{
	public class FC3EditorLauncher : IFC3EditorLauncher
	{
		public bool Run(bool engineRunning, string initMapPath)
		{
			Program.Main();
			return Engine.Instance.Reloading;
		}
	}
}
