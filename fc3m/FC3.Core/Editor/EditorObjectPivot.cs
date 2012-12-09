using System;
using Nomad.Logic;
using Nomad.Maths;
namespace Nomad.Editor
{
	public class EditorObjectPivot
	{
		public Vec3 position;
		public Vec3 normal;
		public Vec3 normalUp;
		public void Unapply(EditorObject obj)
		{
			CoordinateSystem coordinateSystem = CoordinateSystem.FromAngles(obj.Angles);
			AABB localBounds = obj.LocalBounds;
			Vec3 vec = (localBounds.max + localBounds.min) * 0.5f;
			Vec3 vec2 = localBounds.Length * 0.5f;
			this.position -= obj.Position + vec.X * coordinateSystem.axisX + vec.Y * coordinateSystem.axisY;
			this.position = coordinateSystem.ConvertFromWorld(this.position);
			this.normal = coordinateSystem.ConvertFromWorld(this.normal);
			this.normalUp = coordinateSystem.ConvertFromWorld(this.normalUp);
			this.position.X = this.position.X / vec2.X;
			this.position.Y = this.position.Y / vec2.Y;
			if (this.position.X > 1f)
			{
				this.position.X = 1f;
			}
			else
			{
				if (this.position.X < -1f)
				{
					this.position.X = -1f;
				}
			}
			if (this.position.Y > 1f)
			{
				this.position.Y = 1f;
			}
			else
			{
				if (this.position.Y < -1f)
				{
					this.position.Y = -1f;
				}
			}
			if (this.position.Z > 1f)
			{
				this.position.Z = 1f;
			}
			else
			{
				if (this.position.Z < -1f)
				{
					this.position.Z = -1f;
				}
			}
			this.normal.Z = 0f;
			this.normalUp = new Vec3(0f, 0f, 1f);
		}
	}
}
