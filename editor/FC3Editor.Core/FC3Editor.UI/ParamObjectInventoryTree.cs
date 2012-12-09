using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamObjectInventoryTree : UserControl
	{
		private IContainer components;
		private InventoryTree inventoryTree;
		private Label label1;
		private IndentedComboBox categoryComboBox;
		private InventoryList objectList;
		private NomadButton parentButton;
		private TableLayoutPanel tableLayoutPanel1;
		private Label label3;
		private Label label2;
		private Label selObjNameLabel;
		private Label selObjSizeLabel;
		private TextBox textBoxSearchObjInv;
		private Label label4;
		protected IndentedComboBox.Item m_SearchItem;
		private AABB m_objectSize;
		public event EventHandler FolderChanged;
		public event EventHandler ValueChanged;
		public event EventHandler SearchChanged;
		public ObjectInventory.Entry Value
		{
			get
			{
				if (this.objectList.SelectedItems.Count == 0)
				{
					return (ObjectInventory.Entry)this.categoryComboBox.SelectedItem.Tag;
				}
				return (ObjectInventory.Entry)this.objectList.SelectedItems[0].Tag;
			}
			set
			{
				if (value == null)
				{
					this.categoryComboBox.SelectedItem = this.categoryComboBox.FirstItem;
					return;
				}
				if (this.categoryComboBox.SelectedItem != this.m_SearchItem)
				{
					ObjectInventory.Entry entry = (ObjectInventory.Entry)value.Parent;
					ObjectInventory.Entry entry2 = value.IsDirectory ? value : ((entry.Pointer != IntPtr.Zero) ? entry : null);
					if (entry2 != null)
					{
						this.categoryComboBox.SelectedItem = this.FindCategory(entry2);
					}
					this.objectList.SelectedItems.Clear();
					foreach (ListViewItem listViewItem in this.objectList.Items)
					{
						ObjectInventory.Entry entry3 = (ObjectInventory.Entry)listViewItem.Tag;
						if (entry3.Pointer == value.Pointer)
						{
							listViewItem.Selected = true;
							break;
						}
					}
				}
			}
		}
		public ObjectInventory.Entry Folder
		{
			get
			{
				IndentedComboBox.Item selectedItem = this.categoryComboBox.SelectedItem;
				if (selectedItem != null)
				{
					return (ObjectInventory.Entry)selectedItem.Tag;
				}
				return null;
			}
		}
		public string SearchCriteria
		{
			get
			{
				return this.textBoxSearchObjInv.Text;
			}
			set
			{
				this.textBoxSearchObjInv.Text = value;
				this.m_SearchItem.Text = Localizer.Localize("PARAM_OBJECT_SEARCH") + " " + value;
			}
		}
		public AABB ObjectSize
		{
			get
			{
				return this.m_objectSize;
			}
			set
			{
				this.m_objectSize = value;
				this.selObjSizeLabel.Text = this.m_objectSize.ToString();
			}
		}
		public int ObjectListFirstVisibleIndex
		{
			get
			{
				return this.objectList.FirstVisibleIndex;
			}
			set
			{
				this.objectList.FirstVisibleIndex = value;
			}
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
			this.DoDispose();
		}
		private void InitializeComponent()
		{
			this.components = new Container();
			this.label1 = new Label();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.label4 = new Label();
			this.label3 = new Label();
			this.label2 = new Label();
			this.selObjNameLabel = new Label();
			this.selObjSizeLabel = new Label();
			this.textBoxSearchObjInv = new TextBox();
			this.parentButton = new NomadButton();
			this.objectList = new InventoryList(this.components);
			this.categoryComboBox = new IndentedComboBox();
			this.inventoryTree = new InventoryTree();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.label1.Location = new Point(3, 2);
			this.label1.Name = "label1";
			this.label1.Size = new Size(203, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "PARAM_OBJECT_BROWSER";
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.selObjNameLabel, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.selObjSizeLabel, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.textBoxSearchObjInv, 1, 0);
			this.tableLayoutPanel1.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;
			this.tableLayoutPanel1.Location = new Point(3, 365);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 23f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 16f));
			this.tableLayoutPanel1.Size = new Size(203, 55);
			this.tableLayoutPanel1.TabIndex = 9;
			this.label4.Dock = DockStyle.Fill;
			this.label4.Location = new Point(0, 0);
			this.label4.Margin = new Padding(0);
			this.label4.Name = "label4";
			this.label4.Size = new Size(101, 23);
			this.label4.TabIndex = 9;
			this.label4.Text = "PARAM_OBJECT_SEARCH";
			this.label4.TextAlign = ContentAlignment.MiddleLeft;
			this.label3.Dock = DockStyle.Fill;
			this.label3.Location = new Point(0, 39);
			this.label3.Margin = new Padding(0);
			this.label3.Name = "label3";
			this.label3.Size = new Size(101, 16);
			this.label3.TabIndex = 6;
			this.label3.Text = "PARAM_OBJECT_BROWSER_SIZE";
			this.label3.TextAlign = ContentAlignment.MiddleLeft;
			this.label2.Dock = DockStyle.Fill;
			this.label2.Location = new Point(0, 23);
			this.label2.Margin = new Padding(0);
			this.label2.Name = "label2";
			this.label2.Size = new Size(101, 16);
			this.label2.TabIndex = 3;
			this.label2.Text = "PARAM_OBJECT_BROWSER_SELECTED";
			this.label2.TextAlign = ContentAlignment.MiddleLeft;
			this.selObjNameLabel.BorderStyle = BorderStyle.Fixed3D;
			this.selObjNameLabel.Dock = DockStyle.Fill;
			this.selObjNameLabel.Location = new Point(101, 23);
			this.selObjNameLabel.Margin = new Padding(0);
			this.selObjNameLabel.Name = "selObjNameLabel";
			this.selObjNameLabel.Size = new Size(102, 16);
			this.selObjNameLabel.TabIndex = 5;
			this.selObjNameLabel.Text = "None";
			this.selObjNameLabel.TextAlign = ContentAlignment.MiddleRight;
			this.selObjSizeLabel.BorderStyle = BorderStyle.Fixed3D;
			this.selObjSizeLabel.Dock = DockStyle.Fill;
			this.selObjSizeLabel.Location = new Point(101, 39);
			this.selObjSizeLabel.Margin = new Padding(0);
			this.selObjSizeLabel.Name = "selObjSizeLabel";
			this.selObjSizeLabel.Size = new Size(102, 16);
			this.selObjSizeLabel.TabIndex = 7;
			this.selObjSizeLabel.Text = "None";
			this.selObjSizeLabel.TextAlign = ContentAlignment.MiddleRight;
			this.textBoxSearchObjInv.Dock = DockStyle.Fill;
			this.textBoxSearchObjInv.Location = new Point(101, 0);
			this.textBoxSearchObjInv.Margin = new Padding(0);
			this.textBoxSearchObjInv.Name = "textBoxSearchObjInv";
			this.textBoxSearchObjInv.Size = new Size(102, 20);
			this.textBoxSearchObjInv.TabIndex = 8;
			this.textBoxSearchObjInv.KeyDown += new KeyEventHandler(this.textBoxSearchObjInv_KeyDown);
			this.parentButton.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.parentButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.parentButton.Image = Resources.up;
			this.parentButton.Location = new Point(182, 17);
			this.parentButton.Name = "parentButton";
			this.parentButton.Size = new Size(26, 26);
			this.parentButton.TabIndex = 8;
			this.parentButton.UseVisualStyleBackColor = true;
			this.parentButton.Click += new EventHandler(this.parentButton_Click);
			this.objectList.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.objectList.Entries = null;
			this.objectList.FirstVisibleIndex = -1;
			this.objectList.HideSelection = false;
			this.objectList.Location = new Point(3, 43);
			this.objectList.MultiSelect = false;
			this.objectList.Name = "objectList";
			this.objectList.OwnerDraw = true;
			this.objectList.Size = new Size(203, 319);
			this.objectList.TabIndex = 7;
			this.objectList.UseCompatibleStateImageBehavior = false;
			this.objectList.View = View.Tile;
			this.objectList.SelectedIndexChanged += new EventHandler(this.objectList_SelectedIndexChanged);
			this.objectList.DoubleClick += new EventHandler(this.objectList_DoubleClick);
			this.categoryComboBox.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.categoryComboBox.Location = new Point(3, 18);
			this.categoryComboBox.MaximumSize = new Size(9999, 24);
			this.categoryComboBox.MinimumSize = new Size(0, 24);
			this.categoryComboBox.Name = "categoryComboBox";
			this.categoryComboBox.SelectedItem = null;
			this.categoryComboBox.Size = new Size(176, 24);
			this.categoryComboBox.TabIndex = 6;
			this.categoryComboBox.SelectedItemChanged += new EventHandler<IndentedComboboxItemEventArgs>(this.indentedComboBox1_SelectedItemChanged);
			this.inventoryTree.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.inventoryTree.HideSelection = false;
			this.inventoryTree.ImageIndex = 0;
			this.inventoryTree.Location = new Point(3, 18);
			this.inventoryTree.Name = "inventoryTree";
			this.inventoryTree.OnlyDirectories = false;
			this.inventoryTree.Root = null;
			this.inventoryTree.SelectedImageIndex = 0;
			this.inventoryTree.ShowRoot = false;
			this.inventoryTree.Size = new Size(203, 190);
			this.inventoryTree.TabIndex = 0;
			this.inventoryTree.Value = null;
			this.inventoryTree.Visible = false;
			this.inventoryTree.ValueChanged += new EventHandler(this.inventoryTree_ValueChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.parentButton);
			base.Controls.Add(this.objectList);
			base.Controls.Add(this.categoryComboBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.inventoryTree);
			base.Name = "ParamObjectInventoryTree";
			base.Size = new Size(209, 425);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamObjectInventoryTree(ObjectInventory.Entry searchDirectory)
		{
			this.InitializeComponent();
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.label2.Text = Localizer.Localize(this.label2.Text);
			this.label3.Text = Localizer.Localize(this.label3.Text);
			this.label4.Text = Localizer.Localize(this.label4.Text);
			this.FillCategories((ObjectInventory.Entry)ObjectInventory.Instance.Root, this.categoryComboBox.Root);
			this.m_SearchItem = new IndentedComboBox.Item();
			this.m_SearchItem.Tag = searchDirectory;
			this.m_SearchItem.Image = searchDirectory.Icon;
			this.SearchCriteria = searchDirectory.DisplayName;
			this.categoryComboBox.Root.Add(this.m_SearchItem);
			this.categoryComboBox.UpdateItems();
		}
		private void DoDispose()
		{
		}
		private void FillCategories(ObjectInventory.Entry entry, IndentedComboBox.Item item)
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
				ObjectInventory.Entry entry2 = (ObjectInventory.Entry)children[i];
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
		private IndentedComboBox.Item FindCategory(ObjectInventory.Entry entry)
		{
			foreach (IndentedComboBox.Item current in this.categoryComboBox.GetItems())
			{
				ObjectInventory.Entry entry2 = (ObjectInventory.Entry)current.Tag;
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
				this.objectList.Entries = entry.Children;
				this.parentButton.Enabled = entry.Parent.IsValid;
				return;
			}
			this.parentButton.Enabled = false;
		}
		private void OnFolderChanged()
		{
			if (this.FolderChanged != null)
			{
				this.FolderChanged(this, new EventArgs());
			}
		}
		private void OnValueChanged()
		{
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
			if (this.Value == null || this.Value.IsDirectory)
			{
				this.selObjNameLabel.Text = Localizer.Localize("PARAM_OBJECT_BROWSER_NONE");
			}
			else
			{
				this.selObjNameLabel.Text = this.Value.DisplayName;
			}
			this.selObjSizeLabel.Text = Localizer.Localize("PARAM_NA");
		}
		private void OnSearchChanged()
		{
			if (this.SearchChanged != null)
			{
				this.SearchChanged(this, new EventArgs());
			}
			this.SearchCriteria = (this.m_SearchItem.Tag as ObjectInventory.Entry).DisplayName;
			this.categoryComboBox.SelectedItem = this.m_SearchItem;
			this.categoryComboBox.UpdateItems();
		}
		private void inventoryTree_ValueChanged(object sender, EventArgs e)
		{
			this.OnValueChanged();
		}
		private void indentedComboBox1_SelectedItemChanged(object sender, IndentedComboboxItemEventArgs e)
		{
			this.RefreshList();
			this.objectList.Focus();
			this.OnValueChanged();
			this.OnFolderChanged();
		}
		private void parentButton_Click(object sender, EventArgs e)
		{
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.categoryComboBox.SelectedItem.Tag;
			if (entry.IsValid)
			{
				this.categoryComboBox.SelectedItem = this.FindCategory((ObjectInventory.Entry)entry.Parent);
			}
		}
		private void objectList_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.OnValueChanged();
		}
		private void textBoxSearchObjInv_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return)
			{
				this.OnSearchChanged();
				e.SuppressKeyPress = true;
			}
		}
		private void objectList_DoubleClick(object sender, EventArgs e)
		{
			if (this.objectList.SelectedItems.Count == 0)
			{
				return;
			}
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.objectList.SelectedItems[0].Tag;
			if (entry.IsDirectory)
			{
				this.categoryComboBox.SelectedItem = this.FindCategory(entry);
			}
		}
	}
}
