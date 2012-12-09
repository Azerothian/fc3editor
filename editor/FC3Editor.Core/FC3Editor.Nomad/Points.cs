using System;
namespace FC3Editor.Nomad
{
	internal struct Points
	{
		public static Points Null = new Points(IntPtr.Zero);
		private IntPtr m_pointsPtr;
		public IntPtr Pointer
		{
			get
			{
				return this.m_pointsPtr;
			}
		}
		public Points(IntPtr pointsPtr)
		{
			this.m_pointsPtr = pointsPtr;
		}
		public static Points Create()
		{
			return new Points(Binding.FCE_Core_Points_Create());
		}
		public void Destroy()
		{
			Binding.FCE_Core_Points_Destroy(this.m_pointsPtr);
			this.m_pointsPtr = IntPtr.Zero;
		}
	}
}
