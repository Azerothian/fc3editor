using System;
using System.Drawing;
using System.Runtime.InteropServices;
using FC3.Properties;
namespace Nomad.Inventory
{
	public class WildernessInventory : Inventory
	{
		
		private static WildernessInventory s_instance = new WildernessInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new WildernessInventoryEntry(Binding.FCE_Inventory_Wilderness_GetRoot());
			}
		}
		public static WildernessInventory Instance
		{
			get
			{
				return WildernessInventory.s_instance;
			}
		}
	}
}
