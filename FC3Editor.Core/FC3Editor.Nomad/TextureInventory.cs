using FC3Editor.Properties;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	internal class TextureInventory : Inventory
	{
		public new class Entry : Inventory.Entry
		{
			public static TextureInventory.Entry Null = new TextureInventory.Entry(IntPtr.Zero);
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
			public override Inventory.Entry Parent
			{
				get
				{
					return new TextureInventory.Entry(Binding.FCE_Inventory_Texture_GetParent(this.m_entryPtr));
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
			public override Inventory.Entry[] Children
			{
				get
				{
					int count = this.Count;
					Inventory.Entry[] array = new Inventory.Entry[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new TextureInventory.Entry(Binding.FCE_Inventory_Texture_GetChild(this.m_entryPtr, i));
					}
					return array;
				}
			}
			public Entry(IntPtr ptr) : base(ptr)
			{
			}
		}
		private static TextureInventory s_instance = new TextureInventory();
		public override Inventory.Entry Root
		{
			get
			{
				return new TextureInventory.Entry(Binding.FCE_Inventory_Texture_GetRoot());
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
