using AutocompleteVs.Keyboard;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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

		public void StartGeneration()
		{
			// TODO: This only for models allowing fill in the middle
			// Get prefix / suffix text
			int caretIdx = View.Caret.Position.BufferPosition;
			string prefixText = View.TextBuffer.CurrentSnapshot.GetText(0, caretIdx);
			string suffixText;
			int textLength = View.TextBuffer.CurrentSnapshot.Length;
			if (caretIdx >= textLength)
				suffixText = "";
			else
				suffixText = View.TextBuffer.CurrentSnapshot.GetText(caretIdx, View.TextBuffer.CurrentSnapshot.Length - caretIdx);

			_ = AutocompletionGeneration.Instance.GetAutocompletionAsync(this, prefixText, suffixText);
		}

		public void AddAutocompletionAdornment(string autocompleteText)
		{
			// Get caret line
			ITextViewLine caretLine = View.Caret.ContainingTextViewLine;

			int caretIdx = View.Caret.Position.BufferPosition;
			SnapshotSpan span = new SnapshotSpan(View.TextSnapshot, Span.FromBounds(caretIdx, caretIdx + 1));
			Geometry geometry = View.TextViewLines.GetMarkerGeometry(span);

			Label label = new Label();
			label.Content = autocompleteText;
			// TODO: Cache label, there will be a singe adornment in view

			// Debug border
			//label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0xff));
			//label.BorderBrush.Freeze();
			//label.BorderThickness = new System.Windows.Thickness(1);
			//label.Height = geometry.Bounds.Height * 1.1;

			// Set label height
			label.Height = geometry.Bounds.Height;
			// TODO: Check if there is some setting in VS for paddings:
			label.Padding = new System.Windows.Thickness(0);

			// Set font default formatting properties:
			var textProperties = View.FormattedLineSource.DefaultTextProperties;
			var typeFace = textProperties.Typeface;
			label.FontFamily = typeFace.FontFamily;
			label.FontSize = textProperties.FontRenderingEmSize;
			label.FontStretch = typeFace.Stretch;
			label.FontStyle = typeFace.Style;
			label.FontWeight = typeFace.Weight;
			// Make foreground color "grayish":
			label.Foreground = textProperties.ForegroundBrush.Clone();
			label.Foreground.Opacity = 0.5;
			label.Background = textProperties.BackgroundBrush;

			// TODO: If we are not at the end of current line, show suggestion in other line (upper / lower)
			// TODO: Make sure it does not collide with VS single word autocompletion toolwindow

			// TODO: Add line tranformation defined by the VS ??? (see eye fish example in VS extensibility)

			// Align the image with the top of the bounds of the text geometry
			Canvas.SetLeft(label, geometry.Bounds.Left);
			Canvas.SetTop(label, geometry.Bounds.Top);

			Layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, label, null);
		}
	}
}
