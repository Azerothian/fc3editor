using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamFloat : Parameter
	{
		protected float m_value;
		protected float m_minValue = -3.40282347E+38f;
		protected float m_maxValue = 3.40282347E+38f;
		protected float m_resolution = 1f;
		private bool m_enabled = true;
		public event EventHandler ValueChanged;
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
				foreach (Control current in this.m_uiControls.Keys)
				{
					ParamNumberSlider paramNumberSlider = current as ParamNumberSlider;
					if (paramNumberSlider != null)
					{
						paramNumberSlider.Value = this.m_value;
					}
				}
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
			}
		}
		public bool Enabled
		{
			get
			{
				return this.m_enabled;
			}
			set
			{
				this.m_enabled = value;
				base.UpdateUIControls();
			}
		}
		public ParamFloat(string display, float value, float min, float max, float resolution) : base(display)
		{
			this.MinValue = min;
			this.MaxValue = max;
			this.Resolution = resolution;
			this.Value = value;
		}
		protected override Control CreateUIControl()
		{
			ParamNumberSlider paramNumberSlider = new ParamNumberSlider();
			paramNumberSlider.ParameterName = base.DisplayName;
			paramNumberSlider.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnValueChanged(((ParamNumberSlider)sender).Value);
			}
			;
			return paramNumberSlider;
		}
		protected override void UpdateUIControl(Control control)
		{
			ParamNumberSlider paramNumberSlider = (ParamNumberSlider)control;
			paramNumberSlider.Enabled = this.m_enabled;
			paramNumberSlider.MinValue = this.MinValue;
			paramNumberSlider.MaxValue = this.MaxValue;
			paramNumberSlider.Resolution = this.Resolution;
			paramNumberSlider.Value = this.Value;
			paramNumberSlider.UpdateUI();
		}
		protected void OnValueChanged(float value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
