using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Nomad.Interfaces;
namespace FC3Editor
{
	public class FC3EditorLoader : MarshalByRefObject
	{
		public const string editorDll = "fc3m.dll";
		public const string editorDllSource = "output\\fc3m.dll";
		public const string editorPdb = "fc3m.pdb";
		public const string editorPdbSource = "output\\fc3m.pdb";
		public bool Load(string binDir, bool engineRunning, string initMapPath)
		{
			if (engineRunning)
			{
				try
				{
					File.Copy(binDir + "output\\fc3m.dll", binDir + "fc3m.dll", true);
					File.Copy(binDir + "output\\fc3m.pdb", binDir + "fc3m.pdb", true);
				}
				catch (IOException)
				{
				}
			}
			Assembly assembly = Assembly.LoadFrom(Application.StartupPath + "\\fc3m.dll");
			Type type = assembly.GetType("FC3Editor.FC3EditorLauncher");
			IFC3EditorLauncher iFC3EditorLauncher = (IFC3EditorLauncher)Activator.CreateInstance(type, null, null);
			return iFC3EditorLauncher.Run(engineRunning, initMapPath);
		}
	}
}
