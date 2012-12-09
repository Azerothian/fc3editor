using System;
namespace FC3Editor.Nomad
{
	internal class UndoManager
	{
		public static int UndoCount
		{
			get
			{
				return Binding.FCE_UndoManager_GetUndoCount();
			}
		}
		public static int RedoCount
		{
			get
			{
				return Binding.FCE_UndoManager_GetRedoCount();
			}
		}
		public static void Undo()
		{
			Binding.FCE_UndoManager_Undo();
		}
		public static void Redo()
		{
			Binding.FCE_UndoManager_Redo();
		}
		public static void RecordUndo()
		{
			Binding.FCE_UndoManager_RecordUndo();
		}
		public static void CommitUndo()
		{
			Binding.FCE_UndoManager_CommitUndo();
		}
	}
}
