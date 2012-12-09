using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	internal class CollectionInventory : Inventory
	{
		public new class Entry : Inventory.Entry
		{
			public static CollectionInventory.Entry Null = new CollectionInventory.Entry(IntPtr.Zero);
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
					return Marshal.PtrToStringUni(Binding.FCE_Inventory_Collection_GetDisplay(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override Inventory.Entry Parent
			{
				get
				{
					return new CollectionInventory.Entry(Binding.FCE_Inventory_Collection_GetParent(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override int Count
			{
				get
				{
					return Binding.FCE_Inventory_Collection_GetChildCount(this.m_entryPtr);
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
						array[i] = new CollectionInventory.Entry(Binding.FCE_Inventory_Collection_GetChild(this.m_entryPtr, i));
					}
					return array;
				}
			}
			public bool HasBurnProfile
			{
				get
				{
					return Binding.FCE_Inventory_Collection_GetBurnProfile(this.m_entryPtr) != 2416722677u;
				}
			}
			public Entry(IntPtr ptr) : base(ptr)
			{
			}
		}
		private static CollectionInventory s_instance = new CollectionInventory();
		public override Inventory.Entry Root
		{
			get
			{
				return new CollectionInventory.Entry(Binding.FCE_Inventory_Collection_GetRoot());
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
