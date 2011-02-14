using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
//using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

#pragma warning disable 67

namespace Linguist
{
	// This is the class that does the real work. It gets a SnapshotSpan, figures out which language to
	// use, applies a regex to the snapshot text, and uses the result to apply styles.
	internal sealed class Linguist : IClassifier
	{		
		internal Linguist(IClassifier aggregator, IClassificationTypeRegistryService registry)
		{
			InstallFiles.Install();
			Styles.Init();
			Languages.Init(registry);

			m_aggregator = aggregator;
		}

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			List<ClassificationSpan> spans = ms_noSpans;

			object prop;
			if (span.Snapshot.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out prop))
			{
				var doc = (ITextDocument)prop;
				string fileName = System.IO.Path.GetFileName(doc.FilePath);
				Language lang = Languages.FindLanguage(fileName);

				if (lang != null)
				{
					List<ClassificationSpan> strings = new List<ClassificationSpan>();
					List<ClassificationSpan> comments = new List<ClassificationSpan>();
					DoGetStringSpans(lang, span, strings, comments);

					if (!strings.Any(s => s.Span.Contains(span) && !comments.Any(t => t.Span.Contains(span))))
					{
						IEnumerable<ClassificationSpan> mine = lang.GetClassificationSpans(span);
						spans.AddRange(from c in mine where !strings.Any(d => d.Span.OverlapsWith(c.Span)) && !comments.Any(d => d.Span.OverlapsWith(c.Span)) select c);
					}
					spans.AddRange(strings);
					spans.AddRange(comments);
				}
			}

			return spans;
		}

		// Classifiers can invoke this to notify the editor of changes in classifications.
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

		#region Private Methods
		private void DoGetStringSpans(Language lang, SnapshotSpan span, List<ClassificationSpan> strings, List<ClassificationSpan> comments)
		{
			IEnumerable<ClassificationSpan> spans = m_aggregator.GetClassificationSpans(span);

			strings.AddRange(from c in spans
			            let n = c.ClassificationType.Classification.ToLower()
			            where n.Contains("string")
			            select lang.GetStringClassification(c));

			comments.AddRange(from c in spans 
			            let n = c.ClassificationType.Classification.ToLower() 
			            where n.Contains("comment")
			            select lang.GetCommentClassification(c));
		}
		#endregion

		#region Fields
		private IClassifier m_aggregator;
		private List<ClassificationSpan> ms_noSpans = new List<ClassificationSpan>();
		#endregion
	}
}
