using System;
using System.Drawing;
using Nomad.Maths;
namespace Nomad.Logic
{
	public class TerrainManipulator
	{
		public enum NoiseType
		{
			Normal,
			Absolute,
			InverseAbsolute
		}
		public static void Bump(Vec2 center, float amount, PaintBrush brush)
		{
			Binding.FCE_Terrain_Bump(center.X, center.Y, amount, brush.Pointer);
		}
		public static void Bump_End()
		{
			Binding.FCE_Terrain_Bump_End();
		}
		public static void RaiseLower(Vec2 center, float amount, PaintBrush brush)
		{
			Binding.FCE_Terrain_RaiseLower(center.X, center.Y, amount, brush.Pointer);
		}
		public static void RaiseLower_End()
		{
			Binding.FCE_Terrain_RaiseLower_End();
		}
		public static void SetHeight(Vec2 center, float height, PaintBrush brush)
		{
			Binding.FCE_Terrain_SetHeight(center.X, center.Y, height, brush.Pointer);
		}
		public static void SetHeight_End()
		{
			Binding.FCE_Terrain_SetHeight_End();
		}
		public static float GetAverageHeight(Vec2 center, PaintBrush brush)
		{
			return Binding.FCE_Terrain_GetAverageHeight(center.X, center.Y, brush.Pointer);
		}
		public static void Average(Vec2 center, PaintBrush brush)
		{
			Binding.FCE_Terrain_Average(center.X, center.Y, brush.Pointer);
		}
		public static void Average_End()
		{
			Binding.FCE_Terrain_Average_End();
		}
		public static void Grab_Begin(float x, float y, PaintBrush brush)
		{
			Binding.FCE_Terrain_Grab_Begin(x, y, brush.Pointer);
		}
		public static void Grab(float ratio)
		{
			Binding.FCE_Terrain_Grab(ratio);
		}
		public static void Grab_End()
		{
			Binding.FCE_Terrain_Grab_End();
		}
		public static void Smooth(Vec2 center, PaintBrush brush)
		{
			Binding.FCE_Terrain_Smooth(center.X, center.Y, brush.Pointer);
		}
		public static void Smooth_End()
		{
			Binding.FCE_Terrain_Smooth_End();
		}
		public static void Ramp(Vec2 ptStart, Vec2 ptEnd, float radius, float hardness)
		{
			Binding.FCE_Terrain_Ramp(ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y, radius, hardness);
		}
		public static void Terrace(Vec2 center, float height, float falloff, PaintBrush brush)
		{
			Binding.FCE_Terrain_Terrace(center.X, center.Y, height, falloff, brush.Pointer);
		}
		public static void Terrace_End()
		{
			Binding.FCE_Terrain_Terrace_End();
		}
		public static void Noise_Begin(int numOctaves, float noiseSize, float persistence, TerrainManipulator.NoiseType noiseType)
		{
			Binding.FCE_Terrain_Noise_Begin(numOctaves, noiseSize, persistence, (int)noiseType);
		}
		public static void Noise(Vec2 center, float amount, PaintBrush brush)
		{
			Binding.FCE_Terrain_Noise(center.X, center.Y, amount, brush.Pointer);
		}
		public static void Noise_End()
		{
			Binding.FCE_Terrain_Noise_End();
		}
		public static void Erosion(Vec2 center, float radius, float density, float deformation, float channelDepth, float randomness)
		{
			Binding.FCE_Terrain_Erosion(center.X, center.Y, radius, density, deformation, channelDepth, randomness);
		}
		public static void Erosion_End()
		{
			Binding.FCE_Terrain_Erosion_End();
		}
		public static void Hole(Rectangle rect, bool hole)
		{
			Binding.FCE_Terrain_Hole(rect.Left, rect.Top, rect.Right, rect.Bottom, hole);
		}
		public static void Hole_End()
		{
			Binding.FCE_Terrain_Hole_End();
		}
	}
}
