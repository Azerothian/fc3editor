using System;
namespace FC3Editor.Nomad
{
	internal class EditorEvent
	{
		protected uint m_typeID;
		protected IntPtr m_eventPtr;
		protected EditorEvent(uint typeID, IntPtr eventPtr)
		{
			this.m_typeID = typeID;
			this.m_eventPtr = eventPtr;
		}
	}
}
