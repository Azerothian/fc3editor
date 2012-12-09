using System;
namespace Nomad.Manager
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
