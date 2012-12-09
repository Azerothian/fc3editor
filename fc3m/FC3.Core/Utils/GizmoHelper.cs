using System;
using Nomad.Components;
using Nomad.Logic;
using Nomad.Maths;
namespace Nomad.Utils
{
	public class GizmoHelper
	{
		private Axis m_axisConstraint;
		private Plane m_virtualPlane;
		private Vec3 m_virtualPlanePos;
		private CoordinateSystem m_virtualPlaneBase;
		private CoordinateSystem m_virtualPlaneCoords;
		public void InitVirtualPlane(Vec3 planePos, CoordinateSystem planeBase, Axis axisConstraint)
		{
			this.m_virtualPlanePos = planePos;
			this.m_virtualPlaneBase = planeBase;
			this.m_axisConstraint = axisConstraint;
			CoordinateSystem virtualPlaneCoords = default(CoordinateSystem);
			switch (this.m_axisConstraint)
			{
			case Axis.X:
				virtualPlaneCoords.axisX = planeBase.axisX;
				virtualPlaneCoords.axisY = Vec3.Cross(Camera.FrontVector, planeBase.axisX);
				virtualPlaneCoords.axisY.Normalize();
				virtualPlaneCoords.axisZ = Vec3.Cross(planeBase.axisX, virtualPlaneCoords.axisY);
				virtualPlaneCoords.axisZ.Normalize();
				break;

			case Axis.Y:
				virtualPlaneCoords.axisX = planeBase.axisY;
				virtualPlaneCoords.axisY = Vec3.Cross(Camera.FrontVector, planeBase.axisY);
				virtualPlaneCoords.axisY.Normalize();
				virtualPlaneCoords.axisZ = Vec3.Cross(planeBase.axisY, virtualPlaneCoords.axisY);
				virtualPlaneCoords.axisZ.Normalize();
				break;

			case Axis.XY:
				virtualPlaneCoords.axisX = planeBase.axisX;
				virtualPlaneCoords.axisY = planeBase.axisY;
				virtualPlaneCoords.axisZ = planeBase.axisZ;
				break;

			case Axis.Z:
				virtualPlaneCoords.axisX = planeBase.axisZ;
				virtualPlaneCoords.axisY = Vec3.Cross(Camera.FrontVector, planeBase.axisZ);
				virtualPlaneCoords.axisY.Normalize();
				virtualPlaneCoords.axisZ = Vec3.Cross(planeBase.axisZ, virtualPlaneCoords.axisY);
				virtualPlaneCoords.axisZ.Normalize();
				break;

			case Axis.XZ:
				virtualPlaneCoords.axisX = planeBase.axisX;
				virtualPlaneCoords.axisY = planeBase.axisZ;
				virtualPlaneCoords.axisZ = planeBase.axisY;
				break;

			case Axis.YZ:
				virtualPlaneCoords.axisX = planeBase.axisY;
				virtualPlaneCoords.axisY = planeBase.axisZ;
				virtualPlaneCoords.axisZ = planeBase.axisX;
				break;
			}
			this.m_virtualPlane = Plane.FromPointNormal(this.m_virtualPlanePos, virtualPlaneCoords.axisZ);
			this.m_virtualPlaneCoords = virtualPlaneCoords;
		}
		public bool GetVirtualPos(out Vec3 pos)
		{
			Vec3 raySrc;
			Vec3 rayDir;
			pos = Vec3.Zero;

			//TODO: Editor GetWorldRayFromScreenPoint Useful? :D
			//Editor.GetWorldRayFromScreenPoint(Editor.Viewport.NormalizedMousePos, out raySrc, out rayDir);
		//	if (!this.m_virtualPlane.RayIntersect(raySrc, rayDir, out pos))
		//	{
		//		return false;
		//	}
			switch (this.m_axisConstraint)
			{
			case Axis.X:
				pos = Vec3.Dot(pos, this.m_virtualPlaneBase.axisX) * this.m_virtualPlaneBase.axisX;
				break;

			case Axis.Y:
				pos = Vec3.Dot(pos, this.m_virtualPlaneBase.axisY) * this.m_virtualPlaneBase.axisY;
				break;

			case Axis.Z:
				pos = Vec3.Dot(pos, this.m_virtualPlaneBase.axisZ) * this.m_virtualPlaneBase.axisZ;
				break;
			}
			return true;
		}
	}
}
