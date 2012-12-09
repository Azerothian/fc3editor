using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace FC3Editor
{
	public class FC3EditorLoader : MarshalByRefObject
	{
		public const string editorDll = "FC3Editor.Core.dll";
		public const string editorDllSource = "output\\FC3Editor.Core.dll";
		public const string editorPdb = "FC3Editor.Core.pdb";
		public const string editorPdbSource = "output\\FC3Editor.Core.pdb";
		public bool Load(string binDir, bool engineRunning, string initMapPath)
		{
			if (engineRunning)
			{
				try
				{
					File.Copy(binDir + "output\\FC3Editor.Core.dll", binDir + "FC3Editor.Core.dll", true);
					File.Copy(binDir + "output\\FC3Editor.Core.pdb", binDir + "FC3Editor.Core.pdb", true);
				}
				catch (IOException)
				{
				}
			}
			Assembly assembly = Assembly.LoadFrom(Application.StartupPath + "\\FC3Editor.Core.dll");
			Type type = assembly.GetType("FC3Editor.FC3EditorLauncher");
			IFC3EditorLauncher iFC3EditorLauncher = (IFC3EditorLauncher)Activator.CreateInstance(type, null, null);
			return iFC3EditorLauncher.Run(engineRunning, initMapPath);
		}
	}
}
