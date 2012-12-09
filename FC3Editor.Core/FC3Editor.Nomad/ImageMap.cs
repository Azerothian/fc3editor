using System;
using System.Drawing;
namespace FC3Editor.Nomad
{
	internal class ImageMap
	{
		private Image m_image;
		private float m_min;
		private float m_max;
		public Image Image
		{
			get
			{
				return this.m_image;
			}
		}
		public float Minimum
		{
			get
			{
				return this.m_min;
			}
		}
		public float Maximum
		{
			get
			{
				return this.m_max;
			}
		}
		public ImageMap(Image img, float min, float max)
		{
			this.m_image = img;
			this.m_min = min;
			this.m_max = max;
		}
		public void Dispose()
		{
			this.m_image.Dispose();
		}
	}
}
