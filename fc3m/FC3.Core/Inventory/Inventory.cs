using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
namespace Nomad.Inventory
{
	public abstract class Inventory : IDisposable
	{
		
		private List<InventoryEntry> m_ownedDirectories = new List<InventoryEntry>();
		public abstract InventoryEntry Root
		{
			get;
		}
		public void Dispose()
		{
			foreach (InventoryEntry current in this.m_ownedDirectories)
			{
				this.DestroyFilterDirectory(current);
			}
		}
		public InventoryEntry CreateDirectory()
		{
			InventoryEntry entry = this.CreateFilterDirectory();
			if (entry != null)
			{
				this.m_ownedDirectories.Add(entry);
			}
			return entry;
		}
		protected virtual InventoryEntry CreateFilterDirectory()
		{
			return null;
		}
		protected virtual void DestroyFilterDirectory(InventoryEntry entry)
		{
		}
		public virtual void SearchInventory(string criteria, InventoryEntry resultEntry)
		{
		}
	}
}
