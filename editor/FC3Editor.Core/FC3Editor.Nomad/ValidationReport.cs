using System;
namespace FC3Editor.Nomad
{
	internal struct ValidationReport
	{
		public static ValidationReport Null = new ValidationReport(IntPtr.Zero);
		private IntPtr m_pointer;
		public int Count
		{
			get
			{
				return Binding.FCE_ValidationReport_GetCount(this.m_pointer);
			}
		}
		public ValidationRecord this[int index]
		{
			get
			{
				return new ValidationRecord(Binding.FCE_ValidationReport_GetRecord(this.m_pointer, index));
			}
		}
		public bool IsValid
		{
			get
			{
				return this.m_pointer != IntPtr.Zero;
			}
		}
		public ValidationReport(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
		public void Destroy()
		{
			Binding.FCE_ValidationReport_Destroy(this.m_pointer);
			this.m_pointer = IntPtr.Zero;
		}
	}
}
