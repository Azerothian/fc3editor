using System;
using System.Collections.Generic;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Text;
using FC3.Properties;

namespace Nomad.Inventory
{

	public class SplineInventoryEntry : InventoryEntry
	{
		public static SplineInventoryEntry Null = new SplineInventoryEntry(IntPtr.Zero);
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
				return Marshal.PtrToStringUni(Binding.FCE_Inventory_Spline_GetDisplay(this.m_entryPtr));
			}
			set
			{
			}
		}
		public override InventoryEntry Parent
		{
			get
			{
				return new SplineInventoryEntry(Binding.FCE_Inventory_Spline_GetParent(this.m_entryPtr));
			}
			set
			{
			}
		}
		public override int Count
		{
			get
			{
				return Binding.FCE_Inventory_Spline_GetChildCount(this.m_entryPtr);
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
					array[i] = new SplineInventoryEntry(Binding.FCE_Inventory_Spline_GetChild(this.m_entryPtr, i));
				}
				return array;
			}
		}
		public float DefaultWidth
		{
			get
			{
				return Binding.FCE_Inventory_Spline_GetDefaultWidth(this.m_entryPtr);
			}
		}
		public SplineInventoryEntry(IntPtr ptr)
			: base(ptr)
		{
		}
	}

}
