using System;
using Nomad.Enums;
namespace Nomad.Logic
{
	public class Validation
	{
		public static ValidationReport ValidateGameMode(GameModes gameMode)
		{
			return new ValidationReport(Binding.FCE_Validation_GameMode((int)gameMode));
		}
		public static ValidationReport ValidateGame()
		{
			return new ValidationReport(Binding.FCE_Validation_Game());
		}
	}
}
