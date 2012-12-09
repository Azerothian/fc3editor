using System;
using System.Drawing;
using System.Drawing.Imaging;
namespace FC3Editor.Nomad
{
	internal struct Snapshot
	{
		private IntPtr m_pointer;
		public IntPtr Pointer
		{
			get
			{
				return this.m_pointer;
			}
		}
		public Snapshot(IntPtr ptr)
		{
			this.m_pointer = ptr;
		}
		public static Snapshot Create(int width, int height)
		{
			return new Snapshot(Binding.FCE_Snapshot_Create(width, height));
		}
		public void Destroy()
		{
			Binding.FCE_Snapshot_Destroy(this.m_pointer);
			this.m_pointer = IntPtr.Zero;
		}
		public Image GetImage()
		{
			IntPtr intPtr;
			uint width;
			uint height;
			uint num;
			Binding.FCE_Snapshot_GetData(this.m_pointer, out intPtr, out width, out height, out num);
			Bitmap bitmap = new Bitmap((int)width, (int)height);
			BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			for (int i = 0; i < bitmap.Height; i++)
			{
				Win32.RtlMoveMemory((IntPtr)(bitmapData.Scan0.ToInt32() + i * bitmapData.Stride), (IntPtr)((long)intPtr.ToInt32() + (long)i * (long)((ulong)num)), bitmap.Width * 4);
			}
			bitmap.UnlockBits(bitmapData);
			return bitmap;
		}
	}
}
