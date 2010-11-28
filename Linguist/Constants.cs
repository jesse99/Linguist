using System;

namespace Linguist
{
	internal static class Constants
	{
		public static string LinguistPath
		{
			get
			{
				string dir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);
				return System.IO.Path.Combine(dir, "Linguist");
			}
		}

		public static string StandardPath
		{
			get
			{
				return System.IO.Path.Combine(LinguistPath, "standard");
			}
		}

		public static string CustomPath
		{
			get
			{
				return System.IO.Path.Combine(LinguistPath, "custom");
			}
		}
	}
}
