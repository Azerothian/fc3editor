using System;
using System.Collections.Generic;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Text;
using FC3.Properties;

namespace Nomad.Inventory
{
	public class TextureInventoryEntry : InventoryEntry
		{
			public static TextureInventoryEntry Null = new TextureInventoryEntry(IntPtr.Zero);
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
					return Marshal.PtrToStringUni(Binding.FCE_Inventory_Texture_GetDisplay(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override InventoryEntry Parent
			{
				get
				{
					return new TextureInventoryEntry(Binding.FCE_Inventory_Texture_GetParent(this.m_entryPtr));
				}
				set
				{
				}
			}
			public override int Count
			{
				get
				{
					return Binding.FCE_Inventory_Texture_GetChildCount(this.m_entryPtr);
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
						array[i] = new TextureInventoryEntry(Binding.FCE_Inventory_Texture_GetChild(this.m_entryPtr, i));
					}
					return array;
				}
			}
			public TextureInventoryEntry(IntPtr ptr)
				: base(ptr)
			{
			}
		}
}
