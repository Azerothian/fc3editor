using FC3Editor.Nomad;
using System;
namespace FC3Editor.UI
{
	internal class ParamSplineSlotList : ParamSlotList
	{
		protected override Inventory.Entry Root
		{
			get
			{
				return SplineInventory.Instance.Root;
			}
		}
		protected override int SlotCount
		{
			get
			{
				return 8;
			}
		}
		public ParamSplineSlotList()
		{
			this.label1.Text = Localizer.Localize("PARAM_ROADS");
		}
		protected override SlotItem GetSlot(int index)
		{
			SplineRoad roadFromId = SplineManager.GetRoadFromId(index);
			if (!roadFromId.IsValid)
			{
				return null;
			}
			return new SlotItem(index, roadFromId.Entry);
		}
		protected override void OnAssignSlot(int id, Inventory.Entry entry)
		{
			base.OnAssignSlot(id, entry);
			if (entry == null || !entry.IsValid)
			{
				SplineManager.DestroyRoad(id);
				return;
			}
			SplineRoad splineRoad = SplineManager.GetRoadFromId(id);
			if (!splineRoad.IsValid)
			{
				splineRoad = SplineManager.CreateRoad(id);
			}
			SplineInventory.Entry entry2 = (SplineInventory.Entry)entry;
			splineRoad.Entry = entry2;
			splineRoad.Width = entry2.DefaultWidth;
			splineRoad.UpdateSpline();
		}
	}
}
