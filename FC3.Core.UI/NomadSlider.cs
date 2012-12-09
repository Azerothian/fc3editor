using System;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class NomadSlider : TrackBar
	{
		private Win32.Rect m_channelRect = default(Win32.Rect);
		private bool m_dragging;
		public NomadSlider()
		{
			base.SetStyle(ControlStyles.UserMouse, true);
		}
		private void UpdateSliderFromMouse(MouseEventArgs e)
		{
			if (e.X < this.m_channelRect.left)
			{
				base.Value = base.Minimum;
			}
			else
			{
				if (e.X >= this.m_channelRect.right)
				{
					base.Value = base.Maximum;
				}
				else
				{
					base.Value = base.Minimum + (e.X - this.m_channelRect.left) * (base.Maximum - base.Minimum) / (this.m_channelRect.right - this.m_channelRect.left);
				}
			}
			this.OnScroll(e);
		}
		protected override void OnMouseDown(MouseEventArgs e)
		{
			Win32.SendMessage(base.Handle, 1050, 0, ref this.m_channelRect);
			this.UpdateSliderFromMouse(e);
			this.m_dragging = true;
			base.OnMouseDown(e);
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			if (this.m_dragging)
			{
				this.UpdateSliderFromMouse(e);
			}
			base.OnMouseMove(e);
		}
		protected override void OnMouseUp(MouseEventArgs e)
		{
			this.m_dragging = false;
			base.OnMouseUp(e);
		}
	}
}
