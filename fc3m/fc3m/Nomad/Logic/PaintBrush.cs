using System;
namespace Nomad.Logic
{
	public struct PaintBrush
	{
		private IntPtr m_pointer;
		public bool IsValid
		{
			get
			{
				return this.m_pointer != IntPtr.Zero;
			}
		}
		public IntPtr Pointer
		{
			get
			{
				return this.m_pointer;
			}
		}
		public PaintBrush(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
		public static PaintBrush Create(bool circle, float radius, float hardness, float opacity, float distortion)
		{
			return new PaintBrush(Binding.FCE_Brush_Create(circle, radius, hardness, opacity, distortion));
		}
		public void Destroy()
		{
			Binding.FCE_Brush_Destroy(this.m_pointer);
			this.m_pointer = IntPtr.Zero;
		}
	}
}
