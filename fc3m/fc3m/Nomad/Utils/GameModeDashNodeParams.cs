using System;
namespace Nomad.Utils
{
	public class GameModeDashNodeParams
	{
		private int m_index;
		public float AssaultTime
		{
			get
			{
				return Binding.FCE_Dash_GetNodeAssaultTime(this.m_index);
			}
			set
			{
				Binding.FCE_Dash_SetNodeAssaultTime(this.m_index, value);
			}
		}
		public GameModeDashNodeParams(int index)
		{
			this.m_index = index;
		}
	}
}
