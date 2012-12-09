using System;
using Nomad.Maths;
namespace Nomad.Components
{
	public class SplineZone : Spline
	{
		public new static SplineZone Null = new SplineZone(IntPtr.Zero);
		public SplineZone(IntPtr ptr) : base(ptr)
		{
		}
		public void Reset()
		{
			Binding.FCE_SplineZone_Reset(this.m_splinePtr);
		}
	}
}
