using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamEnumCombo : UserControl
	{
		private class Item
		{
			private string m_display;
			private object m_value;
			public object Value
			{
				get
				{
					return this.m_value;
				}
			}
			public Item(string display, object value)
			{
				this.m_display = display;
				this.m_value = value;
			}
			public override string ToString()
			{
				return this.m_display;
			}
		}
		private IContainer components;
		private Label parameterName;
		private ComboBox valueCombo;
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
			this.parameterName = new Label();
			this.valueCombo = new ComboBox();
			base.SuspendLayout();
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(262, 13);
			this.parameterName.TabIndex = 0;
			this.parameterName.Text = "Parameter name";
			this.valueCombo.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.valueCombo.DropDownStyle = ComboBoxStyle.DropDownList;
			this.valueCombo.FormattingEnabled = true;
			this.valueCombo.Location = new Point(3, 16);
			this.valueCombo.Name = "valueCombo";
			this.valueCombo.Size = new Size(256, 21);
			this.valueCombo.TabIndex = 1;
			this.valueCombo.SelectionChangeCommitted += new EventHandler(this.valueCombo_SelectionChangeCommitted);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.valueCombo);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamEnumCombo";
			base.Size = new Size(262, 42);
			base.ResumeLayout(false);
		}
		public ParamEnumCombo()
		{
			this.InitializeComponent();
			this.UpdateUI();
		}
		public void UpdateUI()
		{
			for (int i = 0; i < this.valueCombo.Items.Count; i++)
			{
				if (((ParamEnumCombo.Item)this.valueCombo.Items[i]).Value.Equals(this.Value))
				{
					this.valueCombo.SelectedIndex = i;
					return;
				}
			}
		}
		private void valueCombo_SelectionChangeCommitted(object sender, EventArgs e)
		{
			this.OnValueChanged(((ParamEnumCombo.Item)this.valueCombo.SelectedItem).Value);
		}
		public void Add(string display, object value)
		{
			this.valueCombo.Items.Add(new ParamEnumCombo.Item(display, value));
		}
		protected void OnValueChanged(object value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
