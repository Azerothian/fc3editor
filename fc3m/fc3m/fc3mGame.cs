using System;
using System.Collections.Generic;
using System.Text;
using Nomad;

namespace fc3m
{
	public class fc3mGame : Nomad.Game
	{
		private frmMainGame _mainForm;

		public override Nomad.NomadForm MainForm
		{
			get { return _mainForm; }
		}

		public override Nomad.ViewportControl Viewport
		{
			get { return _mainForm.Viewport; }
		}


		public fc3mGame()
		{
			_mainForm = new frmMainGame(this);
			MainForm.Show();
			OnPostLoad += fc3mGame_OnPostLoad;
			Run(false, @"C:\Users\Azeroth\Documents\My Games\Far Cry 3\user maps\testmap.fc3map");
		}

		void fc3mGame_OnPostLoad(object sender, EventArgs e)
		{

		}
	}
}
