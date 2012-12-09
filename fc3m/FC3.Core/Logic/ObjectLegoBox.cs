using System;
using Nomad.Inventory;
using Nomad.Maths;
namespace Nomad.Logic
{
	public class ObjectLegoBox
	{
		private static bool m_active;
		public static bool Active
		{
			get
			{
				return ObjectLegoBox.m_active;
			}
			set
			{
				ObjectLegoBox.m_active = value;
				Binding.FCE_ObjectLegoBox_SetActive(ObjectLegoBox.m_active);
			}
		}
		public static void AddEntry(ObjectInventoryEntry entry)
		{
			Binding.FCE_ObjectLegoBox_AddEntry(entry.Pointer);
		}
		public static void ClearEntries()
		{
			Binding.FCE_ObjectLegoBox_ClearEntries();
		}
		public static void CreateLegoBox()
		{
			Binding.FCE_ObjectLegoBox_CreateLegoBox();
		}
		public static ObjectInventoryEntry GetEntryFromScreenPoint(Vec2 screenPoint)
		{
			return new ObjectInventoryEntry(Binding.FCE_ObjectLegoBox_GetEntryFromScreenPoint(screenPoint.X, screenPoint.Y));
		}
	}
}
