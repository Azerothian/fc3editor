using FC3Editor.Nomad;
using FC3Editor.Tools;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class ToolParametersDock : UserDockableWindow
	{
		private IContainer components;
		private Panel panel1;
		private Label toolName;
		private PictureBox toolImage;
		private GroupBox groupBox;
		private ParametersList toolParametersList;
		private Label label1;
		private ITool m_tool;
		public ITool Tool
		{
			get
			{
				return this.m_tool;
			}
			set
			{
				this.m_tool = value;
				bool visible = this.m_tool != null;
				this.groupBox.Visible = visible;
				this.toolName.Visible = visible;
				this.toolImage.Visible = visible;
				this.toolParametersList.Visible = visible;
				this.toolParametersList.Parameters = value;
				if (this.m_tool != null)
				{
					this.toolName.Text = StringUtils.EscapeUIString(this.m_tool.GetToolName());
					this.toolImage.Image = this.m_tool.GetToolImage();
				}
			}
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
			this.panel1 = new Panel();
			this.toolName = new Label();
			this.toolImage = new PictureBox();
			this.groupBox = new GroupBox();
			this.toolParametersList = new ParametersList();
			this.label1 = new Label();
			this.panel1.SuspendLayout();
			((ISupportInitialize)this.toolImage).BeginInit();
			this.groupBox.SuspendLayout();
			base.SuspendLayout();
			this.panel1.Controls.Add(this.toolName);
			this.panel1.Controls.Add(this.toolImage);
			this.panel1.Dock = DockStyle.Top;
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(250, 45);
			this.panel1.TabIndex = 1;
			this.toolName.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.toolName.Font = new Font("Microsoft Sans Serif", 12f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.toolName.Location = new Point(46, 3);
			this.toolName.Name = "toolName";
			this.toolName.Size = new Size(202, 41);
			this.toolName.TabIndex = 1;
			this.toolName.TextAlign = ContentAlignment.MiddleLeft;
			this.toolImage.Location = new Point(6, 6);
			this.toolImage.Name = "toolImage";
			this.toolImage.Size = new Size(32, 32);
			this.toolImage.TabIndex = 0;
			this.toolImage.TabStop = false;
			this.groupBox.Controls.Add(this.toolParametersList);
			this.groupBox.Dock = DockStyle.Fill;
			this.groupBox.Location = new Point(0, 45);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new Size(250, 355);
			this.groupBox.TabIndex = 2;
			this.groupBox.TabStop = false;
			this.groupBox.Text = "EDITOR_PARAMETERS";
			this.toolParametersList.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.toolParametersList.AutoScroll = true;
			this.toolParametersList.Location = new Point(6, 19);
			this.toolParametersList.Name = "toolParametersList";
			this.toolParametersList.Parameters = null;
			this.toolParametersList.Size = new Size(238, 330);
			this.toolParametersList.TabIndex = 0;
			this.label1.Dock = DockStyle.Fill;
			this.label1.Location = new Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(250, 400);
			this.label1.TabIndex = 1;
			this.label1.Text = "TOOL_NOT_SELECTED";
			this.label1.TextAlign = ContentAlignment.MiddleCenter;
			base.Controls.Add(this.groupBox);
			base.Controls.Add(this.panel1);
			base.Controls.Add(this.label1);
			base.Name = "ToolParametersDock";
			base.ShowOptions = false;
			this.Text = "DOCK_TOOL_PARAMETERS";
			this.panel1.ResumeLayout(false);
			((ISupportInitialize)this.toolImage).EndInit();
			this.groupBox.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public ToolParametersDock()
		{
			this.InitializeComponent();
			this.Tool = null;
			this.Text = Localizer.Localize(this.Text);
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.groupBox.Text = Localizer.Localize(this.groupBox.Text);
		}
	}
}
