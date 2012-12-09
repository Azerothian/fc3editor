using System;
namespace Nomad.Interfaces
{
	public interface IFC3EditorLauncher
	{
		bool Run(bool engineRunning, string initMapPath);
	}
}
