using System;
using Nomad.Maths;
namespace Nomad.Logic
{
	public class Navmesh
	{
		public enum Layer
		{
			Character,
			Vehicle
		}
		public static float DebugAlpha
		{
			get
			{
				return Binding.FCE_Navmesh_GetDebugAlpha();
			}
			set
			{
				Binding.FCE_Navmesh_SetDebugAlpha(value);
			}
		}
		public static int PendingTilesCount
		{
			get
			{
				return Binding.FCE_Navmesh_GetPendingTilesCount();
			}
		}
		public static void Show(Navmesh.Layer layer)
		{
			//TODO: Fix Below

			//Binding.FCE_Navmesh_SetDisplay((int)(layer + Navmesh.Layer.Vehicle));
		}
		public static void Hide()
		{
			Binding.FCE_Navmesh_SetDisplay(0);
		}
		public static void RegenerateTileAt(Vec2 pos, bool debugMode)
		{
			Binding.FCE_Navmesh_RegenerateTileAt(pos.X, pos.Y, debugMode);
		}
		public static void ShowActionPoints()
		{
			Binding.FCE_Navmesh_SetAPDisplay(1);
		}
		public static void HideActionPoints()
		{
			Binding.FCE_Navmesh_SetAPDisplay(0);
		}
	}
}
