using System;
using Nomad.Enums;
using Nomad.Logic;
namespace Nomad.Utils
{
	internal class EditorSettings
	{
		
		private static float m_viewportQuality = 1f;
		private static bool m_invertMouseView;
		private static bool m_invertMousePan;
		public static bool ShowCollections
		{
			get
			{
				return Binding.FCE_EditorSettings_IsCollectionVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowCollections(value);
			}
		}
		public static bool ShowFog
		{
			get
			{
				return Binding.FCE_EditorSettings_IsFogVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowFog(value);
			}
		}
		public static bool ShowExposure
		{
			get
			{
				return Binding.FCE_EditorSettings_IsExposureVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowExposure(value);
			}
		}
		public static bool ShowShadow
		{
			get
			{
				return Binding.FCE_EditorSettings_IsShadowVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowShadow(value);
			}
		}
		public static bool ShowWater
		{
			get
			{
				return Binding.FCE_EditorSettings_IsWaterVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowWater(value);
			}
		}
		public static bool ShowIcons
		{
			get
			{
				return Binding.FCE_EditorSettings_IsIconsVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowIcons(value);
			}
		}
		public static bool SoundEnabled
		{
			get
			{
				return Binding.FCE_EditorSettings_IsSoundEnabled();
			}
			set
			{
				Binding.FCE_EditorSettings_SetSoundEnabled(value);
			}
		}
		public static bool ShowGrid
		{
			get
			{
				return Binding.FCE_EditorSettings_IsGridVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowGrid(value);
			}
		}
		public static int GridResolution
		{
			get
			{
				return Binding.FCE_EditorSettings_GetGridResolution();
			}
			set
			{
				Binding.FCE_EditorSettings_SetGridResolution(value);
			}
		}
		public static bool IsNavmeshVisible
		{
			get
			{
				return Binding.FCE_EditorSettings_IsNavmeshVisible();
			}
		}
		public static Navmesh.Layer NavmeshLayer
		{
			get
			{
				return (Navmesh.Layer)Binding.FCE_EditorSettings_GetNavmeshLayer();
			}
		}
		public static bool ShowCovers
		{
			get
			{
				return Binding.FCE_EditorSettings_IsCoversVisible();
			}
			set
			{
				Binding.FCE_EditorSettings_ShowCovers(value);
			}
		}
		public static bool Invincible
		{
			get
			{
				return Binding.FCE_EditorSettings_IsInvincible();
			}
			set
			{
				Binding.FCE_EditorSettings_SetInvincible(value);
			}
		}
		public static bool Invisible
		{
			get
			{
				return Binding.FCE_EditorSettings_IsInvisible();
			}
			set
			{
				Binding.FCE_EditorSettings_SetInvisible(value);
			}
		}
		public static bool SnapObjectsToTerrain
		{
			get
			{
				return Binding.FCE_EditorSettings_IsSnappingObjectsToTerrain();
			}
			set
			{
				Binding.FCE_EditorSettings_SetSnapObjectsToTerrain(value);
			}
		}
		public static bool AutoSnappingObjects
		{
			get
			{
				return Binding.FCE_EditorSettings_IsAutoSnappingObjects();
			}
			set
			{
				Binding.FCE_EditorSettings_SetAutoSnappingObjects(value);
			}
		}
		public static bool AutoSnappingObjectsRotation
		{
			get
			{
				return Binding.FCE_EditorSettings_IsAutoSnappingObjectsRotation();
			}
			set
			{
				Binding.FCE_EditorSettings_SetAutoSnappingObjectsRotation(value);
			}
		}
		public static bool AutoSnappingObjectsTerrain
		{
			get
			{
				return Binding.FCE_EditorSettings_IsAutoSnappingObjectsTerrain();
			}
			set
			{
				Binding.FCE_EditorSettings_SetAutoSnappingObjectsTerrain(value);
			}
		}
		public static bool CameraClipTerrain
		{
			get
			{
				return Binding.FCE_EditorSettings_IsCameraClippedTerrain();
			}
			set
			{
				Binding.FCE_EditorSettings_SetCameraClipTerrain(value);
			}
		}
		public static bool CameraCollision
		{
			get
			{
				return Binding.FCE_EditorSettings_IsCameraCollision();
			}
			set
			{
				Binding.FCE_EditorSettings_SetCameraCollision(value);
			}
		}
		public static QualityLevel EngineQuality
		{
			get
			{
				return (QualityLevel)Binding.FCE_EditorSettings_GetEngineQuality();
			}
			set
			{
				Binding.FCE_EditorSettings_SetEngineQuality((int)value);
			}
		}
		public static float ViewportQuality
		{
			get
			{
				return EditorSettings.m_viewportQuality;
			}
			set
			{
				EditorSettings.m_viewportQuality = value;
			}
		}
		public static bool KillDistanceOverride
		{
			get
			{
				return Binding.FCE_EditorSettings_IsKillDistanceOverride();
			}
			set
			{
				Binding.FCE_EditorSettings_SetKillDistanceOverride(value);
			}
		}
		public static bool InvertMouseView
		{
			get
			{
				return EditorSettings.m_invertMouseView;
			}
			set
			{
				EditorSettings.m_invertMouseView = value;
			}
		}
		public static bool InvertMousePan
		{
			get
			{
				return EditorSettings.m_invertMousePan;
			}
			set
			{
				EditorSettings.m_invertMousePan = value;
			}
		}
		public static void ShowNavmesh(Navmesh.Layer layer)
		{
			Binding.FCE_EditorSettings_ShowNavmesh((int)layer);
		}
		public static void HideNavmesh()
		{
			Binding.FCE_EditorSettings_HideNavmesh();
		}
	}
}
