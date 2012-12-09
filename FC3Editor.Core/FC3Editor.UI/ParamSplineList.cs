using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamSplineList : UserControl
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
		public ParamSplineList()
		{
			this.InitializeComponent();
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonAssign.Text = Localizer.Localize(this.buttonAssign.Text);
			this.buttonClear.Text = Localizer.Localize(this.buttonClear.Text);
		}
		private void ParamSplineList_Load(object sender, EventArgs e)
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
				SplineRoad roadFromId = SplineManager.GetRoadFromId(i);
				bool flag = false;
				string text = null;
				string text2 = null;
				if (roadFromId.IsValid)
				{
					SplineInventory.Entry entry = roadFromId.Entry;
					if (entry.IsValid)
					{
						text2 = entry.IconName;
						if (!this.imageList.Images.ContainsKey(text2))
						{
							this.imageList.Images.Add(text2, entry.Icon);
						}
						text = entry.DisplayName;
						flag = true;
					}
				}
				if (!flag)
				{
					text2 = "empty16";
					if (!this.imageList.Images.ContainsKey(text2))
					{
						this.imageList.Images.Add(text2, Resources.empty16);
					}
					text = Localizer.Localize("PARAM_EMPTY");
				}
				SplineTreeItem tag = new SplineTreeItem(i, roadFromId);
				TreeNode treeNode = this.treeView.Nodes.Add(text);
				treeNode.ImageKey = text2;
				treeNode.SelectedImageKey = text2;
				treeNode.Tag = tag;
			}
			this.UpdateSelection();
			this.treeView.EndUpdate();
		}
		private void UpdateSelection()
		{
			if (this.m_value >= 0 && this.m_value < this.treeView.Nodes.Count && this.treeView.SelectedNode != this.treeView.Nodes[this.m_value])
			{
				this.treeView.SelectedNode = this.treeView.Nodes[this.m_value];
			}
			this.UpdateButtons();
		}
		private void UpdateButtons()
		{
			SplineTreeItem splineTreeItem = (this.treeView.SelectedNode != null) ? ((SplineTreeItem)this.treeView.SelectedNode.Tag) : null;
			if (splineTreeItem != null)
			{
				this.buttonAssign.Enabled = true;
				this.buttonClear.Enabled = (splineTreeItem.Spline.IsValid && splineTreeItem.Spline.Entry.IsValid);
				return;
			}
			this.buttonAssign.Enabled = false;
			this.buttonClear.Enabled = false;
		}
		private SplineTreeItem GetCurrentItem()
		{
			if (this.treeView.SelectedNode == null)
			{
				return null;
			}
			return (SplineTreeItem)this.treeView.SelectedNode.Tag;
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
			SplineTreeItem currentItem = this.GetCurrentItem();
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
			SplineTreeItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			using (PromptInventory promptInventory = new PromptInventory())
			{
				promptInventory.Root = SplineInventory.Instance.Root;
				promptInventory.Value = (currentItem.Spline.IsValid ? currentItem.Spline.Entry : SplineInventory.Entry.Null);
				if (promptInventory.ShowDialog(this) != DialogResult.Cancel)
				{
					this.AssignSplineId(currentItem.Id, (SplineInventory.Entry)promptInventory.Value);
				}
			}
		}
		private void buttonClear_Click(object sender, EventArgs e)
		{
			SplineTreeItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			this.AssignSplineId(currentItem.Id, SplineInventory.Entry.Null);
		}
		private void AssignSplineId(int id, SplineInventory.Entry entry)
		{
			Win32.SetRedraw(this, false);
			UndoManager.RecordUndo();
			if (!entry.IsValid)
			{
				SplineManager.DestroyRoad(id);
			}
			else
			{
				SplineRoad splineRoad = SplineManager.GetRoadFromId(id);
				if (!splineRoad.IsValid)
				{
					splineRoad = SplineManager.CreateRoad(id);
				}
				splineRoad.Entry = entry;
				splineRoad.UpdateSpline();
			}
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
			this.treeView = new TreeView();
			this.imageList = new ImageList(this.components);
			this.buttonClear = new NomadButton();
			this.buttonAssign = new NomadButton();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(89, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "PARAM_ROADS";
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Controls.Add(this.buttonClear, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonAssign, 0, 0);
			this.tableLayoutPanel1.Location = new Point(3, 156);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Size = new Size(212, 29);
			this.tableLayoutPanel1.TabIndex = 4;
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
			this.treeView.Size = new Size(212, 137);
			this.treeView.TabIndex = 5;
			this.treeView.MouseDoubleClick += new MouseEventHandler(this.treeView_MouseDoubleClick);
			this.treeView.AfterSelect += new TreeViewEventHandler(this.treeView_AfterSelect);
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new Size(16, 16);
			this.imageList.TransparentColor = Color.Transparent;
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
			this.buttonAssign.UseVisualStyleBackColor = true;
			this.buttonAssign.Click += new EventHandler(this.buttonAssign_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.treeView);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.label1);
			base.Name = "ParamSplineList";
			base.Size = new Size(218, 188);
			base.Load += new EventHandler(this.ParamSplineList_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
