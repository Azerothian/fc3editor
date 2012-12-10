using System;
using Nomad.Maths;
namespace Nomad.Logic
{
	public class CollectionManipulator
	{
		public static void Paint(Vec2 center, int id, PaintBrush brush)
		{
			Binding.FCE_Collection_Paint(center.X, center.Y, id, brush.Pointer);
		}
		public static void Paint_End()
		{
			Binding.FCE_Collection_Paint_End();
		}
	}
}
