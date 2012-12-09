using FC3Editor.Nomad;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TD.SandDock;
namespace FC3Editor.UI
{
	internal class CodeMapViewerDock : UserDockableWindow
	{
		private ImageMap m_imageMap;
		private IContainer components;
		private PictureBox pictureBox1;
		private Label statsLabel;
		public ImageMap Image
		{
			get
			{
				return this.m_imageMap;
			}
			set
			{
				this.m_imageMap = value;
				this.UpdateUI();
			}
		}
		public CodeMapViewerDock()
		{
			this.InitializeComponent();
		}
		private void UpdateUI()
		{
			if (this.m_imageMap == null)
			{
				this.pictureBox1.Image = null;
				this.statsLabel.Text = "";
				return;
			}
			this.pictureBox1.Image = this.m_imageMap.Image;
			this.statsLabel.Text = "Minimum: " + this.m_imageMap.Minimum.ToString("F3") + "  Maximum: " + this.m_imageMap.Maximum.ToString("F3");
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
			this.pictureBox1 = new PictureBox();
			this.statsLabel = new Label();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.pictureBox1.Location = new Point(0, 0);
			this.pictureBox1.Margin = new Padding(0);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(250, 232);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.statsLabel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.statsLabel.Location = new Point(0, 232);
			this.statsLabel.Margin = new Padding(0);
			this.statsLabel.Name = "statsLabel";
			this.statsLabel.Size = new Size(250, 23);
			this.statsLabel.TabIndex = 1;
			this.statsLabel.Text = "Minimum: 0  Maximum: 255";
			this.statsLabel.TextAlign = ContentAlignment.MiddleCenter;
			base.Controls.Add(this.statsLabel);
			base.Controls.Add(this.pictureBox1);
			base.Name = "CodeMapViewerDock";
			base.ShowOptions = false;
			base.Size = new Size(250, 255);
			this.Text = "Map Viewer";
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
