using FC3Editor.Nomad;
using FC3Editor.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamPickButton : Parameter
	{
		public delegate void PickDelegate(Vec2 normalizedMousePos);
		public delegate void StdDelegate();
		private Control m_captureControl;
		private bool m_keepCapture;
		private bool m_enabled = true;
		private ParamPickButton.PickDelegate m_pickCallback;
		private ParamPickButton.PickDelegate m_updateCallback;
		private ParamPickButton.StdDelegate m_cancelCallback;
		public bool KeepCapture
		{
			get
			{
				return this.m_keepCapture;
			}
			set
			{
				this.m_keepCapture = value;
			}
		}
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
				base.UpdateUIControls();
			}
		}
		public ParamPickButton.PickDelegate PickCallback
		{
			get
			{
				return this.m_pickCallback;
			}
			set
			{
				this.m_pickCallback = value;
			}
		}
		public ParamPickButton.PickDelegate UpdateCallback
		{
			get
			{
				return this.m_updateCallback;
			}
			set
			{
				this.m_updateCallback = value;
			}
		}
		public ParamPickButton.StdDelegate CancelCallback
		{
			get
			{
				return this.m_cancelCallback;
			}
			set
			{
				this.m_cancelCallback = value;
			}
		}
		public ParamPickButton(string display) : base(display)
		{
		}
		protected override Control CreateUIControl()
		{
			NomadCheckButton nomadCheckButton = new NomadCheckButton();
			nomadCheckButton.AutoCheck = false;
			nomadCheckButton.Text = base.DisplayName;
			nomadCheckButton.Click += new EventHandler(this.button_Click);
			return nomadCheckButton;
		}
		protected override void UpdateUIControl(Control control)
		{
			NomadCheckButton nomadCheckButton = (NomadCheckButton)control;
			nomadCheckButton.Enabled = this.m_enabled;
			nomadCheckButton.Checked = (this.m_captureControl != null && this.m_captureControl.Capture);
		}
		private void button_Click(object sender, EventArgs e)
		{
			this.InitCapture();
		}
		public void InitCapture()
		{
			this.m_captureControl = new Control();
			this.m_captureControl.Capture = true;
			this.m_captureControl.MouseCaptureChanged += new EventHandler(this.captureControl_MouseCaptureChanged);
			this.m_captureControl.MouseMove += new MouseEventHandler(this.captureControl_MouseMove);
			this.m_captureControl.MouseUp += new MouseEventHandler(this.captureControl_MouseUp);
			this.m_captureControl.KeyUp += new KeyEventHandler(this.captureControl_KeyUp);
			this.m_captureControl.Parent = MainForm.Instance;
			this.m_captureControl.Focus();
			Cursor.Current = Cursors.Cross;
			base.UpdateUIControls();
		}
		public void FinalizeCapture()
		{
			if (this.m_cancelCallback != null)
			{
				this.m_cancelCallback();
			}
			base.UpdateUIControls();
			Cursor.Current = Cursors.Default;
			this.m_captureControl.Dispose();
		}
		private bool GetCapturePos(out Vec2 normalizedMousePos)
		{
			Point position = Cursor.Position;
			Point pt = Editor.Viewport.PointToClient(position);
			Rectangle clientRectangle = Editor.Viewport.ClientRectangle;
			if (clientRectangle.Contains(pt))
			{
				normalizedMousePos = new Vec2((float)pt.X / (float)clientRectangle.Width, (float)pt.Y / (float)clientRectangle.Height);
				return true;
			}
			normalizedMousePos = default(Vec2);
			return false;
		}
		private void captureControl_MouseMove(object sender, MouseEventArgs e)
		{
			Vec2 normalizedMousePos;
			if (!this.GetCapturePos(out normalizedMousePos))
			{
				return;
			}
			if (this.m_updateCallback != null)
			{
				this.m_updateCallback(normalizedMousePos);
			}
		}
		private void captureControl_MouseUp(object sender, MouseEventArgs e)
		{
			Vec2 normalizedMousePos;
			if (!this.GetCapturePos(out normalizedMousePos))
			{
				return;
			}
			if (this.m_pickCallback != null)
			{
				this.m_pickCallback(normalizedMousePos);
			}
		}
		private void captureControl_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.KeepCapture = false;
				this.FinalizeCapture();
			}
		}
		private void captureControl_MouseCaptureChanged(object sender, EventArgs e)
		{
			if (this.KeepCapture)
			{
				this.KeepCapture = false;
				this.m_captureControl.Capture = true;
				return;
			}
			this.FinalizeCapture();
		}
	}
}
