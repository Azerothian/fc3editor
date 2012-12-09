using FC3Editor.Nomad;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class CodeHelper : UserControl
	{
		private IContainer components;
		private Label prototypeLabel;
		private Label descriptionLabel;
		public CodeHelper()
		{
			this.InitializeComponent();
		}
		public void Setup(Wilderness.FunctionDef function)
		{
			this.prototypeLabel.Text = function.Prototype;
			this.descriptionLabel.Text = function.Description;
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
			this.prototypeLabel = new Label();
			this.descriptionLabel = new Label();
			base.SuspendLayout();
			this.prototypeLabel.AutoSize = true;
			this.prototypeLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.prototypeLabel.Location = new Point(-1, 2);
			this.prototypeLabel.Name = "prototypeLabel";
			this.prototypeLabel.Size = new Size(68, 13);
			this.prototypeLabel.TabIndex = 0;
			this.prototypeLabel.Text = "Function();";
			this.descriptionLabel.AutoSize = true;
			this.descriptionLabel.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.descriptionLabel.Location = new Point(-1, 18);
			this.descriptionLabel.MaximumSize = new Size(500, 0);
			this.descriptionLabel.Name = "descriptionLabel";
			this.descriptionLabel.Size = new Size(218, 13);
			this.descriptionLabel.TabIndex = 1;
			this.descriptionLabel.Text = "Some helper text just to see how it looks like.";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.BackColor = SystemColors.Info;
			base.BorderStyle = BorderStyle.FixedSingle;
			base.Controls.Add(this.descriptionLabel);
			base.Controls.Add(this.prototypeLabel);
			base.Name = "CodeHelper";
			base.Padding = new Padding(0, 2, 0, 2);
			base.Size = new Size(220, 33);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
