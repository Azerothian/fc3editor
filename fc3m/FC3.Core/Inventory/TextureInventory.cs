using FC3.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace Nomad.Inventory
{
	public class TextureInventory : Inventory
	{
		private static TextureInventory s_instance = new TextureInventory();
		public override InventoryEntry Root
		{
			get
			{
				return new TextureInventoryEntry(Binding.FCE_Inventory_Texture_GetRoot());
			}
		}
		public static TextureInventory Instance
		{
			get
			{
				return TextureInventory.s_instance;
			}
		}
	}
}
