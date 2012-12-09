using System;
using System.Drawing;
namespace FC3Editor
{
	internal class ColorUtils
	{
		public static void RGBToHSL(Color c, out float h, out float s, out float l)
		{
			float num = (float)c.R / 255f;
			float num2 = (float)c.G / 255f;
			float num3 = (float)c.B / 255f;
			float num4 = num;
			float num5 = num;
			if (num2 < num4)
			{
				num4 = num2;
			}
			else
			{
				if (num2 > num5)
				{
					num5 = num2;
				}
			}
			if (num3 < num4)
			{
				num4 = num3;
			}
			else
			{
				num5 = num3;
			}
			if (num5 == num4)
			{
				h = 0f;
				s = 0f;
				l = num5;
				return;
			}
			if (num5 == num)
			{
				if (num2 >= num3)
				{
					h = 0.1666f * (num2 - num3) / (num5 - num4);
				}
				else
				{
					h = 0.1666f * (num2 - num3) / (num5 - num4) + 1f;
				}
			}
			else
			{
				if (num5 == num2)
				{
					h = 0.1666f * (num3 - num) / (num5 - num4) + 0.3333f;
				}
				else
				{
					h = 0.1666f * (num - num2) / (num5 - num4) + 0.6666f;
				}
			}
			l = (num5 + num4) * 0.5f;
			if (l <= 0.5f)
			{
				s = (num5 - num4) / (2f * l);
				return;
			}
			s = (num5 - num4) / (2f - 2f * l);
		}
		public static Color HSLToRGB(float h, float s, float l)
		{
			float num = (l < 0.5f) ? (l * (1f + s)) : (l + s - l * s);
			float num2 = 2f * l - num;
			float num3 = h + 0.3333f;
			float num4 = h - 0.3333f;
			if (num3 > 1f)
			{
				num3 -= 1f;
			}
			if (num4 < 0f)
			{
				num4 += 1f;
			}
			if (num3 < 0.1666f)
			{
				num3 = num2 + (num - num2) * 6f * num3;
			}
			else
			{
				if (num3 < 0.5f)
				{
					num3 = num;
				}
				else
				{
					if (num3 < 0.6666f)
					{
						num3 = num2 + (num - num2) * (0.6666f - num3) * 6f;
					}
					else
					{
						num3 = num2;
					}
				}
			}
			float num5;
			if (h < 0.1666f)
			{
				num5 = num2 + (num - num2) * 6f * h;
			}
			else
			{
				if (h < 0.5f)
				{
					num5 = num;
				}
				else
				{
					if (h < 0.6666f)
					{
						num5 = num2 + (num - num2) * (0.6666f - h) * 6f;
					}
					else
					{
						num5 = num2;
					}
				}
			}
			if (num4 < 0.1666f)
			{
				num4 = num2 + (num - num2) * 6f * num4;
			}
			else
			{
				if (num4 < 0.5f)
				{
					num4 = num;
				}
				else
				{
					if (num4 < 0.6666f)
					{
						num4 = num2 + (num - num2) * (0.6666f - num4) * 6f;
					}
					else
					{
						num4 = num2;
					}
				}
			}
			int red = (int)(num3 * 255f);
			int green = (int)(num5 * 255f);
			int blue = (int)(num4 * 255f);
			return Color.FromArgb(red, green, blue);
		}
	}
}
