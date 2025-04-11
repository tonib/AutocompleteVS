using AutocompleteVs.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
    /// <summary>
    /// Builds the suggestion string and checks if suggestion generation must continue
    /// </summary>
    class SuggestionStringBuilder
    {
        private int NOpenBrackets;
        private StringBuilder sb = new StringBuilder();
        private GenerationParameters GenerationParameters;

        /// <summary>
        /// True if suggestion generation must be stopped
        /// </summary>
        public bool StopGeneration { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameters">Generation parameters</param>
        public SuggestionStringBuilder(GenerationParameters parameters)
        {
            GenerationParameters = parameters;
        }

        /// <summary>
        /// Adds a text to the suggestion. If it will close the current block, suggestion will be stopped
        /// </summary>
        public void Add(string textToAppend)
        {
            char[] lineBreaks = new char[] { '\n', '\r' };
            for(int i=0; i<textToAppend.Length; i++)
            {
                if (GenerationParameters.GenerateSingleLine && lineBreaks.Contains(textToAppend[i]))
                {
                    // This is a single line. Do not include it and stop here
                    sb.Append(textToAppend.Substring(0, i));
                    StopGeneration = true;
                    OutputPaneHandler.Instance.Log("Suggestion generation stopped by linea break", LogLevel.Debug);
                    return;
                }

                if (textToAppend[i] == '{')
                    NOpenBrackets++;
                else if (textToAppend[i] == '}')
                {
                    NOpenBrackets--;
                    if (NOpenBrackets < 0)
                    {
                        // This is closing the current block. Do not include it and stop here
                        sb.Append(textToAppend.Substring(0, i));
                        StopGeneration = true;
                        OutputPaneHandler.Instance.Log("Suggestion generation stoped to avoid close current block", LogLevel.Debug);
                        return;
                    }
                }
            }

            sb.Append(textToAppend);
        }

        /// <summary>
        /// Normalize line breaks in suggestion text
        /// </summary>
        /// <param name="text">Text to normalize</param>
        /// <returns>Text with normalized line breaks</returns>
        static public string NormalizeLineBreaks(string text)
        {
            // Sometimes i get wrong line breaks. Normalize them
            // TODO: Check if there is some setting in editor for line breaks character
            return text.Replace("\r\n", "\n").Replace('\r', '\n').Replace("\n", Environment.NewLine);
        }

        /// <summary>
        /// Returns the effective suggestion to use
        /// </summary>
        public override string ToString() => NormalizeLineBreaks(sb.ToString());

    }
}
