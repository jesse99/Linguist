using System;
//using System.Collections.Generic;
using System.IO;
//using System.Linq;
using System.Reflection;
//using System.Text;

namespace Linguist
{
	internal static class InstallFiles
	{
		public static void Install()
		{
			DoCreateDirs();
			DoInstallFiles();
		}

		#region Private Methods
		private static void DoCreateDirs()
		{
			if (!Directory.Exists(Constants.LinguistPath))
				Directory.CreateDirectory(Constants.LinguistPath);

			if (!Directory.Exists(Constants.StandardPath))
				Directory.CreateDirectory(Constants.StandardPath);

			if (!Directory.Exists(Constants.CustomPath))
				Directory.CreateDirectory(Constants.CustomPath);
		}

		private static void DoInstallFiles()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			DateTime time = File.GetLastWriteTime(assembly.Location);

			string prefix = "Linguist.languages.";
			foreach (string name in assembly.GetManifestResourceNames())
			{
				// Names look like: Linguist.languages.Field.lang
				System.Diagnostics.Debug.Assert(name.StartsWith(prefix));
				string dstFile = Path.Combine(Constants.StandardPath, name.Substring(prefix.Length));
				DoInstall(assembly, time, name, dstFile);
			}
		}

		private static void DoInstall(Assembly assembly, DateTime time, string name, string dstFile)
		{
			if (!File.Exists(dstFile) || File.GetLastWriteTime(dstFile) < time)
			{
				Log.WriteLine("Installing {0}", dstFile);
				using (Stream srcStream = assembly.GetManifestResourceStream(name))
				{
					using (FileStream dstStream = File.Open(dstFile, FileMode.OpenOrCreate, FileAccess.Write))
					{
						dstStream.SetLength(0);
						srcStream.CopyTo(dstStream);
					}
				}
			}
		}
		#endregion
	}
}
