using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamEnumButtons : UserControl
	{
		private class Item
		{
			private string m_name;
			private Image m_image;
			private object m_value;
			public string Name
			{
				get
				{
					return this.m_name;
				}
			}
			public Image Image
			{
				get
				{
					return this.m_image;
				}
			}
			public object Value
			{
				get
				{
					return this.m_value;
				}
			}
			public Item(string name, Image img, object value)
			{
				this.m_name = name;
				this.m_image = img;
				this.m_value = value;
			}
			public override string ToString()
			{
				return this.m_name;
			}
		}
		private List<ParamEnumButtons.Item> m_itemList = new List<ParamEnumButtons.Item>();
		private Dictionary<ParamEnumButtons.Item, NomadRadioButton> m_buttonList = new Dictionary<ParamEnumButtons.Item, NomadRadioButton>();
		private object m_value;
		private IContainer components;
		private Label parameterName;
		private FlowLayoutPanel flowLayoutPanel;
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
				if (value.Equals(this.m_value))
				{
					return;
				}
				this.m_value = value;
				for (int i = 0; i < this.m_itemList.Count; i++)
				{
					NomadRadioButton nomadRadioButton;
					if (this.m_itemList[i].Value.Equals(value) && this.m_buttonList.TryGetValue(this.m_itemList[i], out nomadRadioButton))
					{
						nomadRadioButton.Checked = true;
						return;
					}
				}
			}
		}
		public ParamEnumButtons()
		{
			this.InitializeComponent();
		}
		private void DisposeInternal()
		{
			this.ClearUI();
		}
		public void Add(string name, Image img, object value)
		{
			this.m_itemList.Add(new ParamEnumButtons.Item(name, img, value));
		}
		private void ClearUI()
		{
			foreach (NomadRadioButton current in this.m_buttonList.Values)
			{
				MainForm.Instance.ToolTip.SetToolTip(current, null);
				current.Dispose();
			}
			this.flowLayoutPanel.Controls.Clear();
			this.m_buttonList.Clear();
		}
		public void UpdateUI()
		{
			base.SuspendLayout();
			this.ClearUI();
			foreach (ParamEnumButtons.Item current in this.m_itemList)
			{
				NomadRadioButton nomadRadioButton = new NomadRadioButton();
				nomadRadioButton.Appearance = Appearance.Button;
				nomadRadioButton.AutoSize = true;
				nomadRadioButton.Margin = new Padding(1);
				nomadRadioButton.Checked = current.Value.Equals(this.Value);
				nomadRadioButton.Image = current.Image;
				if (current.Image == null)
				{
					nomadRadioButton.Text = current.Name;
				}
				nomadRadioButton.CheckedChanged += new EventHandler(this.button_CheckedChanged);
				nomadRadioButton.Tag = current;
				MainForm.Instance.ToolTip.SetToolTip(nomadRadioButton, current.Name);
				this.flowLayoutPanel.Controls.Add(nomadRadioButton);
				this.m_buttonList.Add(current, nomadRadioButton);
			}
			base.ResumeLayout();
		}
		private void button_CheckedChanged(object sender, EventArgs e)
		{
			NomadRadioButton nomadRadioButton = (NomadRadioButton)sender;
			object value = ((ParamEnumButtons.Item)nomadRadioButton.Tag).Value;
			if (nomadRadioButton.Checked && this.m_value != value)
			{
				this.OnValueChanged(value);
			}
		}
		private void button_Click(object sender, EventArgs e)
		{
		}
		protected void OnValueChanged(object value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
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
			this.parameterName = new Label();
			this.flowLayoutPanel = new FlowLayoutPanel();
			base.SuspendLayout();
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(3, 13);
			this.parameterName.TabIndex = 2;
			this.parameterName.Text = "Lalalaaaa:";
			this.parameterName.TextAlign = ContentAlignment.MiddleLeft;
			this.flowLayoutPanel.AutoSize = true;
			this.flowLayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel.Location = new Point(0, 15);
			this.flowLayoutPanel.Name = "flowLayoutPanel";
			this.flowLayoutPanel.Size = new Size(0, 0);
			this.flowLayoutPanel.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoSize = true;
			base.AutoSizeMode = AutoSizeMode.GrowAndShrink;
			base.Controls.Add(this.flowLayoutPanel);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamEnumButtons";
			base.Size = new Size(3, 18);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
