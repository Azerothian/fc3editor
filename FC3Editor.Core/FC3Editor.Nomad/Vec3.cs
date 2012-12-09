using System;
namespace FC3Editor.Nomad
{
	internal struct Vec3
	{
		public float X;
		public float Y;
		public float Z;
		public Vec2 XY
		{
			get
			{
				return new Vec2(this.X, this.Y);
			}
			set
			{
				this.X = value.X;
				this.Y = value.Y;
			}
		}
		public Vec2 XZ
		{
			get
			{
				return new Vec2(this.X, this.Z);
			}
			set
			{
				this.X = value.X;
				this.Z = value.Y;
			}
		}
		public Vec2 YZ
		{
			get
			{
				return new Vec2(this.Y, this.Z);
			}
			set
			{
				this.Y = value.X;
				this.Z = value.Y;
			}
		}
		public float LengthSquare
		{
			get
			{
				return this.X * this.X + this.Y * this.Y + this.Z * this.Z;
			}
		}
		public float Length
		{
			get
			{
				return (float)Math.Sqrt((double)this.LengthSquare);
			}
		}
		public bool IsZero
		{
			get
			{
				return Math.Abs(this.X) < 0.001f && Math.Abs(this.Y) < 0.001f && Math.Abs(this.Z) < 0.001f;
			}
		}
		public Vec3(float x, float y, float z)
		{
			this.X = x;
			this.Y = y;
			this.Z = z;
		}
		public static Vec3 operator +(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
		}
		public static Vec3 operator -(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);
		}
		public static Vec3 operator -(Vec3 v)
		{
			return new Vec3(-v.X, -v.Y, -v.Z);
		}
		public static Vec3 operator *(float s, Vec3 v)
		{
			return new Vec3(v.X * s, v.Y * s, v.Z * s);
		}
		public static Vec3 operator *(Vec3 v, float s)
		{
			return new Vec3(v.X * s, v.Y * s, v.Z * s);
		}
		public static Vec3 operator *(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.X * v2.X, v1.Y * v2.Y, v1.Z * v2.Z);
		}
		public static Vec3 operator /(Vec3 v, float s)
		{
			return new Vec3(v.X / s, v.Y / s, v.Z / s);
		}
		public static bool operator ==(Vec3 v1, Vec3 v2)
		{
			return v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z;
		}
		public static bool operator !=(Vec3 v1, Vec3 v2)
		{
			return !(v1 == v2);
		}
		public override bool Equals(object obj)
		{
			return obj is Vec3 && this == (Vec3)obj;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public static float Dot(Vec3 v1, Vec3 v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y + v1.Z * v2.Z;
		}
		public static Vec3 Cross(Vec3 v1, Vec3 v2)
		{
			return new Vec3(v1.Y * v2.Z - v1.Z * v2.Y, v1.Z * v2.X - v1.X * v2.Z, v1.X * v2.Y - v1.Y * v2.X);
		}
		public float Normalize()
		{
			float length = this.Length;
			this /= length;
			return length;
		}
		public void Snap(float resolution)
		{
			this.X -= (float)Math.IEEERemainder((double)this.X, (double)resolution);
			this.Y -= (float)Math.IEEERemainder((double)this.Y, (double)resolution);
			this.Z -= (float)Math.IEEERemainder((double)this.Z, (double)resolution);
		}
		public void Snap(Vec3 resolutionVector)
		{
			this.X -= (float)Math.IEEERemainder((double)this.X, (double)resolutionVector.X);
			this.Y -= (float)Math.IEEERemainder((double)this.Y, (double)resolutionVector.Y);
			this.Z -= (float)Math.IEEERemainder((double)this.Z, (double)resolutionVector.Z);
		}
		public Vec3 ToAngles()
		{
			Vec3 result = default(Vec3);
			Binding.FCE_Core_GetAnglesFromDir(out result.X, out result.Y, out result.Z, this.X, this.Y, this.Z);
			return result;
		}
		public string ToString(string format)
		{
			return string.Concat(new string[]
			{
				"(",
				this.X.ToString(format),
				", ",
				this.Y.ToString(format),
				", ",
				this.Z.ToString(format),
				")"
			});
		}
		public override string ToString()
		{
			return this.ToString("F4");
		}
	}
}
