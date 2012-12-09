using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class NomadRadioButton : RadioButton
	{
		private bool mouseOver;
		private bool pushed;
		public NomadRadioButton()
		{
			ButtonShader.InitShaders();
		}
		protected override void OnMouseEnter(EventArgs e)
		{
			base.OnMouseEnter(e);
			this.mouseOver = true;
			this.Refresh();
		}
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			this.mouseOver = false;
			this.Refresh();
		}
		protected override void OnMouseDown(MouseEventArgs mevent)
		{
			base.OnMouseDown(mevent);
			this.pushed = true;
			this.Refresh();
		}
		protected override void OnMouseUp(MouseEventArgs mevent)
		{
			base.OnMouseUp(mevent);
			this.pushed = false;
			this.Refresh();
		}
		protected override void OnPaint(PaintEventArgs pevent)
		{
			Graphics graphics = pevent.Graphics;
			ButtonShader buttonShader = ButtonShader.normalShader;
			if (this.mouseOver)
			{
				buttonShader = ButtonShader.hoverShader;
			}
			if (this.pushed)
			{
				buttonShader = ButtonShader.pushShader;
			}
			if (base.Checked)
			{
				buttonShader = ButtonShader.checkShader;
			}
			if (!base.Enabled)
			{
				buttonShader = ButtonShader.disableShader;
			}
			Rectangle clientRectangle = base.ClientRectangle;
			using (Pen pen = new Pen(this.BackColor))
			{
				graphics.DrawRectangle(pen, clientRectangle.X, clientRectangle.Y, clientRectangle.Width - 1, clientRectangle.Height - 1);
			}
			clientRectangle.Inflate(-1, -1);
			buttonShader.DrawButton(graphics, clientRectangle, this.BackColor);
			if (base.Image != null)
			{
				ImageAttributes imageAttributes = new ImageAttributes();
				if (!base.Enabled)
				{
					imageAttributes.SetColorMatrix(NomadButton.disableMatrix);
				}
				Rectangle destRect = new Rectangle(clientRectangle.X + (clientRectangle.Width - base.Image.Width) / 2, clientRectangle.Y + (clientRectangle.Height - base.Image.Height) / 2, base.Image.Width, base.Image.Height);
				graphics.DrawImage(base.Image, destRect, 0, 0, base.Image.Width, base.Image.Height, GraphicsUnit.Pixel, imageAttributes);
				return;
			}
			Rectangle rect = clientRectangle;
			Color color = SystemColors.ControlText;
			if (!base.Enabled)
			{
				color = NomadButton.disableText;
			}
			buttonShader.DrawText(graphics, rect, this.Text, this.Font, color);
		}
	}
}
