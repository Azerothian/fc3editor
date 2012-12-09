using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FC3.Core;

namespace fc3m
{
	public class fc3mGame : FC3.Core.Game
	{
		private frmMainGame _mainForm;
		private ViewportControl _viewport;

		public override FC3.Core.NomadForm MainForm
		{
			get { return _mainForm; }
		}

		public override FC3.Core.ViewportControl Viewport
		{
			get { return _mainForm.Viewport; }
		}


		public fc3mGame()
		{
			_mainForm = new frmMainGame(this);
			OnPostLoad += fc3mGame_OnPostLoad;
			Run(false, "");
		}

		void fc3mGame_OnPostLoad(object sender, EventArgs e)
		{
			MainForm.Show();
		}
	}
}
