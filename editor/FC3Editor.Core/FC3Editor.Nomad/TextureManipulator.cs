using System;
namespace FC3Editor.Nomad
{
	internal class TextureManipulator
	{
		public static void Paint(Vec2 center, float amount, int id, PaintBrush brush)
		{
			Binding.FCE_Texture_Paint(center.X, center.Y, amount, id, brush.Pointer);
		}
		public static void Paint_End()
		{
			Binding.FCE_Texture_Paint_End();
		}
		public static void PaintConstraints_Begin(float minHeight, float maxHeight, float heightFuzziness, float minSlope, float maxSlope)
		{
			Binding.FCE_Texture_PaintConstraints_Begin(minHeight, maxHeight, heightFuzziness, minSlope, maxSlope);
		}
		public static void PaintConstraints(Vec2 center, float amount, int id, PaintBrush brush)
		{
			Binding.FCE_Texture_PaintConstraints(center.X, center.Y, amount, id, brush.Pointer);
		}
		public static void PaintConstraints_End()
		{
			Binding.FCE_Texture_PaintConstraints_End();
		}
	}
}
