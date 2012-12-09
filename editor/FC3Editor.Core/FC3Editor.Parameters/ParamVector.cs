using FC3Editor.Nomad;
using FC3Editor.UI;
using System;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal class ParamVector : Parameter
	{
		protected Vec3 m_value;
		private ParamVectorUIType m_uiType;
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
				foreach (Control current in this.m_uiControls.Keys)
				{
					ParamVectorEdit paramVectorEdit = current as ParamVectorEdit;
					if (paramVectorEdit != null)
					{
						paramVectorEdit.Value = this.m_value;
					}
				}
			}
		}
		public ParamVector(string display, Vec3 value, ParamVectorUIType uiType) : base(display)
		{
			this.Value = value;
			this.m_uiType = uiType;
		}
		protected override Control CreateUIControl()
		{
			ParamVectorEdit paramVectorEdit = new ParamVectorEdit();
			paramVectorEdit.Value = this.Value;
			paramVectorEdit.ParameterName = base.DisplayName;
			paramVectorEdit.ValueChanged += delegate(object sender, EventArgs e)
			{
				this.OnValueChanged(((ParamVectorEdit)sender).Value);
			}
			;
			if (this.m_uiType == ParamVectorUIType.Angles)
			{
				paramVectorEdit.ValueType = ParamVectorEditValueType.Angles;
			}
			paramVectorEdit.UpdateUI();
			return paramVectorEdit;
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
