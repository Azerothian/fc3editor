using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	public abstract class Inventory : IDisposable
	{
		public abstract class Entry
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
			public abstract Inventory.Entry Parent
			{
				get;
				set;
			}
			public abstract int Count
			{
				get;
			}
			public abstract Inventory.Entry[] Children
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
			public Entry(IntPtr ptr)
			{
				this.m_entryPtr = ptr;
			}
			public IEnumerable<Inventory.Entry> GetRecursiveEntries()
			{
				try
				{
					Inventory.Entry[] children = this.Children;
					for (int i = 0; i < children.Length; i++)
					{
						Inventory.Entry entry = children[i];
						if (entry.IsDirectory)
						{
							foreach (Inventory.Entry current in entry.GetRecursiveEntries())
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
				Inventory.Entry entry = obj as Inventory.Entry;
				if (entry == null)
				{
					return base.Equals(obj);
				}
				return this.Pointer == entry.Pointer;
			}
			public static bool operator ==(Inventory.Entry x, Inventory.Entry y)
			{
				if (object.ReferenceEquals(x, null))
				{
					return object.ReferenceEquals(y, null);
				}
				return x.Equals(y);
			}
			public static bool operator !=(Inventory.Entry x, Inventory.Entry y)
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
			public void AddChild(Inventory.Entry child)
			{
				Binding.FCE_Inventory_Entry_AddChild(this.Pointer, child.Pointer);
			}
			public void SetChildIndex(Inventory.Entry child, int index)
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
		private List<Inventory.Entry> m_ownedDirectories = new List<Inventory.Entry>();
		public abstract Inventory.Entry Root
		{
			get;
		}
		public void Dispose()
		{
			foreach (Inventory.Entry current in this.m_ownedDirectories)
			{
				this.DestroyFilterDirectory(current);
			}
		}
		public Inventory.Entry CreateDirectory()
		{
			Inventory.Entry entry = this.CreateFilterDirectory();
			if (entry != null)
			{
				this.m_ownedDirectories.Add(entry);
			}
			return entry;
		}
		protected virtual Inventory.Entry CreateFilterDirectory()
		{
			return null;
		}
		protected virtual void DestroyFilterDirectory(Inventory.Entry entry)
		{
		}
		public virtual void SearchInventory(string criteria, Inventory.Entry resultEntry)
		{
		}
	}
}
