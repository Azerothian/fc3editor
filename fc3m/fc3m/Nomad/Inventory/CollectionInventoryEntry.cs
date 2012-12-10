using System;
using System.Collections.Generic;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Text;
using FC3.Properties;

namespace Nomad.Inventory
{
	public class CollectionInventoryEntry : InventoryEntry
		{
			public static CollectionInventoryEntry Null = new CollectionInventoryEntry(IntPtr.Zero);
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
			public override InventoryEntry Parent
			{
				get
				{
					return new CollectionInventoryEntry(Binding.FCE_Inventory_Collection_GetParent(this.m_entryPtr));
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
			public override InventoryEntry[] Children
			{
				get
				{
					int count = this.Count;
					InventoryEntry[] array = new InventoryEntry[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new CollectionInventoryEntry(Binding.FCE_Inventory_Collection_GetChild(this.m_entryPtr, i));
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
			public CollectionInventoryEntry(IntPtr ptr)
				: base(ptr)
			{
			}
		}
}
