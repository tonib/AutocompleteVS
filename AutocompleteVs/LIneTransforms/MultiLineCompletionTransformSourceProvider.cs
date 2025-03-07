using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.LIneTransforms
{
	[Export(typeof(ILineTransformSourceProvider))]
	[ContentType("text")]
	[TextViewRole(PredefinedTextViewRoles.Interactive)]
	internal sealed class MultiLineCompletionTransformSourceProvider : ILineTransformSourceProvider
	{
		public ILineTransformSource Create(IWpfTextView textView)
		{
			return MultiLineCompletionTransformSource.Attached(textView);
		}
	}
}
