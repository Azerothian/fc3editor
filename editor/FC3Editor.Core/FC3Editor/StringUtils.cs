using System;
namespace FC3Editor
{
	internal static class StringUtils
	{
		public const int MAX_MAPNAME_LENGTH = 20;
		public const int MAX_AUTHORNAME_LENGTH = 20;
		public const int MAX_CREATORNAME_LENGTH = 20;
		public const int MAX_TEXTFIELD_LENGTH = 128;
		public static string EscapeUIString(string s)
		{
			return s.Replace("&", "&&");
		}
	}
}
