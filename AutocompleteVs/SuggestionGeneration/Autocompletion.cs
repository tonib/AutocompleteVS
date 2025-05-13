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
        /// <param name="newPrefix">Text to check</param>
        /// <returns>null if the text does not follow the autocompletion. If it follows it, is the text following the autocompletion
        /// that has been added</returns>
        public string TextFollowsAutocompletion(string newPrefix)
        {
            if (newPrefix.Length < Parameters.OriginalPrompt.PrefixText.Length)
            {
                // The prefix has changed (srinked)
                // TODO: This must to be handled: VS REMOVES and re-adds typed characteres. I dont understand why, but it does it
                // TODO: So, it must to be handled. Instead of returning the added text, return an int with the number of
                // TODO: Characters added (positive) or removed (negative)
                OutputPaneHandler.Instance.Log("TextFollowsAutocompletion: The prefix has changed (srinked)", LogLevel.Debug);
                return null;
            }

            // Length of text added after the prefix
            int lengthIncrease = newPrefix.Length - Parameters.OriginalPrompt.PrefixText.Length;
            if (lengthIncrease > Text.Length)
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
