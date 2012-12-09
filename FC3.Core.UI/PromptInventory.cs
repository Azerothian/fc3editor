using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class PromptInventory : Form
	{
		private IContainer components;
		private Label label1;
		private InventoryTree inventoryTree;
		private NomadButton buttonCancel;
		private NomadButton buttonOK;
		public Inventory.Entry Root
		{
			get
			{
				return this.inventoryTree.Root;
			}
			set
			{
				this.inventoryTree.Root = value;
			}
		}
		public Inventory.Entry Value
		{
			get
			{
				return this.inventoryTree.Value;
			}
			set
			{
				this.inventoryTree.Value = value;
			}
		}
		public PromptInventory()
		{
			this.InitializeComponent();
			base.Icon = Resources.appIcon;
			this.Text = Localizer.Localize(this.Text);
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonOK.Text = Localizer.Localize(this.buttonOK.Text);
			this.buttonCancel.Text = Localizer.Localize(this.buttonCancel.Text);
		}
		private void inventoryTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			if (this.Value != null && !this.Value.IsDirectory)
			{
				base.DialogResult = DialogResult.OK;
			}
		}
		private void inventoryTree_ValueChanged(object sender, EventArgs e)
		{
			this.buttonOK.Enabled = (this.Value != null && !this.Value.IsDirectory);
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
			this.components = new Container();
			this.label1 = new Label();
			this.buttonCancel = new NomadButton();
			this.buttonOK = new NomadButton();
			this.inventoryTree = new InventoryTree();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(156, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "PROMPT_INVENTORY_TEXT";
			this.buttonCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(209, 273);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "PROMPT_CANCEL";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Enabled = false;
			this.buttonOK.Location = new Point(128, 273);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "PROMPT_OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.inventoryTree.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.inventoryTree.HideSelection = false;
			this.inventoryTree.ImageIndex = 0;
			this.inventoryTree.Location = new Point(15, 25);
			this.inventoryTree.Name = "inventoryTree";
			this.inventoryTree.Root = null;
			this.inventoryTree.SelectedImageIndex = 0;
			this.inventoryTree.Size = new Size(269, 244);
			this.inventoryTree.TabIndex = 1;
			this.inventoryTree.Value = null;
			this.inventoryTree.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(this.inventoryTree_NodeMouseDoubleClick);
			this.inventoryTree.ValueChanged += new EventHandler(this.inventoryTree_ValueChanged);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(296, 300);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.inventoryTree);
			base.Controls.Add(this.label1);
			base.Name = "PromptInventory";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "PROMPT_INVENTORY_TITLE";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
