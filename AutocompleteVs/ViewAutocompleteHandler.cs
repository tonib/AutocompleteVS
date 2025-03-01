using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace AutocompleteVs
{
	/// <summary>
	/// Handles autocomplete on a view
	/// </summary>
	internal sealed class ViewAutocompleteHandler
	{
		/// <summary>
		/// The layer of the adornment.
		/// </summary>
		private readonly IAdornmentLayer Layer;
		private readonly IWpfTextView View;
		// Label Adornment;

		private ViewAutocompleteHandler(IWpfTextView view)
		{
			View = view;
			Layer = view.GetAdornmentLayer(VsTextViewListener.AUTOCOMPLETE_ADORNMENT_LAYER_ID);
		}

		static public ViewAutocompleteHandler AttachedHandler(IWpfTextView view) => 
			view.Properties.GetOrCreateSingletonProperty(() => new ViewAutocompleteHandler(view));

		public void AddAutocompletionAdornment(string autocompleteText)
		{
			// Get caret line
			var caretLine = View.Caret.ContainingTextViewLine;

			int caretIdx = View.Caret.Position.BufferPosition;
			SnapshotSpan span = new SnapshotSpan(View.TextSnapshot, Span.FromBounds(caretIdx, caretIdx + 1));
			Geometry geometry = View.TextViewLines.GetMarkerGeometry(span);

			Label label = new Label();
			label.Content = autocompleteText;

			// Align the image with the top of the bounds of the text geometry
			Canvas.SetLeft(label, geometry.Bounds.Left);
			Canvas.SetTop(label, geometry.Bounds.Top);

			Layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, label, null);
		}
	}
}
