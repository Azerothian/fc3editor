using System;
using System.Collections.Generic;
using System.Drawing;
namespace FC3Editor.Nomad
{
	internal static class ObjectManager
	{
		public static int ObjectCount
		{
			get
			{
				return Binding.FCE_ObjectManager_GetObjectCount();
			}
		}
		public static EditorObject GetObjectFromScreenPoint(Vec2 pt, out Vec3 hitPos)
		{
			return ObjectManager.GetObjectFromScreenPoint(pt, out hitPos, false, EditorObject.Null);
		}
		public static EditorObject GetObjectFromScreenPoint(Vec2 pt, out Vec3 hitPos, bool includeFrozen)
		{
			return ObjectManager.GetObjectFromScreenPoint(pt, out hitPos, includeFrozen, EditorObject.Null);
		}
		public static EditorObject GetObjectFromScreenPoint(Vec2 pt, out Vec3 hitPos, bool includeFrozen, EditorObject ignore)
		{
			PhysEntityVector vector = PhysEntityVector.Null;
			if (ignore.IsValid)
			{
				vector = PhysEntityVector.Create();
				ignore.GetPhysEntities(vector);
			}
			EditorObject result = new EditorObject(Binding.FCE_ObjectManager_GetObjectFromScreenPoint(pt.X, pt.Y, out hitPos.X, out hitPos.Y, out hitPos.Z, includeFrozen, vector.Pointer));
			if (vector.IsValid)
			{
				vector.Dispose();
			}
			return result;
		}
		public static EditorObject GetObjectFromScreenPoint(Vec2 pt, out Vec3 hitPos, bool includeFrozen, EditorObjectSelection ignore)
		{
			EditorObject result;
			using (PhysEntityVector vector = PhysEntityVector.Create())
			{
				ignore.GetPhysEntities(vector);
				result = new EditorObject(Binding.FCE_ObjectManager_GetObjectFromScreenPoint(pt.X, pt.Y, out hitPos.X, out hitPos.Y, out hitPos.Z, includeFrozen, vector.Pointer));
			}
			return result;
		}
		public static void GetObjectsFromScreenRect(EditorObjectSelection selection, RectangleF rect)
		{
			ObjectManager.GetObjectsFromScreenRect(selection, rect, false);
		}
		public static void GetObjectsFromScreenRect(EditorObjectSelection selection, RectangleF rect, bool includeFrozen)
		{
			Binding.FCE_ObjectManager_GetObjectsFromScreenRect(selection.Pointer, rect.Left, rect.Top, rect.Right, rect.Bottom, includeFrozen);
		}
		public static void GetObjectsFromMagicWand(EditorObjectSelection selection, EditorObject obj)
		{
			Binding.FCE_ObjectManager_GetObjectsFromMagicWand(selection.Pointer, obj.Pointer);
		}
		public static void SetViewportPickingPos(Vec2 pt)
		{
			Binding.FCE_ObjectManager_SetViewportPickingPos(pt.X, pt.Y);
		}
		public static void UnfreezeObjects()
		{
			Binding.FCE_ObjectManager_UnfreezeObjects();
		}
		public static EditorObject GetObject(int index)
		{
			return new EditorObject(Binding.FCE_ObjectManager_GetObject(index));
		}
		public static IEnumerable<EditorObject> GetObjects()
		{
			int objectCount = ObjectManager.ObjectCount;
			for (int i = 0; i < objectCount; i++)
			{
				yield return ObjectManager.GetObject(i);
			}
			yield break;
		}
		public static void OnObjectAddedFromTool(EditorObject obj)
		{
			Binding.FCE_ObjectManager_OnObjectAddedFromTool(obj.Pointer);
		}
	}
}
