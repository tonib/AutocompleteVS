using AutocompleteVs.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
    /// <summary>
    /// A generated autocompletion
    /// </summary>
    class Autocompletion
    {
        /// <summary>
        /// The generation parameters used to generate this autocompletion
        /// </summary>
        public GenerationParameters Parameters { get; set; }

        /// <summary>
        /// The generated autocompletion
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Whether the autocompletion is empty or not
        /// </summary>
        public bool IsEmpty => string.IsNullOrEmpty(Text);

        public Autocompletion(string text, GenerationParameters parameters)
        {
            // Trim end, if any
            Text = text.TrimEnd();
            Parameters = parameters;
        }

        /// <summary>
        /// Gets the next word to insert in the autocompletion
        /// </summary>
        public string GetNextWordToInsert()
        {
            int idx = 0;

            // Spaces previous to word
            while (idx < Text.Length && Char.IsWhiteSpace(Text[idx]))
                idx++;

            if (idx >= Text.Length)
            {
                // All text was spaces
                return Text;
            }

            char wordStart = Text[idx];
            if (IsWordChar(wordStart))
            {
                // A word / number / identifier
                while (idx < Text.Length && IsWordChar(Text[idx]))
                    idx++;
                return Text.Substring(0, idx);
            }

            // Otherwise is a punctuation
            return Text.Substring(0, idx + 1);
        }

        static private bool IsWordChar(char c) => Char.IsLetterOrDigit(c) || c == '_';

        /// <summary>
        /// Checks whether a given text follows this autocompletion
        /// </summary>
        /// <param name="newPrefix">Text before caret</param>
        /// <param name="newSuffix">Text after caret</param>
        /// <param name="nCharsAdded">Number of characters added (positive) / removed (negative)</param>
        /// <returns>null if the text does not follow the autocompletion. If it follows it, 
        /// it's the text added/removed
        /// </returns>
        public string TextFollowsAutocompletion(string newPrefix, string newSuffix, 
            out int nCharsAdded)
        {
            nCharsAdded = 0;

            if (newPrefix.Length < Parameters.OriginalPrompt.PrefixText.Length)
            {
                // The prefix has changed (srinked)
                // TODO: This must to be handled: VS REMOVES and re-adds typed characteres. I dont understand why, but it does it
                // TODO: So, it must to be handled. Instead of returning the added text, return an int with the number of
                // TODO: Characters added (positive) or removed (negative)
                OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: The prefix has srinked", LogLevel.Debug);

                if(!Parameters.OriginalPrompt.PrefixText.StartsWith(newPrefix))
                {
                    OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: Prefix has changed", LogLevel.Debug);
                    return null;
                }

                int lengthDecreased = Parameters.OriginalPrompt.PrefixText.Length - newPrefix.Length;
                //if(Parameters.OriginalPrompt.SuffixText.Length < lengthDecreased)
                //{
                //    OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: Suffix has changed (too small)", LogLevel.Debug);
                //    return null;
                //}
                // TODO: Return lengthDecreased
                nCharsAdded = -lengthDecreased;
                return Parameters.OriginalPrompt.PrefixText.Substring(newPrefix.Length, lengthDecreased);
            }

            // Length of text added after the prefix
            nCharsAdded = newPrefix.Length - Parameters.OriginalPrompt.PrefixText.Length;
            if (nCharsAdded > Text.Length)
            {
                // Text added is larger than the autocompletion
                OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: Text added is larger than the autocompletion", LogLevel.Debug);
                return null;
            }

            if (!newPrefix.StartsWith(Parameters.OriginalPrompt.PrefixText))
            {
                // Prefix has changed
                OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: Prefix has changed", LogLevel.Debug);
                return null;
            }

            string textAdded = newPrefix.Substring(Parameters.OriginalPrompt.PrefixText.Length);
            if(!Text.StartsWith(textAdded))
            {
                // Text added is not a prefix of the autocompletion
                OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: Text added is not a prefix of the autocompletion", LogLevel.Debug);
                return null;
            }

            return textAdded;
        }

    }
}
