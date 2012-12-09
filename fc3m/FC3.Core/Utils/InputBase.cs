using System;
using System.Windows.Forms;
using Nomad.Enums;
using Nomad.Interfaces;
namespace Nomad.Utils
{
	internal abstract class InputBase : IInputSink
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
		public virtual void OnEditorEvent(uint eventType, IntPtr eventPtr)
		{
		}
		public virtual void Update(float dt)
		{
		}
		protected void AcquireInput()
		{

			// TODO:Editor Acquire Input?
			//Editor.PushInput(this);
			//MainForm.Instance.EnableShortcuts = false;
		}
		protected void ReleaseInput()
		{
			// TODO:Editor Release Input?
			//MainForm.Instance.EnableShortcuts = true;
			//Editor.PopInput(this);
		}
	}
}
