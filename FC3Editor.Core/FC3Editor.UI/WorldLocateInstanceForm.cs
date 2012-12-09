using FC3Editor.Nomad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class WorldLocateInstanceForm : Form
	{
		private List<WorldImport.ObjectInstance> m_instances;
		private IContainer components;
		private Label label1;
		private Label labelEntry;
		private ListBox listInstances;
		private Button buttonOK;
		private Button buttonCancel;
		public List<WorldImport.ObjectInstance> Instances
		{
			get
			{
				return this.m_instances;
			}
			set
			{
				this.m_instances = value;
				this.FillInstances();
			}
		}
		public WorldImport.ObjectInstance SelectedInstance
		{
			get
			{
				return (WorldImport.ObjectInstance)this.listInstances.SelectedItem;
			}
		}
		public WorldLocateInstanceForm()
		{
			this.InitializeComponent();
		}
		private void FillInstances()
		{
			this.listInstances.Items.Clear();
			this.listInstances.Items.AddRange(this.m_instances.ToArray());
			if (this.listInstances.Items.Count > 0)
			{
				this.listInstances.SelectedIndex = 0;
			}
		}
		private void listInstances_DoubleClick(object sender, EventArgs e)
		{
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
			this.label1 = new Label();
			this.labelEntry = new Label();
			this.listInstances = new ListBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(127, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Instances found for entry:";
			this.labelEntry.AutoSize = true;
			this.labelEntry.Location = new Point(145, 9);
			this.labelEntry.Name = "labelEntry";
			this.labelEntry.Size = new Size(0, 13);
			this.labelEntry.TabIndex = 1;
			this.listInstances.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.listInstances.FormattingEnabled = true;
			this.listInstances.Location = new Point(15, 25);
			this.listInstances.Name = "listInstances";
			this.listInstances.Size = new Size(418, 277);
			this.listInstances.TabIndex = 2;
			this.listInstances.DoubleClick += new EventHandler(this.listInstances_DoubleClick);
			this.buttonOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Location = new Point(277, 308);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(358, 308);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 4;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(445, 344);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.listInstances);
			base.Controls.Add(this.labelEntry);
			base.Controls.Add(this.label1);
			base.Name = "WorldLocateInstanceForm";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "Locate instances";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
