using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class PromptInventoryList : Form
	{
		private IContainer components;
		private Label label1;
		private NomadButton buttonCancel;
		private NomadButton buttonOK;
		private InventoryList inventoryList;
		private IndentedComboBox categoryComboBox;
		private Inventory.Entry m_root;
		public Inventory.Entry Root
		{
			get
			{
				return this.m_root;
			}
			set
			{
				this.m_root = value;
				this.FillCategories(this.m_root, this.categoryComboBox.Root);
				this.categoryComboBox.UpdateItems();
			}
		}
		public Inventory.Entry Value
		{
			get
			{
				if (this.inventoryList.SelectedItems.Count == 0)
				{
					return null;
				}
				return (Inventory.Entry)this.inventoryList.SelectedItems[0].Tag;
			}
			set
			{
				this.categoryComboBox.SelectedItem = this.FindCategory(value.Parent);
				this.inventoryList.SelectedItems.Clear();
				ListViewItem listViewItem = this.inventoryList.FindEntry(value);
				if (listViewItem != null)
				{
					listViewItem.Selected = true;
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
			this.components = new Container();
			this.label1 = new Label();
			this.buttonCancel = new NomadButton();
			this.buttonOK = new NomadButton();
			this.inventoryList = new InventoryList(this.components);
			this.categoryComboBox = new IndentedComboBox();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new Size(156, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "PROMPT_INVENTORY_TEXT";
			this.buttonCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(535, 492);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "PROMPT_CANCEL";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Enabled = false;
			this.buttonOK.Location = new Point(454, 492);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 2;
			this.buttonOK.Text = "PROMPT_OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.inventoryList.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.inventoryList.Entries = null;
			this.inventoryList.Location = new Point(15, 55);
			this.inventoryList.Name = "inventoryList";
			this.inventoryList.OwnerDraw = true;
			this.inventoryList.Size = new Size(595, 431);
			this.inventoryList.TabIndex = 4;
			this.inventoryList.UseCompatibleStateImageBehavior = false;
			this.inventoryList.View = View.Tile;
			this.inventoryList.SelectedIndexChanged += new EventHandler(this.inventoryList_SelectedIndexChanged);
			this.inventoryList.DoubleClick += new EventHandler(this.inventoryList_DoubleClick);
			this.categoryComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.categoryComboBox.Location = new Point(15, 25);
			this.categoryComboBox.MaximumSize = new Size(9999, 24);
			this.categoryComboBox.MinimumSize = new Size(0, 24);
			this.categoryComboBox.Name = "categoryComboBox";
			this.categoryComboBox.SelectedItem = null;
			this.categoryComboBox.Size = new Size(595, 24);
			this.categoryComboBox.TabIndex = 5;
			this.categoryComboBox.SelectedItemChanged += new EventHandler<IndentedComboboxItemEventArgs>(this.categoryComboBox_SelectedItemChanged);
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(622, 519);
			base.Controls.Add(this.categoryComboBox);
			base.Controls.Add(this.inventoryList);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.label1);
			base.Name = "PromptInventoryList";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "PROMPT_INVENTORY_TITLE";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public PromptInventoryList()
		{
			this.InitializeComponent();
			base.Icon = Resources.appIcon;
			this.Text = Localizer.Localize(this.Text);
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonOK.Text = Localizer.Localize(this.buttonOK.Text);
			this.buttonCancel.Text = Localizer.Localize(this.buttonCancel.Text);
			this.buttonOK.Enabled = false;
		}
		private void FillCategories(Inventory.Entry entry, IndentedComboBox.Item item)
		{
			if (!entry.IsDirectory)
			{
				return;
			}
			if (entry.Deleted)
			{
				return;
			}
			IndentedComboBox.Item item2 = new IndentedComboBox.Item();
			item2.Tag = entry;
			item2.Text = entry.DisplayName;
			item2.Image = entry.Icon;
			item.Add(item2);
			int num = 0;
			Inventory.Entry[] children = entry.Children;
			for (int i = 0; i < children.Length; i++)
			{
				Inventory.Entry entry2 = children[i];
				if (entry2.IsDirectory)
				{
					this.FillCategories(entry2, item2);
				}
				else
				{
					num++;
				}
			}
			if (num > 0)
			{
				item2.SubText = "(" + num + ")";
			}
		}
		private IndentedComboBox.Item FindCategory(Inventory.Entry entry)
		{
			foreach (IndentedComboBox.Item current in this.categoryComboBox.GetItems())
			{
				Inventory.Entry entry2 = (Inventory.Entry)current.Tag;
				if (entry2.Pointer == entry.Pointer)
				{
					return current;
				}
			}
			return null;
		}
		private void RefreshList()
		{
			IndentedComboBox.Item selectedItem = this.categoryComboBox.SelectedItem;
			if (selectedItem != null)
			{
				Inventory.Entry entry = (Inventory.Entry)selectedItem.Tag;
				this.inventoryList.Entries = entry.Children;
			}
		}
		private void categoryComboBox_SelectedItemChanged(object sender, IndentedComboboxItemEventArgs e)
		{
			this.RefreshList();
			this.inventoryList.Focus();
		}
		private void inventoryList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.buttonOK.Enabled = (this.Value != null && !this.Value.IsDirectory);
		}
		private void inventoryList_DoubleClick(object sender, EventArgs e)
		{
			if (this.buttonOK.Enabled)
			{
				base.DialogResult = DialogResult.OK;
				return;
			}
			if (this.Value != null && this.Value.IsDirectory)
			{
				this.categoryComboBox.SelectedItem = this.FindCategory(this.Value);
			}
		}
	}
}
