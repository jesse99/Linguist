using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
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
		internal Linguist(IClassificationTypeRegistryService registry)
		{
			InstallFiles.Install();
			Styles.Init();
			Languages.Init(registry);
		}

		public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
		{
			IList<ClassificationSpan> spans = ms_noSpans;

			object prop;
			if (span.Snapshot.TextBuffer.Properties.TryGetProperty(typeof(ITextDocument), out prop))
			{
				var doc = (ITextDocument)prop;
				string fileName = System.IO.Path.GetFileName(doc.FilePath);
				Language lang = Languages.FindLanguage(fileName);

				if (lang != null)
					spans = lang.GetClassificationSpans(span);
			}

			return spans;
		}

		// This is called by Studio if a user action could change text classification. Classifiers
		// can use this to update their internal state.
		public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

		#region Fields
		private List<ClassificationSpan> ms_noSpans = new List<ClassificationSpan>();
		#endregion
	}
}
