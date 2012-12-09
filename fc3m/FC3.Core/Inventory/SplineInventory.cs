using FC3.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Nomad.Inventory
{
	public class SplineInventory : Inventory
	{
		private static SplineInventory s_instance = new SplineInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new SplineInventoryEntry(Binding.FCE_Inventory_Spline_GetRoot());
			}
		}
		public static SplineInventory Instance
		{
			get
			{
				return SplineInventory.s_instance;
			}
		}
	}
}
