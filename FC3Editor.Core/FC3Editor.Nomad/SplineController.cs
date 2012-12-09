using System;
using System.Drawing;
namespace FC3Editor.Nomad
{
	internal struct SplineController
	{
		public enum SelectMode
		{
			Replace,
			Add,
			Toggle
		}
		public static SplineController Null = new SplineController(IntPtr.Zero);
		private IntPtr m_controllerPtr;
		public IntPtr Pointer
		{
			get
			{
				return this.m_controllerPtr;
			}
		}
		public SplineController(IntPtr ptr)
		{
			this.m_controllerPtr = ptr;
		}
		public static SplineController Create()
		{
			return new SplineController(Binding.FCE_SplineController_Create());
		}
		public void Dispose()
		{
			Binding.FCE_SplineController_Destroy(this.m_controllerPtr);
		}
		public void SetSpline(Spline spline)
		{
			Binding.FCE_SplineController_SetSpline(this.m_controllerPtr, spline.Pointer);
		}
		public void ClearSelection()
		{
			Binding.FCE_SplineController_ClearSelection(this.m_controllerPtr);
		}
		public bool IsSelected(int index)
		{
			return Binding.FCE_SplineController_IsSelected(this.m_controllerPtr, index);
		}
		public void SetSelected(int index, bool selected)
		{
			Binding.FCE_SplineController_SetSelected(this.m_controllerPtr, index, selected);
		}
		public void SelectFromScreenRect(RectangleF rect, float penWidth, SplineController.SelectMode selectMode)
		{
			Binding.FCE_SplineController_SelectFromScreenRect(this.m_controllerPtr, rect.X, rect.Y, rect.Right, rect.Bottom, penWidth, (int)selectMode);
		}
		public void MoveSelection(Vec2 delta)
		{
			Binding.FCE_SplineController_MoveSelection(this.m_controllerPtr, delta.X, delta.Y);
		}
		public void DeleteSelection()
		{
			Binding.FCE_SplineController_DeleteSelection(this.m_controllerPtr);
		}
	}
}
