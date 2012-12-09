using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	internal class SplineInventory : Inventory
	{
		public new class Entry : Inventory.Entry
		{
			public static SplineInventory.Entry Null = new SplineInventory.Entry(IntPtr.Zero);
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
			public override Inventory.Entry Parent
			{
				get
				{
					return new SplineInventory.Entry(Binding.FCE_Inventory_Spline_GetParent(this.m_entryPtr));
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
			public override Inventory.Entry[] Children
			{
				get
				{
					int count = this.Count;
					Inventory.Entry[] array = new Inventory.Entry[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new SplineInventory.Entry(Binding.FCE_Inventory_Spline_GetChild(this.m_entryPtr, i));
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
			public Entry(IntPtr ptr) : base(ptr)
			{
			}
		}
		private static SplineInventory s_instance = new SplineInventory();
		public override Inventory.Entry Root
		{
			get
			{
				return new SplineInventory.Entry(Binding.FCE_Inventory_Spline_GetRoot());
			}
		}
		public static SplineInventory Instance
		{
			get
			{
				return SplineInventory.s_instance;
			}
		}
	}
}
