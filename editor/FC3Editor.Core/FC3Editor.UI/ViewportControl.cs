using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ViewportControl : UserControl
	{
		private enum CameraModes
		{
			None,
			Lookaround,
			Panning
		}
		private const float kSpeedBoost = 5f;
		private bool m_blockNextKeyRepeats;
		private Vec2 m_normalizedMousePos;
		private bool m_captureMouse;
		private Point m_captureMousePos;
		private bool m_captureWheel;
		private Cursor m_invisibleCursor;
		private ViewportControl.CameraModes m_cameraMode;
		private bool m_cameraEnabled = true;
		private bool m_mouseOver;
		private Cursor m_defaultCursor = Cursors.Default;
		private IContainer components;
		public bool BlockNextKeyRepeats
		{
			get
			{
				return this.m_blockNextKeyRepeats;
			}
			set
			{
				this.m_blockNextKeyRepeats = value;
			}
		}
		public Vec2 NormalizedMousePos
		{
			get
			{
				return this.m_normalizedMousePos;
			}
			set
			{
				Cursor.Position = base.PointToScreen(new Point((int)(value.X * (float)base.ClientSize.Width), (int)(value.Y * (float)base.ClientSize.Height)));
			}
		}
		public bool CaptureMouse
		{
			get
			{
				return this.m_captureMouse;
			}
			set
			{
				if (this.m_captureMouse == value)
				{
					return;
				}
				this.m_captureMouse = value;
				this.UpdateCaptureMouse();
			}
		}
		public Vec2 CaptureMousePos
		{
			set
			{
				this.m_captureMousePos = base.PointToScreen(new Point((int)(value.X * (float)base.ClientSize.Width), (int)(value.Y * (float)base.ClientSize.Height)));
			}
		}
		public bool CaptureWheel
		{
			get
			{
				return this.m_captureWheel;
			}
			set
			{
				this.m_captureWheel = value;
			}
		}
		private ViewportControl.CameraModes CameraMode
		{
			get
			{
				return this.m_cameraMode;
			}
			set
			{
				this.m_cameraMode = value;
				this.UpdateCameraMode();
			}
		}
		public bool CameraEnabled
		{
			get
			{
				return this.m_cameraEnabled;
			}
			set
			{
				this.m_cameraEnabled = value;
			}
		}
		public bool MouseOver
		{
			get
			{
				return this.m_mouseOver;
			}
		}
		public new Cursor DefaultCursor
		{
			get
			{
				return this.m_defaultCursor;
			}
			set
			{
				if (this.Cursor == this.m_defaultCursor)
				{
					this.Cursor = value;
				}
				this.m_defaultCursor = value;
			}
		}
		public ViewportControl()
		{
			this.InitializeComponent();
			this.BackColor = SystemColors.AppWorkspace;
			base.MouseWheel += new MouseEventHandler(this.ViewportControl_MouseWheel);
			this.m_invisibleCursor = new Cursor(new MemoryStream(Resources.invisible_cursor));
		}
		protected override bool IsInputKey(Keys keyData)
		{
			return true;
		}
		public override bool PreProcessMessage(ref Message msg)
		{
			bool flag = msg.Msg == 256 || msg.Msg == 260;
			bool flag2 = (msg.LParam.ToInt32() & 1073741824) != 0;
			if (flag)
			{
				if (!flag2)
				{
					this.BlockNextKeyRepeats = false;
				}
				else
				{
					if (this.BlockNextKeyRepeats)
					{
						return true;
					}
				}
			}
			return base.PreProcessMessage(ref msg);
		}
		protected override bool ProcessKeyMessage(ref Message msg)
		{
			bool flag = msg.Msg == 256 || msg.Msg == 260;
			bool flag2 = msg.Msg == 257 || msg.Msg == 261;
			bool flag3 = (msg.LParam.ToInt32() & 1073741824) != 0;
			Keys keyData = (Keys)(msg.WParam.ToInt32() | (int)Control.ModifierKeys);
			KeyEventArgs keyEventArgs = new KeyEventArgs(keyData);
			if (!Editor.IsIngame)
			{
				this.UpdateCameraState();
			}
			if (!Engine.ConsoleOpened)
			{
				if (flag)
				{
					if (!flag3)
					{
						Editor.OnKeyEvent(Editor.KeyEvent.KeyDown, keyEventArgs);
					}
					Editor.OnKeyEvent(Editor.KeyEvent.KeyChar, keyEventArgs);
				}
				else
				{
					if (flag2)
					{
						Editor.OnKeyEvent(Editor.KeyEvent.KeyUp, keyEventArgs);
					}
				}
			}
			return base.ProcessKeyMessage(ref msg);
		}
		private void ViewportControl_MouseDown(object sender, MouseEventArgs e)
		{
			if (this.CameraMode == ViewportControl.CameraModes.None)
			{
				MouseButtons button = e.Button;
				if (button == MouseButtons.Left)
				{
					Editor.OnMouseEvent(Editor.MouseEvent.MouseDown, e);
					return;
				}
				if (button != MouseButtons.Right)
				{
					if (button != MouseButtons.Middle)
					{
						return;
					}
					if (!Editor.IsIngame && this.CameraEnabled)
					{
						this.CameraMode = ViewportControl.CameraModes.Panning;
						return;
					}
				}
				else
				{
					if (!Editor.IsIngame && this.CameraEnabled)
					{
						this.CameraMode = ViewportControl.CameraModes.Lookaround;
					}
				}
			}
		}
		private void ViewportControl_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.CameraMode == ViewportControl.CameraModes.None)
			{
				if (e.Button == MouseButtons.Left)
				{
					Editor.OnMouseEvent(Editor.MouseEvent.MouseUp, e);
					return;
				}
			}
			else
			{
				if (e.Button == MouseButtons.Middle || e.Button == MouseButtons.Right)
				{
					this.CameraMode = ViewportControl.CameraModes.None;
				}
			}
		}
		private void Viewport_MouseMove(object sender, MouseEventArgs e)
		{
			if (this.CaptureMouse)
			{
				if (MainForm.IsActive)
				{
					Point position = base.PointToScreen(new Point(base.Width / 2, base.Height / 2));
					int num = Cursor.Position.X - position.X;
					int num2 = Cursor.Position.Y - position.Y;
					if (num != 0 || num2 != 0)
					{
						switch (this.CameraMode)
						{
						case ViewportControl.CameraModes.Lookaround:
							Camera.Rotate((float)(EditorSettings.InvertMouseView ? num2 : (-(float)num2)) * 0.005f, 0f, (float)(-(float)num) * 0.005f);
							break;

						case ViewportControl.CameraModes.Panning:
							Camera.Position += Camera.RightVector * (float)num * 0.125f + Camera.UpVector * (float)(EditorSettings.InvertMousePan ? num2 : (-(float)num2)) * 0.125f;
							break;

						default:
							Editor.OnMouseEvent(Editor.MouseEvent.MouseMoveDelta, new MouseEventArgs(e.Button, e.Clicks, num, num2, e.Delta));
							break;
						}
						Cursor.Position = position;
						return;
					}
				}
			}
			else
			{
				this.m_normalizedMousePos = new Vec2((float)e.X / (float)base.ClientSize.Width, (float)e.Y / (float)base.ClientSize.Height);
				ObjectManager.SetViewportPickingPos(this.m_normalizedMousePos);
				Editor.OnMouseEvent(Editor.MouseEvent.MouseMove, e);
			}
		}
		private void ViewportControl_MouseEnter(object sender, EventArgs e)
		{
			if (MainForm.IsActive)
			{
				base.Focus();
			}
			this.m_mouseOver = true;
			Editor.OnMouseEvent(Editor.MouseEvent.MouseEnter, null);
		}
		private void ViewportControl_Paint(object sender, PaintEventArgs e)
		{
		}
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
		}
		public void UpdateFocus()
		{
			if (MainForm.IsActive)
			{
				if (this.CaptureMouse)
				{
					Point position = base.PointToScreen(new Point(base.Width / 2, base.Height / 2));
					Cursor.Position = position;
					this.Cursor = this.m_invisibleCursor;
					return;
				}
			}
			else
			{
				if (this.CaptureMouse)
				{
					this.Cursor = this.m_defaultCursor;
				}
			}
		}
		private void ViewportControl_MouseLeave(object sender, EventArgs e)
		{
			if (this.CameraMode != ViewportControl.CameraModes.None)
			{
				this.CameraMode = ViewportControl.CameraModes.None;
			}
			this.m_mouseOver = false;
			Editor.OnMouseEvent(Editor.MouseEvent.MouseLeave, null);
		}
		private void ViewportControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (!this.m_captureWheel)
			{
				if (!Editor.IsIngame)
				{
					Camera.Position += Camera.FrontVector * (float)e.Delta * 0.0625f;
					return;
				}
			}
			else
			{
				Editor.OnMouseEvent(Editor.MouseEvent.MouseWheel, e);
			}
		}
		private void Viewport_Leave(object sender, EventArgs e)
		{
			if (this.CameraMode != ViewportControl.CameraModes.None)
			{
				this.CameraMode = ViewportControl.CameraModes.None;
			}
			this.ResetCameraState();
		}
		public void UpdateSize()
		{
			if (base.ParentForm == null || base.ParentForm.WindowState == FormWindowState.Minimized)
			{
				return;
			}
			Size clientSize = base.ClientSize;
			if (clientSize.Width < 16)
			{
				clientSize.Width = 16;
			}
			if (clientSize.Height < 16)
			{
				clientSize.Height = 16;
			}
			clientSize.Width = (int)((float)clientSize.Width * EditorSettings.ViewportQuality);
			clientSize.Height = (int)((float)clientSize.Height * EditorSettings.ViewportQuality);
			Engine.UpdateResolution(clientSize);
		}
		private void ViewportControl_Resize(object sender, EventArgs e)
		{
			this.UpdateSize();
		}
		private void UpdateCaptureMouse()
		{
			if (this.CaptureMouse)
			{
				this.Cursor = this.m_invisibleCursor;
				this.m_captureMousePos = Cursor.Position;
				Cursor.Position = base.PointToScreen(new Point(base.Width / 2, base.Height / 2));
				return;
			}
			Cursor.Position = this.m_captureMousePos;
			this.Cursor = this.m_defaultCursor;
		}
		private void ResetCameraState()
		{
			Camera.ForwardInput = 0f;
			Camera.LateralInput = 0f;
			Camera.SpeedFactor = 1f;
		}
		private void UpdateCameraState()
		{
			if (!Engine.Initialized)
			{
				return;
			}
			if (Engine.ConsoleOpened || !this.Focused)
			{
				this.ResetCameraState();
				return;
			}
			IntPtr keyboardLayout = Win32.GetKeyboardLayout(0);
			int nVirtKey = Win32.MapVirtualKeyEx(17, 1, keyboardLayout);
			int nVirtKey2 = Win32.MapVirtualKeyEx(31, 1, keyboardLayout);
			int nVirtKey3 = Win32.MapVirtualKeyEx(30, 1, keyboardLayout);
			int nVirtKey4 = Win32.MapVirtualKeyEx(32, 1, keyboardLayout);
			if (Win32.IsKeyDown(nVirtKey))
			{
				Camera.ForwardInput = 1f;
			}
			else
			{
				if (Win32.IsKeyDown(nVirtKey2))
				{
					Camera.ForwardInput = -1f;
				}
				else
				{
					Camera.ForwardInput = 0f;
				}
			}
			if (Win32.IsKeyDown(nVirtKey3))
			{
				Camera.LateralInput = -1f;
			}
			else
			{
				if (Win32.IsKeyDown(nVirtKey4))
				{
					Camera.LateralInput = 1f;
				}
				else
				{
					Camera.LateralInput = 0f;
				}
			}
			if (Win32.IsKeyDown(160) || Win32.IsKeyDown(161))
			{
				Camera.SpeedFactor = 5f;
				return;
			}
			Camera.SpeedFactor = 1f;
		}
		private void UpdateCameraMode()
		{
			if (this.CameraMode != ViewportControl.CameraModes.None)
			{
				this.CaptureMouse = true;
				this.UpdateCameraState();
				return;
			}
			this.CaptureMouse = false;
			Camera.ForwardInput = 0f;
			Camera.LateralInput = 0f;
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Name = "ViewportControl";
			base.MouseDown += new MouseEventHandler(this.ViewportControl_MouseDown);
			base.MouseMove += new MouseEventHandler(this.Viewport_MouseMove);
			base.Leave += new EventHandler(this.Viewport_Leave);
			base.Resize += new EventHandler(this.ViewportControl_Resize);
			base.MouseEnter += new EventHandler(this.ViewportControl_MouseEnter);
			base.Paint += new PaintEventHandler(this.ViewportControl_Paint);
			base.MouseLeave += new EventHandler(this.ViewportControl_MouseLeave);
			base.MouseUp += new MouseEventHandler(this.ViewportControl_MouseUp);
			base.ResumeLayout(false);
		}
	}
}
