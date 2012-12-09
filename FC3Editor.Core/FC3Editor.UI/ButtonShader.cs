using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
namespace FC3Editor.UI
{
	internal class ButtonShader
	{
		private Color[] colorStart;
		private Color[] colorEnd;
		private ColorBlend colorMiddle;
		private Color colorEdge;
		private int border;
		public static bool shadersInit;
		public static ButtonShader normalShader;
		public static ButtonShader hoverShader;
		public static ButtonShader pushShader;
		public static ButtonShader disableShader;
		public static ButtonShader checkShader;
		public ButtonShader(float hue, float saturation, float start, float start2, float end2, float end, float edge, int border)
		{
			this.ComputeColors(hue, saturation, start, start2, end2, end, edge, border);
		}
		public void ComputeColors(float hue, float saturation, float start, float start2, float end2, float end, float edge, int border)
		{
			this.border = border;
			this.colorStart = new Color[border];
			this.colorEnd = new Color[border];
			float num = (start2 - start) / (float)border;
			float num2 = (end2 - end) / (float)border;
			float num3 = start;
			float num4 = end;
			for (int i = 0; i < border; i++)
			{
				this.colorStart[i] = ColorUtils.HSLToRGB(hue, saturation, num3);
				this.colorEnd[i] = ColorUtils.HSLToRGB(hue, saturation, num4);
				num3 += num;
				num4 += num2;
			}
			this.colorEdge = ColorUtils.HSLToRGB(hue, saturation, edge);
			float num5 = (end2 - start2) / 9f;
			float num6 = start2;
			this.colorMiddle = new ColorBlend(10);
			for (int j = 0; j < 10; j++)
			{
				this.colorMiddle.Positions[j] = (float)j / 9f;
				this.colorMiddle.Colors[j] = ColorUtils.HSLToRGB(hue, saturation, num6);
				num6 += num5;
			}
		}
		public void DrawButton(Graphics g, Rectangle rect, Color backColor)
		{
			Rectangle rect2 = rect;
			Pen pen = new Pen(this.colorEdge);
			g.DrawRectangle(pen, rect2.X, rect2.Y, rect2.Width - 1, rect2.Height - 1);
			rect2.Inflate(-1, -1);
			for (int i = 0; i < this.border; i++)
			{
				using (Pen pen2 = new Pen(this.colorStart[i]))
				{
					g.DrawLine(pen2, rect2.X + i, rect2.Y + i, rect2.Right - 1 - i, rect2.Y + i);
					g.DrawLine(pen2, rect2.X + i, rect2.Y + i, rect2.X + i, rect2.Bottom - 1 - i);
				}
				using (Pen pen3 = new Pen(this.colorEnd[i]))
				{
					g.DrawLine(pen3, rect2.Right - 1 - i, rect2.Y + i, rect2.Right - 1 - i, rect2.Bottom - 1 - i);
					g.DrawLine(pen3, rect2.X + i, rect2.Bottom - 1 - i, rect2.Right - 1 - i, rect2.Bottom - 1 - i);
				}
			}
			rect2.Inflate(-this.border, -this.border);
			using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(rect2, Color.Black, Color.White, 90f))
			{
				linearGradientBrush.InterpolationColors = this.colorMiddle;
				g.FillRectangle(linearGradientBrush, rect2);
			}
			rect2 = rect;
			using (Pen pen4 = new Pen(backColor))
			{
				g.DrawRectangle(pen4, rect2.X, rect2.Y, 1, 1);
				g.DrawRectangle(pen4, rect2.Right - 2, rect2.Y, 1, 1);
				g.DrawRectangle(pen4, rect2.X, rect2.Bottom - 2, 1, 1);
				g.DrawRectangle(pen4, rect2.Right - 2, rect2.Bottom - 2, 1, 1);
			}
			g.DrawLine(pen, rect2.X, rect2.Y + 2, rect2.X + 2, rect2.Y);
			g.DrawLine(pen, rect2.Right - 3, rect2.Y, rect2.Right - 1, rect2.Y + 2);
			g.DrawLine(pen, rect2.X, rect2.Bottom - 3, rect2.X + 2, rect2.Bottom - 1);
			g.DrawLine(pen, rect2.Right - 3, rect2.Bottom - 1, rect2.Right - 1, rect2.Bottom - 3);
			pen.Dispose();
		}
		public void DrawText(Graphics g, Rectangle rect, string text, Font font, Color color)
		{
			using (SolidBrush solidBrush = new SolidBrush(color))
			{
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Center;
				stringFormat.LineAlignment = StringAlignment.Center;
				stringFormat.Trimming = StringTrimming.EllipsisCharacter;
				stringFormat.HotkeyPrefix = HotkeyPrefix.Show;
				Rectangle r = rect;
				r.Inflate(-this.border - 2, -this.border - 2);
				g.DrawString(text, font, solidBrush, r, stringFormat);
			}
		}
		public static void InitShaders()
		{
			if (ButtonShader.shadersInit)
			{
				return;
			}
			float hue;
			float saturation;
			float num;
			ColorUtils.RGBToHSL(SystemColors.ControlLight, out hue, out saturation, out num);
			ButtonShader.normalShader = new ButtonShader(hue, saturation, 1f, 1f, 0.72f, 0.63f, 0.33f, 1);
			ButtonShader.hoverShader = new ButtonShader(hue, saturation, 1f, 1f, 0.87f, 0.72f, 0.33f, 1);
			ButtonShader.pushShader = new ButtonShader(hue, saturation, 0.5f, 0.65f, 0.95f, 1f, 0.33f, 0);
			ButtonShader.disableShader = new ButtonShader(hue, saturation, 0.97f, 0.93f, 0.9f, 0.78f, 0.83f, 0);
			ColorUtils.RGBToHSL(SystemColors.Highlight, out hue, out saturation, out num);
			ButtonShader.checkShader = new ButtonShader(hue, 0.75f, 0.8f, 0.7f, 0.95f, 1f, 0.24f, 0);
			ButtonShader.shadersInit = true;
		}
	}
}
