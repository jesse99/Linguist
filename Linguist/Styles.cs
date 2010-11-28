using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Text;
using System.Windows.Media;
//using Microsoft.VisualStudio.Text.Classification;

namespace Linguist
{
	// Used to initialize format definitions from a Styles.field file.
	internal static class Styles
	{
		static public void Init()
		{
			if (ms_styles.Count == 0)
			{
				string path = Path.Combine(Constants.CustomPath, "Styles.field");
				if (File.Exists(path))
					DoLoadStyles(path);

				path = Path.Combine(Constants.StandardPath, "Styles.field");
				DoLoadStyles(path);
			}
		}

		// Name is "comment", "keyword", etc.
		public static Style FindStyle(string name)
		{
			Style style;
			if (!ms_styles.TryGetValue(name, out style))
			{
				if (!ms_missingElements.Contains(name))
				{
					ms_missingElements.Add(name);
					Log.WriteLine("No style for element {0}.", name);
				}
			}

			return style;
		}

		#region Private Methods
		private static void DoLoadStyles(string path)
		{
			Log.WriteLine("Loading {0}", path);

			try
			{
				string contents = File.ReadAllText(path);
				Field[] fields = FieldParser.Parse(contents);

				for (int i = 0; i < fields.Length; ++i)
				{
					if (fields[i].Name == "Name")
						DoProcessStyle(fields, i);
				}

				ms_default = FindStyle("default");
			}
			catch (Exception e)
			{
				Log.WriteLine(e.Message);
			}

			Log.WriteLine(string.Empty);
		}

		private static void DoProcessStyle(Field[] fields, int i)
		{
			try
			{
				Log.Indent();

				string name = fields[i++].Value;
				Log.WriteLine("Processing {0} style", name);
				var style = new Style();

				while (i < fields.Length && fields[i].Name != "Name")
				{
					DoProcessField(style, fields[i++]);
				}

				if (!ms_styles.ContainsKey(name))
					ms_styles.Add(name, style);
				else
					Log.WriteLine("Ignoring style {0} (it was already defined).", name);
			}
			catch (Exception e)
			{
				Log.WriteLine(e.Message);
			}
			finally
			{
				Log.Unndent();
			}
		}

		private static void DoProcessField(Style style, Field field)
		{
			switch (field.Name)
			{
				case "BackColor":
					style.BackColor = DoParseColor(field.Name, field.Value);
					break;

				case "Bold":
					style.Bold = DoParseBool(field.Name, field.Value);
					break;

				case "FontName":
					style.FontName = field.Value;
					break;

				case "ForeColor":
					style.ForeColor = DoParseColor(field.Name, field.Value);
					break;

				case "Italic":
					style.Italic = DoParseBool(field.Name, field.Value);
					break;

				case "PointSize":
					style.PointSize = DoParseDouble(field.Name, field.Value);
					break;

				default:
					Log.WriteLine("Ignoring field {0},", field.Name);
					break;
			}
		}

		private static bool DoParseBool(string name, string text)
		{
			bool result = false;

			if (text == "true")
				result = true;
			else
				Log.WriteLine("Expected 'true' or 'false' for {0} not '{1}'.", name, text);

			return result;
		}

		private static Color DoParseColor(string name, string text)
		{
			Color result = Colors.Black;

			if (!ColorFromString.TryParse(text, out result))
				Log.WriteLine("Expected an X11 color name ('Red'), a hex RGB color ('#0000FF'), or a decimal RGB color ('0,0,255') for {0} not '{1}'.", name, text);

			return result;
		}

		private static double DoParseDouble(string name, string text)
		{
			double result = 12.0;

			if (!double.TryParse(text, out result))
				Log.WriteLine("Expected a floating point number for {0} not '{1}'.", name, text);

			return result;
		}
		#endregion

		#region Fields
		private static Dictionary<string, Style> ms_styles = new Dictionary<string, Style>();
		private static HashSet<string> ms_missingElements = new HashSet<string>();
		private static Style ms_default;
		#endregion
	}
}
