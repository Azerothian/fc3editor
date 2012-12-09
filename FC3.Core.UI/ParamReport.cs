using FC3Editor.Nomad;
using FC3Editor.Properties;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamReport : UserControl
	{
		private IContainer components;
		private ListView listView;
		private Label parameterName;
		private ImageList imageList;
		private ColumnHeader columnHeader1;
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
		public ParamReport()
		{
			this.InitializeComponent();
			this.imageList.Images.AddRange(new Image[]
			{
				Resources.valid16,
				Resources.warning16,
				Resources.error16
			});
		}
		public void UpdateUI(ValidationReport report)
		{
			this.listView.BeginUpdate();
			this.listView.Items.Clear();
			if (report.IsValid)
			{
				for (int i = 0; i < report.Count; i++)
				{
					ValidationRecord validationRecord = report[i];
					int imageIndex;
					if (validationRecord.Severity == ValidationRecord.Severities.Success)
					{
						imageIndex = 0;
					}
					else
					{
						if (validationRecord.Severity == ValidationRecord.Severities.Warning)
						{
							imageIndex = 1;
						}
						else
						{
							imageIndex = 2;
						}
					}
					ListViewItem listViewItem = new ListViewItem(validationRecord.Message, imageIndex);
					listViewItem.Tag = validationRecord;
					this.listView.Items.Add(listViewItem);
				}
			}
			this.listView.EndUpdate();
		}
		private void listView_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem listViewItem = this.listView.SelectedItems[0];
			if (listViewItem.Tag is ValidationRecord)
			{
				ValidationRecord validationRecord = (ValidationRecord)listViewItem.Tag;
				EditorObject arg_44_0 = validationRecord.Object;
				if (validationRecord.Object.IsValid)
				{
					Camera.Focus(validationRecord.Object);
				}
			}
		}
		private void listView_Click(object sender, EventArgs e)
		{
			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem listViewItem = this.listView.SelectedItems[0];
			if (listViewItem.Tag is ValidationRecord)
			{
				ValidationRecord validationRecord = (ValidationRecord)listViewItem.Tag;
				EditorObject arg_44_0 = validationRecord.Object;
				if (validationRecord.Object.IsValid)
				{
					Camera.Focus(validationRecord.Object);
				}
			}
		}
		private void listView_Layout(object sender, LayoutEventArgs e)
		{
			this.listView.Columns[0].Width = this.listView.ClientSize.Width - SystemInformation.VerticalScrollBarWidth - 5;
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
			this.listView = new ListView();
			this.columnHeader1 = new ColumnHeader();
			this.imageList = new ImageList(this.components);
			this.parameterName = new Label();
			base.SuspendLayout();
			this.listView.Columns.AddRange(new ColumnHeader[]
			{
				this.columnHeader1
			});
			this.listView.Dock = DockStyle.Fill;
			this.listView.HeaderStyle = ColumnHeaderStyle.None;
			this.listView.Location = new Point(0, 18);
			this.listView.MultiSelect = false;
			this.listView.Name = "listView";
			this.listView.Size = new Size(150, 199);
			this.listView.SmallImageList = this.imageList;
			this.listView.TabIndex = 0;
			this.listView.UseCompatibleStateImageBehavior = false;
			this.listView.View = View.Details;
			this.listView.SelectedIndexChanged += new EventHandler(this.listView_SelectedIndexChanged);
			this.listView.Layout += new LayoutEventHandler(this.listView_Layout);
			this.listView.Click += new EventHandler(this.listView_Click);
			this.imageList.ColorDepth = ColorDepth.Depth32Bit;
			this.imageList.ImageSize = new Size(16, 16);
			this.imageList.TransparentColor = Color.Transparent;
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(150, 18);
			this.parameterName.TabIndex = 1;
			this.parameterName.TextAlign = ContentAlignment.MiddleLeft;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.listView);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamReport";
			base.Size = new Size(150, 217);
			base.ResumeLayout(false);
		}
	}
}
