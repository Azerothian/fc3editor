using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.Text;
using Nomad.Logic;

namespace Nomad.Utils
{
	public class EngineUtils
	{
		//FCXDash
		//FCXEditor

		public static void EnterIngame(string gameMode)
		{
			if (!Binding.FCE_Editor_ValidateIngame())
			{

				Log.error(Localizer.LocalizeCommon("MSG_DESC_INGAME_INVALID_OBJECTS"));

				//LocalizedMessageBox.Show(MainForm.Instance, , Localizer.Localize("WARNING"), Localizer.Localize("Generic", "GENERIC_OK"), null, null, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
			}

			//TODO: Need to fix this MainForm EnterIngame
			//MainForm.Instance.EnterIngame();
			Binding.FCE_Editor_EnterIngame(gameMode);
		}
		public static void ExitIngame()
		{
			Binding.FCE_Editor_ExitIngame();


			//TODO: Need to fix this MainForm ExitIngame
			//MainForm.Instance.ExitIngame();
		}
		
		public static bool HasArgument(string key)
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			for (int i = 0; i < commandLineArgs.Length; i++)
			{
				string a = commandLineArgs[i];
				if (a == key)
				{
					return true;
				}
			}
			return false;
		}
		public static string GetMapArgument()
		{
			string[] commandLineArgs = Environment.GetCommandLineArgs();
			if (commandLineArgs.Length >= 2 && !commandLineArgs[1].StartsWith("-") && commandLineArgs[1].EndsWith(".fc3map"))
			{
				return commandLineArgs[1];
			}
			return null;
		}
	}
}
