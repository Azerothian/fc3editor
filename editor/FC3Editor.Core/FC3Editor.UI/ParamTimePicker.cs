using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	public class ParamTimePicker : UserControl
	{
		private bool updatePicker;
		private TimeSpan m_value = default(TimeSpan);
		private TimeSpan MaxValue = new TimeSpan(23, 59, 59);
		private IContainer components;
		private Label parameterName;
		private DateTimePicker dateTimePicker;
		private NomadSlider timeSlider;
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
		public TimeSpan Value
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
		public ParamTimePicker()
		{
			this.InitializeComponent();
		}
		private void UpdateSlider()
		{
			this.timeSlider.Value = (int)(this.Value.Ticks * (long)this.timeSlider.Maximum / this.MaxValue.Ticks);
		}
		private void UpdatePicker()
		{
			this.updatePicker = true;
			DateTime now = DateTime.Now;
			this.dateTimePicker.Value = new DateTime(now.Year, now.Month, now.Day, this.Value.Hours, this.Value.Minutes, this.Value.Seconds, this.Value.Milliseconds);
			this.updatePicker = false;
		}
		private void UpdateUI()
		{
			this.UpdateSlider();
			this.UpdatePicker();
		}
		private void dateTimePicker_ValueChanged(object sender, EventArgs e)
		{
			if (this.updatePicker)
			{
				return;
			}
			this.OnValueChanged(this.dateTimePicker.Value.TimeOfDay);
			this.UpdateSlider();
		}
		private void timeSlider_Scroll(object sender, EventArgs e)
		{
			this.OnValueChanged(new TimeSpan((long)this.timeSlider.Value * this.MaxValue.Ticks / (long)this.timeSlider.Maximum));
			this.UpdatePicker();
		}
		private void OnValueChanged(TimeSpan value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
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
			this.dateTimePicker = new DateTimePicker();
			this.timeSlider = new NomadSlider();
			((ISupportInitialize)this.timeSlider).BeginInit();
			base.SuspendLayout();
			this.parameterName.AutoSize = true;
			this.parameterName.Location = new Point(3, 0);
			this.parameterName.Name = "parameterName";
			this.parameterName.Size = new Size(30, 13);
			this.parameterName.TabIndex = 0;
			this.parameterName.Text = "Time";
			this.dateTimePicker.Anchor = (AnchorStyles.Top | AnchorStyles.Right);
			this.dateTimePicker.Format = DateTimePickerFormat.Time;
			this.dateTimePicker.Location = new Point(138, 14);
			this.dateTimePicker.Name = "dateTimePicker";
			this.dateTimePicker.ShowUpDown = true;
			this.dateTimePicker.Size = new Size(107, 20);
			this.dateTimePicker.TabIndex = 1;
			this.dateTimePicker.ValueChanged += new EventHandler(this.dateTimePicker_ValueChanged);
			this.timeSlider.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.timeSlider.AutoSize = false;
			this.timeSlider.Location = new Point(0, 16);
			this.timeSlider.Maximum = 1000;
			this.timeSlider.Name = "timeSlider";
			this.timeSlider.Size = new Size(137, 20);
			this.timeSlider.TabIndex = 2;
			this.timeSlider.TickFrequency = 50;
			this.timeSlider.TickStyle = TickStyle.None;
			this.timeSlider.Scroll += new EventHandler(this.timeSlider_Scroll);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.timeSlider);
			base.Controls.Add(this.dateTimePicker);
			base.Controls.Add(this.parameterName);
			base.Name = "ParamTimePicker";
			base.Size = new Size(248, 43);
			((ISupportInitialize)this.timeSlider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
