using FC3Editor.Nomad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamObjectSelectionList : UserControl
	{
		private IContainer components;
		private Label labelTitle;
		private TreeView treeViewObjectSelection;
		private EditorObjectSelection m_objectSelection;
		private bool m_updatingList;
		private int m_value;
		private EditorObject m_currentObject;
		public event EventHandler ValueChanged;
		public event EventHandler CurrentObjectChanged;
		public event EventHandler PerformAction;
		public EditorObjectSelection ObjectSelection
		{
			get
			{
				return this.m_objectSelection;
			}
			set
			{
				this.m_objectSelection = value;
				this.UpdateSelectionList();
			}
		}
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
		public EditorObject CurrentObject
		{
			get
			{
				return this.m_currentObject;
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
			this.labelTitle = new Label();
			this.treeViewObjectSelection = new TreeView();
			base.SuspendLayout();
			this.labelTitle.AutoSize = true;
			this.labelTitle.Location = new Point(3, 0);
			this.labelTitle.Name = "labelTitle";
			this.labelTitle.Size = new Size(158, 13);
			this.labelTitle.TabIndex = 2;
			this.labelTitle.Text = "PARAM_OBJECT_SELECTION";
			this.treeViewObjectSelection.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.treeViewObjectSelection.HideSelection = false;
			this.treeViewObjectSelection.Location = new Point(6, 16);
			this.treeViewObjectSelection.Name = "treeViewObjectSelection";
			this.treeViewObjectSelection.ShowLines = false;
			this.treeViewObjectSelection.ShowPlusMinus = false;
			this.treeViewObjectSelection.ShowRootLines = false;
			this.treeViewObjectSelection.Size = new Size(185, 175);
			this.treeViewObjectSelection.TabIndex = 6;
			this.treeViewObjectSelection.AfterSelect += new TreeViewEventHandler(this.treeViewObjectSelection_AfterSelect);
			this.treeViewObjectSelection.DoubleClick += new EventHandler(this.treeViewObjectSelection_DoubleClick);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeViewObjectSelection);
			base.Controls.Add(this.labelTitle);
			base.Name = "ParamObjectSelectionList";
			base.Size = new Size(196, 194);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamObjectSelectionList()
		{
			this.InitializeComponent();
			this.labelTitle.Text = Localizer.Localize(this.labelTitle.Text);
		}
		private void UpdateSelectionList()
		{
			if (this.m_updatingList)
			{
				return;
			}
			this.m_updatingList = true;
			bool flag = false;
			List<EditorObject> list = this.m_objectSelection.IsValid ? new List<EditorObject>(this.m_objectSelection.GetObjects()) : new List<EditorObject>();
			int count = list.Count;
			int count2 = this.treeViewObjectSelection.Nodes.Count;
			int num = Math.Min(count, count2);
			for (int i = 0; i < num; i++)
			{
				TreeNode treeNode = this.treeViewObjectSelection.Nodes[i];
				EditorObject editorObject = list[i];
				if (((EditorObject)treeNode.Tag).Pointer != editorObject.Pointer)
				{
					if (!flag)
					{
						this.treeViewObjectSelection.BeginUpdate();
						flag = true;
					}
					treeNode.Text = editorObject.Entry.DisplayName;
					treeNode.Tag = editorObject;
				}
			}
			if (count != count2)
			{
				if (!flag)
				{
					this.treeViewObjectSelection.BeginUpdate();
					flag = true;
				}
				if (count > count2)
				{
					for (int j = count2; j < count; j++)
					{
						EditorObject editorObject2 = list[j];
						TreeNode treeNode2 = this.treeViewObjectSelection.Nodes.Add(editorObject2.Entry.DisplayName);
						treeNode2.Tag = editorObject2;
					}
				}
				else
				{
					for (int k = count; k < count2; k++)
					{
						this.treeViewObjectSelection.Nodes.RemoveAt(count);
					}
				}
			}
			if (flag)
			{
				this.treeViewObjectSelection.EndUpdate();
			}
			this.m_updatingList = false;
			this.UpdateSelection();
		}
		private EditorObject GetCurrentItem()
		{
			if (this.treeViewObjectSelection.SelectedNode == null)
			{
				return EditorObject.Null;
			}
			return (EditorObject)this.treeViewObjectSelection.SelectedNode.Tag;
		}
		private void UpdateSelection()
		{
			if (this.m_value >= 0 && this.m_value < this.treeViewObjectSelection.Nodes.Count)
			{
				this.treeViewObjectSelection.SelectedNode = this.treeViewObjectSelection.Nodes[this.m_value];
				return;
			}
			this.treeViewObjectSelection.SelectedNode = null;
		}
		private void OnSelectionChanged()
		{
			if (this.m_updatingList)
			{
				return;
			}
			int index = this.treeViewObjectSelection.SelectedNode.Index;
			if (index == this.m_value)
			{
				return;
			}
			this.m_value = index;
			this.m_currentObject = (EditorObject)this.treeViewObjectSelection.SelectedNode.Tag;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
			if (this.CurrentObjectChanged != null)
			{
				this.CurrentObjectChanged(this, new EventArgs());
			}
		}
		private void OnPerformAction()
		{
			if (this.PerformAction != null)
			{
				this.PerformAction(this, new EventArgs());
			}
		}
		private void treeViewObjectSelection_DoubleClick(object sender, EventArgs e)
		{
			this.OnPerformAction();
		}
		private void treeViewObjectSelection_AfterSelect(object sender, TreeViewEventArgs e)
		{
			this.OnSelectionChanged();
		}
	}
}
