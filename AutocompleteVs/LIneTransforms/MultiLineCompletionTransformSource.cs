using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocompleteVs.LIneTransforms
{
	class MultiLineCompletionTransformSource : ILineTransformSource
	{
		private IWpfTextView View;

		private MultiLineCompletionTransformSource(IWpfTextView view)
		{
			View = view;
		}

		static public MultiLineCompletionTransformSource Attached(IWpfTextView view)
			=> view.Properties.GetOrCreateSingletonProperty(() => new MultiLineCompletionTransformSource(view));

		public LineTransform GetLineTransform(ITextViewLine line, double yPosition, ViewRelativePosition placement)
		{
			return new LineTransform(0, 50, 1.0);
		}
	}
}
