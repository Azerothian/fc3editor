using System;
using System.Drawing;
using Nomad.Maths;
namespace Nomad.Renderers
{
	public class Render
	{
		public static void BeginGroup()
		{
			Binding.FCE_Draw_BeginGroup();
		}
		public static void EndGroup()
		{
			Binding.FCE_Draw_EndGroup();
		}
		public static void DrawScreenCircleOutlined(Vec2 center, float z, float radius, float penWidth, Color color)
		{
			Binding.FCE_Draw_ScreenCircleOutlined(center.X, center.Y, z, radius, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);
		}
		public static void DrawScreenRectangleOutlined(RectangleF rect, float z, float penWidth, Color color)
		{
			SizeF size = rect.Size;
			Vec2 vec = new Vec2(rect.X + size.Width / 2f, rect.Y + size.Height / 2f);
			Binding.FCE_Draw_ScreenRectangleOutlined(vec.X, vec.Y, z, size.Width, size.Height, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);
		}
		public static void DrawQuad(Vec3 center, float width, float height, Color color)
		{
			Binding.FCE_Draw_Quad(center.X, center.Y, center.Z, width, height, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);
		}
		public static void DrawSquare(Vec3 center, float radius, float penWidth, Color color, float zOrder, Color borderColor)
		{
			Binding.FCE_Draw_Square(center.X, center.Y, center.Z, radius, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f, zOrder, (float)borderColor.R / 255f, (float)borderColor.G / 255f, (float)borderColor.B / 255f, (float)borderColor.A / 255f);
		}
		public static void DrawTerrainCircle(Vec2 center, float radius, float penWidth, Color color, float zOrder, float zOffset)
		{
			Render.DrawTerrainCircle(center, radius, penWidth, color, zOrder, zOffset, Color.Black);
		}
		public static void DrawTerrainCircle(Vec2 center, float radius, float penWidth, Color color, float zOrder, float zOffset, Color borderColor)
		{
			Binding.FCE_Draw_Terrain_Circle(center.X, center.Y, radius, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f, zOrder, zOffset, (float)borderColor.R / 255f, (float)borderColor.G / 255f, (float)borderColor.B / 255f, (float)borderColor.A / 255f);
		}
		public static void DrawTerrainSquare(Vec2 center, float radius, float penWidth, Color color, float zOrder, float zOffset)
		{
			Render.DrawTerrainSquare(center, radius, penWidth, color, zOrder, zOffset, Color.Black);
		}
		public static void DrawTerrainSquare(Vec2 center, float radius, float penWidth, Color color, float zOrder, float zOffset, Color borderColor)
		{
			Binding.FCE_Draw_Terrain_Square(center.X, center.Y, radius, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f, zOrder, zOffset, (float)borderColor.R / 255f, (float)borderColor.G / 255f, (float)borderColor.B / 255f, (float)borderColor.A / 255f);
		}
		public static void DrawArrow(Vec3 center, Vec3 direction, float length, float radius, float headLength, float headRadius, Color color)
		{
			Binding.FCE_Draw_Arrow(center.X, center.Y, center.Z, direction.X, direction.Y, direction.Z, length, radius, headLength, headRadius, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, (float)color.A / 255f);
		}
		public static void DrawDot(Vec3 center, float radius, Color color, bool back, bool startGroup)
		{
			Binding.FCE_Draw_Dot(center.X, center.Y, center.Z, radius, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, back, startGroup);
		}
		public static void DrawSegmentedLineSegment(Vec3 p1, Vec3 p2, float penRadius, float penRadius2, Color color, bool back)
		{
			Binding.FCE_Draw_SegmentedLineSegment(p1.X, p1.Y, p1.Z, p2.X, p2.Y, p2.Z, penRadius, penRadius2, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f, back);
		}
		public static void DrawWireBoxFromBottomZ(Vec3 pos, Vec3 size, float penWidth)
		{
			Binding.FCE_Draw_WireBoxFromBottomZ(pos.X, pos.Y, pos.Z, size.X, size.Y, size.Z, penWidth);
		}
		public static void DrawWireRegionFromTerrain(Points points, float penWidth, Color color)
		{
			Binding.FCE_Draw_WireRegionFromTerrain(points.Pointer, penWidth, (float)color.R / 255f, (float)color.G / 255f, (float)color.B / 255f);
		}
	}
}
