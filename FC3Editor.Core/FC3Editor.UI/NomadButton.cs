using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class NomadButton : Button
	{
		private bool mouseOver;
		private bool pushed;
		public static Color disableText = Color.FromArgb(180, 180, 180);
		public static ColorMatrix disableMatrix;
		public NomadButton()
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
		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			base.OnKeyDown(kevent);
			if (kevent.KeyCode == Keys.Space)
			{
				this.pushed = true;
				this.Refresh();
			}
		}
		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			base.OnKeyUp(kevent);
			if (kevent.KeyCode == Keys.Space)
			{
				this.pushed = false;
				this.Refresh();
			}
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
			if (this.Focused)
			{
				Rectangle rectangle = clientRectangle;
				rectangle.Inflate(-2, -2);
				ControlPaint.DrawFocusRectangle(graphics, rectangle);
			}
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
			if (this.pushed)
			{
				rect.Offset(1, 1);
			}
			Color controlText = SystemColors.ControlText;
			if (!base.Enabled)
			{
				controlText = NomadButton.disableText;
			}
			buttonShader.DrawText(graphics, rect, this.Text, this.Font, controlText);
		}
		static NomadButton()
		{
			// Note: this type is marked as 'beforefieldinit'.
			float[][] array = new float[5][];
			array[0] = new float[]
			{
				0.3f,
				0.3f,
				0.3f,
				0f,
				0f
			};
			array[1] = new float[]
			{
				0.59f,
				0.59f,
				0.59f,
				0f,
				0f
			};
			array[2] = new float[]
			{
				0.11f,
				0.11f,
				0.11f,
				0f,
				0f
			};
			float[][] arg_6E_0 = array;
			int arg_6E_1 = 3;
			float[] array2 = new float[5];
			array2[3] = 0.33f;
			arg_6E_0[arg_6E_1] = array2;
			array[4] = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			};
			NomadButton.disableMatrix = new ColorMatrix(array);
		}
	}
}
