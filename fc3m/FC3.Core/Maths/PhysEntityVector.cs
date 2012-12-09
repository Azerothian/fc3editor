using System;
namespace Nomad.Maths
{
	public struct PhysEntityVector : IDisposable
	{
		public static PhysEntityVector Null = default(PhysEntityVector);
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
		public static PhysEntityVector Create()
		{
			return new PhysEntityVector(Binding.FCE_PhysEntityVector_Create());
		}
		public PhysEntityVector(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
		public void Dispose()
		{
			Binding.FCE_PhysEntityVector_Destroy(this.m_pointer);
			this.m_pointer = IntPtr.Zero;
		}
	}
}
