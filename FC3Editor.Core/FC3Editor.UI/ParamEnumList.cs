using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamEnumList : UserControl
	{
		private IContainer components;
		private Label parameterName;
		private ListView valueList;
		private ImageList imageListEnum;
		private object m_value;
		public event EventHandler ValueChanged;
		public string ParameterName
		{
			get
			{
				return this.parameterName.Text;
			}
			set
			{
				this.parameterName.Text = value;
			}
		}
		public object Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				this.UpdateUI();
			}
		}
		public Image[] Images
		{
			set
			{
				this.imageListEnum.Images.Clear();
				this.imageListEnum.Images.AddRange(value);
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
			this.parameterName = new Label();
			this.valueList = new ListView();
			this.imageListEnum = new ImageList(this.components);
			ColumnHeader columnHeader = new ColumnHeader();
			base.SuspendLayout();
			columnHeader.Width = 100;
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(245, 13);
			this.parameterName.TabIndex = 1;
			this.parameterName.Text = "Parameter name";
			this.valueList.Columns.AddRange(new ColumnHeader[]
			{
				columnHeader
			});
			this.valueList.Dock = DockStyle.Fill;
			this.valueList.HeaderStyle = ColumnHeaderStyle.None;
			this.valueList.HideSelection = false;
			this.valueList.Location = new Point(0, 13);
			this.valueList.MultiSelect = false;
			this.valueList.Name = "valueList";
			this.valueList.Size = new Size(245, 106);
			this.valueList.SmallImageList = this.imageListEnum;
			this.valueList.TabIndex = 2;
			this.valueList.UseCompatibleStateImageBehavior = false;
			this.valueList.View = View.Details;
			this.valueList.SelectedIndexChanged += new EventHandler(this.valueList_SelectedIndexChanged);
			this.valueList.Layout += new LayoutEventHandler(this.valueList_Layout);
			this.imageListEnum.ColorDepth = ColorDepth.Depth32Bit;
			this.imageListEnum.ImageSize = new Size(16, 16);
			this.imageListEnum.TransparentColor = Color.Transparent;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.valueList);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamEnumList";
			base.Size = new Size(245, 119);
			base.ResumeLayout(false);
		}
		public ParamEnumList()
		{
			this.InitializeComponent();
		}
		public void UpdateUI()
		{
			for (int i = 0; i < this.valueList.Items.Count; i++)
			{
				if (this.valueList.Items[i].Tag.Equals(this.Value))
				{
					this.valueList.Items[i].Selected = true;
					return;
				}
			}
		}
		private void valueList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.valueList.SelectedItems.Count > 0)
			{
				this.OnValueChanged(this.valueList.SelectedItems[0].Tag);
			}
		}
		public void Add(string display, int imageIndex, object value)
		{
			ListViewItem listViewItem = new ListViewItem();
			listViewItem.Text = display;
			listViewItem.Tag = value;
			listViewItem.ImageIndex = imageIndex;
			this.valueList.Items.Add(listViewItem);
		}
		protected void OnValueChanged(object value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
		private void valueList_Layout(object sender, LayoutEventArgs e)
		{
			this.valueList.Columns[0].Width = this.valueList.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 5;
		}
	}
}
