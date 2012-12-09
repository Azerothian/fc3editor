using System;
namespace FC3.Core.Manager
{
	public class BudgetManager
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
