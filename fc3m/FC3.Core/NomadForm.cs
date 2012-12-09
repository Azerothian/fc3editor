using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using Nomad.Enums;
using Nomad.Interfaces;

namespace Nomad
{
	public abstract class NomadForm : Form, IInputSink
	{
		public virtual void OnInputAcquire()
		{
		}

		public virtual void OnInputRelease()
		{
		}

		public virtual bool OnMouseEvent(MouseEvent mouseEvent, MouseEventArgs mouseEventArgs)
		{

			return false;
		}

		public virtual bool OnKeyEvent(KeyEvent keyEvent, KeyEventArgs keyEventArgs)
		{
			return false;
		}

		public void Update(float dt)
		{

		}

		public abstract ViewportControl Viewport { get; }

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// NomadForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "NomadForm";
			this.ResumeLayout(false);

		}

	}
}
