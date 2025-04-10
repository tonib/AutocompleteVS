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
            if (Char.IsLetterOrDigit(wordStart))
            {
                // A word / number / identifier
                while (idx < Text.Length && Char.IsLetterOrDigit(Text[idx]))
                    idx++;
                return Text.Substring(0, idx);
            }

            // Otherwise is a punctuation
            return Text.Substring(0, idx + 1);
        }

        /// <summary>
        /// Checks whether a given text follows this autocompletion
        /// </summary>
        /// <param name="text">Text to check</param>
        /// <returns>null if the text does not follow the autocompletion. If it follows it, is the text following the autocompletion
        /// that has been added</returns>
        public string TextFollowsAutocompletion(string text)
        {
            if (text.Length <= Parameters.OriginalPrompt.PrefixText.Length)
            {
                // The prefix has changed (srinked)
                return null;
            }

            // Length of text added after the prefix
            int lengthIncrease = text.Length - Parameters.OriginalPrompt.PrefixText.Length;
            if (lengthIncrease > Text.Length)
            {
                // Text added is larger than the autocompletion
                return null;
            }

            if (!text.StartsWith(Parameters.OriginalPrompt.PrefixText))
            {
                // The prefix has changed
                return null;
            }

            string textAdded = text.Substring(Parameters.OriginalPrompt.PrefixText.Length);
            if(!Text.StartsWith(textAdded))
            {
                // Text added is not a prefix of the autocompletion
                return null;
            }

            return textAdded;
        }

    }
}
