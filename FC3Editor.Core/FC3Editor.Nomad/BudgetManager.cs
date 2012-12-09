using System;
namespace FC3Editor.Nomad
{
	internal class BudgetManager
	{
		public static int MemoryUsage
		{
			get
			{
				return Binding.FCE_BudgetManager_GetMemoryUsage();
			}
		}
		public static float ObjectUsage
		{
			get
			{
				return Binding.FCE_BudgetManager_GetObjectUsage();
			}
		}
	}
}
