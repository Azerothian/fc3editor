using System;
using Nomad.Components;
namespace Nomad.Manager
{
	internal static class SplineManager
	{
		public const int MaxRoads = 8;
		public static SplineRoad CreateRoad(int id)
		{
			return new SplineRoad(Binding.FCE_SplineManager_CreateRoad(id));
		}
		public static void DestroyRoad(int id)
		{
			Binding.FCE_SplineManager_DestroyRoad(id);
		}
		public static SplineRoad GetRoadFromId(int id)
		{
			return new SplineRoad(Binding.FCE_SplineManager_GetRoadFromId(id));
		}
		public static SplineZone GetPlayableZone()
		{
			return new SplineZone(Binding.FCE_SplineManager_GetPlayableZone());
		}
	}
}
