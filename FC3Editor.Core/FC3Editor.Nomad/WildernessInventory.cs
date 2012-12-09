using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	internal class WildernessInventory : Inventory
	{
		public new class Entry : Inventory.Entry
		{
			public static WildernessInventory.Entry Null = new WildernessInventory.Entry(IntPtr.Zero);
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
					return Marshal.PtrToStringUni(Binding.FCE_Inventory_Wilderness_GetDisplay(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override Inventory.Entry Parent
			{
				get
				{
					return new WildernessInventory.Entry(Binding.FCE_Inventory_Wilderness_GetParent(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override int Count
			{
				get
				{
					return Binding.FCE_Inventory_Wilderness_GetChildCount(this.m_entryPtr);
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
						array[i] = new WildernessInventory.Entry(Binding.FCE_Inventory_Wilderness_GetChild(this.m_entryPtr, i));
					}
					return array;
				}
			}
			public Entry(IntPtr ptr) : base(ptr)
			{
			}
		}
		private static WildernessInventory s_instance = new WildernessInventory();
		public override Inventory.Entry Root
		{
			get
			{
				return new WildernessInventory.Entry(Binding.FCE_Inventory_Wilderness_GetRoot());
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
