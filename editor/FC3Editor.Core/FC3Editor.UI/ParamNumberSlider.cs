using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamNumberSlider : UserControl
	{
		private IContainer components;
		private Label parameterName;
		private NomadSlider valueSlider;
		private TextBox valueText;
		private float m_minValue;
		private float m_maxValue = 1f;
		private float m_resolution = 0.1f;
		private int m_numTicks = 10;
		private float m_value;
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
		public float MinValue
		{
			get
			{
				return this.m_minValue;
			}
			set
			{
				this.m_minValue = value;
			}
		}
		public float MaxValue
		{
			get
			{
				return this.m_maxValue;
			}
			set
			{
				this.m_maxValue = value;
			}
		}
		public float Resolution
		{
			get
			{
				return this.m_resolution;
			}
			set
			{
				this.m_resolution = value;
				this.m_numTicks = (int)Math.Round((double)((this.MaxValue - this.MinValue) / this.m_resolution));
			}
		}
		public float Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				if (this.m_value < this.MinValue)
				{
					this.m_value = this.MinValue;
				}
				if (this.m_value > this.MaxValue)
				{
					this.m_value = this.MaxValue;
				}
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
			this.valueText = new TextBox();
			this.valueSlider = new NomadSlider();
			((ISupportInitialize)this.valueSlider).BeginInit();
			base.SuspendLayout();
			this.parameterName.Location = new Point(0, 1);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(103, 22);
			this.parameterName.TabIndex = 0;
			this.parameterName.Text = "Parameter name";
			this.parameterName.TextAlign = ContentAlignment.MiddleLeft;
			this.valueText.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.valueText.Location = new Point(214, 3);
			this.valueText.Name = "valueText";
			this.valueText.Size = new Size(45, 20);
			this.valueText.TabIndex = 2;
			this.valueText.KeyDown += new KeyEventHandler(this.valueText_KeyDown);
			this.valueText.Leave += new EventHandler(this.valueText_Leave);
			this.valueSlider.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.valueSlider.AutoSize = false;
			this.valueSlider.Location = new Point(98, 3);
			this.valueSlider.Maximum = 1000;
			this.valueSlider.Name = "valueSlider";
			this.valueSlider.Size = new Size(118, 20);
			this.valueSlider.TabIndex = 1;
			this.valueSlider.TickFrequency = 50;
			this.valueSlider.TickStyle = TickStyle.None;
			this.valueSlider.Scroll += new EventHandler(this.valueSlider_Scroll);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.valueText);
			base.Controls.Add(this.valueSlider);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamNumberSlider";
			base.Size = new Size(262, 26);
			((ISupportInitialize)this.valueSlider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamNumberSlider()
		{
			this.InitializeComponent();
			this.UpdateUI();
		}
		private void UpdateSlider()
		{
			this.valueSlider.Value = (int)((this.Value - this.MinValue) / (this.MaxValue - this.MinValue) * (float)this.valueSlider.Maximum);
		}
		private void UpdateText()
		{
			this.valueText.Text = this.Value.ToString("F2");
		}
		private void UpdateFromText()
		{
			float num;
			if (float.TryParse(this.valueText.Text, out num) && num >= this.MinValue && num <= this.MaxValue)
			{
				this.OnValueChanged(num);
				this.UpdateSlider();
				return;
			}
			this.UpdateText();
		}
		public void UpdateUI()
		{
			if (this.valueSlider.Maximum != this.m_numTicks)
			{
				this.valueSlider.Maximum = this.m_numTicks;
			}
			this.UpdateSlider();
			this.UpdateText();
		}
		private void valueSlider_Scroll(object sender, EventArgs e)
		{
			this.OnValueChanged((float)this.valueSlider.Value / (float)this.valueSlider.Maximum * (this.MaxValue - this.MinValue) + this.MinValue);
			this.UpdateText();
		}
		private void valueText_Leave(object sender, EventArgs e)
		{
			this.UpdateFromText();
		}
		private void valueText_KeyDown(object sender, KeyEventArgs e)
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
		public void OnValueChanged(float value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
