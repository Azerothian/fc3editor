using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Nomad.Editor;
using Nomad.Maths;
using FC3.Properties;
namespace Nomad.Inventory
{
	public class ObjectInventory : Inventory
	{
		private static ObjectInventory s_instance = new ObjectInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new ObjectInventoryEntry(Binding.FCE_Inventory_Object_GetRoot());
			}
		}
		public static ObjectInventory Instance
		{
			get
			{
				return ObjectInventory.s_instance;
			}
		}
		public void SaveChanges()
		{
			Binding.FCE_Inventory_Object_SaveChanges();
		}
		public ObjectInventoryEntry CreateDirectory(ObjectInventoryEntry parent)
		{
			return new ObjectInventoryEntry(Binding.FCE_Inventory_Object_CreateDirectory(parent.Pointer));
		}
		public ObjectInventoryEntry CreatePrefabObject(ObjectInventoryEntry parent, string id)
		{
			return new ObjectInventoryEntry(Binding.FCE_Inventory_Object_CreatePrefabObject(parent.Pointer, id));
		}
		protected override InventoryEntry CreateFilterDirectory()
		{
			return new ObjectInventoryEntry(Binding.FCE_Inventory_Object_CreateFilterDirectory());
		}
		protected override void DestroyFilterDirectory(InventoryEntry entry)
		{
			Binding.FCE_Inventory_Object_DestroyFilterDirectory(entry.Pointer);
		}
		public override void SearchInventory(string criteria, InventoryEntry resultEntry)
		{
			Binding.FCE_Inventory_Object_SearchInventoryEntry(this.Root.Pointer, criteria, resultEntry.Pointer);
		}
	}
}
