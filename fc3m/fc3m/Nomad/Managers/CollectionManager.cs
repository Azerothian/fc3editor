using System;
using System.Drawing;
using Nomad.Inventory;
namespace Nomad.Manager
{
	internal static class CollectionManager
	{
		public static int EmptyCollectionId = 8;
		public static CollectionInventoryEntry GetCollectionEntryFromId(int id)
		{
			return new CollectionInventoryEntry(Binding.FCE_CollectionManager_GetCollectionEntryFromId(id));
		}
		public static void AssignCollectionId(int id, CollectionInventoryEntry entry)
		{
			Binding.FCE_CollectionManager_AssignCollectionId(id, entry.Pointer);
		}
		public static void WriteMaskCircle(float x, float y, float radius, int id, bool update)
		{
			Binding.FCE_CollectionManager_WriteMaskCircle(x, y, radius, id, update);
		}
		public static void WriteMaskSquare(float x, float y, float radius, int id, bool update)
		{
			Binding.FCE_CollectionManager_WriteMaskSquare(x, y, radius, id, update);
		}
		public static void ClearMaskId(int id)
		{
			Binding.FCE_CollectionManager_ClearMaskId(id);
		}
		public static void UpdateCollections(Rectangle rect)
		{
			Binding.FCE_CollectionManager_UpdateCollections(rect.Left, rect.Top, rect.Width, rect.Height);
		}
	}
}
