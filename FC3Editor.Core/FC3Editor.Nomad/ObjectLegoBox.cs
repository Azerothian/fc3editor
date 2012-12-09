using System;
namespace FC3Editor.Nomad
{
	internal class ObjectLegoBox
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
		public static void AddEntry(ObjectInventory.Entry entry)
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
		public static ObjectInventory.Entry GetEntryFromScreenPoint(Vec2 screenPoint)
		{
			return new ObjectInventory.Entry(Binding.FCE_ObjectLegoBox_GetEntryFromScreenPoint(screenPoint.X, screenPoint.Y));
		}
	}
}
