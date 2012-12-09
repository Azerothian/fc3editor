using FC3Editor.Nomad;
using System;
namespace FC3Editor.UI
{
	internal class ParamCollectionSlotList : ParamSlotList
	{
		protected override Inventory.Entry Root
		{
			get
			{
				return CollectionInventory.Instance.Root;
			}
		}
		protected override int SlotCount
		{
			get
			{
				return 8;
			}
		}
		public ParamCollectionSlotList()
		{
			this.label1.Text = Localizer.Localize("PARAM_COLLECTIONS");
		}
		protected override SlotItem GetSlot(int index)
		{
			CollectionInventory.Entry collectionEntryFromId = CollectionManager.GetCollectionEntryFromId(index);
			if (!collectionEntryFromId.IsValid)
			{
				return null;
			}
			return new SlotItem(index, collectionEntryFromId);
		}
		protected override void OnAssignSlot(int id, Inventory.Entry entry)
		{
			base.OnAssignSlot(id, entry);
			if (entry == null || !entry.IsValid)
			{
				CollectionManager.ClearMaskId(id);
				CollectionManager.AssignCollectionId(id, CollectionInventory.Entry.Null);
				return;
			}
			CollectionManager.AssignCollectionId(id, (CollectionInventory.Entry)entry);
		}
	}
}
