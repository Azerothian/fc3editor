using System;
using System.Windows.Forms;
using Nomad.Enums;
namespace Nomad.Interfaces
{
	public interface IInputSink
	{
		void OnInputAcquire();
		void OnInputRelease();
		bool OnMouseEvent(MouseEvent mouseEvent, MouseEventArgs mouseEventArgs);
		bool OnKeyEvent(KeyEvent keyEvent, KeyEventArgs keyEventArgs);
		//TODO: Do we need this?
		//void OnEditorEvent(uint eventType, IntPtr eventPtr);
		void Update(float dt);
	}
}
