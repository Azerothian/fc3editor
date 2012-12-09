using System;
using FC3.Core.Editor;
namespace FC3.Core.Logic
{
	public class ObjectViewer
	{
		private static bool m_active;
		private static EditorObject m_object;
		public static bool Active
		{
			get
			{
				return ObjectViewer.m_active;
			}
			set
			{
				ObjectViewer.m_active = value;
				Binding.FCE_ObjectViewer_SetActive(ObjectViewer.m_active);
			}
		}
		public static EditorObject Object
		{
			get
			{
				return ObjectViewer.m_object;
			}
			set
			{
				ObjectViewer.m_object = value;
				Binding.FCE_ObjectViewer_SetObject(ObjectViewer.m_object.Pointer);
			}
		}
	}
}
