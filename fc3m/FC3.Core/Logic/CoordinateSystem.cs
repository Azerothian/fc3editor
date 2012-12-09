using System;
using Nomad.Maths;
namespace Nomad.Logic
{
	public struct CoordinateSystem
	{
		public static CoordinateSystem Standard = new CoordinateSystem(new Vec3(1f, 0f, 0f), new Vec3(0f, 1f, 0f), new Vec3(0f, 0f, 1f));
		public Vec3 axisX;
		public Vec3 axisY;
		public Vec3 axisZ;
		public CoordinateSystem(Vec3 x, Vec3 y, Vec3 z)
		{
			this.axisX = x;
			this.axisY = y;
			this.axisZ = z;
		}
		public static CoordinateSystem FromAngles(Vec3 angles)
		{
			CoordinateSystem result = default(CoordinateSystem);
			Binding.FCE_Core_GetAxisFromAngles(angles.X, angles.Y, angles.Z, out result.axisX.X, out result.axisX.Y, out result.axisX.Z, out result.axisY.X, out result.axisY.Y, out result.axisY.Z, out result.axisZ.X, out result.axisZ.Y, out result.axisZ.Z);
			return result;
		}
		public Vec3 ToAngles()
		{
			Vec3 result = default(Vec3);
			Binding.FCE_Core_GetAnglesFromAxis(out result.X, out result.Y, out result.Z, this.axisX.X, this.axisX.Y, this.axisX.Z, this.axisY.X, this.axisY.Y, this.axisY.Z, this.axisZ.X, this.axisZ.Y, this.axisZ.Z);
			return result;
		}
		public Vec3 ConvertFromWorld(Vec3 pos)
		{
			return new Vec3(Vec3.Dot(pos, this.axisX), Vec3.Dot(pos, this.axisY), Vec3.Dot(pos, this.axisZ));
		}
		public Vec3 ConvertToWorld(Vec3 pos)
		{
			return pos.X * this.axisX + pos.Y * this.axisY + pos.Z * this.axisZ;
		}
		public Vec3 ConvertFromSystem(Vec3 pos, CoordinateSystem coords)
		{
			Vec3 pos2 = coords.ConvertToWorld(pos);
			return this.ConvertFromWorld(pos2);
		}
		public Vec3 ConvertToSystem(Vec3 pos, CoordinateSystem coords)
		{
			Vec3 pos2 = this.ConvertToWorld(pos);
			return coords.ConvertFromWorld(pos2);
		}
		public Vec3 GetPivotPoint(Vec3 center, AABB bounds, Pivot pivot)
		{
			Vec3 vec = center;
			switch (pivot)
			{
			case Pivot.Left:
				vec += this.axisX * bounds.min.X;
				break;

			case Pivot.Right:
				vec += this.axisX * bounds.max.X;
				break;

			case Pivot.Down:
				vec += this.axisY * bounds.min.Y;
				break;

			case Pivot.Up:
				vec += this.axisY * bounds.max.Y;
				break;
			}
			return vec;
		}
	}
}
