using FC3.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Nomad.Inventory
{
	public class CollectionInventory : Inventory
	{
		private static CollectionInventory s_instance = new CollectionInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new CollectionInventoryEntry(Binding.FCE_Inventory_Collection_GetRoot());
			}
		}
		public static CollectionInventory Instance
		{
			get
			{
				return CollectionInventory.s_instance;
			}
		}
	}
}
