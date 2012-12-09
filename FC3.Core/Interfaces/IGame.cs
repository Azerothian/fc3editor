using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FC3.Core.Enums;

namespace FC3.Core.Interfaces
{
	public interface IGame
	{
		void OnMouseEvent(MouseEvent me, MouseEventArgs args);
		bool IsInGame { get; }
		void OnKeyEvent(KeyEvent ke, KeyEventArgs args);
	}
}
