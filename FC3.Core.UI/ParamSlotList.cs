using FC3Editor.Nomad;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamSlotList : UserControl
	{
		private IContainer components;
		protected Label label1;
		private TableLayoutPanel tableLayoutPanel1;
		private NomadButton buttonRemove;
		private NomadButton buttonAdd;
		private ImageList imageList;
		private Label labelCount;
		private InventoryList inventoryList;
		private bool m_doubleClick;
		private SlotItem[] m_slots = new SlotItem[0];
		private int m_value;
		public event EventHandler ValueChanged;
		protected virtual Inventory.Entry Root
		{
			get
			{
				return null;
			}
		}
		protected virtual int SlotCount
		{
			get
			{
				return 0;
			}
		}
		protected virtual bool KeepFirst
		{
			get
			{
				return false;
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
			this.buttonRemove = new NomadButton();
			this.buttonAdd = new NomadButton();
			this.imageList = new ImageList(this.components);
			this.labelCount = new Label();
			this.inventoryList = new InventoryList(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(42, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "Header";
			this.tableLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 0, 0);
			this.tableLayoutPanel1.Location = new Point(3, 193);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
			this.tableLayoutPanel1.Size = new Size(212, 29);
			this.tableLayoutPanel1.TabIndex = 4;
			this.buttonRemove.Dock = DockStyle.Fill;
			this.buttonRemove.Location = new Point(109, 3);
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.Size = new Size(100, 23);
			this.buttonRemove.TabIndex = 4;
			this.buttonRemove.Text = "PARAM_REMOVE";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			this.buttonAdd.Dock = DockStyle.Fill;
			this.buttonAdd.Location = new Point(3, 3);
			this.buttonAdd.Name = "buttonAdd";
			this.buttonAdd.Size = new Size(100, 23);
			this.buttonAdd.TabIndex = 3;
			this.buttonAdd.Text = "PARAM_ADD";
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new Size(16, 16);
			this.imageList.TransparentColor = Color.Transparent;
			this.labelCount.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.labelCount.Location = new Point(165, 0);
			this.labelCount.Name = "labelCount";
			this.labelCount.Size = new Size(50, 13);
			this.labelCount.TabIndex = 6;
			this.labelCount.Text = "0/0";
			this.labelCount.TextAlign = ContentAlignment.TopRight;
			this.inventoryList.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.inventoryList.Entries = null;
			this.inventoryList.Location = new Point(6, 16);
			this.inventoryList.Name = "inventoryList";
			this.inventoryList.OwnerDraw = true;
			this.inventoryList.Size = new Size(206, 174);
			this.inventoryList.TabIndex = 7;
			this.inventoryList.UseCompatibleStateImageBehavior = false;
			this.inventoryList.View = View.Tile;
			this.inventoryList.SelectedIndexChanged += new EventHandler(this.inventoryList_SelectedIndexChanged);
			this.inventoryList.MouseDown += new MouseEventHandler(this.inventoryList_MouseDown);
			this.inventoryList.MouseUp += new MouseEventHandler(this.inventoryList_MouseUp);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.inventoryList);
			base.Controls.Add(this.labelCount);
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.label1);
			base.Name = "ParamSlotList";
			base.Size = new Size(218, 225);
			base.Load += new EventHandler(this.ParamSlotList_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamSlotList()
		{
			this.InitializeComponent();
			this.buttonAdd.Text = Localizer.Localize(this.buttonAdd.Text);
			this.buttonRemove.Text = Localizer.Localize(this.buttonRemove.Text);
			this.inventoryList.SlotMode = true;
		}
		private void ParamSlotList_Load(object sender, EventArgs e)
		{
			this.UpdateList();
		}
		public void UpdateUI()
		{
			this.UpdateList();
		}
		protected virtual SlotItem GetSlot(int index)
		{
			return null;
		}
		private void UpdateList()
		{
			List<SlotItem> list = new List<SlotItem>();
			for (int i = 0; i < this.SlotCount; i++)
			{
				SlotItem slot = this.GetSlot(i);
				if (slot != null)
				{
					list.Add(slot);
				}
			}
			this.m_slots = list.ToArray();
			Inventory.Entry[] array = new Inventory.Entry[this.m_slots.Length];
			for (int j = 0; j < this.m_slots.Length; j++)
			{
				array[j] = this.m_slots[j].Entry;
			}
			this.inventoryList.Entries = array;
			this.labelCount.Text = this.m_slots.Length + "/" + this.SlotCount;
			this.UpdateSelection();
		}
		private void UpdateSelection()
		{
			for (int i = 0; i < this.m_slots.Length; i++)
			{
				if (this.m_slots[i].Id == this.m_value)
				{
					this.inventoryList.SelectedIndices.Clear();
					this.inventoryList.SelectedIndices.Add(i);
					break;
				}
			}
			this.UpdateButtons();
		}
		private SlotItem GetCurrentItem()
		{
			if (this.inventoryList.SelectedIndices.Count == 0)
			{
				return null;
			}
			int num = this.inventoryList.SelectedIndices[0];
			if (num < 0 || num >= this.m_slots.Length)
			{
				return null;
			}
			return this.m_slots[num];
		}
		private void UpdateButtons()
		{
			this.buttonAdd.Enabled = (this.m_slots.Length < this.SlotCount);
			SlotItem currentItem = this.GetCurrentItem();
			if (currentItem != null)
			{
				this.buttonRemove.Enabled = (currentItem.Entry.IsValid && (!this.KeepFirst || currentItem.Id != 0));
				return;
			}
			this.buttonRemove.Enabled = false;
		}
		private void buttonAdd_Click(object sender, EventArgs e)
		{
			this.PromptSlotAssignment(null);
		}
		private void buttonRemove_Click(object sender, EventArgs e)
		{
			SlotItem currentItem = this.GetCurrentItem();
			if (currentItem == null)
			{
				return;
			}
			if (this.KeepFirst && currentItem.Id == 0)
			{
				return;
			}
			this.AssignSlot(currentItem.Id, null);
		}
		private void inventoryList_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Clicks == 2)
			{
				this.m_doubleClick = true;
				return;
			}
			this.m_doubleClick = false;
		}
		private void inventoryList_MouseUp(object sender, MouseEventArgs e)
		{
			if (this.m_doubleClick)
			{
				this.m_doubleClick = false;
				SlotItem currentItem = this.GetCurrentItem();
				if (currentItem == null && this.m_slots.Length >= this.SlotCount)
				{
					return;
				}
				this.PromptSlotAssignment(currentItem);
			}
		}
		private void inventoryList_SelectedIndexChanged(object sender, EventArgs e)
		{
			SlotItem currentItem = this.GetCurrentItem();
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
		private void PromptSlotAssignment(SlotItem item)
		{
			using (PromptInventoryList promptInventoryList = new PromptInventoryList())
			{
				promptInventoryList.Root = this.Root;
				if (item != null)
				{
					promptInventoryList.Value = item.Entry;
				}
				if (promptInventoryList.ShowDialog(this) != DialogResult.Cancel)
				{
					int num = (item != null) ? item.Id : this.FindFreeSlot();
					if (num >= 0)
					{
						this.AssignSlot(num, promptInventoryList.Value);
					}
				}
			}
		}
		private int FindFreeSlot()
		{
			for (int i = 0; i < this.SlotCount; i++)
			{
				if (this.GetSlot(i) == null)
				{
					return i;
				}
			}
			return -1;
		}
		private void AssignSlot(int id, Inventory.Entry entry)
		{
			Win32.SetRedraw(this, false);
			UndoManager.RecordUndo();
			this.OnAssignSlot(id, entry);
			UndoManager.CommitUndo();
			if (entry != null)
			{
				this.OnValueChanged(id);
			}
			else
			{
				this.OnValueChanged(-1);
			}
			this.UpdateList();
			Win32.SetRedraw(this, true);
			this.Refresh();
		}
		protected virtual void OnAssignSlot(int id, Inventory.Entry entry)
		{
		}
		private void OnValueChanged(int value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
