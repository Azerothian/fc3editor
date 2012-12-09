using System;
namespace Nomad.Maths
{
	public struct AABB
	{
		public Vec3 min;
		public Vec3 max;
		public Vec3 Length
		{
			get
			{
				return this.max - this.min;
			}
		}
		public Vec3 Center
		{
			get
			{
				return (this.max + this.min) * 0.5f;
			}
		}
		public AABB(Vec3 min, Vec3 max)
		{
			this.min = min;
			this.max = max;
		}
		public static AABB operator -(AABB a, Vec3 b)
		{
			Vec3 vec = a.min - b;
			Vec3 vec2 = a.max - b;
			return new AABB(vec, vec2);
		}
		public override string ToString()
		{
			Vec3 length = this.Length;
			return string.Concat(new string[]
			{
				length.X.ToString("F1"),
				" x ",
				length.Y.ToString("F1"),
				" x ",
				length.Z.ToString("F1"),
				" m"
			});
		}
	}
}
