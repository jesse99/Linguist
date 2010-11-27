using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Linguist
{
	internal struct Field
	{
		public Field(string name, string value, int line)
			: this()
		{
			Debug.Assert(!string.IsNullOrEmpty(name), "name is empty or null");
			Debug.Assert(value != null, "value is null");
			Debug.Assert(line >= 1, "line isn't positive");

			Name = name;
			Value = value;
			Line = line;
		}

		public string Name { get; private set; }
		public string Value { get; private set; }
		public int Line { get; private set; }

		public override string ToString()
		{
			if (Value.Length < 32)
				return string.Format("{0} = '{1}'", Name, Value);
			else
				return string.Format("{0} = '{1}...'", Name, Value.Substring(0, 32));
		}
	}

	// Simple field oriented parser based on a personal project. The syntax is as follows:
	//
	// Comment lines start with a '#' character.
	//
	// Blank lines contain only whitespace and are ignored (if they are not indented).
	//
	// Text fields start with an identifier immediately followed by a ':'.
	// The text appears after the colon and, if multiple lines are used, the additional lines must be indented.
	// Whitespace at the start and end of the text are stripped off. Runs of interior whitespace are replaced by a single space.
	//
	// Literal fields start with an identifier immediately followed by a '!'.
	// The text appears after the colon and, if multiple lines are used, the additional lines must be indented.
	// Whitespace is left as is.
	//
	// Identifiers start with a letter followed by alpha-numeric characters, underscores, and dashes.
	//
	// Field names need not be unique.
	internal static class FieldParser
	{
		// Used to optionally filter each line in a test or literal field.
		public delegate string Filter(string fieldName, string line);

		public static Field[] Parse(string contents)
		{
			return Parse(contents, null);
		}

		public static Field[] Parse(string[] lines)
		{
			return Parse(lines, null);
		}

		public static Field[] Parse(string contents, Filter filter)
		{
			var lines = new List<string>();

			int i = 0;
			while (i < contents.Length)
			{
				int j = contents.IndexOfAny(new[] { '\r', '\n' }, i);
				if (j >= 0)
				{
					if (j + 1 < contents.Length && contents[j] == '\r' && contents[j + 1] == '\n')
						++j;
					lines.Add(contents.Substring(i, j - i + 1));
					i = j + 1;
				}
				else
				{
					lines.Add(contents.Substring(i));
					i = contents.Length;
				}
			}

			return Parse(lines.ToArray(), filter);
		}

		public static Field[] Parse(string[] lines, Filter filter)
		{
			var fields = new List<Item>();

			bool canContinue = false;
			for (int i = 0; i < lines.Length; ++i)
			{
				string line = lines[i];

				// field
				if (line.Length > 1 && char.IsLetter(line[0]))
				{
					fields.Add(DoParseField(line, i + 1, filter));
					canContinue = true;
				}

				// continued
				else if (line.Length > 0 && line[0] == '\t')
				{
					if (!canContinue)
						throw new FormatException("Line " + (i + 1) + " is a continued line, but the previous line is not a field or continued line");

					if (filter != null)
						line = filter(fields[fields.Count - 1].Name, line);

					fields[fields.Count - 1].Value.Append(line);
					canContinue = true;
				}

				// comment
				else if (line.Length >= 2 && line.StartsWith("#"))
				{
					canContinue = false;
					continue;
				}

				// blank
				else if (line == "\n" || line == "\r" || line == "\r\n" || line.Length == 0)
				{
					canContinue = false;
					continue;
				}

				// error
				else
					throw new FormatException("Couldn't parse line " + (i + 1));
			}

			Field[] result = new Field[fields.Count];
			for (int j = 0; j < fields.Count; ++j)
				result[j] = fields[j].ToField();

			return result;
		}

		#region Private Methods
		private static Item DoParseField(string line, int lineNum, Filter filter)
		{
			int i = line.IndexOfAny(new[] { ':', '!' });
			if (i <= 0)
				throw new FormatException("Line " + lineNum + " has no colon or exclamation mark");

			string name = line.Substring(0, i);
			string value = line.Substring(i + 1);

			if (filter != null)
				value = filter(name, value);

			return new Item(name, value, lineNum, line[i] == ':');
		}
		#endregion

		#region Private Types
		private struct Item
		{
			public Item(string name, string value, int line, bool textField)
				: this()
			{
				Debug.Assert(!string.IsNullOrEmpty(name), "name is empty or null");
				Debug.Assert(value != null, "value is null");
				Debug.Assert(line >= 1, "value isn't positive");

				Name = name;
				Value = new StringBuilder();
				Line = line;
				TextField = textField;

				Value.Append(value);
			}

			public Field ToField()
			{
				string value = Value.ToString();
				if (TextField)
					value = DoSqueeze(value.Trim(), new[] { ' ', '\r', '\n', '\t' }, ' ');

				return new Field(Name, value, Line);
			}

			public string Name { get; private set; }
			public StringBuilder Value { get; private set; }
			public int Line { get; private set; }
			public bool TextField { get; private set; }

			private static string DoSqueeze(string str, char[] from, char to)
			{
				var builder = new StringBuilder(str.Length);

				int i = 0;
				while (i < str.Length)
				{
					if (Array.IndexOf(from, str[i]) >= 0)
					{
						builder.Append(to);
						while (i < str.Length && Array.IndexOf(from, str[i]) >= 0)
							++i;
					}
					else
						builder.Append(str[i++]);
				}

				return builder.ToString();
			}
		}
		#endregion
	}
}
