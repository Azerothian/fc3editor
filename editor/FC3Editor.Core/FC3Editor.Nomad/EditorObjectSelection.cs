using System;
using System.Collections.Generic;
namespace FC3Editor.Nomad
{
	internal struct EditorObjectSelection : IDisposable
	{
		public enum MoveMode
		{
			MoveNormal,
			MoveKeepHeight,
			MoveSnapToTerrain,
			MoveKeepAboveTerrain
		}
		public static EditorObject Null = new EditorObject(IntPtr.Zero);
		private IntPtr m_selPtr;
		public IntPtr Pointer
		{
			get
			{
				return this.m_selPtr;
			}
		}
		public bool IsValid
		{
			get
			{
				return this.Pointer != IntPtr.Zero;
			}
		}
		public int Count
		{
			get
			{
				return Binding.FCE_ObjectSelection_GetCount(this.m_selPtr);
			}
		}
		public EditorObject this[int index]
		{
			get
			{
				return new EditorObject(Binding.FCE_ObjectSelection_Get(this.m_selPtr, index));
			}
		}
		public Vec3 Center
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_ObjectSelection_GetCenter(this.m_selPtr, out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_ObjectSelection_SetCenter(this.m_selPtr, value.X, value.Y, value.Z);
			}
		}
		public AABB WorldBounds
		{
			get
			{
				AABB result = default(AABB);
				Binding.FCE_ObjectSelection_GetWorldBounds(this.m_selPtr, out result.min.X, out result.min.Y, out result.min.Z, out result.max.X, out result.max.Y, out result.max.Z);
				return result;
			}
		}
		public EditorObjectSelection(IntPtr ptr)
		{
			this.m_selPtr = ptr;
		}
		public static EditorObjectSelection Create()
		{
			return new EditorObjectSelection(Binding.FCE_ObjectSelection_Create());
		}
		public void Dispose()
		{
			Binding.FCE_ObjectSelection_Destroy(this.m_selPtr);
			this.m_selPtr = IntPtr.Zero;
		}
		public IEnumerable<EditorObject> GetObjects()
		{
			for(int i = 0; i < this.Count ; i++)
			{
				yield return this[i];
			}
		}
		public void Clear()
		{
			Binding.FCE_ObjectSelection_Clear(this.m_selPtr);
		}
		public void AddObject(EditorObject obj)
		{
			Binding.FCE_ObjectSelection_Add(this.m_selPtr, obj.Pointer);
		}
		public void AddSelection(EditorObjectSelection selection)
		{
			Binding.FCE_ObjectSelection_AddSelection(this.m_selPtr, selection.Pointer);
		}
		public void GetValidObjects(EditorObjectSelection selection)
		{
			Binding.FCE_ObjectSelection_GetValidObjects(this.m_selPtr, selection.Pointer);
		}
		public void RemoveInvalidObjects()
		{
			Binding.FCE_ObjectSelection_RemoveInvalidObjects(this.m_selPtr);
		}
		public void Clone(EditorObjectSelection newSelection, bool cloneObjects)
		{
			Binding.FCE_ObjectSelection_Clone(this.m_selPtr, newSelection.Pointer, cloneObjects);
		}
		public void Delete()
		{
			Binding.FCE_ObjectSelection_Delete(this.m_selPtr);
		}
		public void ToggleObject(EditorObject obj)
		{
			Binding.FCE_ObjectSelection_ToggleObject(this.m_selPtr, obj.Pointer);
		}
		public void ToggleSelection(EditorObjectSelection selection)
		{
			Binding.FCE_ObjectSelection_ToggleSelection(this.m_selPtr, selection.Pointer);
		}
		public void RemoveObject(EditorObject obj)
		{
			Binding.FCE_ObjectSelection_RemoveObject(this.m_selPtr, obj.Pointer);
		}
		public void RemoveSelection(EditorObjectSelection selection)
		{
			Binding.FCE_ObjectSelection_RemoveSelection(this.m_selPtr, selection.Pointer);
		}
		public int IndexOf(EditorObject obj)
		{
			for (int i = 0; i < this.Count; i++)
			{
				if (this[i].Pointer == obj.Pointer)
				{
					return i;
				}
			}
			return -1;
		}
		public bool Contains(EditorObject obj)
		{
			return this.IndexOf(obj) != -1;
		}
		public Vec3 GetComputeCenter()
		{
			Vec3 result = default(Vec3);
			Binding.FCE_ObjectSelection_GetComputeCenter(this.m_selPtr, out result.X, out result.Y, out result.Z);
			return result;
		}
		public void ComputeCenter()
		{
			Binding.FCE_ObjectSelection_ComputeCenter(this.m_selPtr);
		}
		public void MoveTo(Vec3 pos, EditorObjectSelection.MoveMode mode)
		{
			Binding.FCE_ObjectSelection_MoveTo(this.m_selPtr, pos.X, pos.Y, pos.Z, (int)mode);
		}
		public void Rotate(float angle, Vec3 axis, Vec3 pivot, bool affectCenter)
		{
			Binding.FCE_ObjectSelection_Rotate(this.m_selPtr, angle, axis.X, axis.Y, axis.Z, pivot.X, pivot.Y, pivot.Z, affectCenter);
		}
		public void Rotate(Vec3 angles, Vec3 axis, Vec3 pivot, bool affectCenter)
		{
			Binding.FCE_ObjectSelection_Rotate3(this.m_selPtr, angles.X, angles.Y, angles.Z, axis.X, axis.Y, axis.Z, pivot.X, pivot.Y, pivot.Z, affectCenter);
		}
		public void RotateCenter(float angle, Vec3 axis)
		{
			Binding.FCE_ObjectSelection_RotateCenter(this.m_selPtr, angle, axis.X, axis.Y, axis.Z);
		}
		public void RotateLocal(Vec3 angles)
		{
			Binding.FCE_ObjectSelection_RotateLocal3(this.m_selPtr, angles.X, angles.Y, angles.Z);
		}
		public void RotateGimbal(Vec3 angles)
		{
			Binding.FCE_ObjectSelection_RotateGimbal(this.m_selPtr, angles.X, angles.Y, angles.Z);
		}
		public void SetPos(Vec3 pos)
		{
			foreach (EditorObject current in this.GetObjects())
			{
				current.Position = pos;
			}
		}
		public void SetAngles(Vec3 angles)
		{
			foreach (EditorObject current in this.GetObjects())
			{
				current.Angles = angles;
			}
		}
		public void DropToGround(bool physics, bool group)
		{
			Binding.FCE_ObjectSelection_DropToGround(this.m_selPtr, physics, group);
		}
		public void SnapToPivot(EditorObjectPivot source, EditorObjectPivot target, bool preserveOrientation, float snapAngle)
		{
			Binding.FCE_ObjectSelection_SnapToPivot(this.m_selPtr, source.position.X, source.position.Y, source.position.Z, source.normal.X, source.normal.Y, source.normal.Z, source.normalUp.X, source.normalUp.Y, source.normalUp.Z, target.position.X, target.position.Y, target.position.Z, target.normal.X, target.normal.Y, target.normal.Z, target.normalUp.X, target.normalUp.Y, target.normalUp.Z, preserveOrientation, snapAngle);
		}
		public void SnapToClosestObjects()
		{
			Binding.FCE_ObjectSelection_SnapToClosestObjects(this.m_selPtr);
		}
		public void GetPhysEntities(PhysEntityVector vector)
		{
			Binding.FCE_ObjectSelection_GetPhysEntities(this.m_selPtr, vector.Pointer);
		}
		public void ClearState()
		{
			Binding.FCE_ObjectSelection_ClearState(this.m_selPtr);
		}
		public void LoadState()
		{
			Binding.FCE_ObjectSelection_LoadState(this.m_selPtr);
		}
		public void SaveState()
		{
			Binding.FCE_ObjectSelection_SaveState(this.m_selPtr);
		}
		public bool LoadFromXml(string xml, bool managed)
		{
			return Binding.FCE_ObjectSelection_LoadFromXml(this.m_selPtr, xml, managed);
		}
		public string SaveToXml()
		{
			return Binding.FCE_ObjectSelection_SaveToXml(this.m_selPtr);
		}
	}
}
