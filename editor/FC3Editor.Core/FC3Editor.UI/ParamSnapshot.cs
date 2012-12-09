using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamSnapshot : UserControl
	{
		private IContainer components;
		private Label label1;
		private PictureBox screenshotPicture;
		private TableLayoutPanel tableLayoutPanel1;
		private NomadButton buttonGotoCamera;
		private NomadButton buttonSetCamera;
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
			this.label1 = new Label();
			this.screenshotPicture = new PictureBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonGotoCamera = new NomadButton();
			this.buttonSetCamera = new NomadButton();
			((ISupportInitialize)this.screenshotPicture).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.Dock = DockStyle.Top;
			this.label1.Location = new Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(212, 15);
			this.label1.TabIndex = 0;
			this.label1.Text = "PARAM_SNAPSHOT";
			this.screenshotPicture.Dock = DockStyle.Fill;
			this.screenshotPicture.Location = new Point(0, 15);
			this.screenshotPicture.Name = "screenshotPicture";
			this.screenshotPicture.Size = new Size(212, 156);
			this.screenshotPicture.SizeMode = PictureBoxSizeMode.Zoom;
			this.screenshotPicture.TabIndex = 3;
			this.screenshotPicture.TabStop = false;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Controls.Add(this.buttonGotoCamera, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonSetCamera, 0, 0);
			this.tableLayoutPanel1.Dock = DockStyle.Bottom;
			this.tableLayoutPanel1.Location = new Point(0, 171);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Size = new Size(212, 29);
			this.tableLayoutPanel1.TabIndex = 5;
			this.buttonGotoCamera.Dock = DockStyle.Fill;
			this.buttonGotoCamera.Location = new Point(109, 3);
			this.buttonGotoCamera.Name = "buttonGotoCamera";
			this.buttonGotoCamera.Size = new Size(100, 23);
			this.buttonGotoCamera.TabIndex = 4;
			this.buttonGotoCamera.Text = "PARAM_SNAPSHOT_GOTO";
			this.buttonGotoCamera.UseVisualStyleBackColor = true;
			this.buttonGotoCamera.Click += new EventHandler(this.buttonGotoCamera_Click);
			this.buttonSetCamera.Dock = DockStyle.Fill;
			this.buttonSetCamera.Location = new Point(3, 3);
			this.buttonSetCamera.Name = "buttonSetCamera";
			this.buttonSetCamera.Size = new Size(100, 23);
			this.buttonSetCamera.TabIndex = 3;
			this.buttonSetCamera.Text = "PARAM_SNAPSHOT_SET";
			this.buttonSetCamera.UseVisualStyleBackColor = true;
			this.buttonSetCamera.Click += new EventHandler(this.buttonSetCamera_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.screenshotPicture);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.label1);
			base.Name = "ParamSnapshot";
			base.Size = new Size(212, 200);
			((ISupportInitialize)this.screenshotPicture).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
		public ParamSnapshot()
		{
			this.InitializeComponent();
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonSetCamera.Text = Localizer.Localize(this.buttonSetCamera.Text);
			this.buttonGotoCamera.Text = Localizer.Localize(this.buttonGotoCamera.Text);
			this.UpdateSnapshot();
		}
		private void UpdateSnapshot()
		{
			if (EditorDocument.IsSnapshotSet)
			{
				Snapshot snapshot = Snapshot.Create(320, 256);
				EditorDocument.TakeSnapshot(snapshot, 4);
				this.screenshotPicture.Image = snapshot.GetImage();
				this.buttonGotoCamera.Enabled = true;
				return;
			}
			this.screenshotPicture.Image = Resources.emptySnapshot;
			this.buttonGotoCamera.Enabled = false;
		}
		private void buttonSetCamera_Click(object sender, EventArgs e)
		{
			EditorDocument.SnapshotPos = Camera.Position;
			EditorDocument.SnapshotAngle = Camera.Angles;
			this.UpdateSnapshot();
		}
		private void buttonGotoCamera_Click(object sender, EventArgs e)
		{
			if (EditorDocument.IsSnapshotSet)
			{
				Camera.Position = EditorDocument.SnapshotPos;
				Camera.Angles = EditorDocument.SnapshotAngle;
			}
		}
	}
}
