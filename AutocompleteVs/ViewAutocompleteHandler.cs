using AutocompleteVs.Keyboard;
using AutocompleteVs.LIneTransforms;
using AutocompleteVs.SuggestionGeneration;
using AutocompleteVs.TestIntraTextAdorments;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
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

        /// <summary>
        /// Buffer index to the place where suggestion has been added
        /// </summary>
        private int IdxSuggestionPosition;

		/// <summary>
		/// Brush to draw suggestion borders, when suggestion is drawn ouside the caret line
		/// </summary>
		private SolidColorBrush SuggestionBorderBrush;

		private bool HandleSuggestionsContextChange = true;

		private ViewAutocompleteHandler(IWpfTextView view)
		{
			View = view;
			Layer = view.GetAdornmentLayer(VsTextViewListener.AUTOCOMPLETE_ADORNMENT_LAYER_ID);

			View.LayoutChanged += View_LayoutChanged;
			View.Caret.PositionChanged += Caret_PositionChanged;
            View.TextBuffer.PostChanged += TextBuffer_PostChanged;
            View.Closed += View_Closed;
		}

        /// <summary>
        /// View has been closed
        /// </summary>
        private void View_Closed(object sender, EventArgs e)
		{
			try
			{
				// Cancel current generation, do not touch UI
				AutocompletionGeneration.Instance?.CancelCurrentGeneration();
			}
			catch(Exception ex)
			{
				// TODO: Log exception
				Debug.WriteLine(ex.ToString());
			}
		}

        /// <summary>
        /// Document text has changed (text typed, deleted, paste, etc)
        /// </summary>
        private void TextBuffer_PostChanged(object sender, EventArgs e) => SuggestionContextChanged();

        /// <summary>
        /// Caret position changed in view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e) => SuggestionContextChanged();

        /// <summary>
        /// Called when suggestion context has changed. This will cancel current suggestion and, if configured, start a new one
        /// </summary>
        private void SuggestionContextChanged()
		{
			try
			{
				if (!HandleSuggestionsContextChange)
					return;

				CancelCurrentAutocompletion();

				if (AutocompleteVsPackage.Instance?.Settings.AutomaticSuggestions ?? false)
				{
					// Check if we are in a valid position to start a new suggestion
					// By now, if we are at a line end
					ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
					string caretLineText = View.TextSnapshot.GetText(caretLine.Start, caretLine.Length);
					string lineTextAfterCaret = caretLineText.Substring(View.Caret.Position.BufferPosition - caretLine.Start);
					if (string.IsNullOrWhiteSpace(lineTextAfterCaret))
						StartGeneration();
				}
			}
			catch(Exception ex)
			{
                // TODO: Log exception
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Cancels current suggestion on ui, and current autocompletion generation
        /// </summary>
        public void CancelCurrentAutocompletion()
		{
            AutocompletionGeneration.Instance?.CancelCurrentGeneration();
            RemoveAdornment();
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
						// Debug.WriteLine("View_LayoutChanged: Re-adding adornment");
						AddAdornment();
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

		/// <summary>
		/// Launchs the autompletion process in the current caret position. Cancels current running process, if there is one
		/// </summary>
		public void StartGeneration()
		{
            // Cancel current generation / suggestion
            CancelCurrentAutocompletion();

            // TODO: This only for models allowing fill in the middle
            // Get prefix / suffix text
            int caretIdx = View.Caret.Position.BufferPosition;

			// TOOD: Add caret virtual spaces to the end of prefix, otherwise, indentation is suggested wrong
			string prefixText = View.TextBuffer.CurrentSnapshot.GetText(0, caretIdx);

			string suffixText;
			int textLength = View.TextBuffer.CurrentSnapshot.Length;
			if (caretIdx >= textLength)
				suffixText = "";
			else
				suffixText = View.TextBuffer.CurrentSnapshot.GetText(caretIdx, View.TextBuffer.CurrentSnapshot.Length - caretIdx);

			AutocompletionGeneration.Instance?.StartAutocompletion(new GenerationParameters(this, prefixText, suffixText));
		}

		/// <summary>
		/// Initialize LabelAdornment, if needed
		/// </summary>
		private void SetupLabel()
		{
			if (LabelAdornment != null)
				return;

			LabelAdornment = new Label();

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
			SetLabelSolid(false);

			// TODO: Check if there is some setting in VS for line padding (top / bottom):
			LabelAdornment.Padding = new System.Windows.Thickness(0);
		}

		/// <summary>
		/// Sets the adorment label style to display it with a solid background ot transparent
		/// </summary>
		/// <param name="solid">True if label background must to be solid, also adds a border to the label.
		/// False to render the label transparent</param>
		private void SetLabelSolid(bool solid)
		{
			if (solid)
			{
				if(SuggestionBorderBrush == null)
				{
					SuggestionBorderBrush = new SolidColorBrush(Color.FromArgb(255, 0xff, 0, 0));
					SuggestionBorderBrush.Freeze();
				}
				LabelAdornment.BorderBrush = SuggestionBorderBrush;
				LabelAdornment.BorderThickness = new System.Windows.Thickness(1);

				// Solid bacground color
				LabelAdornment.Background = View.Background;
			}
			else
			{
				LabelAdornment.BorderBrush = Brushes.Transparent;
				LabelAdornment.BorderThickness = new System.Windows.Thickness(0);

				// Im my tests this is alaways the bacground brush
				LabelAdornment.Background = View.FormattedLineSource.DefaultTextProperties.BackgroundBrush;
			}
		}

		/// <summary>
		/// Removes the current suggestion adornment
		/// </summary>
		private void RemoveAdornment()
		{
			if (!SuggstionAdornmentVisible)
				return;

			MultiLineCompletionTransformSource.Attached(View).RemoveCurrentTransform();
			Layer.RemoveAdornment(LabelAdornment);
			SuggstionAdornmentVisible = false;
		}

        public void AutocompletionGenerationFinished(string viewSuggestionText)
		{
			try
			{
                // Debug text:
                viewSuggestionText = "COLUMN__NAME";

                RemoveAdornment();

				if (viewSuggestionText == null || string.IsNullOrEmpty(viewSuggestionText.Trim()))
					return;

				SetupLabel();

				// Add virtual spaces to the text, if needed
				// It seems VS keeps cursor position in new line, adding virtual spaces that are not yet added to the current line
				// So, to get rigth suggestion / suggestion insertion, virtual spaces are needed. So, here are the damn spaces:
				//string virtualSpaces = new string(' ', View.Caret.Position.VirtualBufferPosition.VirtualSpaces);
				//CurrentSuggestionText = virtualSpaces + viewSuggestionText;

				CurrentSuggestionText = viewSuggestionText;

				// Get caret position and line
				ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
				IdxSuggestionPosition = View.Caret.Position.BufferPosition;

				// Text to show in adornment
				string suggestionTextToShow = CurrentSuggestionText;

                // Two cases: 1) In the middle of a non empty line, 2) At the end of a line, or in a empty line
                string caretLineText = View.TextSnapshot.GetText(caretLine.Start, caretLine.Length);
                string lineTextAfterCaret = caretLineText.Substring(IdxSuggestionPosition - caretLine.Start);
				if(!string.IsNullOrWhiteSpace(lineTextAfterCaret))
				{
                    // Case 1: In the middle of a non empty line

					// Get line geometry for caret position
                    var caretSpan = new SnapshotSpan(View.TextSnapshot,
                    Span.FromBounds(IdxSuggestionPosition, IdxSuggestionPosition + 1));
                    Geometry geometry = View.TextViewLines.GetMarkerGeometry(caretSpan);

                    // Caret in middle of text. Render text in previous line.
                    // TODO: This could hide text, so do it only if suggestion has been fired manually
                    SetLabelSolid(true);
                    // Align left with caret, in upper line
                    Canvas.SetLeft(LabelAdornment, geometry.Bounds.Left);
                    Canvas.SetTop(LabelAdornment, geometry.Bounds.Top - geometry.Bounds.Height);

                    LabelAdornment.Height = geometry.Bounds.Height;

                    // If suggestion is multiline, suggest only the first line
                    int idx = CurrentSuggestionText.IndexOf('\n');
                    if (idx >= 0)
                        CurrentSuggestionText = CurrentSuggestionText.Substring(0, idx);
                    suggestionTextToShow = CurrentSuggestionText;
                }
				else
				{
                    // Case 2: At the end of a line, or in a empty line
                    SetLabelSolid(false);

                    // TODO: This will render wrong if font is not monospaced, and should be rendered with two labels (unsupported)
					string lineTextBeforeCaret = caretLineText.Substring(0, IdxSuggestionPosition - caretLine.Start);

                    // Put current space up to the caret position
                    // TODO: Tabs may not be 4 spaces, as it is configurable !!!
                    string padding = lineTextBeforeCaret.Replace("\t", "    ");
                    suggestionTextToShow = new string(' ', padding.Length) + CurrentSuggestionText;

                    // In this case, is not safe to call GetMarkerGeometry, as the caret can be at the end of the document, 
                    // and GetMarkerGeometry needs a non empty span. So calculate the position manually
                    // Start rendering from left border
                    Canvas.SetTop(LabelAdornment, caretLine.TextTop);
                    Canvas.SetLeft(LabelAdornment, 0);

                    // Add a transform to the line to see all the autocmpletion, if needed
                    LabelAdornment.Height = AddMultilineSuggestionTransform(suggestionTextToShow, caretLine, View.LineHeight);

                }

                // Replace undescore chars ("_") by double underscores. In WPF, underscores are markers for access keys
                LabelAdornment.Content = suggestionTextToShow.Replace("_", "__");

				AddAdornment();
			}
			catch (Exception ex)
			{
				// TODO: Log exceptions
				Debug.WriteLine(ex.ToString());
			}
		}

        private double AddMultilineSuggestionTransform(string viewSuggestionText, ITextViewLine caretLine, double lineHeight)
		{
			// Count line breaks in suggestion
			int nLineBreaks = 0;
			for(int i=0; i<viewSuggestionText.Length; i++)
			{
				if (viewSuggestionText[i] == '\n')
					nLineBreaks++;
			}
			if (nLineBreaks == 0)
				return lineHeight;

			// Multiline suggestions: Add a transform to make place to show all the suggestion
			MultiLineCompletionTransformSource transformSource = MultiLineCompletionTransformSource.Attached(View);
			double extraBottomSpace = lineHeight * nLineBreaks;
			transformSource.AddTransform(caretLine, (int)extraBottomSpace);
			return lineHeight + extraBottomSpace;
		}

		private void AddAdornment()
		{
            // var span = new SnapshotSpan(View.TextSnapshot, Span.FromBounds(IdxSuggestionPosition, IdxSuggestionPosition + 1));
            var span = new SnapshotSpan(View.TextSnapshot, Span.FromBounds(IdxSuggestionPosition, IdxSuggestionPosition));
            Layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, span, null, LabelAdornment, null);
			SuggstionAdornmentVisible = true;
		}

		/// <summary>
		/// Adds currently visible suggestion to the view
		/// </summary>
		/// <returns>False if nothing has been added</returns>
		public bool AddCurrentSuggestionToView(bool singleWord)
		{
			if (!SuggstionAdornmentVisible)
				return false;

			// Check if cursor is at virtual space
			bool inVirtualSpace = View.Caret.InVirtualSpace;

			// Remove adorment BEFORE doing any change. Otherwise, references to buffers and lines will go invalid
			string currentSuggestion = CurrentSuggestionText;
			RemoveAdornment();

			string textToInsert;
			if (singleWord)
			{
				textToInsert = GetNextWordToInsert();
			}
			else
				textToInsert = currentSuggestion;

			textToInsert = NormalizeLineBreaks(textToInsert);

			try
			{
				// Disable context change while we are inserting text in editor
				HandleSuggestionsContextChange = false;

                // https://stackoverflow.com/questions/13788221/how-to-insert-the-text-in-the-editor-in-the-textadornment-template-in-visual-stu
                ITextEdit textEdit = View.TextBuffer.CreateEdit();
				textEdit.Insert(View.Caret.Position.BufferPosition, textToInsert);
				textEdit.Apply();

				if (inVirtualSpace)
				{
					// Move cursor at the end of line. Needed, otherwhise the virtual spaces are keept after the current insertion
					View.Caret.MoveTo(View.Caret.Position.BufferPosition);
				}

				if (singleWord)
				{
					// Re-add suggestion for remaining words
					string newSuggestion = currentSuggestion.Substring(textToInsert.Length);
					if (!string.IsNullOrWhiteSpace(newSuggestion))
						AutocompletionGenerationFinished(newSuggestion);
				}
			}
			finally
			{
				HandleSuggestionsContextChange = true;
            }

			return true;
		}

		private string NormalizeLineBreaks(string text)
		{
            // Sometimes i get wrong line breaks. Normalize them
            // TODO: Check if there is some setting in editor for line breaks character
            return text.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", Environment.NewLine);
        }

		private string GetNextWordToInsert()
		{
			int idx = 0;

			// Spaces previous to word
			while (idx < CurrentSuggestionText.Length && Char.IsWhiteSpace(CurrentSuggestionText[idx]))
				idx++;

			if(idx >= CurrentSuggestionText.Length)
			{
				// All text was spaces
				return CurrentSuggestionText;
			}

			char wordStart = CurrentSuggestionText[idx];
			if (Char.IsLetterOrDigit(wordStart))
			{
				// A word / number / identifier
				while (idx < CurrentSuggestionText.Length && Char.IsLetterOrDigit(CurrentSuggestionText[idx]))
					idx++;
				return CurrentSuggestionText.Substring(0, idx);
			}

			// Otherwise is a punctuation
			return CurrentSuggestionText.Substring(0, idx + 1);
		}
	}
}
