using System;
namespace FC3Editor.Parameters
{
	internal class ParamEnumAngles : ParamEnumBase<float>
	{
		private static string[] s_names = new string[]
		{
			"5",
			"10",
			"20",
			"45",
			"90"
		};
		private static float[] s_values = new float[]
		{
			5f,
			10f,
			20f,
			45f,
			90f
		};
		public ParamEnumAngles(string display, float value, ParamEnumUIType uiType) : base(display, value, uiType)
		{
			base.Names = ParamEnumAngles.s_names;
			base.Values = ParamEnumAngles.s_values;
		}
	}
}
