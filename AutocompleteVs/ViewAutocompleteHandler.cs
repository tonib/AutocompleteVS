using AutocompleteVs.LIneTransforms;
using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration;
using Microsoft.VisualStudio.Language.Intellisense;
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
		/// Last generated autocompletion for this view
		/// </summary>
		private Autocompletion CurrentAutocompletion;

        /// <summary>
        /// Buffer index to the place where suggestion has been added
        /// </summary>
        private int IdxSuggestionPosition;

		/// <summary>
		/// Brush to draw suggestion borders, when suggestion is drawn ouside the caret line
		/// </summary>
		private SolidColorBrush SuggestionBorderBrush;

		private bool HandleSuggestionsContextChange = true;

		/// <summary>
		/// The VS intellisense autocompletion broker
		/// </summary>
		internal ICompletionBroker CompletionBroker;

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
                OutputPaneHandler.Instance.Log("View closed", LogLevel.Debug);
				// Cancel current generation, do not touch UI
				AutocompletionsGenerator.Instance?.CancelCurrentGeneration();
			}
			catch(Exception ex)
			{
				OutputPaneHandler.Instance.Log(ex);
			}
		}

		/// <summary>
		/// Document text has changed (text typed, deleted, paste, etc)
		/// </summary>
		private void TextBuffer_PostChanged(object sender, EventArgs e)
		{
			// TODO: Still now working: It fails sometimes because prompt srinks (I dont know why yet)
			// Check if we have typed something that follows the current suggestion
			if(TextAddedFollowingAutocompletion())
			{
				// It did it. Do not cancell the current autocompletion
				return;
			}

			SuggestionContextChanged();
		}

        private bool TextAddedFollowingAutocompletion()
        {
			if (CurrentAutocompletion == null)
				return false;

            GenerationParameters currentParms = ViewGenerationParameters();

            string textAdded = CurrentAutocompletion.TextFollowsAutocompletion(currentParms.OriginalPrompt.PrefixText);
            if (textAdded == null)
				return false;

			string newAutocompletionText = CurrentAutocompletion.Text.Substring(textAdded.Length);
			if(newAutocompletionText.Length == 0)
			{
				// All autocompletion has been added: Cancel the current generation
				this.CancelCurrentAutocompletion();
				return true;
			}

            // Create the new Autocompletion
            var newAutocompletion = new Autocompletion(newAutocompletionText, currentParms);
			AutocompletionGenerationFinished(newAutocompletion);

            return true;
        }

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

				// If there is a selection, do not start a new suggestion
				if (!View.Selection.IsEmpty)
					return;

                if (AutocompleteVsPackage.Instance?.Settings.AutomaticSuggestions ?? false)
				{
                    // Check if we are in a valid position to start a new suggestion
                    // By now, if we are at a line end
                    ITextViewLine caretLine;
					try
					{
                        // This is trowing nullexception when closing editors, inside ContainingTextViewLine property call
                        caretLine = View.Caret.ContainingTextViewLine;
					}
					catch (NullReferenceException)
					{
						return;
					}

					string caretLineText = View.TextSnapshot.GetText(caretLine.Start, caretLine.Length);
					string lineTextAfterCaret = caretLineText.Substring(View.Caret.Position.BufferPosition - caretLine.Start);
					if (string.IsNullOrWhiteSpace(lineTextAfterCaret))
						StartGeneration();
				}
			}
			catch(Exception ex)
			{
                OutputPaneHandler.Instance.Log(ex);
			}
        }

        /// <summary>
        /// Cancels current suggestion on ui, and current autocompletion generation
        /// </summary>
        public void CancelCurrentAutocompletion()
		{
            AutocompletionsGenerator.Instance?.CancelCurrentGeneration();
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
                OutputPaneHandler.Instance.Log("View_LayoutChanged", LogLevel.Debug);

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
                        OutputPaneHandler.Instance.Log("View_LayoutChanged: Re-adding adornment", LogLevel.Debug);
						AddAdornment();
						return;
					}
				}
			}
			catch (Exception ex)
			{
				// TODO: Log exception
				OutputPaneHandler.Instance.Log(ex);
			}
		}

		static public ViewAutocompleteHandler AttachedHandler(IWpfTextView view) => 
			view.Properties.GetOrCreateSingletonProperty(() => new ViewAutocompleteHandler(view));

		private GenerationParameters ViewGenerationParameters()
		{
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

			Prompt originalPrompt = new Prompt(prefixText, suffixText);

            Settings settings = AutocompleteVsPackage.Instance?.Settings;
            return new GenerationParameters(this, originalPrompt, originalPrompt.CropToSettings(settings));
        }

		/// <summary>
		/// Launchs the autompletion process in the current caret position. Cancels current running process, if there is one
		/// </summary>
		public void StartGeneration()
		{
            // Cancel current generation / suggestion
            CancelCurrentAutocompletion();

            // Start generation
            GenerationParameters parms = ViewGenerationParameters();
            AutocompletionsGenerator.Instance?.StartAutocompletion(parms);
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

        public void AutocompletionGenerationFinished(Autocompletion autocompletion)
		{
			try
			{
                // Debug text:
                // viewSuggestionText = "COLUMN__NAME";

                RemoveAdornment();

				if (autocompletion.IsEmpty)
					return;

				SetupLabel();

                // Add virtual spaces to the text, if needed
                // It seems VS keeps cursor position in new line, adding virtual spaces that are not yet added to the current line
                // So, to get rigth suggestion / suggestion insertion, virtual spaces are needed. So, here are the damn spaces:
                //string virtualSpaces = new string(' ', View.Caret.Position.VirtualBufferPosition.VirtualSpaces);
                //CurrentSuggestionText = virtualSpaces + viewSuggestionText;

                CurrentAutocompletion = autocompletion;

				// Get caret position and line
				ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
				IdxSuggestionPosition = View.Caret.Position.BufferPosition;

				// Text to show in adornment
				string suggestionTextToShow = autocompletion.Text;

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
                    int idx = autocompletion.Text.IndexOf('\n');
                    if (idx >= 0)
                        CurrentAutocompletion.Text = CurrentAutocompletion.Text.Substring(0, idx);
                    suggestionTextToShow = CurrentAutocompletion.Text;
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
                    suggestionTextToShow = new string(' ', padding.Length) + CurrentAutocompletion.Text;
					
                    // Add a transform to the line to see all the autocmpletion, if needed
                    LabelAdornment.Height = AddMultilineSuggestionTransform(suggestionTextToShow, caretLine, View.LineHeight);

                    // This must to be done AFTER adding the line transform: It seems the transform can change the caretLine.TextTop
                    // In this case, is not safe to call GetMarkerGeometry, as the caret can be at the end of the document, 
                    // and GetMarkerGeometry needs a non empty span. So calculate the position manually
                    // Start rendering from left border
                    Canvas.SetTop(LabelAdornment, caretLine.TextTop);
                    Canvas.SetLeft(LabelAdornment, 0);

                }

                // Replace undescore chars ("_") by double underscores. In WPF, underscores are markers for access keys
                LabelAdornment.Content = suggestionTextToShow.Replace("_", "__");

				AddAdornment();
			}
			catch (Exception ex)
			{
				// TODO: Log exceptions
				OutputPaneHandler.Instance.Log(ex);
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
			string currentSuggestion = CurrentAutocompletion.Text;
			RemoveAdornment();

			string textToInsert;
			if (singleWord)
			{
				textToInsert = CurrentAutocompletion.GetNextWordToInsert();
			}
			else
				textToInsert = currentSuggestion;

			textToInsert = NormalizeLineBreaks(textToInsert);

			try
			{
				// Disable context change while we are inserting text in editor
				HandleSuggestionsContextChange = false;

				// Insert text in editor
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
					{
                        GenerationParameters parms = ViewGenerationParameters();
                        Autocompletion newAutocompletion = new Autocompletion(newSuggestion, parms);
                        AutocompletionGenerationFinished(newAutocompletion);
                    }
				}

				// Close VS autocompletion tooltip
				CompletionBroker.DismissAllSessions(View);
            }
			finally
			{
                HandleSuggestionsContextChange = true;
            }

			return true;
		}

		// TODO: Move this to Autocompletion class
		private string NormalizeLineBreaks(string text)
		{
            // Sometimes i get wrong line breaks. Normalize them
            // TODO: Check if there is some setting in editor for line breaks character
            return text.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", Environment.NewLine);
        }

    }
}
