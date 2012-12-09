using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace FC3Editor.Nomad
{
	internal struct ImageMapEngine
	{
		private IntPtr m_pointer;
		public Size Size
		{
			get
			{
				int width;
				int height;
				Binding.FCE_ImageMap_GetSize(this.m_pointer, out width, out height);
				return new Size(width, height);
			}
		}
		public ImageMapEngine(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
		public void Dispose()
		{
			Binding.FCE_ImageMap_Destroy(this.m_pointer);
			this.m_pointer = IntPtr.Zero;
		}
		public ImageMapEngine Clone()
		{
			return new ImageMapEngine(Binding.FCE_ImageMap_Clone(this.m_pointer));
		}
		public ImageMap GetImage()
		{
			Size size = this.Size;
			Bitmap bitmap = new Bitmap(size.Width, size.Height);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
			float min;
			float max;
			Binding.FCE_ImageMap_ConvertTo24bit(this.m_pointer, bitmapData.Scan0, bitmapData.Stride, out min, out max);
			bitmap.UnlockBits(bitmapData);
			return new ImageMap(bitmap, min, max);
		}
	}
}
