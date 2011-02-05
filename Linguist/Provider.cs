using System;
using System.ComponentModel.Composition;
//using Microsoft.VisualStudio.ApplicationModel.Environments;
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
	internal sealed class Provider : IClassifierProvider
	{
		[Import]
		internal IClassificationTypeRegistryService ClassificationRegistry = null; // Set via MEF

		[Import]
		internal IClassifierAggregatorService AggregatorService = null;				// ditto

		public IClassifier GetClassifier(ITextBuffer buffer)
		{
            if (m_instantiating)
                return null;

            try
            {
                m_instantiating = true;
				return buffer.Properties.GetOrCreateSingletonProperty<Linguist>(delegate 
				{
					return new Linguist(AggregatorService.GetClassifier(buffer), ClassificationRegistry); 
				});
			}
			finally
			{
				m_instantiating = false;
			}
		}

		private bool m_instantiating;		// used to prevent infinite recurse in the AggregatorService.GetClassifier call
	}
}
