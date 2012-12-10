using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Nomad.Logic;
using Nomad.Maths;
using Nomad.Utils;
namespace Nomad.Editor
{
	public class EditorDocument
	{
		public delegate void LoadCompletedCallback(bool success);
		public delegate void SaveCompletedCallback(bool success);
		public enum BattlefieldSizes
		{
			Small,
			Medium,
			Large
		}
		public enum PlayerSizes
		{
			Small,
			Medium,
			Large,
			XLarge
		}
		private static EditorDocument.LoadCompletedCallback m_loadCompletedCallback;
		private static EditorDocument.SaveCompletedCallback m_saveCompletedCallback;
		public static Guid MapId
		{
			get
			{
				ulong num;
				ulong num2;
				Binding.FCE_Document_GetMapID(out num, out num2);
				string g = num.ToString("X16") + num2.ToString("X16");
				Guid result = Guid.Empty;
				try
				{
					result = new Guid(g);
				}
				catch (Exception)
				{
				}
				return result;
			}
			set
			{
				string text = value.ToString("N");
				ulong mapIdHigh = Convert.ToUInt64(text.Substring(0, 16), 16);
				ulong mapIdLow = Convert.ToUInt64(text.Substring(16, 16), 16);
				Binding.FCE_Document_SetMapID(mapIdHigh, mapIdLow);
			}
		}
		public static Guid VersionId
		{
			get
			{
				ulong num;
				ulong num2;
				Binding.FCE_Document_GetVersionID(out num, out num2);
				string g = num.ToString("X16") + num2.ToString("X16");
				return new Guid(g);
			}
		}
		public static string DefaultMapName
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Document_GetMapDefaultName());
			}
		}
		public static string MapName
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Document_GetMapName());
			}
			set
			{
				Binding.FCE_Document_SetMapName(value);
			}
		}
		public static string CreatorName
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Document_GetCreatorName());
			}
			set
			{
				Binding.FCE_Document_SetCreatorName(value);
			}
		}
		public static string AuthorName
		{
			get
			{
				return Marshal.PtrToStringUni(Binding.FCE_Document_GetAuthorName());
			}
			set
			{
				Binding.FCE_Document_SetAuthorName(value);
			}
		}
		public static EditorDocument.BattlefieldSizes BattlefieldSize
		{
			get
			{
				return (EditorDocument.BattlefieldSizes)Binding.FCE_Document_GetBattlefieldSize();
			}
			set
			{
				Binding.FCE_Document_SetBattlefieldSize((int)value);
			}
		}
		public static EditorDocument.PlayerSizes PlayerSize
		{
			get
			{
				return (EditorDocument.PlayerSizes)Binding.FCE_Document_GetPlayerSize();
			}
			set
			{
				Binding.FCE_Document_SetPlayerSize((int)value);
			}
		}
		public static bool IsSnapshotSet
		{
			get
			{
				return Binding.FCE_Document_IsSnapshotSet();
			}
		}
		public static Vec3 SnapshotPos
		{
			get
			{
				float x;
				float y;
				float z;
				Binding.FCE_Document_GetSnapshotPos(out x, out y, out z);
				return new Vec3(x, y, z);
			}
			set
			{
				Binding.FCE_Document_SetSnapshotPos(value.X, value.Y, value.Z);
			}
		}
		public static Vec3 SnapshotAngle
		{
			get
			{
				float x;
				float y;
				float z;
				Binding.FCE_Document_GetSnapshotAngle(out x, out y, out z);
				return new Vec3(x, y, z);
			}
			set
			{
				Binding.FCE_Document_SetSnapshotAngle(value.X, value.Y, value.Z);
			}
		}
		public static bool NavmeshEnabled
		{
			get
			{
				return Binding.FCE_Document_IsNavmeshEnabled();
			}
			set
			{
				Binding.FCE_Document_SetNavmeshEnabled(value);
			}
		}
		public static void Reset()
		{
			Binding.FCE_Document_Reset();
			EditorDocument.AuthorName = Win32.GetUserNameEx(Win32.EXTENDED_NAME_FORMAT.NameDisplay);
		}
		public static bool LoadPhysical(string path)
		{
			return Binding.FCE_Document_LoadPhysical(path);
		}
		public static bool Load(string fileName, EditorDocument.LoadCompletedCallback callback)
		{
			string s = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;
			string fileName2 = Path.GetFileName(fileName);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] bytes2 = Encoding.UTF8.GetBytes(fileName2);
			EditorDocument.m_loadCompletedCallback = callback;
			return Binding.FCE_Document_Load(bytes, bytes2);
		}
		public static void OnLoadCompleted(bool success)
		{
			if (!success)
			{
				Log.error("{0}", Localizer.Localize("ERROR_LOAD_FAILED"));

				//LocalizedMessageBox.Show(Localizer.Localize("ERROR_LOAD_FAILED"), Localizer.Localize("ERROR"), Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
				
				//TODO: fix Clear map path?
				//MainForm.Instance.ClearMapPath();
			}
			if (EditorDocument.m_loadCompletedCallback != null)
			{
				EditorDocument.m_loadCompletedCallback(success);
			}
		}
		public static void Save(string fileName, EditorDocument.SaveCompletedCallback callback)
		{
			string s = Path.GetDirectoryName(fileName) + Path.DirectorySeparatorChar;
			string fileName2 = Path.GetFileName(fileName);
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			byte[] bytes2 = Encoding.UTF8.GetBytes(fileName2);
			EditorDocument.m_saveCompletedCallback = callback;
			Binding.FCE_Document_Save(bytes, bytes2);
		}
		public static void OnSaveCompleted(bool success)
		{
			if (!success)
			{
				Log.error("{0}", Localizer.Localize("ERROR_SAVE_FAILED"));
				//LocalizedMessageBox.Show(Localizer.Localize("ERROR_SAVE_FAILED"), Localizer.Localize("ERROR"), Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
			}
			if (EditorDocument.m_saveCompletedCallback != null)
			{
				EditorDocument.m_saveCompletedCallback(success);
			}
		}
		public static bool Validate()
		{
			return Binding.FCE_Document_Validate();
		}
		public static void ClearSnapshot()
		{
			Binding.FCE_Document_ClearSnapshot();
		}
		public static void TakeSnapshot(Snapshot snapshot, int sampleFactor)
		{
			Binding.FCE_Document_TakeSnapshot(snapshot.Pointer, sampleFactor);
		}
		public static void FinalizeMap()
		{
			Binding.FCE_Document_FinalizeMap();
		}
		public static void Export(string mapFile, string exportPath, bool toConsole)
		{
			Binding.FCE_Document_Export(mapFile, exportPath, toConsole);
		}
		public static void Dump(string mapFile, string dumpPath)
		{
			Binding.FCE_Document_Dump(mapFile, dumpPath);
		}
		public static void ExtractBigFile(string mapFile, string bfPath, string bfName)
		{
			Binding.FCE_Document_ExtractBigFile(mapFile, bfPath, bfName);
		}
	}
}
