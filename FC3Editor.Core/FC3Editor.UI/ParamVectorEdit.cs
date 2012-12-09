using FC3Editor.Nomad;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParamVectorEdit : UserControl
	{
		private IContainer components;
		private Label parameterName;
		private Label label1;
		private TextBox xBox;
		private TextBox yBox;
		private Label label2;
		private TextBox zBox;
		private Label label3;
		private Vec3 m_value;
		private ParamVectorEditValueType m_valueType;
		public event EventHandler ValueChanged;
		public Vec3 Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				if (this.m_value == value)
				{
					return;
				}
				this.m_value = value;
				this.UpdateUI();
			}
		}
		public ParamVectorEditValueType ValueType
		{
			get
			{
				return this.m_valueType;
			}
			set
			{
				this.m_valueType = value;
				this.UpdateUI();
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
			this.label1 = new Label();
			this.xBox = new TextBox();
			this.yBox = new TextBox();
			this.label2 = new Label();
			this.zBox = new TextBox();
			this.label3 = new Label();
			base.SuspendLayout();
			this.parameterName.Dock = DockStyle.Top;
			this.parameterName.Location = new Point(0, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(226, 13);
			this.parameterName.TabIndex = 0;
			this.parameterName.Text = "Name";
			this.label1.Location = new Point(1, 20);
			this.label1.Name = "label1";
			this.label1.Size = new Size(21, 13);
			this.label1.TabIndex = 1;
			this.label1.Text = "X:";
			this.xBox.Location = new Point(21, 17);
			this.xBox.Name = "xBox";
			this.xBox.Size = new Size(51, 20);
			this.xBox.TabIndex = 2;
			this.xBox.Leave += new EventHandler(this.valueBox_Leave);
			this.xBox.KeyDown += new KeyEventHandler(this.value_KeyDown);
			this.yBox.Location = new Point(93, 17);
			this.yBox.Name = "yBox";
			this.yBox.Size = new Size(51, 20);
			this.yBox.TabIndex = 4;
			this.yBox.Leave += new EventHandler(this.valueBox_Leave);
			this.yBox.KeyDown += new KeyEventHandler(this.value_KeyDown);
			this.label2.Location = new Point(73, 20);
			this.label2.Name = "label2";
			this.label2.Size = new Size(21, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Y:";
			this.zBox.Location = new Point(167, 17);
			this.zBox.Name = "zBox";
			this.zBox.Size = new Size(51, 20);
			this.zBox.TabIndex = 6;
			this.zBox.Leave += new EventHandler(this.valueBox_Leave);
			this.zBox.KeyDown += new KeyEventHandler(this.value_KeyDown);
			this.label3.Location = new Point(147, 20);
			this.label3.Name = "label3";
			this.label3.Size = new Size(22, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Z:";
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.zBox);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.yBox);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.xBox);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamVectorEdit";
			base.Size = new Size(226, 44);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
		public ParamVectorEdit()
		{
			this.InitializeComponent();
		}
		private void UpdateText()
		{
			float angleRad = this.Value.X;
			float angleRad2 = this.Value.Y;
			float angleRad3 = this.Value.Z;
			if (this.m_valueType == ParamVectorEditValueType.Angles)
			{
				angleRad = MathUtils.Rad2Deg(angleRad);
				angleRad2 = MathUtils.Rad2Deg(angleRad2);
				angleRad3 = MathUtils.Rad2Deg(angleRad3);
			}
			this.xBox.Text = angleRad.ToString("F2");
			this.yBox.Text = angleRad2.ToString("F2");
			this.zBox.Text = angleRad3.ToString("F2");
		}
		public void UpdateUI()
		{
			this.UpdateText();
		}
		private void UpdateFromText()
		{
			float num;
			float num2;
			float num3;
			if (!float.TryParse(this.xBox.Text, out num) || !float.TryParse(this.yBox.Text, out num2) || !float.TryParse(this.zBox.Text, out num3))
			{
				this.UpdateText();
				return;
			}
			if (this.m_valueType == ParamVectorEditValueType.Angles)
			{
				num = MathUtils.Deg2Rad(num);
				num2 = MathUtils.Deg2Rad(num2);
				num3 = MathUtils.Deg2Rad(num3);
			}
			this.OnValueChanged(new Vec3(num, num2, num3));
		}
		private void valueBox_Leave(object sender, EventArgs e)
		{
			this.UpdateFromText();
		}
		private void value_KeyDown(object sender, KeyEventArgs e)
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
		private void OnValueChanged(Vec3 value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
