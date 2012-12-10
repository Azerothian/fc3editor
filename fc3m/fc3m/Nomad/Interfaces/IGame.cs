using System;
using System.Collections.Generic;

using System.Text;
using System.Windows.Forms;
using Nomad.Enums;

namespace Nomad.Interfaces
{
	public interface IGame
	{
		void OnMouseEvent(MouseEvent me, MouseEventArgs args);
		bool IsInGame { get; }
		void OnKeyEvent(KeyEvent ke, KeyEventArgs args);
	}
}
