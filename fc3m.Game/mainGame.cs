using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FC3Editor.UI;

namespace fc3m.Game
{
	public partial class mainGame : Form, FC3.Core.Nomad.IInputSink
	{
		public mainGame()
		{
			InitializeComponent();
		}

		public void OnInputAcquire()
		{
		}

		public void OnInputRelease()
		{
		}

		public bool OnMouseEvent(FC3.Core.Nomad.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{

			return false;
		}

		public bool OnKeyEvent(FC3.Core.Nomad.KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}

		public void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}

		public void Update(float dt)
		{
			
		}

		public ViewportControl Viewport
		{
			get
			{
				return this.viewport;
			}
		}

	}
}
