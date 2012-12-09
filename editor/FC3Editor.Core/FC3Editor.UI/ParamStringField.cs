using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamStringField : UserControl
	{
		private IContainer components;
		private Label parameterName;
		private TextBox parameterField;
		private string m_value;
		private int m_maxLength;
		private Rectangle m_originalClip = Cursor.Clip;
		public event EventHandler ValueChanged;
		public string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				if (value.Length > this.m_maxLength)
				{
					value = this.m_value.Substring(0, this.m_maxLength);
				}
				if (this.m_value == value)
				{
					return;
				}
				this.m_value = value;
				this.UpdateUI();
			}
		}
		public int MaxLength
		{
			get
			{
				return this.m_maxLength;
			}
			set
			{
				if (this.m_maxLength == value)
				{
					return;
				}
				if (this.m_value.Length > value)
				{
					this.m_value = this.m_value.Substring(0, value);
					this.UpdateText();
				}
				this.m_maxLength = value;
				this.UpdateMaxLength();
			}
		}
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
			this.parameterField = new TextBox();
			base.SuspendLayout();
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(285, 13);
			this.parameterName.TabIndex = 2;
			this.parameterName.Text = "Parameter name";
			this.parameterField.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.parameterField.Location = new Point(3, 16);
			this.parameterField.Name = "parameterField";
			this.parameterField.Size = new Size(279, 20);
			this.parameterField.TabIndex = 3;
			this.parameterField.Leave += new EventHandler(this.parameterField_Leave);
			this.parameterField.KeyDown += new KeyEventHandler(this.parameterField_KeyDown);
			this.parameterField.MouseDown += new MouseEventHandler(this.parameterField_MouseDown);
			this.parameterField.MouseUp += new MouseEventHandler(this.parameterField_MouseUp);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.parameterField);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamStringField";
			base.Size = new Size(285, 41);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamStringField()
		{
			this.InitializeComponent();
			this.m_maxLength = 128;
		}
		private void UpdateText()
		{
			this.parameterField.Text = this.m_value;
		}
		private void UpdateMaxLength()
		{
			this.parameterField.MaxLength = this.m_maxLength;
		}
		public void UpdateUI()
		{
			this.UpdateText();
		}
		private void UpdateFromText()
		{
			this.OnValueChanged(this.parameterField.Text);
		}
		private void parameterField_Leave(object sender, EventArgs e)
		{
			this.UpdateFromText();
		}
		private void parameterField_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				this.UpdateText();
				return;
			}
			if (e.KeyCode == Keys.Return)
			{
				this.UpdateFromText();
			}
		}
		private void OnValueChanged(string value)
		{
			if (this.m_value == value)
			{
				return;
			}
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
		private void parameterField_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				Cursor.Clip = new Rectangle(base.PointToScreen(this.parameterField.Location), this.parameterField.Size);
			}
		}
		private void parameterField_MouseUp(object sender, MouseEventArgs e)
		{
			Cursor.Clip = this.m_originalClip;
		}
	}
}
