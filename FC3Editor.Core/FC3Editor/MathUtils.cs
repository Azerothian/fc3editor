using System;
namespace FC3Editor
{
	internal static class MathUtils
	{
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
			{
				value = min;
			}
			else
			{
				if (value > max)
				{
					value = max;
				}
			}
			return value;
		}
		public static float Deg2Rad(float angleDeg)
		{
			return angleDeg * 2f * 3.14159274f / 360f;
		}
		public static float Rad2Deg(float angleRad)
		{
			return angleRad * 360f / 6.28318548f;
		}
	}
}
