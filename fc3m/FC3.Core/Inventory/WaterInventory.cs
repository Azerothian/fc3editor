using FC3.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Nomad.Inventory
{
	public class WaterInventory : Inventory
	{
		private static WaterInventory s_instance = new WaterInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new WaterInventoryEntry(Binding.FCE_Inventory_Water_GetRoot());
			}
		}
		public static WaterInventory Instance
		{
			get
			{
				return WaterInventory.s_instance;
			}
		}
		public WaterInventoryEntry GetFromId(string id)
		{
			return new WaterInventoryEntry(Binding.FCE_Inventory_Water_GetFromId(id));
		}
	}
}
