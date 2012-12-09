using FC3Editor.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class ParametersList : UserControl
	{
		private Dictionary<IParameter, Control> m_controls = new Dictionary<IParameter, Control>();
		private IParameterProvider m_parameters;
		private EventHandler m_paramsChangedHandler;
		private IContainer components;
		public IParameterProvider Parameters
		{
			get
			{
				return this.m_parameters;
			}
			set
			{
				if (this.m_parameters is IParameterProviderDynamic)
				{
					((IParameterProviderDynamic)this.m_parameters).ParamsChanged -= this.m_paramsChangedHandler;
				}
				this.m_parameters = value;
				if (this.m_parameters is IParameterProviderDynamic)
				{
					((IParameterProviderDynamic)this.m_parameters).ParamsChanged += this.m_paramsChangedHandler;
				}
				this.UpdateUI();
			}
		}
		public ParametersList()
		{
			this.InitializeComponent();
			this.m_paramsChangedHandler = new EventHandler(this._ParamsChanged);
		}
		private void DisposeInternal()
		{
			this.ClearUI();
		}
		private void ClearUI()
		{
			foreach (KeyValuePair<IParameter, Control> current in this.m_controls)
			{
				MainForm.Instance.ToolTip.SetToolTip(current.Value, null);
				foreach (Control control in current.Value.Controls)
				{
					MainForm.Instance.ToolTip.SetToolTip(control, null);
				}
				current.Key.ReleaseUIControl(current.Value);
			}
			this.m_controls.Clear();
		}
		private void UpdateUI()
		{
			Win32.SetRedraw(this, false);
			base.SuspendLayout();
			this.ClearUI();
			if (this.Parameters != null)
			{
				IParameter mainParameter = this.Parameters.GetMainParameter();
				bool flag = true;
				foreach (IParameter current in this.Parameters.GetParameters())
				{
					Control control = current.AcquireUIControl();
					base.Controls.Add(control);
					string toolTip = current.ToolTip;
					if (toolTip != null)
					{
						MainForm.Instance.ToolTip.SetToolTip(control, current.ToolTip);
						foreach (Control control2 in control.Controls)
						{
							MainForm.Instance.ToolTip.SetToolTip(control2, current.ToolTip);
						}
					}
					if (current == mainParameter)
					{
						control.Dock = DockStyle.Fill;
						base.Controls.SetChildIndex(control, 0);
						flag = false;
					}
					else
					{
						control.Dock = (flag ? DockStyle.Top : DockStyle.Bottom);
						base.Controls.SetChildIndex(control, flag ? 0 : (base.Controls.Count - 1));
					}
					this.m_controls.Add(current, control);
				}
			}
			base.AutoScrollPosition = default(Point);
			base.ResumeLayout();
			Win32.SetRedraw(this, true);
			this.Refresh();
		}
		private void _ParamsChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
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
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoScroll = true;
			base.Name = "ParametersList";
			base.ResumeLayout(false);
		}
	}
}
