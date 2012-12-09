using System;
namespace FC3Editor.Nomad
{
	internal class Validation
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
