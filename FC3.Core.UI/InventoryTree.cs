using FC3Editor.Nomad;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class InventoryTree : TreeView
	{
		private Inventory.Entry m_root;
		private bool m_onlyDirectories;
		private bool m_showRoot;
		private Dictionary<Inventory.Entry, TreeNode> m_nodes = new Dictionary<Inventory.Entry, TreeNode>();
		private ImageList imageList;
		public event EventHandler ValueChanged;
		public Inventory.Entry Root
		{
			get
			{
				return this.m_root;
			}
			set
			{
				this.m_root = value;
				this.UpdateTree();
			}
		}
		public bool OnlyDirectories
		{
			get
			{
				return this.m_onlyDirectories;
			}
			set
			{
				this.m_onlyDirectories = value;
				this.UpdateTree();
			}
		}
		public bool ShowRoot
		{
			get
			{
				return this.m_showRoot;
			}
			set
			{
				this.m_showRoot = value;
				this.UpdateTree();
			}
		}
		public Inventory.Entry Value
		{
			get
			{
				if (base.SelectedNode == null)
				{
					return null;
				}
				return (Inventory.Entry)base.SelectedNode.Tag;
			}
			set
			{
				TreeNode treeNode;
				if (value == null || !this.m_nodes.TryGetValue(value, out treeNode))
				{
					return;
				}
				base.SelectedNode = treeNode;
				treeNode.EnsureVisible();
			}
		}
		public InventoryTree()
		{
			this.imageList = new ImageList();
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new Size(16, 16);
			base.ImageList = this.imageList;
			base.HideSelection = false;
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			this.imageList.Dispose();
		}
		private TreeNode CreateNode(Inventory.Entry entry)
		{
			TreeNode treeNode = new TreeNode();
			treeNode.Text = entry.DisplayName;
			treeNode.Tag = entry;
			string iconName = entry.IconName;
			if (!this.imageList.Images.ContainsKey(iconName))
			{
				this.imageList.Images.Add(iconName, entry.Icon);
			}
			treeNode.ImageKey = iconName;
			treeNode.SelectedImageKey = iconName;
			this.m_nodes.Add(entry, treeNode);
			return treeNode;
		}
		private void FillNodes(Inventory.Entry root, TreeNodeCollection nodes)
		{
			Inventory.Entry[] children = root.Children;
			for (int i = 0; i < children.Length; i++)
			{
				Inventory.Entry entry = children[i];
				if ((!this.m_onlyDirectories || entry.IsDirectory) && !entry.Deleted)
				{
					TreeNode treeNode = this.CreateNode(entry);
					nodes.Add(treeNode);
					this.FillNodes(entry, treeNode.Nodes);
				}
			}
		}
		private void UpdateTree()
		{
			base.BeginUpdate();
			this.imageList.Images.Clear();
			base.Nodes.Clear();
			this.m_nodes.Clear();
			if (this.m_root != null)
			{
				TreeNodeCollection nodes;
				if (this.ShowRoot)
				{
					TreeNode treeNode = base.Nodes.Add("Root");
					treeNode.Tag = this.m_root;
					this.m_nodes.Add(this.m_root, treeNode);
					nodes = treeNode.Nodes;
				}
				else
				{
					nodes = base.Nodes;
				}
				this.FillNodes(this.m_root, nodes);
			}
			base.EndUpdate();
		}
		protected override void OnAfterSelect(TreeViewEventArgs e)
		{
			base.OnAfterSelect(e);
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, e);
			}
		}
		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			if (base.LabelEdit && e.KeyData == Keys.F2 && base.SelectedNode != null)
			{
				base.SelectedNode.BeginEdit();
			}
		}
		public void AddEntry(Inventory.Entry entry)
		{
			if (this.OnlyDirectories && !entry.IsDirectory)
			{
				return;
			}
			TreeNode treeNode;
			this.m_nodes.TryGetValue(entry.Parent, out treeNode);
			if (treeNode == null)
			{
				return;
			}
			TreeNode treeNode2 = this.CreateNode(entry);
			int num = -1;
			Inventory.Entry[] children = entry.Parent.Children;
			for (int i = 0; i < children.Length; i++)
			{
				Inventory.Entry entry2 = children[i];
				if (entry2 == entry)
				{
					break;
				}
				TreeNode treeNode3;
				this.m_nodes.TryGetValue(entry2, out treeNode3);
				if (treeNode3 != null && treeNode3.Parent == treeNode)
				{
					num = treeNode3.Index + 1;
				}
			}
			if (num == -1)
			{
				treeNode.Nodes.Add(treeNode2);
			}
			else
			{
				treeNode.Nodes.Insert(num, treeNode2);
			}
			this.FillNodes(entry, treeNode2.Nodes);
			this.Value = entry;
		}
		public void RemoveEntry(Inventory.Entry entry)
		{
			TreeNode treeNode;
			this.m_nodes.TryGetValue(entry, out treeNode);
			if (treeNode == null)
			{
				return;
			}
			Inventory.Entry[] children = entry.Children;
			for (int i = 0; i < children.Length; i++)
			{
				Inventory.Entry entry2 = children[i];
				this.RemoveEntry(entry2);
			}
			treeNode.Remove();
			this.m_nodes.Remove(entry);
		}
	}
}
