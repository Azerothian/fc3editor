using System;
using System.Collections.Generic;
namespace FC3Editor.Parameters
{
	internal interface IParameterProvider
	{
		IEnumerable<IParameter> GetParameters();
		IParameter GetMainParameter();
	}
}
