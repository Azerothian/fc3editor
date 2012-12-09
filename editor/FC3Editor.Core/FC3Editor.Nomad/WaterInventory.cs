using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	public class WaterInventory : Inventory
	{
		public new class Entry : Inventory.Entry
		{
			public static WaterInventory.Entry Null = new WaterInventory.Entry(IntPtr.Zero);
			public override Image Icon
			{
				get
				{
					if (!base.IsDirectory)
					{
						return Resources.icon_object;
					}
					return Resources.icon_folder;
				}
			}
			public override string IconName
			{
				get
				{
					if (!base.IsDirectory)
					{
						return "icon_object";
					}
					return "icon_folder";
				}
			}
			public override string DisplayName
			{
				get
				{
					return Marshal.PtrToStringUni(Binding.FCE_Inventory_Water_GetDisplay(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override Inventory.Entry Parent
			{
				get
				{
					return new WaterInventory.Entry(Binding.FCE_Inventory_Water_GetParent(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override int Count
			{
				get
				{
					return Binding.FCE_Inventory_Water_GetChildCount(this.m_entryPtr);
				}
			}
			public override Inventory.Entry[] Children
			{
				get
				{
					int count = this.Count;
					Inventory.Entry[] array = new Inventory.Entry[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new WaterInventory.Entry(Binding.FCE_Inventory_Water_GetChild(this.m_entryPtr, i));
					}
					return array;
				}
			}
			public Entry(IntPtr ptr) : base(ptr)
			{
			}
		}
		private static WaterInventory s_instance = new WaterInventory();
		public override Inventory.Entry Root
		{
			get
			{
				return new WaterInventory.Entry(Binding.FCE_Inventory_Water_GetRoot());
			}
		}
		public static WaterInventory Instance
		{
			get
			{
				return WaterInventory.s_instance;
			}
		}
		public WaterInventory.Entry GetFromId(string id)
		{
			return new WaterInventory.Entry(Binding.FCE_Inventory_Water_GetFromId(id));
		}
	}
}
