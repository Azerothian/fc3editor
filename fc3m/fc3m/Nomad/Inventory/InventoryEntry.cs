using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using System.Runtime.InteropServices;
using System.Text;

namespace Nomad.Inventory
{
	public abstract class InventoryEntry
	{
		protected IntPtr m_entryPtr;
		public bool IsDirectory
		{
			get
			{
				return Binding.FCE_Inventory_Entry_IsDirectory(this.Pointer);
			}
		}
		public bool IsObsolete
		{
			get
			{
				return Binding.FCE_Inventory_Entry_IsObsolete(this.Pointer);
			}
		}
		public bool Deleted
		{
			get
			{
				return Binding.FCE_Inventory_Entry_IsDeleted(this.Pointer);
			}
			set
			{
				Binding.FCE_Inventory_Entry_SetDeleted(this.Pointer, value);
			}
		}
		public abstract Image Icon
		{
			get;
		}
		public abstract string IconName
		{
			get;
		}
		public abstract string DisplayName
		{
			get;
			set;
		}
		public abstract InventoryEntry Parent
		{
			get;
			set;
		}
		public abstract int Count
		{
			get;
		}
		public abstract InventoryEntry[] Children
		{
			get;
		}
		public bool IsValid
		{
			get
			{
				return this.m_entryPtr != IntPtr.Zero;
			}
		}
		public IntPtr Pointer
		{
			get
			{
				return this.m_entryPtr;
			}
		}
		public InventoryEntry(IntPtr ptr)
		{
			this.m_entryPtr = ptr;
		}
		public IEnumerable<InventoryEntry> GetRecursiveEntries()
		{
			try
			{
				InventoryEntry[] children = this.Children;
				for (int i = 0; i < children.Length; i++)
				{
					InventoryEntry entry = children[i];
					if (entry.IsDirectory)
					{
						foreach (InventoryEntry current in entry.GetRecursiveEntries())
						{
							yield return current;
						}
					}
					else
					{
						yield return entry;
					}
				}
			}
			finally
			{
			}
			yield break;
		}
		public override bool Equals(object obj)
		{
			InventoryEntry entry = obj as InventoryEntry;
			if (entry == null)
			{
				return base.Equals(obj);
			}
			return this.Pointer == entry.Pointer;
		}
		public static bool operator ==(InventoryEntry x, InventoryEntry y)
		{
			if (object.ReferenceEquals(x, null))
			{
				return object.ReferenceEquals(y, null);
			}
			return x.Equals(y);
		}
		public static bool operator !=(InventoryEntry x, InventoryEntry y)
		{
			return !(x == y);
		}
		public override int GetHashCode()
		{
			return this.Pointer.ToInt32();
		}
		public void ClearChildren()
		{
			Binding.FCE_Inventory_Entry_ClearChildren(this.Pointer);
		}
		public void AddChild(InventoryEntry child)
		{
			Binding.FCE_Inventory_Entry_AddChild(this.Pointer, child.Pointer);
		}
		public void SetChildIndex(InventoryEntry child, int index)
		{
			Binding.FCE_Inventory_Entry_SetChildIndex(this.m_entryPtr, child.Pointer, index);
		}
		public MemoryStream GetThumbnailData()
		{
			IntPtr intPtr;
			int num;
			Binding.FCE_Inventory_Entry_OpenThumbnailData(this.m_entryPtr, out intPtr, out num);
			if (intPtr == IntPtr.Zero)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream(num);
			memoryStream.SetLength((long)num);
			byte[] buffer = memoryStream.GetBuffer();
			Marshal.Copy(intPtr, buffer, 0, num);
			Binding.FCE_Inventory_Entry_CloseThumbnailData(this.m_entryPtr, intPtr);
			return memoryStream;
		}
		public virtual Color? GetBackgroundColor()
		{
			return null;
		}
		public virtual Image GetThumbnailOverlay()
		{
			return null;
		}
		public virtual string GetTextOverlay()
		{
			return null;
		}
	}
}
