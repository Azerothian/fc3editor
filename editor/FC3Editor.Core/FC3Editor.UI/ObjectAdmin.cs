using FC3Editor.Nomad;
using FC3Editor.Properties;
using FC3Editor.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ObjectAdmin : UserControl
	{
		private IContainer components;
		private SplitContainer splitContainer2;
		private SplitContainer splitContainer1;
		private InventoryTree objectTree;
		private InventoryList objectList;
		private PictureBox objectThumbnail;
		private Label label0;
		private ToolStrip toolStrip1;
		private ToolStripButton buttonNewFolder;
		private TextBox textTags;
		private Label label2;
		private TextBox textDisplay;
		private Label label1;
		private ToolStripButton buttonRenameFolder;
		private ToolStripButton buttonDeleteFolder;
		private ToolStrip toolStrip2;
		private ToolStripButton buttonRenameEntry;
		private Button buttonLocateInstances;
		private Button buttonBrowseWorld;
		private Label labelWorld;
		private Label label3;
		private Button buttonSaveDatabase;
		private ToolStripButton buttonAddPrefab;
		private TextBox textId;
		private ToolStripButton buttonSortEntries;
		private bool m_treeInsertMarkAfter;
		private TreeNode m_treeInsertMarkNode;
		private ObjectInventory.Entry m_root;
		private ObjectInventory.Entry m_currentEntry;
		private bool m_dragging;
		private TreeNode m_dragOldTreeNode;
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
			this.splitContainer2 = new SplitContainer();
			this.splitContainer1 = new SplitContainer();
			this.objectTree = new InventoryTree();
			this.toolStrip1 = new ToolStrip();
			this.buttonNewFolder = new ToolStripButton();
			this.buttonRenameFolder = new ToolStripButton();
			this.buttonDeleteFolder = new ToolStripButton();
			this.objectList = new InventoryList(this.components);
			this.toolStrip2 = new ToolStrip();
			this.buttonAddPrefab = new ToolStripButton();
			this.buttonRenameEntry = new ToolStripButton();
			this.buttonSortEntries = new ToolStripButton();
			this.textId = new TextBox();
			this.buttonSaveDatabase = new Button();
			this.buttonLocateInstances = new Button();
			this.buttonBrowseWorld = new Button();
			this.labelWorld = new Label();
			this.label3 = new Label();
			this.textTags = new TextBox();
			this.label2 = new Label();
			this.textDisplay = new TextBox();
			this.label1 = new Label();
			this.label0 = new Label();
			this.objectThumbnail = new PictureBox();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.Panel2.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.toolStrip2.SuspendLayout();
			((ISupportInitialize)this.objectThumbnail).BeginInit();
			base.SuspendLayout();
			this.splitContainer2.Dock = DockStyle.Fill;
			this.splitContainer2.FixedPanel = FixedPanel.Panel2;
			this.splitContainer2.Location = new Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = Orientation.Horizontal;
			this.splitContainer2.Panel1.Controls.Add(this.splitContainer1);
			this.splitContainer2.Panel2.Controls.Add(this.textId);
			this.splitContainer2.Panel2.Controls.Add(this.buttonSaveDatabase);
			this.splitContainer2.Panel2.Controls.Add(this.buttonLocateInstances);
			this.splitContainer2.Panel2.Controls.Add(this.buttonBrowseWorld);
			this.splitContainer2.Panel2.Controls.Add(this.labelWorld);
			this.splitContainer2.Panel2.Controls.Add(this.label3);
			this.splitContainer2.Panel2.Controls.Add(this.textTags);
			this.splitContainer2.Panel2.Controls.Add(this.label2);
			this.splitContainer2.Panel2.Controls.Add(this.textDisplay);
			this.splitContainer2.Panel2.Controls.Add(this.label1);
			this.splitContainer2.Panel2.Controls.Add(this.label0);
			this.splitContainer2.Panel2.Controls.Add(this.objectThumbnail);
			this.splitContainer2.Size = new Size(988, 696);
			this.splitContainer2.SplitterDistance = 476;
			this.splitContainer2.TabIndex = 3;
			this.splitContainer1.Dock = DockStyle.Fill;
			this.splitContainer1.Location = new Point(0, 0);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.objectTree);
			this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer1.Panel2.Controls.Add(this.objectList);
			this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
			this.splitContainer1.Size = new Size(988, 476);
			this.splitContainer1.SplitterDistance = 274;
			this.splitContainer1.TabIndex = 3;
			this.objectTree.AllowDrop = true;
			this.objectTree.Dock = DockStyle.Fill;
			this.objectTree.HideSelection = false;
			this.objectTree.ImageIndex = 0;
			this.objectTree.LabelEdit = true;
			this.objectTree.Location = new Point(0, 25);
			this.objectTree.Name = "objectTree";
			this.objectTree.OnlyDirectories = true;
			this.objectTree.Root = null;
			this.objectTree.SelectedImageIndex = 0;
			this.objectTree.ShowRoot = true;
			this.objectTree.Size = new Size(274, 451);
			this.objectTree.TabIndex = 1;
			this.objectTree.Value = null;
			this.objectTree.BeforeLabelEdit += new NodeLabelEditEventHandler(this.objectTree_BeforeLabelEdit);
			this.objectTree.AfterLabelEdit += new NodeLabelEditEventHandler(this.objectTree_AfterLabelEdit);
			this.objectTree.ItemDrag += new ItemDragEventHandler(this.objectTree_ItemDrag);
			this.objectTree.AfterSelect += new TreeViewEventHandler(this.objectTree_AfterSelect);
			this.objectTree.DragDrop += new DragEventHandler(this.objectTree_DragDrop);
			this.objectTree.DragOver += new DragEventHandler(this.objectTree_DragOver);
			this.objectTree.GiveFeedback += new GiveFeedbackEventHandler(this.objectTree_GiveFeedback);
			this.objectTree.QueryContinueDrag += new QueryContinueDragEventHandler(this.objectTree_QueryContinueDrag);
			this.toolStrip1.Items.AddRange(new ToolStripItem[]
			{
				this.buttonNewFolder,
				this.buttonRenameFolder,
				this.buttonDeleteFolder
			});
			this.toolStrip1.Location = new Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new Size(274, 25);
			this.toolStrip1.TabIndex = 2;
			this.toolStrip1.Text = "toolStrip1";
			this.buttonNewFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonNewFolder.Image = Resources.NewFolderHS;
			this.buttonNewFolder.ImageTransparentColor = Color.Magenta;
			this.buttonNewFolder.Name = "buttonNewFolder";
			this.buttonNewFolder.Size = new Size(23, 22);
			this.buttonNewFolder.Text = "New directory";
			this.buttonNewFolder.Click += new EventHandler(this.buttonNewFolder_Click);
			this.buttonRenameFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonRenameFolder.Image = Resources.RenameFolderHS;
			this.buttonRenameFolder.ImageTransparentColor = Color.Magenta;
			this.buttonRenameFolder.Name = "buttonRenameFolder";
			this.buttonRenameFolder.Size = new Size(23, 22);
			this.buttonRenameFolder.Text = "Rename directory";
			this.buttonRenameFolder.Click += new EventHandler(this.buttonRenameFolder_Click);
			this.buttonDeleteFolder.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonDeleteFolder.Image = Resources.DeleteFolderHS;
			this.buttonDeleteFolder.ImageTransparentColor = Color.Magenta;
			this.buttonDeleteFolder.Name = "buttonDeleteFolder";
			this.buttonDeleteFolder.Size = new Size(23, 22);
			this.buttonDeleteFolder.Text = "Delete directory";
			this.buttonDeleteFolder.Click += new EventHandler(this.buttonDeleteFolder_Click);
			this.objectList.Dock = DockStyle.Fill;
			this.objectList.LabelEdit = true;
			this.objectList.Location = new Point(0, 25);
			this.objectList.Name = "objectList";
			this.objectList.OwnerDraw = true;
			this.objectList.Entries = null;
			this.objectList.Size = new Size(710, 451);
			this.objectList.TabIndex = 2;
			this.objectList.UseCompatibleStateImageBehavior = false;
			this.objectList.View = View.Tile;
			this.objectList.AfterLabelEdit += new LabelEditEventHandler(this.objectList_AfterLabelEdit);
			this.objectList.ItemDrag += new ItemDragEventHandler(this.objectList_ItemDrag);
			this.objectList.SelectedIndexChanged += new EventHandler(this.objectList_SelectedIndexChanged);
			this.objectList.QueryContinueDrag += new QueryContinueDragEventHandler(this.objectList_QueryContinueDrag);
			this.objectList.DoubleClick += new EventHandler(this.objectList_DoubleClick);
			this.toolStrip2.Items.AddRange(new ToolStripItem[]
			{
				this.buttonAddPrefab,
				this.buttonRenameEntry,
				this.buttonSortEntries
			});
			this.toolStrip2.Location = new Point(0, 0);
			this.toolStrip2.Name = "toolStrip2";
			this.toolStrip2.Size = new Size(710, 25);
			this.toolStrip2.TabIndex = 3;
			this.toolStrip2.Text = "toolStrip2";
			this.buttonAddPrefab.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonAddPrefab.Enabled = false;
			this.buttonAddPrefab.Image = Resources.prefab_new;
			this.buttonAddPrefab.ImageTransparentColor = Color.Magenta;
			this.buttonAddPrefab.Name = "buttonAddPrefab";
			this.buttonAddPrefab.Size = new Size(23, 22);
			this.buttonAddPrefab.Text = "Add prefab";
			this.buttonAddPrefab.Click += new EventHandler(this.buttonAddPrefab_Click);
			this.buttonRenameEntry.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonRenameEntry.Image = Resources.RenameFolderHS;
			this.buttonRenameEntry.ImageTransparentColor = Color.Magenta;
			this.buttonRenameEntry.Name = "buttonRenameEntry";
			this.buttonRenameEntry.Size = new Size(23, 22);
			this.buttonRenameEntry.Text = "Rename entry";
			this.buttonRenameEntry.Click += new EventHandler(this.buttonRenameEntry_Click);
			this.buttonSortEntries.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.buttonSortEntries.Image = Resources.SortHS;
			this.buttonSortEntries.ImageTransparentColor = Color.Magenta;
			this.buttonSortEntries.Name = "buttonSortEntries";
			this.buttonSortEntries.Size = new Size(23, 22);
			this.buttonSortEntries.Text = "buttonSortEntries";
			this.buttonSortEntries.Click += new EventHandler(this.buttonSortEntries_Click);
			this.textId.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.textId.Location = new Point(251, 3);
			this.textId.Name = "textId";
			this.textId.Size = new Size(734, 20);
			this.textId.TabIndex = 11;
			this.buttonSaveDatabase.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonSaveDatabase.Location = new Point(870, 191);
			this.buttonSaveDatabase.Name = "buttonSaveDatabase";
			this.buttonSaveDatabase.Size = new Size(115, 23);
			this.buttonSaveDatabase.TabIndex = 10;
			this.buttonSaveDatabase.Text = "Save database";
			this.buttonSaveDatabase.UseVisualStyleBackColor = true;
			this.buttonSaveDatabase.Click += new EventHandler(this.buttonSaveDatabase_Click);
			this.buttonLocateInstances.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.buttonLocateInstances.Location = new Point(870, 82);
			this.buttonLocateInstances.Name = "buttonLocateInstances";
			this.buttonLocateInstances.Size = new Size(115, 23);
			this.buttonLocateInstances.TabIndex = 9;
			this.buttonLocateInstances.Text = "Locate instances...";
			this.buttonLocateInstances.UseVisualStyleBackColor = true;
			this.buttonLocateInstances.Click += new EventHandler(this.buttonLocateInstances_Click);
			this.buttonBrowseWorld.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.buttonBrowseWorld.Location = new Point(772, 82);
			this.buttonBrowseWorld.Name = "buttonBrowseWorld";
			this.buttonBrowseWorld.Size = new Size(92, 23);
			this.buttonBrowseWorld.TabIndex = 8;
			this.buttonBrowseWorld.Text = "Browse world...";
			this.buttonBrowseWorld.UseVisualStyleBackColor = true;
			this.buttonBrowseWorld.Click += new EventHandler(this.buttonBrowseWorld_Click);
			this.labelWorld.AutoSize = true;
			this.labelWorld.Location = new Point(287, 82);
			this.labelWorld.Name = "labelWorld";
			this.labelWorld.Size = new Size(0, 13);
			this.labelWorld.TabIndex = 7;
			this.label3.AutoSize = true;
			this.label3.Location = new Point(201, 82);
			this.label3.Name = "label3";
			this.label3.Size = new Size(80, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Selected world:";
			this.textTags.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.textTags.Location = new Point(251, 55);
			this.textTags.Name = "textTags";
			this.textTags.Size = new Size(734, 20);
			this.textTags.TabIndex = 5;
			this.label2.AutoSize = true;
			this.label2.Location = new Point(201, 58);
			this.label2.Name = "label2";
			this.label2.Size = new Size(34, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Tags:";
			this.textDisplay.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.textDisplay.Location = new Point(251, 29);
			this.textDisplay.Name = "textDisplay";
			this.textDisplay.Size = new Size(734, 20);
			this.textDisplay.TabIndex = 3;
			this.textDisplay.KeyDown += new KeyEventHandler(this.textDisplay_KeyDown);
			this.textDisplay.Leave += new EventHandler(this.textDisplay_Leave);
			this.label1.AutoSize = true;
			this.label1.Location = new Point(201, 32);
			this.label1.Name = "label1";
			this.label1.Size = new Size(44, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Display:";
			this.label0.AutoSize = true;
			this.label0.Location = new Point(201, 6);
			this.label0.Name = "label0";
			this.label0.Size = new Size(21, 13);
			this.label0.TabIndex = 1;
			this.label0.Text = "ID:";
			this.objectThumbnail.Location = new Point(3, 3);
			this.objectThumbnail.Name = "objectThumbnail";
			this.objectThumbnail.Size = new Size(192, 192);
			this.objectThumbnail.SizeMode = PictureBoxSizeMode.Zoom;
			this.objectThumbnail.TabIndex = 0;
			this.objectThumbnail.TabStop = false;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.splitContainer2);
			base.Name = "ObjectAdmin";
			base.Size = new Size(988, 696);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel2.ResumeLayout(false);
			this.splitContainer2.Panel2.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			this.splitContainer1.ResumeLayout(false);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.toolStrip2.ResumeLayout(false);
			this.toolStrip2.PerformLayout();
			((ISupportInitialize)this.objectThumbnail).EndInit();
			base.ResumeLayout(false);
		}
		public ObjectAdmin()
		{
			this.InitializeComponent();
			this.objectTree.Root = ObjectInventory.Instance.Root;
			this.UpdateProperties();
		}
		private bool IsValidEntry(Inventory.Entry entry)
		{
			return entry != null && entry.IsValid;
		}
		private bool IsNotRootEntry(Inventory.Entry entry)
		{
			return this.IsValidEntry(entry) && entry != ObjectInventory.Instance.Root;
		}
		private void buttonNewFolder_Click(object sender, EventArgs e)
		{
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.objectTree.Value;
			if (!this.IsValidEntry(entry))
			{
				return;
			}
			ObjectInventory.Entry entry2 = ObjectInventory.Instance.CreateDirectory(entry);
			if (!this.IsNotRootEntry(entry2))
			{
				return;
			}
			entry2.DisplayName = "New folder";
			this.objectTree.AddEntry(entry2);
			this.objectTree.SelectedNode.BeginEdit();
		}
		private void buttonRenameFolder_Click(object sender, EventArgs e)
		{
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.objectTree.Value;
			if (this.IsNotRootEntry(entry))
			{
				return;
			}
			this.objectTree.SelectedNode.BeginEdit();
		}
		private void buttonDeleteFolder_Click(object sender, EventArgs e)
		{
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.objectTree.Value;
			if (!this.IsNotRootEntry(entry))
			{
				return;
			}
			if (entry.Count > 0)
			{
				MessageBox.Show("Cannot delete a directory that is not empty.");
				return;
			}
			if (MessageBox.Show("Do you really want to delete the directory '" + entry.DisplayName + "'?", "Delete confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return;
			}
			entry.Deleted = true;
			this.objectTree.RemoveEntry(entry);
		}
		private void objectTree_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (this.m_dragging)
			{
				return;
			}
			ObjectInventory.Entry entry = (ObjectInventory.Entry)e.Node.Tag;
			this.m_root = entry;
			this.objectList.Entries = entry.Children;
		}
		private void objectTree_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			ObjectInventory.Entry entry = (ObjectInventory.Entry)e.Node.Tag;
			if (!this.IsNotRootEntry(entry))
			{
				e.CancelEdit = true;
			}
		}
		private void objectTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			if (e.CancelEdit)
			{
				return;
			}
			ObjectInventory.Entry entry = (ObjectInventory.Entry)e.Node.Tag;
			if (entry != null && entry.IsValid)
			{
				entry.DisplayName = e.Label;
			}
		}
		private void objectTree_ItemDrag(object sender, ItemDragEventArgs e)
		{
			TreeNode treeNode = (TreeNode)e.Item;
			ObjectInventory.Entry entry = (ObjectInventory.Entry)treeNode.Tag;
			if (!this.IsNotRootEntry(entry))
			{
				return;
			}
			this.OnDragStart();
			this.objectTree.DoDragDrop(entry, DragDropEffects.Move);
		}
		private void objectTree_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.Action != DragAction.Continue)
			{
				this.OnDragStop();
			}
		}
		private void objectTree_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
		}
		private List<ObjectInventory.Entry> GetDragContent(IDataObject data)
		{
			List<ObjectInventory.Entry> list = new List<ObjectInventory.Entry>();
			if (data.GetDataPresent(typeof(ObjectInventory.Entry)))
			{
				ObjectInventory.Entry item = data.GetData(typeof(ObjectInventory.Entry)) as ObjectInventory.Entry;
				list.Add(item);
			}
			else
			{
				if (data.GetDataPresent(typeof(ObjectInventory.Entry[])))
				{
					ObjectInventory.Entry[] collection = data.GetData(typeof(ObjectInventory.Entry[])) as ObjectInventory.Entry[];
					list.AddRange(collection);
				}
			}
			return list;
		}
		private void objectTree_DragOver(object sender, DragEventArgs e)
		{
			List<ObjectInventory.Entry> dragContent = this.GetDragContent(e.Data);
			if (dragContent.Count == 0 || (e.AllowedEffect & DragDropEffects.Move) == DragDropEffects.None)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			bool flag = true;
			foreach (ObjectInventory.Entry current in dragContent)
			{
				if (!current.IsDirectory)
				{
					flag = false;
					break;
				}
			}
			bool treeInsertMarkAfter = this.m_treeInsertMarkAfter;
			TreeNode treeInsertMarkNode = this.m_treeInsertMarkNode;
			Point pt = this.objectTree.PointToClient(new Point(e.X, e.Y));
			TreeNode nodeAt = this.objectTree.GetNodeAt(pt);
			if (nodeAt != null)
			{
				e.Effect = DragDropEffects.Move;
				if (flag && pt.Y <= nodeAt.Bounds.Top + 3)
				{
					this.m_treeInsertMarkAfter = false;
					this.m_treeInsertMarkNode = nodeAt;
					this.objectTree.SelectedNode = null;
				}
				else
				{
					if (flag && pt.Y >= nodeAt.Bounds.Bottom - 3)
					{
						this.m_treeInsertMarkAfter = true;
						this.m_treeInsertMarkNode = nodeAt;
						this.objectTree.SelectedNode = null;
					}
					else
					{
						this.m_treeInsertMarkNode = null;
						this.objectTree.SelectedNode = nodeAt;
					}
				}
			}
			else
			{
				e.Effect = DragDropEffects.None;
				this.m_treeInsertMarkNode = null;
			}
			if (treeInsertMarkNode != this.m_treeInsertMarkNode || treeInsertMarkAfter != this.m_treeInsertMarkAfter)
			{
				Win32.SendMessage(this.objectTree.Handle, 4378, this.m_treeInsertMarkAfter ? 1 : 0, (this.m_treeInsertMarkNode != null) ? this.m_treeInsertMarkNode.Handle.ToInt32() : 0);
			}
		}
		private void objectTree_DragDrop(object sender, DragEventArgs e)
		{
			Win32.SendMessage(this.objectTree.Handle, 4378, 0, 0);
			if (e.Effect != DragDropEffects.Move)
			{
				return;
			}
			Point pt = this.objectTree.PointToClient(new Point(e.X, e.Y));
			TreeNode nodeAt = this.objectTree.GetNodeAt(pt);
			if (nodeAt == null)
			{
				return;
			}
			ObjectInventory.Entry entry;
			if (this.m_treeInsertMarkNode == null)
			{
				entry = (ObjectInventory.Entry)nodeAt.Tag;
			}
			else
			{
				entry = (ObjectInventory.Entry)nodeAt.Parent.Tag;
			}
			List<ObjectInventory.Entry> dragContent = this.GetDragContent(e.Data);
			foreach (ObjectInventory.Entry current in dragContent)
			{
				if (this.IsNotRootEntry(current))
				{
					ObjectInventory.Entry entry2 = entry;
					while (entry2 != null && entry2.IsValid)
					{
						if (entry2 == current)
						{
							return;
						}
						entry2 = (ObjectInventory.Entry)entry2.Parent;
					}
					this.objectTree.BeginUpdate();
					this.objectList.BeginUpdate();
					this.objectTree.RemoveEntry(current);
					this.objectList.RemoveEntry(current);
					current.Parent = entry;
					if (this.m_treeInsertMarkNode != null)
					{
						int num = 0;
						Inventory.Entry[] children = entry.Children;
						for (int i = 0; i < children.Length; i++)
						{
							Inventory.Entry x = children[i];
							if (x == (Inventory.Entry)this.m_treeInsertMarkNode.Tag)
							{
								entry.SetChildIndex(current, this.m_treeInsertMarkAfter ? (num + 1) : num);
								break;
							}
							num++;
						}
					}
					this.objectTree.AddEntry(current);
					this.objectTree.EndUpdate();
					this.objectList.EndUpdate();
				}
			}
		}
		private void objectList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.objectList.SelectedItems.Count == 0)
			{
				this.m_currentEntry = null;
			}
			else
			{
				this.m_currentEntry = (ObjectInventory.Entry)this.objectList.SelectedItems[0].Tag;
			}
			this.UpdateProperties();
		}
		private void objectList_AfterLabelEdit(object sender, LabelEditEventArgs e)
		{
			if (e.CancelEdit)
			{
				return;
			}
			ObjectInventory.Entry entry = (ObjectInventory.Entry)this.objectList.Items[e.Item].Tag;
			entry.DisplayName = e.Label;
			this.UpdateProperties();
		}
		private void objectList_ItemDrag(object sender, ItemDragEventArgs e)
		{
			List<ObjectInventory.Entry> list = new List<ObjectInventory.Entry>();
			foreach (ListViewItem listViewItem in this.objectList.SelectedItems)
			{
				list.Add((ObjectInventory.Entry)listViewItem.Tag);
			}
			this.OnDragStart();
			this.objectList.DoDragDrop(list.ToArray(), DragDropEffects.Move);
		}
		private void objectList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.Action != DragAction.Continue)
			{
				this.OnDragStop();
			}
		}
		private void buttonAddPrefab_Click(object sender, EventArgs e)
		{
		}
		private void buttonRenameEntry_Click(object sender, EventArgs e)
		{
			if (this.objectList.SelectedItems.Count == 0)
			{
				return;
			}
			this.objectList.SelectedItems[0].BeginEdit();
		}
		private void buttonSortEntries_Click(object sender, EventArgs e)
		{
			List<Inventory.Entry> list = new List<Inventory.Entry>(this.m_root.Children);
			list.Sort(delegate(Inventory.Entry x, Inventory.Entry y)
			{
				if (x.IsDirectory && !y.IsDirectory)
				{
					return -1;
				}
				if (!x.IsDirectory && y.IsDirectory)
				{
					return 1;
				}
				return x.DisplayName.CompareTo(y.DisplayName);
			}
			);
			this.m_root.ClearChildren();
			foreach (Inventory.Entry current in list)
			{
				this.m_root.AddChild(current);
			}
			this.objectList.Entries = this.m_root.Children;
		}
		private void objectList_DoubleClick(object sender, EventArgs e)
		{
			if (this.objectList.SelectedItems.Count == 0)
			{
				return;
			}
			MainForm.Instance.CurrentTool = ToolObject.Instance;
			ToolObject.Instance.SwitchMode(ToolObject.Instance.AddModeObj);
			ToolObject.Instance.AddModeObj.SetGotoObject((ObjectInventory.Entry)this.objectList.SelectedItems[0].Tag);
		}
		private void OnDragStart()
		{
			this.m_dragging = true;
			this.m_dragOldTreeNode = this.objectTree.SelectedNode;
		}
		private void OnDragStop()
		{
			this.objectTree.SelectedNode = this.m_dragOldTreeNode;
			this.m_dragging = false;
		}
		private void UpdateProperties()
		{
			this.UpdateWorldText();
			bool flag = this.m_currentEntry != null;
			this.textId.Enabled = false;
			this.textDisplay.Enabled = flag;
			this.textTags.Enabled = flag;
			this.buttonLocateInstances.Enabled = flag;
			if (!flag)
			{
				this.objectThumbnail.Image = null;
				this.textId.Text = "No entry selected";
				this.textDisplay.Text = "";
				this.textTags.Text = "";
				return;
			}
			MemoryStream thumbnailData = this.m_currentEntry.GetThumbnailData();
			if (thumbnailData != null)
			{
				Image image = Image.FromStream(thumbnailData);
				this.objectThumbnail.Image = image;
				thumbnailData.Dispose();
			}
			this.textId.Text = this.m_currentEntry.IdString;
			this.textDisplay.Text = this.m_currentEntry.DisplayName;
			this.textTags.Text = this.m_currentEntry.Tags;
		}
		private void textDisplay_Leave(object sender, EventArgs e)
		{
			this.m_currentEntry.DisplayName = this.textDisplay.Text;
		}
		private void textDisplay_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.textDisplay.Text = this.m_currentEntry.DisplayName;
				return;
			}
			if (e.KeyCode == Keys.Return)
			{
				this.m_currentEntry.DisplayName = this.textDisplay.Text;
			}
		}
		private void UpdateWorldText()
		{
			this.labelWorld.Text = WorldImport.Instance.WorldFile;
		}
		private void buttonBrowseWorld_Click(object sender, EventArgs e)
		{
			if (!WorldImport.Instance.LoadWorld())
			{
				return;
			}
			this.UpdateWorldText();
		}
		private void buttonLocateInstances_Click(object sender, EventArgs e)
		{
			if (!File.Exists(WorldImport.Instance.WorldFile))
			{
				MessageBox.Show("Need a valid world to search from.");
				return;
			}
			List<WorldImport.ObjectInstance> list = WorldImport.Instance.FindInstances(this.m_currentEntry.IdString);
			if (list.Count == 0)
			{
				MessageBox.Show("No instance of this entry could be found.");
				return;
			}
			using (WorldLocateInstanceForm worldLocateInstanceForm = new WorldLocateInstanceForm())
			{
				worldLocateInstanceForm.Instances = list;
				if (worldLocateInstanceForm.ShowDialog(this) == DialogResult.OK)
				{
					WorldImport.ObjectInstance selectedInstance = worldLocateInstanceForm.SelectedInstance;
					if (selectedInstance != null)
					{
						int num = selectedInstance.sectorX - 3;
						int num2 = selectedInstance.sectorY - 3;
						if (WorldImport.Instance.ImportWorld(num, num2, (!string.IsNullOrEmpty(selectedInstance.layer)) ? new string[]
						{
							selectedInstance.layer
						} : new string[0]))
						{
							Vec3 v = new Vec3(selectedInstance.localX, selectedInstance.localY, selectedInstance.localZ);
							v.X += (float)((selectedInstance.sectorX - num) * 64);
							v.Y += (float)((selectedInstance.sectorY - num2) * 64);
							foreach (EditorObject current in ObjectManager.GetObjects())
							{
								if (current.IsValid && current.Entry.Id == this.m_currentEntry.Id)
								{
									Vec3 position = current.Position;
									if ((position - v).Length <= 0.1f)
									{
										MainForm.Instance.CurrentTool = ToolObject.Instance;
										ToolObject.Instance.SwitchMode(ToolObject.Instance.MoveModeObj);
										EditorObjectSelection selection = EditorObjectSelection.Create();
										selection.AddObject(current);
										ToolObject.Instance.SetSelection(selection, current);
										Camera.Focus(current);
										break;
									}
								}
							}
						}
					}
				}
			}
		}
		private void buttonAddTags_Click(object sender, EventArgs e)
		{
			if (MainForm.Instance.CurrentTool != ToolObject.Instance)
			{
				MessageBox.Show("The object tool must be active to perform this action.");
				return;
			}
			EditorObjectSelection selection = ToolObject.Instance.Selection;
			if (!selection.IsValid || selection.Count == 0)
			{
				MessageBox.Show("Must select one or more objects in the map to perform this action.");
				return;
			}
			using (PromptForm promptForm = new PromptForm("List of tags to add to selection"))
			{
				if (promptForm.ShowDialog() == DialogResult.OK)
				{
					string[] array = promptForm.Input.Split(new char[]
					{
						','
					}, StringSplitOptions.RemoveEmptyEntries);
					if (!string.IsNullOrEmpty(promptForm.Input))
					{
						foreach (EditorObject current in selection.GetObjects())
						{
							ObjectInventory.Entry entry = current.Entry;
							List<string> list = new List<string>(entry.Tags.Split(new char[]
							{
								','
							}, StringSplitOptions.RemoveEmptyEntries));
							string[] array2 = array;
							for (int i = 0; i < array2.Length; i++)
							{
								string item = array2[i];
								if (!list.Contains(item))
								{
									list.Add(item);
								}
							}
							list.Sort();
							entry.Tags = string.Join(",", list.ToArray());
						}
					}
				}
			}
		}
		private void buttonSaveDatabase_Click(object sender, EventArgs e)
		{
			ObjectInventory.Instance.SaveChanges();
		}
	}
}
