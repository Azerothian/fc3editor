using System;
using System.Collections.Generic;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Text;
using Nomad.Editor;
using Nomad.Maths;
using FC3.Properties;

namespace Nomad.Inventory
{
	public class ObjectInventoryEntry : InventoryEntry
	{
		public enum SourceTypes
		{
			Archetype,
			InlinePrefab
		}
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
		public uint Id
		{
			get
			{
				return Binding.FCE_Inventory_Object_GetId(this.m_entryPtr);
			}
		}
		public string IdString
		{
			get
			{
				return Marshal.PtrToStringAnsi(Binding.FCE_Inventory_Object_GetIdString(this.m_entryPtr));
			}
		}
		public override string DisplayName
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Inventory_Object_GetDisplay(this.m_entryPtr));
			}
			set
			{
				Binding.FCE_Inventory_Object_SetDisplay(this.m_entryPtr, value);
			}
		}
		public override InventoryEntry Parent
		{
			get
			{
				return new ObjectInventoryEntry(Binding.FCE_Inventory_Object_GetParent(this.m_entryPtr));
			}
			set
			{
				Binding.FCE_Inventory_Object_SetParent(this.m_entryPtr, value.Pointer);
			}
		}
		public override int Count
		{
			get
			{
				return Binding.FCE_Inventory_Object_GetChildCount(this.m_entryPtr);
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
					array[i] = new ObjectInventoryEntry(Binding.FCE_Inventory_Object_GetChild(this.m_entryPtr, i));
				}
				return array;
			}
		}
		public string Tags
		{
			get
			{
				return Marshal.PtrToStringAnsi(Binding.FCE_Inventory_Object_GetTags(this.m_entryPtr));
			}
			set
			{
				Binding.FCE_Inventory_Object_SetTags(this.m_entryPtr, value);
			}
		}
		public ObjectInventoryEntry.SourceTypes SourceType
		{
			get
			{
				return (ObjectInventoryEntry.SourceTypes)Binding.FCE_Inventory_Object_GetSourceType(this.m_entryPtr);
			}
		}
		public Vec3 BMin
		{
			get
			{
				float x;
				float y;
				float z;
				Binding.FCE_Inventory_Object_GetBMin(this.m_entryPtr, out x, out y, out z);
				return new Vec3(x, y, z);
			}
		}
		public Vec3 BMax
		{
			get
			{
				float x;
				float y;
				float z;
				Binding.FCE_Inventory_Object_GetBMax(this.m_entryPtr, out x, out y, out z);
				return new Vec3(x, y, z);
			}
		}
		public Vec3 Size
		{
			get
			{
				float x;
				float y;
				float z;
				Binding.FCE_Inventory_Object_GetSize(this.m_entryPtr, out x, out y, out z);
				return new Vec3(x, y, z);
			}
		}
		public bool AutoOrientation
		{
			get
			{
				return Binding.FCE_Inventory_Object_IsAutoOrientation(this.m_entryPtr);
			}
		}
		public float ZOffset
		{
			get
			{
				return Binding.FCE_Inventory_Object_GetZOffset(this.m_entryPtr);
			}
			set
			{
				Binding.FCE_Inventory_Object_SetZOffset(this.m_entryPtr, value);
			}
		}
		public bool IsAI
		{
			get
			{
				return Binding.FCE_Inventory_Object_IsAI(this.m_entryPtr);
			}
		}
		public bool AutoPivot
		{
			get
			{
				return Binding.FCE_Inventory_Object_IsAutoPivot(this.m_entryPtr);
			}
			set
			{
				Binding.FCE_Inventory_Object_SetAutoPivot(this.m_entryPtr, value);
			}
		}
		public int PivotCount
		{
			get
			{
				return Binding.FCE_Inventory_Object_GetPivotCount(this.m_entryPtr);
			}
		}
		public ObjectInventoryEntry(IntPtr ptr)
			: base(ptr)
		{
		}
		public void ClearPivots()
		{
			Binding.FCE_Inventory_Object_ClearPivots(this.m_entryPtr);
		}
		public void AddPivot(EditorObjectPivot pivot)
		{
			Binding.FCE_Inventory_Object_AddPivot(this.m_entryPtr, pivot.position.X, pivot.position.Y, pivot.position.Z, pivot.normal.X, pivot.normal.Y, pivot.normal.Z, pivot.normalUp.X, pivot.normalUp.Y, pivot.normalUp.Z);
		}
		public void SetPivot(int idx, EditorObjectPivot pivot)
		{
			Binding.FCE_Inventory_Object_SetPivot(this.m_entryPtr, idx, pivot.position.X, pivot.position.Y, pivot.position.Z, pivot.normal.X, pivot.normal.Y, pivot.normal.Z, pivot.normalUp.X, pivot.normalUp.Y, pivot.normalUp.Z);
		}
		public void SetPivots(float minX, float maxX, float minY, float maxY)
		{
			Binding.FCE_Inventory_Object_SetPivots(this.m_entryPtr, minX, maxX, minY, maxY);
		}
		public override Color? GetBackgroundColor()
		{
			return base.GetBackgroundColor();
		}
		public override Image GetThumbnailOverlay()
		{
			return base.GetThumbnailOverlay();
		}
		public override string GetTextOverlay()
		{
			if (base.IsDirectory)
			{
				return null;
			}
			Vec3 size = this.Size;
			float num = Math.Max(Math.Max(size.X, size.Y), size.Z);
			if (num < 1f)
			{
				return ((int)Math.Round((double)(num * 100f))).ToString() + "cm";
			}
			return num.ToString("F1") + "m";
		}
	}
}

