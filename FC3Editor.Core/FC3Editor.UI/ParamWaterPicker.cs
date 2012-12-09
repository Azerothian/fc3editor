using FC3Editor.Nomad;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamWaterPicker : UserControl
	{
		private Image m_waterImage;
		private WaterInventory.Entry m_entry;
		private IContainer components;
		private Label label1;
		private TextBox textWaterMaterial;
		private NomadButton buttonAssign;
		private PictureBox pictureBox1;
		public event EventHandler EntryChanged;
		public WaterInventory.Entry Entry
		{
			get
			{
				return this.m_entry;
			}
			set
			{
				if (this.m_entry == value)
				{
					return;
				}
				this.m_entry = value;
				this.UpdateUI();
				this.OnEntryChanged();
			}
		}
		public ParamWaterPicker()
		{
			this.InitializeComponent();
			this.label1.Text = Localizer.Localize(this.label1.Text);
			this.buttonAssign.Text = Localizer.Localize(this.buttonAssign.Text);
		}
		private void DisposeInternal()
		{
			if (this.m_waterImage != null)
			{
				this.m_waterImage.Dispose();
				this.m_waterImage = null;
			}
		}
		public void UpdateUI()
		{
			this.textWaterMaterial.Text = (this.m_entry.IsValid ? this.m_entry.DisplayName : Localizer.Localize("PARAM_OBJECT_BROWSER_NONE"));
			if (this.m_waterImage != null)
			{
				this.pictureBox1.Image = null;
				this.m_waterImage.Dispose();
				this.m_waterImage = null;
			}
			if (this.m_entry.IsValid)
			{
				MemoryStream thumbnailData = this.m_entry.GetThumbnailData();
				this.m_waterImage = Image.FromStream(thumbnailData);
				thumbnailData.Dispose();
			}
			this.pictureBox1.Image = this.m_waterImage;
		}
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.OnAssign();
		}
		private void buttonAssign_Click(object sender, EventArgs e)
		{
			this.OnAssign();
		}
		private void OnAssign()
		{
			using (PromptInventoryList promptInventoryList = new PromptInventoryList())
			{
				promptInventoryList.Root = WaterInventory.Instance.Root;
				promptInventoryList.Value = this.m_entry;
				if (promptInventoryList.ShowDialog(this) == DialogResult.OK)
				{
					this.Entry = (WaterInventory.Entry)promptInventoryList.Value;
				}
			}
		}
		private void OnEntryChanged()
		{
			if (this.EntryChanged != null)
			{
				this.EntryChanged(this, new EventArgs());
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
			this.label1 = new Label();
			this.textWaterMaterial = new TextBox();
			this.pictureBox1 = new PictureBox();
			this.buttonAssign = new NomadButton();
			((ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new Point(3, 0);
			this.label1.Name = "label1";
			this.label1.Size = new Size(151, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "PARAM_WATER_MATERIAL";
			this.textWaterMaterial.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.textWaterMaterial.Location = new Point(3, 197);
			this.textWaterMaterial.Name = "textWaterMaterial";
			this.textWaterMaterial.ReadOnly = true;
			this.textWaterMaterial.Size = new Size(244, 20);
			this.textWaterMaterial.TabIndex = 3;
			this.textWaterMaterial.TextAlign = HorizontalAlignment.Center;
			this.pictureBox1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.pictureBox1.Location = new Point(6, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new Size(241, 175);
			this.pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			this.pictureBox1.Click += new EventHandler(this.pictureBox1_Click);
			this.buttonAssign.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.buttonAssign.Location = new Point(3, 223);
			this.buttonAssign.Name = "buttonAssign";
			this.buttonAssign.Size = new Size(244, 23);
			this.buttonAssign.TabIndex = 4;
			this.buttonAssign.Text = "PARAM_ASSIGN";
			this.buttonAssign.Click += new EventHandler(this.buttonAssign_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.pictureBox1);
			base.Controls.Add(this.buttonAssign);
			base.Controls.Add(this.textWaterMaterial);
			base.Controls.Add(this.label1);
			base.Name = "ParamWaterPicker";
			base.Size = new Size(253, 254);
			((ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
