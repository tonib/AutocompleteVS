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

		/// <summary>
		/// The label to render the current suggestion. Null until first suggestion is added
		/// </summary>
		private Label LabelAdornment;

		/// <summary>
		/// Is currently the adornment displayed?
		/// </summary>
		private bool SuggstionAdornmentVisible;

		/// <summary>
		/// Last generated suggestion for this view
		/// </summary>
		private string CurrentSuggestionText;

		private int IdxSuggestionPosition;

		private ViewAutocompleteHandler(IWpfTextView view)
		{
			View = view;
			Layer = view.GetAdornmentLayer(VsTextViewListener.AUTOCOMPLETE_ADORNMENT_LAYER_ID);

			View.LayoutChanged += View_LayoutChanged;
		}

		/// <summary>
		/// Called when lines must to be re-rendered. This will re-add the suggestion adornment to the line to re-render
		/// </summary>
		private void View_LayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
		{
			try
			{
				if (!SuggstionAdornmentVisible)
					return;
				// Debug.WriteLine("View_LayoutChanged");

				// This does not work. line.IdentityTag.Equals(AdornmentLineIdentityTag) neither. So, not really identity ???
				//foreach (ITextViewLine line in e.NewOrReformattedLines)
				//{
				//	if (line.IdentityTag == AdornmentLineIdentityTag)
				//	{
				//		Debug.WriteLine("View_LayoutChanged: Re-adding adornment");
				//		AddAdornment();
				//		return;
				//	}
				//}

				ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
				foreach (ITextViewLine line in e.NewOrReformattedLines)
				{
					if (line == caretLine)
					{
						Debug.WriteLine("View_LayoutChanged: Re-adding adornment");
						AddAdornment(false);
						return;
					}
				}
			}
			catch (Exception ex)
			{
				// TODO: Log exception
				Debug.WriteLine(ex.ToString());
			}
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

		/// <summary>
		/// Initialize LabelAdornment, if needed
		/// </summary>
		private void SetupLabel()
		{
			if (LabelAdornment != null)
				return;

			LabelAdornment = new Label();

			// Debug border
			//label.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0xff));
			//label.BorderBrush.Freeze();
			//label.BorderThickness = new System.Windows.Thickness(1);
			//label.Height = geometry.Bounds.Height * 1.1;

			// Set font default formatting properties:
			var textProperties = View.FormattedLineSource.DefaultTextProperties;
			var typeFace = textProperties.Typeface;
			LabelAdornment.FontFamily = typeFace.FontFamily;
			LabelAdornment.FontSize = textProperties.FontRenderingEmSize;
			LabelAdornment.FontStretch = typeFace.Stretch;
			LabelAdornment.FontStyle = typeFace.Style;
			LabelAdornment.FontWeight = typeFace.Weight;
			// Make foreground color "grayish":
			LabelAdornment.Foreground = textProperties.ForegroundBrush.Clone();
			LabelAdornment.Foreground.Opacity = 0.5;
			LabelAdornment.Background = textProperties.BackgroundBrush;

			// TODO: Check if there is some setting in VS for line padding (top / bottom):
			LabelAdornment.Padding = new System.Windows.Thickness(0);
		}

		private void RemoveAdornment()
		{
			if (!SuggstionAdornmentVisible)
				return;

			Layer.RemoveAdornment(LabelAdornment);
			SuggstionAdornmentVisible = false;
		}

		public void AutocompletionGenerationFinished(string viewSuggestionText)
		{
			try
			{
				if (viewSuggestionText == null || string.IsNullOrEmpty(viewSuggestionText.Trim()))
					return;
				CurrentSuggestionText = viewSuggestionText;

				// Get caret line
				ITextViewLine caretLine = View.Caret.ContainingTextViewLine;

				SnapshotSpan caretSpan = CaretSpan;
				Geometry geometry = View.TextViewLines.GetMarkerGeometry(caretSpan);

				SetupLabel();
				LabelAdornment.Content = viewSuggestionText;

				// Align the image with the top of the bounds of the text geometry
				LabelAdornment.Height = geometry.Bounds.Height;
				Canvas.SetLeft(LabelAdornment, geometry.Bounds.Left);
				Canvas.SetTop(LabelAdornment, geometry.Bounds.Top);

				// TODO: If we are not at the end of current line, show suggestion in other line (upper / lower)
				// TODO: Make sure it does not collide with VS single word autocompletion toolwindow
				// TODO: Add line tranformation defined by the VS ??? (see eye fish example in VS extensibility)
				// TODO: Support for multiline suggestions

				IdxSuggestionPosition = View.Caret.Position.BufferPosition;
				AddAdornment(true);
			}
			catch (Exception ex)
			{
				// TODO: Log exceptions
				Debug.WriteLine(ex.ToString());
			}
		}

		private SnapshotSpan CaretSpan
		{
			get
			{
				int caretIdx = View.Caret.Position.BufferPosition;
				return new SnapshotSpan(View.TextSnapshot, Span.FromBounds(caretIdx, caretIdx + 1));
			}
		}

		private void AddAdornment(bool removePrevious)
		{
			if(removePrevious)
				RemoveAdornment();
			var span = new SnapshotSpan(View.TextSnapshot, Span.FromBounds(IdxSuggestionPosition, IdxSuggestionPosition + 1));
			Layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, LabelAdornment, null);
			SuggstionAdornmentVisible = true;
		}

		/// <summary>
		/// Adds currently visible suggestion to the view
		/// </summary>
		/// <returns>False if nothing has been added</returns>
		public bool AddCurrentSuggestionToView()
		{
			if (!SuggstionAdornmentVisible)
				return false;

			// https://stackoverflow.com/questions/13788221/how-to-insert-the-text-in-the-editor-in-the-textadornment-template-in-visual-stu
			ITextEdit textEdit = View.TextBuffer.CreateEdit();
			textEdit.Insert(View.Caret.Position.BufferPosition, CurrentSuggestionText);
			textEdit.Apply();
			RemoveAdornment();

			return true;
		}
	}
}
