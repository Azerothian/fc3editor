using System;
using System.Collections.Generic;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Text;

namespace Nomad.Inventory
{
	public class WaterInventoryEntry : InventoryEntry
	{
		public static WaterInventoryEntry Null = new WaterInventoryEntry(IntPtr.Zero);
		public override Image Icon
		{
			get
			{
				//if (!base.IsDirectory)
				//{
				//	return Resources.icon_object;
				//}
				//return Resources.icon_folder;
				return null;
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
		public override InventoryEntry Parent
		{
			get
			{
				return new WaterInventoryEntry(Binding.FCE_Inventory_Water_GetParent(this.m_entryPtr));
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
		public override InventoryEntry[] Children
		{
			get
			{
				int count = this.Count;
				InventoryEntry[] array = new InventoryEntry[count];
				for (int i = 0; i < count; i++)
				{
					array[i] = new WaterInventoryEntry(Binding.FCE_Inventory_Water_GetChild(this.m_entryPtr, i));
				}
				return array;
			}
		}
		public WaterInventoryEntry(IntPtr ptr)
			: base(ptr)
		{
		}
	}
}
