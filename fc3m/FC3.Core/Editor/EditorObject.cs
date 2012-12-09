using System;
using Nomad.Inventory;
using Nomad.Logic;
using Nomad.Maths;
namespace Nomad.Editor
{
	public class EditorObject
	{
		public static EditorObject Null = new EditorObject(IntPtr.Zero);
		private IntPtr m_objPtr;
		public bool IsValid
		{
			get
			{
				return this.Pointer != IntPtr.Zero;
			}
		}
		public IntPtr Pointer
		{
			get
			{
				return this.m_objPtr;
			}
		}
		public bool IsLoaded
		{
			get
			{
				return Binding.FCE_Object_IsLoaded(this.m_objPtr);
			}
		}
		public ObjectInventoryEntry Entry
		{
			get
			{
				return new ObjectInventoryEntry(Binding.FCE_Object_GetEntry(this.m_objPtr));
			}
		}
		public Vec3 Position
		{
			get
			{
				Vec3 result;
				Binding.FCE_Object_GetPos(this.m_objPtr, out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_Object_SetPos(this.m_objPtr, value.X, value.Y, value.Z);
			}
		}
		public Vec3 Angles
		{
			get
			{
				Vec3 result;
				Binding.FCE_Object_GetAngles(this.m_objPtr, out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_Object_SetAngles(this.m_objPtr, value.X, value.Y, value.Z);
			}
		}
		public CoordinateSystem Axis
		{
			get
			{
				return CoordinateSystem.FromAngles(this.Angles);
			}
		}
		public AABB LocalBounds
		{
			get
			{
				AABB result;
				Binding.FCE_Object_GetBounds(this.m_objPtr, false, out result.min.X, out result.min.Y, out result.min.Z, out result.max.X, out result.max.Y, out result.max.Z);
				return result;
			}
		}
		public AABB WorldBounds
		{
			get
			{
				AABB result;
				Binding.FCE_Object_GetBounds(this.m_objPtr, true, out result.min.X, out result.min.Y, out result.min.Z, out result.max.X, out result.max.Y, out result.max.Z);
				return result;
			}
		}
		public bool Visible
		{
			get
			{
				return Binding.FCE_Object_IsVisible(this.m_objPtr);
			}
			set
			{
				Binding.FCE_Object_SetVisible(this.m_objPtr, value);
			}
		}
		public bool HighlightState
		{
			set
			{
				Binding.FCE_Object_SetHighlight(this.m_objPtr, value);
			}
		}
		public bool Frozen
		{
			set
			{
				Binding.FCE_Object_SetFreeze(this.m_objPtr, value);
			}
		}
		public EditorObject(IntPtr objPtr)
		{
			this.m_objPtr = objPtr;
		}
		public static EditorObject CreateFromEntry(ObjectInventoryEntry entry, bool managed)
		{
			return new EditorObject(Binding.FCE_Object_Create_FromEntry(entry.Pointer, managed));
		}
		public void Acquire()
		{
			Binding.FCE_Object_AddRef(this.m_objPtr);
		}
		public void Release()
		{
			Binding.FCE_Object_Release(this.m_objPtr);
		}
		public void Destroy()
		{
			Binding.FCE_Object_Destroy(this.m_objPtr);
			this.m_objPtr = IntPtr.Zero;
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
		public static bool operator ==(EditorObject x, EditorObject y)
		{
			if (object.ReferenceEquals(x, null))
			{
				return object.ReferenceEquals(y, null);
			}
			return x.Equals(y);
		}
		public static bool operator !=(EditorObject x, EditorObject y)
		{
			return !(x == y);
		}
		public override int GetHashCode()
		{
			return this.Pointer.ToInt32();
		}
		public EditorObject Clone()
		{
			return new EditorObject(Binding.FCE_Object_Clone(this.m_objPtr));
		}
		public Vec3 GetPivotPoint(Pivot pivot)
		{
			AABB bounds;
			if (this.IsLoaded)
			{
				bounds = this.LocalBounds;
			}
			else
			{
				bounds = default(AABB);
			}
			return this.Axis.GetPivotPoint(this.Position, bounds, pivot);
		}
		public void DropToGround(bool physics)
		{
			Binding.FCE_Object_DropToGround(this.m_objPtr, physics);
		}
		public void ComputeAutoOrientation(ref Vec3 pos, out Vec3 angles, Vec3 normal)
		{
			angles = default(Vec3);
			Binding.FCE_Object_ComputeAutoOrientation(this.m_objPtr, ref pos.X, ref pos.Y, ref pos.Z, out angles.X, out angles.Y, out angles.Z, normal.X, normal.Y, normal.Z);
		}
		public bool GetPivot(int idx, out EditorObjectPivot pivot)
		{
			pivot = new EditorObjectPivot();
			return Binding.FCE_Object_GetPivot(this.m_objPtr, idx, out pivot.position.X, out pivot.position.Y, out pivot.position.Z, out pivot.normal.X, out pivot.normal.Y, out pivot.normal.Z, out pivot.normalUp.X, out pivot.normalUp.Y, out pivot.normalUp.Z);
		}
		public bool GetClosestPivot(Vec3 pos, out EditorObjectPivot pivot)
		{
			return this.GetClosestPivot(pos, out pivot, 3.40282347E+38f);
		}
		public bool GetClosestPivot(Vec3 pos, out EditorObjectPivot pivot, float minDist)
		{
			pivot = new EditorObjectPivot();
			return Binding.FCE_Object_GetClosestPivot(this.m_objPtr, pos.X, pos.Y, pos.Z, out pivot.position.X, out pivot.position.Y, out pivot.position.Z, out pivot.normal.X, out pivot.normal.Y, out pivot.normal.Z, out pivot.normalUp.X, out pivot.normalUp.Y, out pivot.normalUp.Z, minDist);
		}
		public void SnapToClosestObject()
		{
			Binding.FCE_Object_SnapToClosestObject(this.m_objPtr);
		}
		public void GetPhysEntities(PhysEntityVector vector)
		{
			Binding.FCE_Object_GetPhysEntities(this.m_objPtr, vector.Pointer);
		}
	}
}
