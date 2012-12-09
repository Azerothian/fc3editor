using System;
namespace FC3Editor.UI
{
	public class IndentedComboboxItemEventArgs : EventArgs
	{
		private IndentedComboBox.Item m_item;
		public IndentedComboBox.Item Item
		{
			get
			{
				return this.m_item;
			}
		}
		public IndentedComboboxItemEventArgs(IndentedComboBox.Item item)
		{
			this.m_item = item;
		}
	}
}
