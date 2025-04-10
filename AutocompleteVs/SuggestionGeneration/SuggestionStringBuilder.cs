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

        /// <summary>
        /// True if suggestion generation must be stopped
        /// </summary>
        public bool StopGeneration { get; private set; }

        /// <summary>
        /// Adds a text to the suggestion. If it will close the current block, suggestion will be stopped
        /// </summary>
        public void Add(string textToAppend)
        {
            for(int i=0; i<textToAppend.Length; i++)
            {
                if (textToAppend[i] == '{')
                    NOpenBrackets++;
                else if (textToAppend[i] == '}')
                    NOpenBrackets--;

                if(NOpenBrackets < 0)
                {
                    // This is closing the current block. Do not include it and stop here
                    sb.Append(textToAppend.Substring(0, i));
                    StopGeneration = true;
                    OutputPaneHandler.Instance.Log("Suggestion generation stoped to avoid close current block", LogLevel.Debug);
                    return;
                }
            }

            sb.Append(textToAppend);
        }

        /// <summary>
        /// Returns the effective suggestion to use
        /// </summary>
        public override string ToString() => sb.ToString();
    }
}
