using AutocompleteVs.Logging;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using Microsoft.VisualStudio.TextManager.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Extensions
{

    /// <summary>
    /// Extension methods for the <see cref="ITextView"/> interface.
    /// </summary>
    internal static class ITextViewExtensions
    {
        /// <summary>
        /// Returns the <see cref="ITextViewLine"/> that contains the caret.
        /// </summary>
        /// <param name="textView">The text view</param>
        /// <returns>The <see cref="ITextViewLine"/> that contains the caret.
        /// Null if the caret is not in a valid line</returns>
        /// <remarks>
        /// I'm having exceptions of type:
        /// System.InvalidOperationException: Unable to get TextViewLine containing insertion point.
        /// calling View.Caret.ContainingTextViewLine.
        /// According to https://github.com/VsVim/VsVim/issues/2583 this is a bug in the VS editor.
        /// Let's try this
        /// </remarks>
        public static ITextViewLine CaretLine(this ITextView textView)
        {
            try
            {
                SnapshotPoint caretPoint = textView.Caret.Position.BufferPosition;
                ITextViewLineCollection textViewLines = textView.TextViewLines;
                ITextViewLine line = textViewLines.GetTextViewLineContainingBufferPosition(caretPoint);
                if (line != null && line.IsValid)
                {
                    return line;
                }
                return null;
            }
            catch (Exception ex)
            {
                OutputPaneHandler.Instance.Log(ex);
                return null;
            }
        }
    }
}
