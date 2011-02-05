using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace Linguist
{
	// Classifies text according to the elements provided by a *.lang file.
	internal sealed class Language
	{
		// Elements maps regex to element types.
		public Language(string name, Regex glob, List<KeyValuePair<string, IClassificationType>> elements)
		{
			Name = name;
			Glob = glob;

			m_classifications.Add(elements[0].Value);			// default style
			string comments = string.Empty;
			string strings = string.Empty;
			IClassificationType commentType = null;
			IClassificationType stringType = null;

			var regexen = new List<string>(elements.Count);
			for (int i = 1; i < elements.Count; ++i)
			{
				var entry = elements[i];
//				Log.WriteLine("Group {0} = {1} [{2}]", i, entry.Key, entry.Value.Classification);

				string pattern = entry.Key;
				if (!DoValidateRegex(entry.Value.Classification, pattern))
					pattern = "xxx";							// the classifier gets very confused if the elements have parens so we'll minimize the havoc by setting the pattern to something innocuous

				regexen.Add("(" + pattern + ")");
				m_classifications.Add(entry.Value);

				if (entry.Value.Classification == "Linguist.comment")
				{
					comments += comments.Length == 0 ? "(" + pattern + ")" : "| (" + pattern + ")";
					commentType = entry.Value;
				}
				else if (entry.Value.Classification == "Linguist.string")
				{
					strings += strings.Length == 0 ? "(" + pattern + ")" : "| (" + pattern + ")";
					stringType = entry.Value;
				}
			}

			RegexOptions options = RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline;
			m_regex = new Regex(string.Join(" | ", regexen.ToArray()), options | RegexOptions.Compiled);
			if (commentType != null)
				m_comment = new KeyValuePair<Regex, IClassificationType>(new Regex(comments, options), commentType);
			if (stringType != null)
				m_string = new KeyValuePair<Regex, IClassificationType>(new Regex(strings, options), stringType);
		}

		public string Name { get; private set; }
		public Regex Glob { get; private set; }

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			var spans = new List<ClassificationSpan>();
			string text = span.GetText();

			var newSpan = new SnapshotSpan(span.Start, span.Length);				// default style
			spans.Add(new ClassificationSpan(newSpan, m_classifications[0]));

//			Log.WriteLine("Matching: {0}", text.Trim());
//			Log.Indent();
			MatchCollection matches = m_regex.Matches(text);
			foreach (Match match in matches)
			{
				for (int i = 1; i < m_classifications.Count; ++i)
				{
					Group g = match.Groups[i];
					if (g.Success)
					{
//						Log.WriteLine("{0} matched group {1} [{2}]", text.Substring(g.Index, g.Length), i, m_classifications[i].Classification);
						newSpan = new SnapshotSpan(span.Start + g.Index, g.Length);
						spans.Add(new ClassificationSpan(newSpan, m_classifications[i]));
						break;
					}
				}
			}
//			Log.WriteLine();
//			Log.Unndent();

			return spans;
		}

		public ClassificationSpan GetStringClassification(ClassificationSpan original)
		{
			// We need to match the span against our comment regex or we can wind up
			// with situations where we apply our style to an unterminated span and
			// are not able to reset our style when the span is closed (e.g. verbatim
			// strings in C#).
			if (m_string.Key != null && m_string.Key.IsMatch(original.Span.GetText()))
				return new ClassificationSpan(original.Span, m_string.Value);
			else
				return original;
		}

		public ClassificationSpan GetCommentClassification(ClassificationSpan original)
		{
			if (m_comment.Key != null && m_comment.Key.IsMatch(original.Span.GetText()))
				return new ClassificationSpan(original.Span, m_comment.Value);
			else
				return original;
		}

		#region Private Methods
		private bool DoValidateRegex(string name, string expr)
		{
			bool valid = true;

			for (int i = 0; i < expr.Length && valid; ++i)
			{
				if (expr[i] == '(')
				{
					if ((i > 0 && expr[i - 1] == '\\') || expr[i + 1] == '?')
					{
						continue;
					}
					else
					{
						int k = name.IndexOf('.');
						if (k > 0)
							name = name.Substring(k + 1);

						Log.WriteLine("{0} should use a non-capturing group, .e.g '(?: foo )' instead of '(foo)'.", name);
						valid = false;
					}
				}
			}

			return valid;
		}
		#endregion

		#region Fields
		private Regex m_regex;
		private List<IClassificationType> m_classifications = new List<IClassificationType>();
		private KeyValuePair<Regex, IClassificationType> m_comment;
		private KeyValuePair<Regex, IClassificationType> m_string;
		#endregion
	}
}
