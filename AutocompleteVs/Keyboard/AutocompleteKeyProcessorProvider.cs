using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Keyboard
{

	[Export(typeof(IKeyProcessorProvider))]
	[TextViewRole(PredefinedTextViewRoles.Document)]
	[ContentType(VsTextViewListener.CONTENT_TYPE_ID)]
	[Name("AutocompleteKeyProcessor")]
	[Order(Before = "default")]
	internal sealed class AutocompleteKeyProcessorProvider : IKeyProcessorProvider
	{
		[ImportingConstructor]
		public AutocompleteKeyProcessorProvider() { }

		public KeyProcessor GetAssociatedProcessor(IWpfTextView wpfTextView) => 
			AutocompleteKeyProcesor.AttachedProcessor(wpfTextView);
	}
}
