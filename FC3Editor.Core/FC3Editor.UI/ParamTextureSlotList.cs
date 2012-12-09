using FC3Editor.Nomad;
using System;
namespace FC3Editor.UI
{
	internal class ParamTextureSlotList : ParamSlotList
	{
		protected override Inventory.Entry Root
		{
			get
			{
				return TextureInventory.Instance.Root;
			}
		}
		protected override int SlotCount
		{
			get
			{
				return 4;
			}
		}
		protected override bool KeepFirst
		{
			get
			{
				return true;
			}
		}
		public ParamTextureSlotList()
		{
			this.label1.Text = Localizer.Localize("PARAM_TEXTURES");
		}
		protected override SlotItem GetSlot(int index)
		{
			TextureInventory.Entry textureEntryFromId = TerrainManager.GetTextureEntryFromId(index);
			if (!textureEntryFromId.IsValid)
			{
				return null;
			}
			return new SlotItem(index, textureEntryFromId);
		}
		protected override void OnAssignSlot(int id, Inventory.Entry entry)
		{
			base.OnAssignSlot(id, entry);
			if (entry == null || !entry.IsValid)
			{
				TerrainManager.ClearTextureId(id);
				TerrainManager.AssignTextureId(id, TextureInventory.Entry.Null);
				return;
			}
			TerrainManager.AssignTextureId(id, (TextureInventory.Entry)entry);
		}
	}
}
