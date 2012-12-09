using FC3Editor.Nomad;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ImportWorldForm : Form
	{
		private IContainer components;
		private Label labelWorld;
		private Button buttonLoadWorld;
		private Label label1;
		private TextBox textSectorX;
		private TextBox textSectorY;
		private Label label2;
		private Button buttonImportWorld;
		private OpenFileDialog openWorldDialog;
		public ImportWorldForm()
		{
			this.InitializeComponent();
		}
		private void buttonLoadWorld_Click(object sender, EventArgs e)
		{
			if (!WorldImport.Instance.LoadWorld())
			{
				return;
			}
			this.labelWorld.Text = "Current world: " + WorldImport.Instance.WorldFile;
		}
		private void buttonImportWorld_Click(object sender, EventArgs e)
		{
			int sectorX;
			int sectorY;
			if (!int.TryParse(this.textSectorX.Text, out sectorX) || !int.TryParse(this.textSectorY.Text, out sectorY))
			{
				MessageBox.Show("Invalid sector pos");
				return;
			}
			if (!File.Exists(WorldImport.Instance.WorldFile))
			{
				MessageBox.Show("No world file selected");
				return;
			}
			WorldImport.Instance.ImportWorld(sectorX, sectorY, new string[0]);
			base.DialogResult = DialogResult.OK;
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
			this.labelWorld = new Label();
			this.buttonLoadWorld = new Button();
			this.label1 = new Label();
			this.textSectorX = new TextBox();
			this.textSectorY = new TextBox();
			this.label2 = new Label();
			this.buttonImportWorld = new Button();
			this.openWorldDialog = new OpenFileDialog();
			base.SuspendLayout();
			this.labelWorld.AutoSize = true;
			this.labelWorld.Location = new Point(12, 16);
			this.labelWorld.Name = "labelWorld";
			this.labelWorld.Size = new Size(101, 13);
			this.labelWorld.TabIndex = 0;
			this.labelWorld.Text = "Current world: None";
			this.buttonLoadWorld.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.buttonLoadWorld.Location = new Point(497, 12);
			this.buttonLoadWorld.Name = "buttonLoadWorld";
			this.buttonLoadWorld.Size = new Size(83, 21);
			this.buttonLoadWorld.TabIndex = 1;
			this.buttonLoadWorld.Text = "Load world...";
			this.buttonLoadWorld.UseVisualStyleBackColor = true;
			this.buttonLoadWorld.Click += new EventHandler(this.buttonLoadWorld_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 49);
			this.label1.Name = "label1";
			this.label1.Size = new Size(48, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Sector X";
			this.textSectorX.Location = new Point(66, 42);
			this.textSectorX.Name = "textSectorX";
			this.textSectorX.Size = new Size(47, 20);
			this.textSectorX.TabIndex = 3;
			this.textSectorY.Location = new Point(173, 42);
			this.textSectorY.Name = "textSectorY";
			this.textSectorY.Size = new Size(47, 20);
			this.textSectorY.TabIndex = 5;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(119, 49);
			this.label2.Name = "label2";
			this.label2.Size = new Size(48, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Sector Y";
			this.buttonImportWorld.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.buttonImportWorld.Location = new Point(12, 72);
			this.buttonImportWorld.Name = "buttonImportWorld";
			this.buttonImportWorld.Size = new Size(568, 24);
			this.buttonImportWorld.TabIndex = 6;
			this.buttonImportWorld.Text = "Import world";
			this.buttonImportWorld.UseVisualStyleBackColor = true;
			this.buttonImportWorld.Click += new EventHandler(this.buttonImportWorld_Click);
			this.openWorldDialog.DefaultExt = "xml";
			this.openWorldDialog.FileName = "world.xml";
			this.openWorldDialog.Filter = "World file|world.xml";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(592, 108);
			base.Controls.Add(this.buttonImportWorld);
			base.Controls.Add(this.textSectorY);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.textSectorX);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.buttonLoadWorld);
			base.Controls.Add(this.labelWorld);
			base.Name = "ImportWorldForm";
			this.Text = "Import World";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
