using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
namespace FC3Editor.Nomad
{
	internal class ObjectRenderer
	{
		public interface IListener
		{
			void ProcessObject(ObjectInventory.Entry entry, Image img);
		}
		private static Dictionary<IntPtr, Image> m_cachedImages = new Dictionary<IntPtr, Image>();
		private static List<ObjectRenderer.IListener> m_listeners = new List<ObjectRenderer.IListener>();
		public static string cachePath = Path.GetTempPath() + "\\FarCry3\\Editor\\";
		private static bool IsSnapshotReady
		{
			get
			{
				return Binding.FCE_ObjectRenderer_IsSnapshotReady();
			}
		}
		public static void RegisterListener(ObjectRenderer.IListener listener)
		{
			if (ObjectRenderer.m_listeners.Contains(listener))
			{
				return;
			}
			ObjectRenderer.m_listeners.Add(listener);
		}
		public static void UnregisterListener(ObjectRenderer.IListener listener)
		{
			ObjectRenderer.m_listeners.Remove(listener);
		}
		private static void TriggerListeners(ObjectInventory.Entry entry, Image img)
		{
			ObjectRenderer.IListener[] array = ObjectRenderer.m_listeners.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				ObjectRenderer.IListener listener = array[i];
				listener.ProcessObject(entry, img);
			}
			if (img != null)
			{
				img.Dispose();
			}
		}
		public static void RequestObjectImage(ObjectInventory.Entry entry)
		{
		}
		public static void Update(float dt)
		{
		}
		public static void Clear()
		{
			Binding.FCE_ObjectRenderer_Clear();
		}
		private static bool ThumbnailDummy()
		{
			return false;
		}
		private static void GetSnapshot(out Image img, out ObjectInventory.Entry entry)
		{
			int num;
			int num2;
			int num3;
			int num4;
			Snapshot snapshot = new Snapshot(Binding.FCE_ObjectRenderer_GetSnapshot(out num, out num2, out num3, out num4));
			int num5 = num3 - num;
			int num6 = num4 - num2;
			if (num5 > num6)
			{
				int num7 = (num5 - num6) / 2;
				num2 -= num7;
				num4 += num7;
			}
			else
			{
				int num8 = (num6 - num5) / 2;
				num -= num8;
				num3 += num8;
			}
			Image image = snapshot.GetImage();
			Bitmap bitmap = new Bitmap(256, 256);
			img = bitmap;
			using (Graphics graphics = Graphics.FromImage(img))
			{
				graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
				graphics.DrawImage(image, new Rectangle(3, 3, bitmap.Width - 6, bitmap.Height - 6), new Rectangle(num, num2, num3 - num, num4 - num2), GraphicsUnit.Pixel);
			}
			entry = new ObjectInventory.Entry(Binding.FCE_ObjectRenderer_GetSnapshotEntry());
			Directory.CreateDirectory(ObjectRenderer.cachePath);
			try
			{
				ObjectRenderer.WritePNG(ObjectRenderer.cachePath + entry.Id + ".png", bitmap);
			}
			catch (Exception)
			{
			}
		}
		private static void ClearSnapshot()
		{
			Binding.FCE_ObjectRenderer_ClearSnapshot();
		}
		private static void RenderObject(ObjectInventory.Entry entry)
		{
			Binding.FCE_ObjectRenderer_RenderObject(entry.Pointer);
		}
		public static void ClearCache()
		{
			string[] files = Directory.GetFiles(ObjectRenderer.cachePath);
			for (int i = 0; i < files.Length; i++)
			{
				string path = files[i];
				File.Delete(path);
			}
		}
		private static void WritePNG(string filename, Bitmap bmp)
		{
			BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			Binding.FCE_ObjectRenderer_WritePNG(bitmapData.Scan0, bmp.Width, bmp.Height, filename);
			bmp.UnlockBits(bitmapData);
		}
		public static void GenerateThumbnails()
		{
			Binding.FCE_ObjectRenderer_GenerateThumbnails();
		}
		public static void ResizeThumbnails(string subDir, int targetResolution, int targetColResolution, Color background)
		{
			string path = string.Format("{0}\\ingameeditor\\thumbnails\\png_src\\", Engine.GenericDataPath);
			if (!Directory.Exists(path))
			{
				MessageBox.Show("Cannot find thumbnails path, resizing failed.");
				return;
			}
			string[] files = Directory.GetFiles(path, "*.png");
			for (int i = 0; i < files.Length; i++)
			{
				string text = files[i];
				string fileName = Path.GetFileName(text);
				Image image = Image.FromFile(text);
				Rectangle srcRect = new Rectangle(0, 0, image.Width, image.Height);
				int num = targetResolution;
				if (fileName.StartsWith("col"))
				{
					if ((float)srcRect.Width / (float)srcRect.Height > 2f)
					{
						int num2 = (int)((float)Math.Min(srcRect.Height, srcRect.Width) * 0.8f);
						srcRect = new Rectangle((srcRect.Width - num2) / 2, 0, num2, num2);
					}
					num = targetColResolution;
				}
				int num3 = 3;
				int num4 = num - num3 * 2;
				int num5;
				int num6;
				if (image.Width > image.Height)
				{
					num5 = num4;
					num6 = (int)((float)num4 / (float)srcRect.Width * (float)srcRect.Height);
				}
				else
				{
					num6 = num4;
					num5 = (int)((float)num4 / (float)srcRect.Height * (float)srcRect.Width);
				}
				Bitmap bitmap = new Bitmap(num, num);
				Image image2 = bitmap;
				using (Graphics graphics = Graphics.FromImage(image2))
				{
					using (SolidBrush solidBrush = new SolidBrush(background))
					{
						graphics.FillRectangle(solidBrush, 0, 0, bitmap.Width, bitmap.Height);
					}
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.DrawImage(image, new Rectangle(num3 + (num4 - num5) / 2, num3 + (num4 - num6) / 2, num5, num6), srcRect, GraphicsUnit.Pixel);
				}
				string text2 = string.Format("{0}\\ingameeditor\\thumbnails\\{1}\\", Engine.GenericDataPath, subDir);
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				string filename = text2 + Path.GetFileNameWithoutExtension(text) + ".png";
				BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
				Binding.FCE_ObjectRenderer_WritePNG(bitmapData.Scan0, bitmap.Width, bitmap.Height, filename);
				bitmap.UnlockBits(bitmapData);
				bitmap.Dispose();
				image.Dispose();
			}
		}
		public static void ResizeThumbnails()
		{
			ObjectRenderer.ResizeThumbnails("pc", 256, 256, Color.FromArgb(0, 127, 127, 127));
			ObjectRenderer.ResizeThumbnails("console", 128, 256, Color.FromArgb(0, 32, 32, 32));
		}
	}
}
