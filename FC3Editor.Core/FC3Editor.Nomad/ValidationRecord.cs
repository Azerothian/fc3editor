using System;
using System.Runtime.InteropServices;
namespace FC3Editor.Nomad
{
	internal class ValidationRecord
	{
		public enum Severities
		{
			Error = 1,
			Warning,
			Comment = 4,
			Success = 8,
			All = 15,
			NoSuccess = 7
		}
		public enum Flags
		{
			None,
			Validation = 32
		}
		private IntPtr m_pointer;
		public ValidationRecord.Severities Severity
		{
			get
			{
				return (ValidationRecord.Severities)Binding.FCE_ValidationRecord_GetSeverity(this.m_pointer);
			}
		}
		public ValidationRecord.Flags Flag
		{
			get
			{
				return (ValidationRecord.Flags)Binding.FCE_ValidationRecord_GetFlags(this.m_pointer);
			}
		}
		public string Message
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_ValidationRecord_GetMessage(this.m_pointer));
			}
		}
		public EditorObject Object
		{
			get
			{
				return new EditorObject(Binding.FCE_ValidationRecord_GetObject(this.m_pointer));
			}
		}
		public ValidationRecord(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
	}
}
