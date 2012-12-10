using System;
using System.Collections.Generic;

using System.Text;
using Microsoft.Win32;

namespace Nomad.Utils
{
	public class RegistryUtils
	{
		public static RegistryKey GetRegistrySettings()
		{
			return Registry.CurrentUser.CreateSubKey("Software\\Ubisoft\\Far Cry 3\\Editor");
		}
		public static int GetRegistryInt(string name, int defaultValue)
		{
			int registryInt;
			using (RegistryKey registrySettings = GetRegistrySettings())
			{
				registryInt = GetRegistryInt(registrySettings, name, defaultValue);
			}
			return registryInt;
		}
		public static int GetRegistryInt(RegistryKey key, string name, int defaultValue)
		{
			object value = key.GetValue(name);
			if (value is int)
			{
				return (int)value;
			}
			return defaultValue;
		}
		public static string GetRegistryString(RegistryKey key, string name, string defaultValue)
		{
			object value = key.GetValue(name);
			if (value is string)
			{
				return (string)value;
			}
			return defaultValue;
		}
		public static void SetRegistryInt(string name, int value)
		{
			using (RegistryKey registrySettings = GetRegistrySettings())
			{
				SetRegistryInt(registrySettings, name, value);
			}
		}
		public static void SetRegistryInt(RegistryKey key, string name, int value)
		{
			key.SetValue(name, value);
		}

	}
}
