using System;
namespace FC3Editor.Nomad
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
		public static TextureInventory.Entry GetTextureEntryFromId(int id)
		{
			return new TextureInventory.Entry(Binding.FCE_TerrainManager_GetTextureEntryFromId(id));
		}
		public static void AssignTextureId(int id, TextureInventory.Entry entry)
		{
			Binding.FCE_TerrainManager_AssignTextureId(id, entry.Pointer);
		}
		public static void ClearTextureId(int id)
		{
			Binding.FCE_TerrainManager_ClearTextureId(id);
		}
		public static void SetWaterLevelSector(int sx, int sy, float waterLevel, WaterInventory.Entry entry)
		{
			Binding.FCE_TerrainManager_SetWaterLevelSector(sx, sy, waterLevel, (entry != null) ? entry.Pointer : IntPtr.Zero);
		}
		public static void UpdateWaterLevel()
		{
			Binding.FCE_TerrainManager_UpdateWaterLevel();
		}
	}
}
