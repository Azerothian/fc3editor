using FC3Editor.UI;
using System;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamEnumBase<T> : Parameter
	{
		protected T m_value;
		private bool m_enabled = true;
		private ParamEnumUIType m_uiType;
		protected string[] m_names;
		protected Image[] m_images = new Image[0];
		protected T[] m_values = new T[0];
		public event EventHandler ValueChanged;
		public T Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
				base.UpdateUIControls();
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
		public string[] Names
		{
			get
			{
				return this.m_names;
			}
			set
			{
				if (value == null)
				{
					value = new string[0];
				}
				this.m_names = value;
			}
		}
		public Image[] Images
		{
			get
			{
				return this.m_images;
			}
			set
			{
				if (value == null)
				{
					value = new Image[0];
				}
				this.m_images = value;
			}
		}
		public T[] Values
		{
			get
			{
				return this.m_values;
			}
			set
			{
				if (value == null)
				{
					value = new T[0];
				}
				this.m_values = value;
			}
		}
		public ParamEnumBase(string display, T value, ParamEnumUIType uiType) : base(display)
		{
			this.Value = value;
			this.m_uiType = uiType;
		}
		protected override Control CreateUIControl()
		{
			switch (this.m_uiType)
			{
			case ParamEnumUIType.ComboBox:
				{
					ParamEnumCombo paramEnumCombo = new ParamEnumCombo();
					for (int i = 0; i < this.m_names.Length; i++)
					{
						paramEnumCombo.Add(this.m_names[i], this.m_values.GetValue(i));
					}
					paramEnumCombo.UpdateUI();
					paramEnumCombo.ParameterName = base.DisplayName;
					paramEnumCombo.ValueChanged += delegate(object sender, EventArgs e)
					{
						this.OnValueChanged((T)((ParamEnumCombo)sender).Value);
					}
					;
					return paramEnumCombo;
				}

			case ParamEnumUIType.Buttons:
				{
					ParamEnumButtons paramEnumButtons = new ParamEnumButtons();
					for (int j = 0; j < this.m_names.Length; j++)
					{
						paramEnumButtons.Add(this.m_names[j], (j < this.m_images.Length) ? this.m_images[j] : null, this.m_values.GetValue(j));
					}
					paramEnumButtons.UpdateUI();
					paramEnumButtons.ParameterName = base.DisplayName;
					paramEnumButtons.ValueChanged += delegate(object sender, EventArgs e)
					{
						this.OnValueChanged((T)((ParamEnumButtons)sender).Value);
					}
					;
					return paramEnumButtons;
				}

			case ParamEnumUIType.List:
				{
					ParamEnumList paramEnumList = new ParamEnumList();
					for (int k = 0; k < this.m_names.Length; k++)
					{
						paramEnumList.Add(this.m_names[k], k, this.m_values.GetValue(k));
					}
					paramEnumList.UpdateUI();
					paramEnumList.ParameterName = base.DisplayName;
					paramEnumList.ValueChanged += delegate(object sender, EventArgs e)
					{
						this.OnValueChanged((T)((ParamEnumList)sender).Value);
					}
					;
					return paramEnumList;
				}

			default:
				return null;
			}
		}
		protected override void UpdateUIControl(Control control)
		{
			control.Enabled = this.m_enabled;
			switch (this.m_uiType)
			{
			case ParamEnumUIType.ComboBox:
				{
					ParamEnumCombo paramEnumCombo = (ParamEnumCombo)control;
					paramEnumCombo.Value = this.m_value;
					return;
				}

			case ParamEnumUIType.Buttons:
				{
					ParamEnumButtons paramEnumButtons = (ParamEnumButtons)control;
					paramEnumButtons.Value = this.m_value;
					return;
				}

			case ParamEnumUIType.List:
				{
					ParamEnumList paramEnumList = (ParamEnumList)control;
					paramEnumList.Images = this.m_images;
					paramEnumList.Value = this.m_value;
					return;
				}

			default:
				return;
			}
		}
		protected void OnValueChanged(T value)
		{
			this.m_value = value;
			if (this.ValueChanged != null)
			{
				this.ValueChanged(this, new EventArgs());
			}
		}
	}
}
