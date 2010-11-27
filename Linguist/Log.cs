using System;
using System.IO;

namespace Linguist
{
	// Logs to a text file within the <user>/AppData/Local/Linguist directory.
	internal static class Log
	{
		static Log()
		{
			try
			{
				string dir = Constants.LinguistPath;

				string file = Path.Combine(dir, "Log2.txt");
				ms_writer = new StreamWriter(file);

				WriteLine("Started up on {0}.", DateTime.Now);
			}
			catch (Exception e)
			{
				// TODO: use the event logger?
				Console.Error.WriteLine("Error initializing the logger:");
				Console.Error.WriteLine(e.Message);
			}
		}

		public static void Indent()
		{
			++ms_indent;
		}

		public static void Unndent()
		{
			if (ms_indent > 0)
				--ms_indent;
		}

		public static void WriteLine()
		{
			if (ms_writer != null)
			{
				ms_writer.WriteLine();
			}
		}

		public static void WriteLine(string line)
		{
			if (ms_writer != null)
			{
				if (ms_indent > 0)
					ms_writer.Write(new string('\t', ms_indent));
				ms_writer.WriteLine(line);
				ms_writer.Flush();
			}
		}

		public static void WriteLine(string format, params object[] args)
		{
			WriteLine(string.Format(format, args));
		}

		#region Fields
		private static StreamWriter ms_writer;
		private static int ms_indent;
		#endregion
	}
}
