using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace FC3Editor.Parameters
{
	internal abstract class Parameter : IParameter
	{
		protected string m_displayName = "Unassigned";
		private string m_tooltip;
		protected Dictionary<Control, object> m_uiControls = new Dictionary<Control, object>();
		public string DisplayName
		{
			get
			{
				return this.m_displayName;
			}
			set
			{
				this.m_displayName = value;
				this.OnDisplayChanged();
			}
		}
		public string ToolTip
		{
			get
			{
				return this.m_tooltip;
			}
			set
			{
				this.m_tooltip = value;
			}
		}
		public Parameter(string display)
		{
			this.m_displayName = display;
		}
		protected abstract Control CreateUIControl();
		public Control AcquireUIControl()
		{
			Control control = this.CreateUIControl();
			this.UpdateUIControl(control);
			this.m_uiControls.Add(control, null);
			return control;
		}
		public virtual void ReleaseUIControl(Control control)
		{
			control.Dispose();
			this.m_uiControls.Remove(control);
		}
		protected virtual void UpdateUIControl(Control control)
		{
		}
		public void UpdateUIControls()
		{
			foreach (Control current in this.m_uiControls.Keys)
			{
				this.UpdateUIControl(current);
			}
		}
		protected virtual void OnDisplayChanged()
		{
		}
	}
}
