using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class LocalizedMessageBox : Form
	{
		private Button m_buttonCancel;
		private Button m_buttonDecline;
		private Button m_buttonAccept;
		private Label m_labelMessage;
		private ImageList m_imageList;
		private IContainer components;
		private FlowLayoutPanel m_buttonsLayout;
		private Label m_labelIcon;
		protected LocalizedMessageBox()
		{
			base.Icon = Resources.appIcon;
			this.InitializeComponent();
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
		}
		public static DialogResult Show(string message, string title, string acceptText, string declineText = null, string cancelText = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
		{
			LocalizedMessageBox localizedMessageBox = LocalizedMessageBox.CreateMessageBox(message, title, acceptText, declineText, cancelText, icon, defaultButton);
			return localizedMessageBox.ShowDialog();
		}
		public static DialogResult Show(IWin32Window owner, string message, string title, string acceptText, string declineText = null, string cancelText = null, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
		{
			LocalizedMessageBox localizedMessageBox = LocalizedMessageBox.CreateMessageBox(message, title, acceptText, declineText, cancelText, icon, defaultButton);
			return localizedMessageBox.ShowDialog(owner);
		}
		private static LocalizedMessageBox CreateMessageBox(string message, string title, string acceptText, string declineText, string cancelText, MessageBoxIcon icon, MessageBoxDefaultButton defaultButton)
		{
			LocalizedMessageBox localizedMessageBox = new LocalizedMessageBox();
			localizedMessageBox.Text = title;
			localizedMessageBox.m_labelMessage.Text = message;
			localizedMessageBox.m_buttonAccept.Visible = (acceptText != null);
			localizedMessageBox.m_buttonAccept.Text = acceptText;
			localizedMessageBox.m_buttonDecline.Visible = (declineText != null);
			localizedMessageBox.m_buttonDecline.Text = declineText;
			localizedMessageBox.m_buttonCancel.Visible = (cancelText != null);
			localizedMessageBox.m_buttonCancel.Text = cancelText;
			localizedMessageBox.AcceptButton = localizedMessageBox.m_buttonAccept;
			if (icon <= MessageBoxIcon.Hand)
			{
				if (icon != MessageBoxIcon.None)
				{
					if (icon == MessageBoxIcon.Hand)
					{
						localizedMessageBox.m_labelIcon.Image = SystemIcons.Error.ToBitmap();
					}
				}
				else
				{
					localizedMessageBox.m_labelIcon.Image = null;
				}
			}
			else
			{
				if (icon != MessageBoxIcon.Question)
				{
					if (icon != MessageBoxIcon.Exclamation)
					{
						if (icon == MessageBoxIcon.Asterisk)
						{
							localizedMessageBox.m_labelIcon.Image = SystemIcons.Information.ToBitmap();
						}
					}
					else
					{
						localizedMessageBox.m_labelIcon.Image = SystemIcons.Warning.ToBitmap();
					}
				}
				else
				{
					localizedMessageBox.m_labelIcon.Image = SystemIcons.Question.ToBitmap();
				}
			}
			if (defaultButton != MessageBoxDefaultButton.Button1)
			{
				if (defaultButton != MessageBoxDefaultButton.Button2)
				{
					if (defaultButton == MessageBoxDefaultButton.Button3)
					{
						localizedMessageBox.m_buttonAccept.TabIndex = 1;
						localizedMessageBox.m_buttonDecline.TabIndex = 2;
						localizedMessageBox.m_buttonCancel.TabIndex = 0;
					}
				}
				else
				{
					localizedMessageBox.m_buttonAccept.TabIndex = 2;
					localizedMessageBox.m_buttonDecline.TabIndex = 0;
					localizedMessageBox.m_buttonCancel.TabIndex = 1;
				}
			}
			localizedMessageBox.StartPosition = FormStartPosition.CenterParent;
			return localizedMessageBox;
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.m_buttonsLayout = new FlowLayoutPanel();
			this.m_buttonCancel = new Button();
			this.m_buttonDecline = new Button();
			this.m_buttonAccept = new Button();
			this.m_labelMessage = new Label();
			this.m_labelIcon = new Label();
			this.m_imageList = new ImageList(this.components);
			this.m_buttonsLayout.SuspendLayout();
			base.SuspendLayout();
			this.m_buttonsLayout.BackColor = SystemColors.Control;
			this.m_buttonsLayout.Controls.Add(this.m_buttonCancel);
			this.m_buttonsLayout.Controls.Add(this.m_buttonDecline);
			this.m_buttonsLayout.Controls.Add(this.m_buttonAccept);
			this.m_buttonsLayout.Dock = DockStyle.Bottom;
			this.m_buttonsLayout.FlowDirection = FlowDirection.RightToLeft;
			this.m_buttonsLayout.Location = new Point(0, 125);
			this.m_buttonsLayout.MinimumSize = new Size(310, 42);
			this.m_buttonsLayout.Name = "m_buttonsLayout";
			this.m_buttonsLayout.Padding = new Padding(3, 6, 3, 6);
			this.m_buttonsLayout.Size = new Size(488, 42);
			this.m_buttonsLayout.TabIndex = 0;
			this.m_buttonCancel.DialogResult = DialogResult.Cancel;
			this.m_buttonCancel.Location = new Point(382, 9);
			this.m_buttonCancel.Name = "m_buttonCancel";
			this.m_buttonCancel.Size = new Size(97, 25);
			this.m_buttonCancel.TabIndex = 2;
			this.m_buttonCancel.Text = "Cancel";
			this.m_buttonCancel.UseVisualStyleBackColor = true;
			this.m_buttonDecline.DialogResult = DialogResult.No;
			this.m_buttonDecline.Location = new Point(279, 9);
			this.m_buttonDecline.Name = "m_buttonDecline";
			this.m_buttonDecline.Size = new Size(97, 25);
			this.m_buttonDecline.TabIndex = 1;
			this.m_buttonDecline.Text = "No";
			this.m_buttonDecline.UseVisualStyleBackColor = true;
			this.m_buttonAccept.DialogResult = DialogResult.Yes;
			this.m_buttonAccept.Location = new Point(176, 9);
			this.m_buttonAccept.Name = "m_buttonAccept";
			this.m_buttonAccept.Size = new Size(97, 25);
			this.m_buttonAccept.TabIndex = 0;
			this.m_buttonAccept.Text = "Yes";
			this.m_buttonAccept.UseVisualStyleBackColor = true;
			this.m_labelMessage.Dock = DockStyle.Right;
			this.m_labelMessage.Location = new Point(83, 0);
			this.m_labelMessage.Name = "m_labelMessage";
			this.m_labelMessage.Padding = new Padding(0, 0, 10, 0);
			this.m_labelMessage.Size = new Size(405, 125);
			this.m_labelMessage.TabIndex = 4;
			this.m_labelMessage.Text = "Place message here";
			this.m_labelMessage.TextAlign = ContentAlignment.MiddleLeft;
			this.m_labelIcon.Dock = DockStyle.Left;
			this.m_labelIcon.ImageList = this.m_imageList;
			this.m_labelIcon.Location = new Point(0, 0);
			this.m_labelIcon.Name = "m_labelIcon";
			this.m_labelIcon.Size = new Size(77, 125);
			this.m_labelIcon.TabIndex = 3;
			this.m_labelIcon.TextAlign = ContentAlignment.MiddleCenter;
			this.m_imageList.ColorDepth = ColorDepth.Depth8Bit;
			this.m_imageList.ImageSize = new Size(16, 16);
			this.m_imageList.TransparentColor = Color.Transparent;
			base.AcceptButton = this.m_buttonAccept;
			this.AutoSize = true;
			this.BackColor = SystemColors.ControlLightLight;
			base.CancelButton = this.m_buttonCancel;
			base.ClientSize = new Size(488, 167);
			base.ControlBox = false;
			base.Controls.Add(this.m_labelIcon);
			base.Controls.Add(this.m_labelMessage);
			base.Controls.Add(this.m_buttonsLayout);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LocalizedMessageBox";
			base.ShowIcon = false;
			this.Text = "Message";
			base.TopMost = true;
			this.m_buttonsLayout.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
