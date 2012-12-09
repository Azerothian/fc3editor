using System;
namespace FC3Editor.Nomad
{
	internal class Camera
	{
		public static float ForwardInput
		{
			set
			{
				Binding.FCE_Camera_Input_Forward(value);
			}
		}
		public static float LateralInput
		{
			set
			{
				Binding.FCE_Camera_Input_Lateral(value);
			}
		}
		public static Vec3 Position
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Camera_GetPos(out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_Camera_SetPos(value.X, value.Y, value.Z);
			}
		}
		public static Vec3 Angles
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Camera_GetAngles(out result.X, out result.Y, out result.Z);
				return result;
			}
			set
			{
				Binding.FCE_Camera_SetAngles(value.X, value.Y, value.Z);
			}
		}
		public static Vec3 FrontVector
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Camera_GetFrontVector(out result.X, out result.Y, out result.Z);
				return result;
			}
		}
		public static Vec3 RightVector
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Camera_GetRightVector(out result.X, out result.Y, out result.Z);
				return result;
			}
		}
		public static Vec3 UpVector
		{
			get
			{
				Vec3 result = default(Vec3);
				Binding.FCE_Camera_GetUpVector(out result.X, out result.Y, out result.Z);
				return result;
			}
		}
		public static CoordinateSystem Axis
		{
			get
			{
				return new CoordinateSystem(Camera.RightVector, Camera.FrontVector, Camera.UpVector);
			}
		}
		public static float Speed
		{
			get
			{
				return Binding.FCE_Camera_GetSpeed();
			}
			set
			{
				Binding.FCE_Camera_SetSpeed(value);
			}
		}
		public static float SpeedFactor
		{
			set
			{
				Binding.FCE_Camera_SetSpeedFactor(value);
			}
		}
		public static float FOV
		{
			get
			{
				return Binding.FCE_Camera_GetFOV();
			}
		}
		public static float HalfFOV
		{
			get
			{
				return Camera.FOV * 0.5f;
			}
		}
		public static void Rotate(float pitch, float roll, float yaw)
		{
			Binding.FCE_Camera_Rotate(pitch, roll, yaw);
		}
		public static void Focus(EditorObject obj)
		{
			if (!obj.IsValid)
			{
				return;
			}
			AABB worldBounds = obj.WorldBounds;
			Vec3 center = worldBounds.Center;
			Vec3 v = (worldBounds - center).Length * 0.5f;
			Vec3 vec = -Camera.FrontVector;
			Camera.Position = center + vec * (vec * v).Length * 4f;
		}
	}
}
