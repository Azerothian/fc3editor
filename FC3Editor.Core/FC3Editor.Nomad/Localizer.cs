using System;
using System.Runtime.InteropServices;
using TD.SandBar;
namespace FC3Editor.Nomad
{
	internal static class Localizer
	{
		private static string LocalizeInternal(string section, string key)
		{
			return Marshal.PtrToStringUni(Binding.LocalizeText(section, key));
		}
		public static string Localize(string section, string key)
		{
			if (!Engine.Initialized)
			{
				return "!DLL_NOT_LOADED";
			}
			return Localizer.LocalizeInternal(section, key);
		}
		public static string LocalizeCommon(string key)
		{
			return Localizer.LocalizeInternal("InGameEditor", key);
		}
		public static string Localize(string key)
		{
			if (key.StartsWith("*"))
			{
				return null;
			}
			return Localizer.LocalizeInternal("InGameEditor_PC", key);
		}
		public static void Localize(MenuButtonItem item)
		{
			string text = Localizer.Localize(item.Text);
			if (text == null)
			{
				item.Visible = false;
				return;
			}
			item.Text = text;
			foreach (MenuButtonItem item2 in item.Items)
			{
				Localizer.Localize(item2);
			}
		}
		public static void Localize(MenuBarItem item)
		{
			string text = Localizer.Localize(item.Text);
			if (text == null)
			{
				item.Visible = false;
				return;
			}
			item.Text = text;
			foreach (MenuButtonItem item2 in item.Items)
			{
				Localizer.Localize(item2);
			}
		}
		public static void Localize(MenuBar menuBar)
		{
			foreach (MenuBarItem item in menuBar.Items)
			{
				Localizer.Localize(item);
			}
		}
	}
}
