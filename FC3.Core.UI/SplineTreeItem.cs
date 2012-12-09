using FC3Editor.Nomad;
using System;
namespace FC3Editor.UI
{
	internal class SplineTreeItem
	{
		private int m_id;
		private SplineRoad m_spline;
		public int Id
		{
			get
			{
				return this.m_id;
			}
		}
		public SplineRoad Spline
		{
			get
			{
				return this.m_spline;
			}
		}
		public SplineTreeItem(int id, SplineRoad spline)
		{
			this.m_id = id;
			this.m_spline = spline;
		}
	}
}
