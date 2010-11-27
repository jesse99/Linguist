using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Linguist
{
	// This will add our classifier to the set of classifiers. Because the content 
	// type is "text" it will apply to all text files (our classifier will get the
	// path for the document associated with the text span to figure out what, if
	// any, styles need to be applied).
	[Export(typeof(IClassifierProvider))]
	[ContentType("text")]
	internal class Provider : IClassifierProvider
	{
		[Import]
		internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF

		public IClassifier GetClassifier(ITextBuffer buffer)
		{
			return buffer.Properties.GetOrCreateSingletonProperty<Linguist>(delegate { return new Linguist(ClassificationRegistry); });
		}
	}
}
