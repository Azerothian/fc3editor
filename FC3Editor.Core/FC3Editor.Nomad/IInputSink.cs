using System;
using System.Windows.Forms;
namespace FC3Editor.Nomad
{
	internal interface IInputSink
	{
		void OnInputAcquire();
		void OnInputRelease();
		bool OnMouseEvent(Editor.MouseEvent mouseEvent, MouseEventArgs mouseEventArgs);
		bool OnKeyEvent(Editor.KeyEvent keyEvent, KeyEventArgs keyEventArgs);
		void OnEditorEvent(uint eventType, IntPtr eventPtr);
		void Update(float dt);
	}
}
