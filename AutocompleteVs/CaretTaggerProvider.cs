using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
	[Export(typeof(IViewTaggerProvider))]
	[ContentType("code")]
	[TagType(typeof(IntraTextAdornmentTag))]
	sealed class CaretTaggerProvider : IViewTaggerProvider
	{
		public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) 
			where T : ITag
		{
			if (textView == null)
				throw new ArgumentNullException("textView");

			if (buffer == null)
				throw new ArgumentNullException("buffer");

			if (buffer != textView.TextBuffer)
				return null;

			// TODO: In the	colortagger example, this is not implemented like this.. is rigth?
			return new CaretTagger((IWpfTextView)textView) as ITagger<T>;
		}
	}
}
