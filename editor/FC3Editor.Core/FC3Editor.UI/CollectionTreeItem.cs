using FC3Editor.Nomad;
using System;
namespace FC3Editor.UI
{
	internal class CollectionTreeItem
	{
		private Inventory.Entry m_entry;
		private int m_id;
		public Inventory.Entry Entry
		{
			get
			{
				return this.m_entry;
			}
		}
		public int Id
		{
			get
			{
				return this.m_id;
			}
		}
		public CollectionTreeItem(int id, Inventory.Entry entry)
		{
			this.m_id = id;
			this.m_entry = entry;
		}
	}
}
