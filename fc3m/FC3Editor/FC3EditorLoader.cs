using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
namespace FC3Editor
{
	public class FC3EditorLoader : MarshalByRefObject
	{

		public const string name = "FC3Editor.Core";

		public const string editorDll = name+".dll";
		public const string editorDllSource = "output\\" +name+".dll";
		public const string editorPdb = name + ".pdb";
		public const string editorPdbSource = "output\\" + name + ".pdb";
		public bool Load(string binDir, bool engineRunning, string initMapPath)
		{
			if (engineRunning)
			{
				try
				{
					File.Copy(binDir + "output\\" + name + ".dll", binDir + name + ".dll", true);
					File.Copy(binDir + "output\\" + name + ".pdb", binDir + name + ".pdb", true);
				}
				catch (IOException)
				{
				}
			}
			Assembly assembly = Assembly.LoadFrom(Application.StartupPath + "\\" + name + ".dll");
			Type type = assembly.GetType("FC3Editor.Core.FC3EditorLauncher");
			IFC3EditorLauncher iFC3EditorLauncher = (IFC3EditorLauncher)Activator.CreateInstance(type, null, null);
			return iFC3EditorLauncher.Run(engineRunning, initMapPath);
		}
	}
}
