using System;
namespace FC3Editor.Parameters
{
	internal class ParamEnum<T> : ParamEnumBase<T>
	{
		public ParamEnum(string display, T value, ParamEnumUIType uiType) : base(display, value, uiType)
		{
			base.Names = Enum.GetNames(typeof(T));
			base.Values = (T[])Enum.GetValues(typeof(T));
		}
	}
}
