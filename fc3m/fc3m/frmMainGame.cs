using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Nomad;
using Nomad.Enums;
using Nomad.Interfaces;
using Nomad.Utils;

namespace fc3m
{
	public partial class frmMainGame : NomadForm
	{
		private ViewportControl _viewPort;
		private Game _game;
		public frmMainGame(Game game)
		{
			_game = game;
			InitializeComponent();
			_viewPort = new ViewportControl(this, _game);
			_viewPort.Dock = DockStyle.Fill;
			this.Controls.Add(_viewPort);
			this.Load += frmMainGame_Load;
			
		}


		void frmMainGame_Load(object sender, EventArgs e)
		{


			
			//EngineUtils.EnterIngame("FCXEditor");

			//_viewPort.Height = Height;
			//_viewPort.Width = Width;
		}




		public override ViewportControl Viewport
		{
			get { return _viewPort; }
		}
	}
}
