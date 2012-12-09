using System;
using System.Collections.Generic;

using System.Text;
using Nomad.Components;
using Nomad.Editor;
using Nomad.Logic;
using Nomad.Maths;

namespace Nomad.Utils
{
	public class RayUtils
	{
		public static bool GetScreenPointFromWorldPos(Vec3 worldPos, out Vec2 screenPoint)
		{
			return GetScreenPointFromWorldPos(worldPos, out screenPoint, false);
		}
		public static bool GetScreenPointFromWorldPos(Vec3 worldPos, out Vec2 screenPoint, bool clipped)
		{
			bool flag = Binding.FCE_Editor_GetScreenPointFromWorldPos(worldPos.X, worldPos.Y, worldPos.Z, out screenPoint.X, out screenPoint.Y);
			if (flag && clipped)
			{
				screenPoint.X = Math.Min(Math.Max(0f, screenPoint.X), 1f);
				screenPoint.Y = Math.Min(Math.Max(0f, screenPoint.Y), 1f);
			}
			return flag;
		}
		public static void GetWorldRayFromScreenPoint(Vec2 screenPoint, out Vec3 raySrc, out Vec3 rayDir)
		{
			Binding.FCE_Editor_GetWorldRayFromScreenPoint(screenPoint.X, screenPoint.Y, out raySrc.X, out raySrc.Y, out raySrc.Z, out rayDir.X, out rayDir.Y, out rayDir.Z);
		}
		public static bool RayCastTerrain(Vec3 raySrc, Vec3 rayDir, out Vec3 hitPos, out float hitDist)
		{
			return Binding.FCE_Editor_RayCastTerrain(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObject ignore, out Vec3 hitPos, out float hitDist)
		{
			Vec3 vec;
			return RayCastPhysics(raySrc, rayDir, ignore, out hitPos, out hitDist, out vec);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObject ignore, out Vec3 hitPos, out float hitDist, out Vec3 hitNormal)
		{
			return Binding.FCE_Editor_RayCastPhysics(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, ignore.Pointer, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist, out hitNormal.X, out hitNormal.Y, out hitNormal.Z);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObjectSelection ignore, out Vec3 hitPos, out float hitDist)
		{
			Vec3 vec;
			return RayCastPhysics(raySrc, rayDir, ignore, out hitPos, out hitDist, out vec);
		}
		public static bool RayCastPhysics(Vec3 raySrc, Vec3 rayDir, EditorObjectSelection ignore, out Vec3 hitPos, out float hitDist, out Vec3 hitNormal)
		{
			return Binding.FCE_Editor_RayCastPhysics2(raySrc.X, raySrc.Y, raySrc.Z, rayDir.X, rayDir.Y, rayDir.Z, ignore.Pointer, out hitPos.X, out hitPos.Y, out hitPos.Z, out hitDist, out hitNormal.X, out hitNormal.Y, out hitNormal.Z);
		}
		
		public static bool RayCastTerrainFromScreenPoint(Vec2 screenPoint, out Vec3 hitPos)
		{
			Vec3 raySrc;
			Vec3 rayDir;
			GetWorldRayFromScreenPoint(screenPoint, out raySrc, out rayDir);
			float num;
			return RayCastTerrain(raySrc, rayDir, out hitPos, out num);
		}
		public static bool RayCastTerrainFromMouse(out Vec3 hitPos, ViewportControl viewport)
		{
			return RayCastTerrainFromScreenPoint(viewport.NormalizedMousePos, out hitPos);
		}
		public static bool RayCastPhysicsFromScreenPoint(Vec2 screenPoint, out Vec3 hitPos)
		{
			Vec3 raySrc;
			Vec3 rayDir;
			GetWorldRayFromScreenPoint(screenPoint, out raySrc, out rayDir);
			float num;
			return RayCastPhysics(raySrc, rayDir, EditorObject.Null, out hitPos, out num);
		}
		public static bool RayCastPhysicsFromMouse(out Vec3 hitPos, ViewportControl viewport)
		{
			return RayCastPhysicsFromScreenPoint(viewport.NormalizedMousePos, out hitPos);
		}
		public static void ApplyScreenDeltaToWorldPos(Vec2 screenDelta, ref Vec3 worldPos)
		{
			Vec3 vec = Camera.FrontVector;
			if ((double)Math.Abs(vec.X) < 0.001 && (double)Math.Abs(vec.Y) < 0.001)
			{
				vec = Camera.UpVector;
			}
			Vec2 vec2 = -vec.XY;
			vec2.Normalize();
			Vec2 vec3 = new Vec2(-vec2.Y, vec2.X);
			float num = (float)((double)Vec3.Dot(worldPos - Camera.Position, Camera.FrontVector) * Math.Tan((double)Camera.HalfFOV) * 2.0);
			worldPos.X += num * screenDelta.X * vec3.X + num * screenDelta.Y * vec2.X;
			worldPos.Y += num * screenDelta.X * vec3.Y + num * screenDelta.Y * vec2.Y;
		}
	}
}
