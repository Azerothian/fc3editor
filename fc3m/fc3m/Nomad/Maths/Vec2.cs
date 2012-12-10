using System;
namespace Nomad.Maths
{
	public struct Vec2
	{
		public static Vec2 Zero = new Vec2(0f, 0f);
		public float X;
		public float Y;
		public float Length
		{
			get
			{
				return (float)Math.Sqrt((double)(this.X * this.X + this.Y * this.Y));
			}
		}
		public Vec2(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}
		public static Vec2 operator +(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.X + v2.X, v1.Y + v2.Y);
		}
		public static Vec2 operator -(Vec2 v)
		{
			return new Vec2(-v.X, -v.Y);
		}
		public static Vec2 operator -(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.X - v2.X, v1.Y - v2.Y);
		}
		public static Vec2 operator *(float s, Vec2 v)
		{
			return new Vec2(v.X * s, v.Y * s);
		}
		public static Vec2 operator *(Vec2 v, float s)
		{
			return new Vec2(v.X * s, v.Y * s);
		}
		public static Vec2 operator *(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.X * v2.X, v1.Y * v2.Y);
		}
		public static Vec2 operator /(Vec2 v, float s)
		{
			return new Vec2(v.X / s, v.Y / s);
		}
		public static Vec2 operator /(Vec2 v1, Vec2 v2)
		{
			return new Vec2(v1.X / v2.X, v1.Y / v2.Y);
		}
		public static bool operator ==(Vec2 v1, Vec2 v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y;
		}
		public static bool operator !=(Vec2 v1, Vec2 v2)
		{
			return !(v1 == v2);
		}
		public override bool Equals(object obj)
		{
			return obj is Vec2 && this == (Vec2)obj;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public static float Dot(Vec2 v1, Vec2 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}
		public float Normalize()
		{
			float length = this.Length;
			this /= length;
			return length;
		}
		public void Rotate90CCW()
		{
			float x = this.X;
			this.X = -this.Y;
			this.Y = x;
		}
		public void Rotate90CW()
		{
			float x = this.X;
			this.X = this.Y;
			this.Y = -x;
		}
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				this.X.ToString("F4"),
				", ",
				this.Y.ToString("F4"),
				")"
			});
		}
	}
}
