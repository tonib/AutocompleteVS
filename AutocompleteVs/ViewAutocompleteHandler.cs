using AutocompleteVs.Extensions;
using AutocompleteVs.LIneTransforms;
using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration;
using Microsoft.CodeAnalysis.Text;
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
		// TODO: Move all suggestion rendering to a other class

		/// <summary>
		/// The characters autoclosed by VS
		/// </summary>
		static private char[] AutoclosingChars = new char[] { ')', ']', '"', '\'' };

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

		/// <summary>
		/// The VS intellisense autocompletion broker: Called when the view is created
		/// </summary>
		/// <param name="view">The created view</param>
        private ViewAutocompleteHandler(IWpfTextView view)
		{
			View = view;
			Layer = view.GetAdornmentLayer(VsTextViewListener.AUTOCOMPLETE_ADORNMENT_LAYER_ID);

			View.LayoutChanged += View_LayoutChanged;
			View.Caret.PositionChanged += Caret_PositionChanged;
            // View.TextBuffer.Changed += TextBuffer_Changed;
            View.TextBuffer.PostChanged += TextBuffer_PostChanged;
			// This has problems with cursor position
            // View.TextBuffer.ChangedLowPriority += TextBuffer_PostChanged;
            View.Closed += View_Closed;
		}

		// DO NOT REMOVE THIS, MAYBE USEFUL IN FUTURE (DEBUG)
		/*private void TextBuffer_Changed(object sender, TextContentChangedEventArgs e)
        {
			string ev = $"{e.EditTag} / {e.Options} / {e.BeforeVersion} / {e.AfterVersion} / {e.Changes.Count}";
			List<string> changes = new List<string>();
            foreach (ITextChange change in e.Changes)
			{
				changes.Add($"['{change.OldText}' / '{change.NewText}'");
			}
            OutputPaneHandler.Instance.Log($"TextBuffer_Changed: {ev}, {string.Join(", ", changes)}", LogLevel.Debug);
        }*/

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
            OutputPaneHandler.Instance.Log("TextBuffer_PostChanged", LogLevel.Debug);

            // TODO: Still now working: It fails sometimes because prompt srinks (I dont know why yet)
            // Check if we have typed something that follows the current suggestion
            if (TextAddedFollowingAutocompletion())
			{
				// It did it. Do not cancel the current autocompletion
				return;
			}

			SuggestionContextChanged(false);
		}

        private bool TextAddedFollowingAutocompletion()
        {
			if (CurrentAutocompletion == null)
				return false;

            GenerationParameters currentParms = ViewGenerationParameters();

            OutputPaneHandler.Instance.Log($"TextAddedFollowingAutocompletion: View.TextBuffer.CurrentSnapshot.Length: " +
				$"{View.TextBuffer.CurrentSnapshot.Length}, View.TextSnapshot.Length: {View.TextSnapshot.Length}", LogLevel.Debug);
            

            string textMoved = CurrentAutocompletion.TextFollowsAutocompletion(currentParms.OriginalPrompt.PrefixText,
				currentParms.OriginalPrompt.SuffixText, out int nCharsAdded);
            if (textMoved == null)
				return false;

			string logMsg = nCharsAdded >= 0 ?
				$"TextAddedFollowingAutocompletion: Text added following autocompletion: [{textMoved}]" :
				$"TextAddedFollowingAutocompletion: Text removed following autocompletion: [{textMoved}]";
            OutputPaneHandler.Instance.Log(logMsg, LogLevel.Debug);

			string newAutocompletionText;
            if (nCharsAdded >= 0)
			{
				// Typed text from suggestion: Move the text from suggestion to the current prefix
				newAutocompletionText = CurrentAutocompletion.Text.Substring(nCharsAdded);
				if (newAutocompletionText.Length == 0)
				{
					// All autocompletion has been added: Cancel the current generation
					this.CancelCurrentAutocompletion();
					return true;
				}
			}
			else
			{
                // Text removed from prefix: Move the text from current prefix to the suggestion
                newAutocompletionText = textMoved + CurrentAutocompletion.Text;
				string prefixText = CurrentAutocompletion.Parameters.OriginalPrompt.PrefixText;
				CurrentAutocompletion.Parameters.OriginalPrompt.PrefixText =
                    prefixText.Substring(0, prefixText.Length + nCharsAdded);
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
		private void Caret_PositionChanged(object sender, CaretPositionChangedEventArgs e)
		{
			OutputPaneHandler.Instance.Log("Caret position changed", LogLevel.Debug);
			SuggestionContextChanged(true);
		}

        /// <summary>
        /// Called when suggestion context has changed. This will cancel current suggestion and, if configured, start a new one
        /// </summary>
		/// <param name="isCaretPositonChanged">True if context has changed due to caret relocation</param>
        private void SuggestionContextChanged(bool isCaretPositonChanged)
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
                    CheckStartNewGeneration(isCaretPositonChanged);
                }
            }
			catch(Exception ex)
			{
                OutputPaneHandler.Instance.Log(ex);
			}
        }


		/// <summary>
		/// Check if we are in a valid position to start a new suggestion automatically
		/// </summary>
		/// <param name="isCaretPositonChanged">True if context has changed due to caret relocation</param>
        private void CheckStartNewGeneration(bool isCaretPositonChanged)
        {
            // Get the line where caret is placed
            ITextViewLine caretLine;
            try
            {
                // This is trowing nullexception when closing editors, inside ContainingTextViewLine property call
                //caretLine = View.Caret.ContainingTextViewLine;
                caretLine = View.CaretLine();
				if(caretLine == null)
                    return;
            }
            catch (NullReferenceException)
            {
                return;
            }

            string caretLineText = View.TextSnapshot.GetText(caretLine.Start, caretLine.Length);
            int caretPosition = View.Caret.Position.BufferPosition - caretLine.Start;

            string lineTextAfterCaret = caretLineText.Substring(caretPosition);
            if (string.IsNullOrWhiteSpace(lineTextAfterCaret))
            {
                // We are at a line end, so we could make a suggestion. Check some undesirable cases
                string textBeforeCaret = caretLineText.Substring(0, caretPosition);
				if (isCaretPositonChanged && textBeforeCaret.Length > 0 && 
					!Char.IsWhiteSpace(textBeforeCaret[textBeforeCaret.Length-1]))
				{
					// Caret has moved to the end of a word, do not make suggestions here
					return;
				}

                string textBeforeCaretTrimed = textBeforeCaret.Trim();
                if (textBeforeCaretTrimed.EndsWith("}") || textBeforeCaretTrimed.EndsWith("{") || textBeforeCaretTrimed.EndsWith(";"))
                {
                    // At block start/end or line. Do not make suggestions here
                    return;
                }
                StartGeneration();
            }
			else
			{
				// We are in the middle of a line. Usually you don't want to make suggestions here.
				// Exception: All text after the caret are autoclosing characters, probably auto-added by VS
				if(lineTextAfterCaret.All(c => AutoclosingChars.Contains(c) || char.IsWhiteSpace(c)))
                    StartGeneration();
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
                // OutputPaneHandler.Instance.Log("View_LayoutChanged", LogLevel.Debug);

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

                // ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
                ITextViewLine caretLine = View.CaretLine();
                if (caretLine == null)
                {
                    return;
                }
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

            Settings settings = AutocompleteVsPackage.Instance?.Settings;
            Prompt originalPrompt = new Prompt(prefixText, suffixText);
			// Prompt to feed the model:
			Prompt modelPrompt = originalPrompt.AsModelPrompt(settings);
			// If current line is not empty, limit suggestion generation to a single line
			bool singleLineSuggestion = originalPrompt.CurentLinePrefix.Trim() != "";

			// Get the Roslyn document from the view:
            // https://stackoverflow.com/questions/45653203/how-do-i-retrieve-text-from-the-visual-studio-editor-for-use-with-roslyn-syntaxt
            // TODO: It seems there can be more than one document. Right now supporting only one (where there can be more than one???)
            Microsoft.CodeAnalysis.Document doc = View.TextSnapshot.GetRelatedDocumentsWithChanges().FirstOrDefault();
            return new GenerationParameters(this, originalPrompt, modelPrompt, singleLineSuggestion, 
				doc, caretIdx);
        }

		/// <summary>
		/// Launchs the autompletion process in the current caret position. Cancels current running process, if there is one
		/// </summary>
		public void StartGeneration()
		{
			// If current position is read only, do nothing
			if (View.TextBuffer.IsReadOnly(View.Caret.Position.BufferPosition))
			{
				OutputPaneHandler.Instance.Log("Current position is read only, do not launch generation", LogLevel.Debug);
				return;
			}

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

		/// <summary>
		/// Called when the current suggestion is finished.
		/// It must to be calles in UI thread
		/// </summary>
		/// <param name="autocompletion">Suggestion to show</param>
        public void AutocompletionGenerationFinished(Autocompletion autocompletion)
		{
			try
			{
				// Debug text:
				// viewSuggestionText = "COLUMN__NAME";

				RemoveAdornment();

				if (autocompletion.IsEmpty)
				{
                    // Nothing to show
                    OutputPaneHandler.Instance.Log("Suggestion is empty", LogLevel.Debug);
                    return;
				}

				// If caret location has changed, suggestion is outdated. Do nothing
				int currentCaretPosition = autocompletion.Parameters.View.View.Caret.Position.BufferPosition;
                if (currentCaretPosition != autocompletion.Parameters.CaretBufferLocation)
				{
					OutputPaneHandler.Instance.Log("Caret position has changed, suggestion is outdated. Do nothing", LogLevel.Debug);
					return;
				}
				SetupLabel();

				// Add virtual spaces to the text, if needed
				// It seems VS keeps cursor position in new line, adding virtual spaces that are not yet added to the current line
				// So, to get rigth suggestion / suggestion insertion, virtual spaces are needed. So, here are the damn spaces:
				//string virtualSpaces = new string(' ', View.Caret.Position.VirtualBufferPosition.VirtualSpaces);
				//CurrentSuggestionText = virtualSpaces + viewSuggestionText;

				OutputPaneHandler.Instance.Log("Suggestion:", LogLevel.Debug);
				OutputPaneHandler.Instance.Log(autocompletion.Text, LogLevel.Debug);

				CurrentAutocompletion = autocompletion;

                // Get caret position and line
                // ITextViewLine caretLine = View.Caret.ContainingTextViewLine;
                ITextViewLine caretLine = View.CaretLine();
				if (caretLine == null)
					return;

                IdxSuggestionPosition = View.Caret.Position.BufferPosition;

				// Text to show in adornment
				string suggestionTextToShow = autocompletion.Text;

				// Two cases: 1) In the middle of a non empty line, 2) At the end of a line, or in a empty line
				string caretLineText = View.TextSnapshot.GetText(caretLine.Start, caretLine.Length);
				string lineTextAfterCaret = caretLineText.Substring(IdxSuggestionPosition - caretLine.Start);
				if (!string.IsNullOrWhiteSpace(lineTextAfterCaret))
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

					suggestionTextToShow = AddHelpText(suggestionTextToShow);
				}
				else
				{
					// Case 2: At the end of a line, or in a empty line
					SetLabelSolid(false);

					// TODO: This will render wrong if font is not monospaced, and should be rendered with two labels (unsupported)
					string lineTextBeforeCaret = caretLineText.Substring(0, IdxSuggestionPosition - caretLine.Start);

					// Put current space up to the caret position
					// TODO: Tabs may not be 4 spaces, as it is configurable !!!
					lineTextBeforeCaret = lineTextBeforeCaret.Replace("\t", "    ");
					suggestionTextToShow = new string(' ', lineTextBeforeCaret.Length) + 
						CurrentAutocompletion.Text.Replace("\t", "    ");

					suggestionTextToShow = AddHelpText(suggestionTextToShow);

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

		/// <summary>
		/// Adds help text to the suggestion
		/// </summary>
		/// <param name="suggestionTextToShow"></param>
		/// <returns></returns>
		private static string AddHelpText(string suggestionTextToShow)
		{
			// Add help info for the user in suggestion
			if (suggestionTextToShow.Contains(Environment.NewLine))
				suggestionTextToShow += Environment.NewLine + Environment.NewLine;
			else
				suggestionTextToShow += "           ";
			suggestionTextToShow += "[Ctrl + Enter => Accept all / Ctrl + > => Accept word]";
			return suggestionTextToShow;
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

    }
}
