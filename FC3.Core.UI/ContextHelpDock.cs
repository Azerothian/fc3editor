using FC3Editor.Nomad;
using FC3Editor.Properties;
using FC3Editor.Tools;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class ContextHelpDock : UserDockableWindow
	{
		private string m_contextHelp;
		private ITool m_tool;
		private EventHandler m_contextHelpDynamicHandler;
		private IContainer components;
		private Label contextHelpText;
		public string ContextHelp
		{
			get
			{
				return this.m_contextHelp;
			}
			set
			{
				if (value == null)
				{
					value = "No context help defined.";
				}
				this.m_contextHelp = value;
				this.UpdateLayout();
			}
		}
		public ITool Tool
		{
			get
			{
				return this.m_tool;
			}
			set
			{
				if (this.m_tool is IContextHelpDynamic)
				{
					((IContextHelpDynamic)this.m_tool).ContextHelpChanged -= this.m_contextHelpDynamicHandler;
				}
				this.m_tool = value;
				if (this.m_tool is IContextHelpDynamic)
				{
					((IContextHelpDynamic)this.m_tool).ContextHelpChanged += this.m_contextHelpDynamicHandler;
				}
				this.UpdateContextHelp();
			}
		}
		public ContextHelpDock()
		{
			this.InitializeComponent();
			this.Text = Localizer.Localize(this.Text);
			this.m_contextHelpDynamicHandler = new EventHandler(this._ContextHelpChanged);
		}
		private void UpdateTextSize()
		{
			Size clientSize = base.ClientSize;
			clientSize.Width -= SystemInformation.VerticalScrollBarWidth;
			Size preferredSize = this.contextHelpText.GetPreferredSize(clientSize);
			this.contextHelpText.Size = preferredSize;
		}
		private Size CreateText(int x, int y, int lineHeight, string text)
		{
			Label label = new Label();
			label.BackColor = SystemColors.Info;
			label.ForeColor = SystemColors.InfoText;
			label.Location = new Point(x, y);
			label.Text = text;
			Size clientSize = base.ClientSize;
			clientSize.Width = clientSize.Width - SystemInformation.VerticalScrollBarWidth - x;
			Size preferredSize = label.GetPreferredSize(clientSize);
			label.Size = preferredSize;
			if (lineHeight > 0)
			{
				label.Location = new Point(x, y + (lineHeight - label.Size.Height) / 2);
			}
			base.Controls.Add(label);
			return label.Size;
		}
		private Size CreateImage(int x, int y, int lineHeight, string image)
		{
			Bitmap bitmap = Resources.ResourceManager.GetObject(image) as Bitmap;
			if (bitmap == null)
			{
				return this.CreateText(x, y, lineHeight, "{" + image + "}");
			}
			PictureBox pictureBox = new PictureBox();
			pictureBox.Location = new Point(x, y);
			pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
			pictureBox.Image = bitmap;
			pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
			pictureBox.Size = new Size((int)((float)pictureBox.Size.Width * 0.75f), (int)((float)pictureBox.Size.Height * 0.75f));
			base.Controls.Add(pictureBox);
			return pictureBox.Size;
		}
		private void UpdateLayout()
		{
			Win32.SetRedraw(this, false);
			base.SuspendLayout();
			base.Controls.Clear();
			int num = 0;
			int num2 = 0;
			int i = 0;
			bool flag = true;
			int num3 = 0;
			if (this.m_contextHelp != null)
			{
				while (i < this.m_contextHelp.Length)
				{
					if (flag)
					{
						int num4;
						if (num3 == 0)
						{
							num4 = this.m_contextHelp.IndexOf('{', i);
						}
						else
						{
							num4 = this.m_contextHelp.IndexOfAny(new char[]
							{
								'{',
								'\n'
							}, i);
						}
						char c;
						if (num4 == -1)
						{
							num4 = this.m_contextHelp.Length;
							c = '\0';
						}
						else
						{
							c = this.m_contextHelp[num4];
						}
						string text = this.m_contextHelp.Substring(i, num4 - i);
						if (text.Length > 0)
						{
							Size size = this.CreateText(num, num2, num3, text);
							if (num3 == 0 || c == '\n')
							{
								num = 0;
								num2 += ((num3 == 0) ? size.Height : num3) + 3;
								num3 = 0;
							}
							else
							{
								num += size.Width;
							}
						}
						i = num4 + 1;
						flag = (c != '{');
					}
					else
					{
						int num5 = this.m_contextHelp.IndexOf('}', i);
						if (num5 == -1)
						{
							break;
						}
						string image = this.m_contextHelp.Substring(i, num5 - i);
						Size size2 = this.CreateImage(num, num2, num3, image);
						num += size2.Width;
						num3 = size2.Height + 3;
						i = num5 + 1;
						flag = true;
					}
				}
			}
			base.AutoScrollPosition = default(Point);
			base.ResumeLayout(true);
			Win32.SetRedraw(this, true);
			this.Refresh();
		}
		private void contextHelpText_SizeChanged(object sender, EventArgs e)
		{
		}
		private void ContextHelpDock_SizeChanged(object sender, EventArgs e)
		{
			this.UpdateLayout();
		}
		private void UpdateContextHelp()
		{
			if (this.Tool == null)
			{
				this.ContextHelp = Localizer.Localize("HELP_WELCOME");
				return;
			}
			this.ContextHelp = this.Tool.GetContextHelp();
		}
		private void _ContextHelpChanged(object sender, EventArgs e)
		{
			this.UpdateContextHelp();
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
			this.contextHelpText = new Label();
			base.SuspendLayout();
			this.contextHelpText.BackColor = SystemColors.Info;
			this.contextHelpText.ForeColor = SystemColors.InfoText;
			this.contextHelpText.Location = new Point(0, 0);
			this.contextHelpText.Name = "contextHelpText";
			this.contextHelpText.Padding = new Padding(3);
			this.contextHelpText.Size = new Size(165, 16);
			this.contextHelpText.TabIndex = 0;
			this.contextHelpText.Text = "Placeholder help text.";
			this.AutoScroll = true;
			this.BackColor = SystemColors.Info;
			base.Controls.Add(this.contextHelpText);
			base.Name = "ContextHelpDock";
			base.Padding = new Padding(2);
			base.ShowOptions = false;
			base.Size = new Size(167, 146);
			this.Text = "DOCK_CONTEXT_HELP";
			base.SizeChanged += new EventHandler(this.ContextHelpDock_SizeChanged);
			base.ResumeLayout(false);
		}
	}
}
