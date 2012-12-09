using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
namespace FC3Editor.UI
{
	internal class Watermark : Form
	{
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 524320;
				return createParams;
			}
		}
		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			base.ShowInTaskbar = false;
			base.Width = 700;
			base.Height = 100;
			Bitmap bitmap = new Bitmap(base.Width, base.Height);
			using (Graphics graphics = Graphics.FromImage(bitmap))
			{
				graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
				Font font = new Font("Tahoma", 16f, FontStyle.Bold);
				Brush brush = new SolidBrush(Color.Black);
				StringFormat stringFormat = new StringFormat();
				stringFormat.Alignment = StringAlignment.Far;
				graphics.DrawString("ALPHA BUILD -WORK IN PROGRESS-", font, brush, base.ClientRectangle, stringFormat);
				brush.Dispose();
				font.Dispose();
			}
			Win32.UpdateLayeredWindowHelper(this, bitmap);
		}
	}
}
