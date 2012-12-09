using System;
using Nomad.Inventory;
using Nomad.Maths;
namespace Nomad.Manager
{
	internal static class TerrainManager
	{
		public static float GlobalWaterLevel
		{
			get
			{
				return Binding.FCE_TerrainManager_GetGlobalWaterLevel();
			}
			set
			{
				Binding.FCE_TerrainManager_SetGlobalWaterLevel(value);
			}
		}
		public static float GetHeightAt(Vec2 point)
		{
			return Binding.FCE_TerrainManager_GetHeightAt(point.X, point.Y);
		}
		public static float GetHeightAtWithWater(Vec2 point)
		{
			return Binding.FCE_TerrainManager_GetHeightAtWithWater(point.X, point.Y);
		}
		public static TextureInventoryEntry GetTextureEntryFromId(int id)
		{
			return new TextureInventoryEntry(Binding.FCE_TerrainManager_GetTextureEntryFromId(id));
		}
		public static void AssignTextureId(int id, TextureInventoryEntry entry)
		{
			Binding.FCE_TerrainManager_AssignTextureId(id, entry.Pointer);
		}
		public static void ClearTextureId(int id)
		{
			Binding.FCE_TerrainManager_ClearTextureId(id);
		}
		public static void SetWaterLevelSector(int sx, int sy, float waterLevel, WaterInventoryEntry entry)
		{
			Binding.FCE_TerrainManager_SetWaterLevelSector(sx, sy, waterLevel, (entry != null) ? entry.Pointer : IntPtr.Zero);
		}
		public static void UpdateWaterLevel()
		{
			Binding.FCE_TerrainManager_UpdateWaterLevel();
		}
	}
}
