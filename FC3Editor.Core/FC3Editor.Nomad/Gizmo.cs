using System;
namespace FC3Editor.Nomad
{
	internal struct Gizmo : IDisposable
	{
		public static Gizmo Null = new Gizmo(IntPtr.Zero);
		private IntPtr m_gizmoPtr;
		public Vec3 Position
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Gizmo_GetPos(this.m_gizmoPtr, out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_Gizmo_SetPos(this.m_gizmoPtr, value.X, value.Y, value.Z);
			}
		}
		public CoordinateSystem Axis
		{
			get
			{
				CoordinateSystem result = default(CoordinateSystem);
				Binding.FCE_Gizmo_GetAxis(this.m_gizmoPtr, out result.axisX.X, out result.axisX.Y, out result.axisX.Z, out result.axisY.X, out result.axisY.Y, out result.axisY.Z, out result.axisZ.X, out result.axisZ.Y, out result.axisZ.Z);
				return result;
			}
			set
			{
				Binding.FCE_Gizmo_SetAxis(this.m_gizmoPtr, value.axisX.X, value.axisX.Y, value.axisX.Z, value.axisY.X, value.axisY.Y, value.axisY.Z, value.axisZ.X, value.axisZ.Y, value.axisZ.Z);
			}
		}
		public Axis Active
		{
			get
			{
				return (Axis)Binding.FCE_Gizmo_GetActive(this.m_gizmoPtr);
			}
			set
			{
				Binding.FCE_Gizmo_SetActive(this.m_gizmoPtr, (int)value);
			}
		}
		public bool RotationMode
		{
			get
			{
				return Binding.FCE_Gizmo_IsRotationMode(this.m_gizmoPtr);
			}
			set
			{
				Binding.FCE_Gizmo_SetRotationMode(this.m_gizmoPtr, value);
			}
		}
		public bool IsValid
		{
			get
			{
				return this.m_gizmoPtr != IntPtr.Zero;
			}
		}
		public IntPtr Pointer
		{
			get
			{
				return this.m_gizmoPtr;
			}
		}
		public Gizmo(IntPtr ptr)
		{
			this.m_gizmoPtr = ptr;
		}
		public static Gizmo Create()
		{
			return new Gizmo(Binding.FCE_Gizmo_Create());
		}
		public void Dispose()
		{
			Binding.FCE_Gizmo_Destroy(this.m_gizmoPtr);
			this.m_gizmoPtr = IntPtr.Zero;
		}
		public void Redraw()
		{
			Binding.FCE_Gizmo_Redraw(this.m_gizmoPtr);
		}
		public void Hide()
		{
			Binding.FCE_Gizmo_Hide(this.m_gizmoPtr);
		}
		public Axis HitTest(Vec3 raySrc, Vec3 rayDir)
		{
			return (Axis)Binding.FCE_Gizmo_HitTest(this.m_gizmoPtr, raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z);
		}
	}
}
