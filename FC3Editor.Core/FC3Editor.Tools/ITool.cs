using FC3Editor.Parameters;
using System;
namespace FC3Editor.Tools
{
	internal interface ITool : IToolBase, IParameterProvider
	{
		void Activate();
		void Deactivate();
		string GetContextHelp();
	}
}
