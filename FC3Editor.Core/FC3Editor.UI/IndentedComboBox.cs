using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class IndentedComboBox : UserControl
	{
		public class Item
		{
			private IndentedComboBox m_owner;
			private IndentedComboBox.Item m_parent;
			private Image m_image;
			private string m_text;
			private string m_subText;
			private object m_tag;
			private int m_depth;
			private List<IndentedComboBox.Item> m_childList = new List<IndentedComboBox.Item>();
			public IndentedComboBox Owner
			{
				get
				{
					return this.m_owner;
				}
				set
				{
					this.m_owner = value;
				}
			}
			public IndentedComboBox.Item Parent
			{
				get
				{
					return this.m_parent;
				}
				set
				{
					this.m_parent = value;
				}
			}
			public Image Image
			{
				get
				{
					return this.m_image;
				}
				set
				{
					this.m_image = value;
				}
			}
			public string Text
			{
				get
				{
					return this.m_text;
				}
				set
				{
					this.m_text = value;
				}
			}
			public string SubText
			{
				get
				{
					return this.m_subText;
				}
				set
				{
					this.m_subText = value;
				}
			}
			public object Tag
			{
				get
				{
					return this.m_tag;
				}
				set
				{
					this.m_tag = value;
				}
			}
			public int Depth
			{
				get
				{
					return this.m_depth;
				}
				protected set
				{
					this.m_depth = value;
				}
			}
			public void Clear()
			{
				this.m_childList.Clear();
			}
			public void Add(IndentedComboBox.Item item)
			{
				this.m_childList.Add(item);
				item.Depth = this.Depth + 1;
				item.Owner = this.Owner;
			}
			public IEnumerable<IndentedComboBox.Item> GetChildren()
			{
				foreach (IndentedComboBox.Item current in this.m_childList)
				{
					yield return current;
				}
				yield break;
			}
		}
		private Font m_subFont;
		private IndentedComboBox.Item m_root;
		private IContainer components;
		private ComboBox comboBox;
		public event EventHandler<IndentedComboboxItemEventArgs> SelectedItemChanged;
		public IndentedComboBox.Item Root
		{
			get
			{
				return this.m_root;
			}
		}
		public IndentedComboBox.Item SelectedItem
		{
			get
			{
				return this.comboBox.SelectedItem as IndentedComboBox.Item;
			}
			set
			{
				this.comboBox.SelectedItem = value;
			}
		}
		public IndentedComboBox.Item FirstItem
		{
			get
			{
				if (this.comboBox.Items.Count <= 0)
				{
					return null;
				}
				return (IndentedComboBox.Item)this.comboBox.Items[0];
			}
		}
		public IndentedComboBox()
		{
			this.InitializeComponent();
			this.m_root = new IndentedComboBox.Item();
			this.m_root.Owner = this;
			this.OnFontChanged(new EventArgs());
		}
		private void DisposeInternal()
		{
			if (this.m_subFont != null)
			{
				this.m_subFont.Dispose();
			}
		}
		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			if (this.m_subFont != null)
			{
				this.m_subFont.Dispose();
			}
			this.m_subFont = new Font(this.Font.FontFamily, this.Font.Size * 0.8f);
		}
		private void UpdateItem(IndentedComboBox.Item item, IndentedComboBox.Item selectedItem, ref int index)
		{
			foreach (IndentedComboBox.Item current in item.GetChildren())
			{
				if (current == selectedItem)
				{
					index = this.comboBox.Items.Count;
				}
				this.comboBox.Items.Add(current);
				this.UpdateItem(current, selectedItem, ref index);
			}
		}
		public void UpdateItems()
		{
			this.comboBox.BeginUpdate();
			IndentedComboBox.Item selectedItem = this.comboBox.SelectedItem as IndentedComboBox.Item;
			this.comboBox.Items.Clear();
			int num = -1;
			this.UpdateItem(this.m_root, selectedItem, ref num);
			if (num == -1 && this.comboBox.Items.Count > 0)
			{
				num = 0;
			}
			this.comboBox.SelectedIndex = num;
			this.comboBox.EndUpdate();
			this.comboBox.MaxDropDownItems = Math.Min(this.comboBox.Items.Count, 800 / this.comboBox.ItemHeight);
		}
		private void comboBox_DrawItem(object sender, DrawItemEventArgs e)
		{
			if (e.Index == -1)
			{
				return;
			}
			RectangleF rectangleF = e.Bounds;
			IndentedComboBox.Item item = this.comboBox.Items[e.Index] as IndentedComboBox.Item;
			SizeF sizeF = e.Graphics.MeasureString(item.Text, e.Font);
			SizeF sizeF2;
			if (!string.IsNullOrEmpty(item.SubText))
			{
				sizeF2 = e.Graphics.MeasureString(item.SubText, this.m_subFont);
			}
			else
			{
				sizeF2 = default(SizeF);
			}
			bool flag = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
			if (flag)
			{
				e.DrawBackground();
			}
			else
			{
				int num = 4 + (item.Depth - 1) * 16;
				rectangleF.X += (float)num;
				rectangleF.Width -= (float)num;
			}
			if (item.Image != null)
			{
				e.Graphics.DrawImage(item.Image, new RectangleF(rectangleF.Location, new SizeF(16f, 16f)));
				rectangleF.X += 16f;
			}
			rectangleF.Width = sizeF.Width;
			if (!flag)
			{
				using (Brush brush = new SolidBrush(e.BackColor))
				{
					e.Graphics.FillRectangle(brush, rectangleF);
				}
			}
			using (Brush brush2 = new SolidBrush(e.ForeColor))
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.LineAlignment = StringAlignment.Center;
				e.Graphics.DrawString(item.Text, e.Font, brush2, rectangleF, stringFormat);
			}
			if (!string.IsNullOrEmpty(item.SubText))
			{
				rectangleF.X += rectangleF.Width;
				rectangleF.Width = sizeF2.Width;
				rectangleF.Y -= 1f;
				using (Brush brush3 = new SolidBrush(Color.DarkGray))
				{
					StringFormat stringFormat2 = new StringFormat();
					stringFormat2.LineAlignment = StringAlignment.Center;
					stringFormat2.Alignment = StringAlignment.Near;
					e.Graphics.DrawString(item.SubText, this.m_subFont, brush3, rectangleF, stringFormat2);
				}
				rectangleF.Y += 1f;
			}
		}
		public IEnumerable<IndentedComboBox.Item> GetItems()
		{
			foreach (IndentedComboBox.Item item in this.comboBox.Items)
			{
				yield return item;
			}
			yield break;
		}
		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.SelectedItemChanged != null)
			{
				this.SelectedItemChanged(this, new IndentedComboboxItemEventArgs(this.SelectedItem));
			}
		}
		protected override void Dispose(bool disposing)
		{
			this.DisposeInternal();
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}
		private void InitializeComponent()
		{
			this.comboBox = new ComboBox();
			base.SuspendLayout();
			this.comboBox.Dock = DockStyle.Fill;
			this.comboBox.DrawMode = DrawMode.OwnerDrawFixed;
			this.comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBox.ItemHeight = 18;
			this.comboBox.Location = new Point(0, 0);
			this.comboBox.Margin = new Padding(0);
			this.comboBox.MaxDropDownItems = 16;
			this.comboBox.Name = "comboBox";
			this.comboBox.Size = new Size(245, 24);
			this.comboBox.TabIndex = 0;
			this.comboBox.DrawItem += new DrawItemEventHandler(this.comboBox_DrawItem);
			this.comboBox.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.comboBox);
			this.MaximumSize = new Size(9999, 24);
			this.MinimumSize = new Size(0, 24);
			base.Name = "IndentedComboBox";
			base.Size = new Size(245, 24);
			base.ResumeLayout(false);
		}
	}
}
