using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamCollectionList : UserControl
	{
		private int m_value;
		private IContainer components;
		private Label label1;
		private TableLayoutPanel tableLayoutPanel1;
		private NomadButton buttonClear;
		private NomadButton buttonAssign;
		private TreeView treeView;
		private ImageList imageList;
		public event EventHandler ValueChanged;
		public int Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				this.UpdateSelection();
			}
		}
		public ParamCollectionList()
		{
			this.InitializeComponent();
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonAssign.Text = Localizer.Localize(this.buttonAssign.Text);
			this.buttonClear.Text = Localizer.Localize(this.buttonClear.Text);
		}
		private void ParamCollectionList_Load(object sender, EventArgs e)
		{
			this.UpdateList();
		}
		public void UpdateUI()
		{
			this.UpdateList();
		}
		private void UpdateList()
		{
			this.treeView.BeginUpdate();
			this.treeView.Nodes.Clear();
			this.imageList.Images.Clear();
			for (int i = 0; i < 8; i++)
			{
				CollectionInventory.Entry collectionEntryFromId = CollectionManager.GetCollectionEntryFromId(i);
				string text;
				string text2;
				if (collectionEntryFromId.IsValid)
				{
					text = collectionEntryFromId.IconName;
					if (!this.imageList.Images.ContainsKey(text))
					{
						this.imageList.Images.Add(text, collectionEntryFromId.Icon);
					}
					text2 = collectionEntryFromId.DisplayName;
				}
				else
				{
					text = "empty16";
					if (!this.imageList.Images.ContainsKey(text))
					{
						this.imageList.Images.Add(text, Resources.empty16);
					}
					text2 = Localizer.Localize("PARAM_EMPTY");
				}
				CollectionTreeItem tag = new CollectionTreeItem(i, collectionEntryFromId);
				TreeNode treeNode = this.treeView.Nodes.Add(text2);
				treeNode.ImageKey = text;
				treeNode.SelectedImageKey = text;
				treeNode.Tag = tag;
			}
			this.UpdateSelection();
			this.treeView.EndUpdate();
		}
		private void UpdateSelection()
		{
			if (this.m_value >= 0 && this.m_value < this.treeView.Nodes.Count)
			{
				this.treeView.SelectedNode = this.treeView.Nodes[this.m_value];
			}
			this.UpdateButtons();
		}
		private void UpdateButtons()
		{
			CollectionTreeItem collectionTreeItem = (this.treeView.SelectedNode != null) ? ((CollectionTreeItem)this.treeView.SelectedNode.Tag) : null;
			if (collectionTreeItem != null)
			{
				this.buttonAssign.Enabled = true;
				this.buttonClear.Enabled = collectionTreeItem.Entry.IsValid;
				return;
			}
			this.buttonAssign.Enabled = false;
			this.buttonClear.Enabled = false;
		}
		private CollectionTreeItem GetCurrentItem()
		{
			if (this.treeView.SelectedNode == null)
			{
				return null;
			}
			return (CollectionTreeItem)this.treeView.SelectedNode.Tag;
		}
		private void buttonAssign_Click(object sender, EventArgs e)
		{
			this.AssignToSelected();
		}
		private void treeView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			this.AssignToSelected();
		}
		private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
		{
			CollectionTreeItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				this.OnValueChanged(-1);
			}
			else
			{
				this.OnValueChanged(currentItem.Id);
			}
			this.UpdateButtons();
		}
		private void AssignToSelected()
		{
			CollectionTreeItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			using (PromptInventory promptInventory = new PromptInventory())
			{
				promptInventory.Root = CollectionInventory.Instance.Root;
				promptInventory.Value = currentItem.Entry;
				if (promptInventory.ShowDialog(this) != DialogResult.Cancel)
				{
					this.AssignCollectionId(currentItem.Id, (CollectionInventory.Entry)promptInventory.Value);
				}
			}
		}
		private void buttonClear_Click(object sender, EventArgs e)
		{
			CollectionTreeItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			this.AssignCollectionId(currentItem.Id, CollectionInventory.Entry.Null);
		}
		private void AssignCollectionId(int id, CollectionInventory.Entry entry)
		{
			Win32.SetRedraw(this, false);
			UndoManager.RecordUndo();
			if (!entry.IsValid)
			{
				CollectionManager.ClearMaskId(id);
			}
			CollectionManager.AssignCollectionId(id, entry);
			UndoManager.CommitUndo();
			this.UpdateList();
			Win32.SetRedraw(this, true);
			this.Refresh();
		}
		private void OnValueChanged(int value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
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
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonClear = new NomadButton();
			this.buttonAssign = new NomadButton();
			this.treeView = new TreeView();
			this.imageList = new ImageList(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(125, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "PARAM_COLLECTIONS";
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Controls.Add(this.buttonClear, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonAssign, 0, 0);
			this.tableLayoutPanel1.Location = new Point(3, 193);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Size = new Size(212, 29);
			this.tableLayoutPanel1.TabIndex = 4;
			this.buttonClear.Dock = DockStyle.Fill;
			this.buttonClear.Location = new Point(109, 3);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new Size(100, 23);
			this.buttonClear.TabIndex = 4;
			this.buttonClear.Text = "PARAM_CLEAR";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
			this.buttonAssign.Dock = DockStyle.Fill;
			this.buttonAssign.Location = new Point(3, 3);
			this.buttonAssign.Name = "buttonAssign";
			this.buttonAssign.Size = new Size(100, 23);
			this.buttonAssign.TabIndex = 3;
			this.buttonAssign.Text = "PARAM_ASSIGN";
			this.buttonAssign.Click += new EventHandler(this.buttonAssign_Click);
			this.treeView.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.treeView.HideSelection = false;
			this.treeView.ImageIndex = 0;
			this.treeView.ImageList = this.imageList;
			this.treeView.Location = new Point(6, 16);
			this.treeView.Name = "treeView";
			this.treeView.SelectedImageIndex = 0;
			this.treeView.ShowLines = false;
			this.treeView.ShowPlusMinus = false;
			this.treeView.ShowRootLines = false;
			this.treeView.Size = new Size(212, 174);
			this.treeView.TabIndex = 5;
			this.treeView.MouseDoubleClick += new MouseEventHandler(this.treeView_MouseDoubleClick);
			this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new Size(16, 16);
			this.imageList.TransparentColor = Color.Transparent;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeView);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.label1);
			base.Name = "ParamCollectionList";
			base.Size = new Size(218, 225);
			base.Load += new EventHandler(this.ParamCollectionList_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
