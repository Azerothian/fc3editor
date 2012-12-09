using System;
namespace FC3Editor.Nomad
{
	internal class Spline : IDisposable
	{
		public static Spline Null = new Spline(IntPtr.Zero);
		protected IntPtr m_splinePtr;
		public int Count
		{
			get
			{
				return Binding.FCE_Spline_GetNumPoints(this.m_splinePtr);
			}
		}
		public Vec2 this[int index]
		{
			get
			{
				Vec2 result = default(Vec2);
				Binding.FCE_Spline_GetPoint(this.m_splinePtr, index, out result.X, out result.Y);
				return result;
			}
			set
			{
				Binding.FCE_Spline_SetPoint(this.m_splinePtr, index, value.X, value.Y);
			}
		}
		public bool IsValid
		{
			get
			{
				return this.Pointer != IntPtr.Zero;
			}
		}
		public IntPtr Pointer
		{
			get
			{
				return this.m_splinePtr;
			}
		}
		public Spline(IntPtr ptr)
		{
			this.m_splinePtr = ptr;
		}
		public static Spline Create()
		{
			return new Spline(Binding.FCE_Spline_Create());
		}
		public void Dispose()
		{
			Binding.FCE_Spline_Destroy(this.m_splinePtr);
		}
		public void Clear()
		{
			Binding.FCE_Spline_Clear(this.m_splinePtr);
		}
		public void AddPoint(Vec2 point)
		{
			Binding.FCE_Spline_AddPoint(this.m_splinePtr, point.X, point.Y);
		}
		public void InsertPoint(Vec2 point, int index)
		{
			Binding.FCE_Spline_InsertPoint(this.m_splinePtr, point.X, point.Y, index);
		}
		public void RemovePoint(int index)
		{
			Binding.FCE_Spline_RemovePoint(this.m_splinePtr, index);
		}
		public bool RemoveSimilarPoints()
		{
			return Binding.FCE_Spline_RemoveSimilarPoints(this.m_splinePtr);
		}
		public bool OptimizePoint(int index)
		{
			return Binding.FCE_Spline_OptimizePoint(this.m_splinePtr, index);
		}
		public void UpdateSpline()
		{
			Binding.FCE_Spline_UpdateSpline(this.m_splinePtr);
		}
		public void UpdateSplineHeight()
		{
			Binding.FCE_Spline_UpdateSplineHeight(this.m_splinePtr);
		}
		public void FinalizeSpline()
		{
			Binding.FCE_Spline_FinalizeSpline(this.m_splinePtr);
		}
		public void Draw(float penWidth, SplineController controller)
		{
			Binding.FCE_Spline_Draw(this.m_splinePtr, penWidth, controller.Pointer);
		}
		public bool HitTestPoints(Vec2 point, float penWidth, float hitWidth, out int hitIndex, out Vec2 hitPos)
		{
			return Binding.FCE_Spline_HitTestPoints(this.m_splinePtr, point.X, point.Y, penWidth, hitWidth, out hitIndex, out hitPos.X, out hitPos.Y);
		}
		public bool HitTestSegments(Vec2 center, float radius, out int hitIndex, out Vec2 hitPos)
		{
			return Binding.FCE_Spline_HitTestSegments(this.m_splinePtr, center.X, center.Y, radius, out hitIndex, out hitPos.X, out hitPos.Y);
		}
	}
}
