using AutocompleteVs.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocompleteVs.SuggestionGeneration
{
    class Prompt
    {

        /// <summary>
        /// Text before the cursor
        /// </summary>
        public string PrefixText;

        /// <summary>
        /// Text after the cursor
        /// </summary>
        public string SuffixText;

        public Prompt(string prefixText, string suffixText)
        {
            PrefixText = prefixText;
            SuffixText = suffixText;
        }

        /// <summary>
        /// Crops the prefix / suffix so that they fit within the maximum length of the prompt defined in settings.
        /// </summary>
        /// <param name="settings">Autocompletion settings</param>
        /// <returns>The new parameters. If this prompt length is under the max, return this instance. 
        /// Otherwise, return a new instance with the cropped text</returns>
        public Prompt AsModelPrompt(Settings settings)
        {
            if (!settings?.IsInfillModel ?? false)
            {
                // Not an infill model, so don't crop the prompt. Suffix makes no sense
                return new Prompt(PrefixText, "");
            }

            // Is an infill model, so crop the prompt, if needed
            if (settings?.MaxPromptCharacters != null && (PrefixText.Length + SuffixText.Length) > (int)settings.MaxPromptCharacters)
            {
                Prompt croppedParms = new Prompt(PrefixText, SuffixText);

                // Text must be cropped. Calculate theoerical lengths to keep
                int prefixLengthToKeep = (int)(settings.MaxPromptCharacters * (settings.InfillPrefixPercentage / 100.0));
                int suffixLengthToKeep = (int)settings.MaxPromptCharacters - prefixLengthToKeep;

                if (suffixLengthToKeep > croppedParms.SuffixText.Length)
                {
                    // Suffix is not long enough. Add more text to prefix
                    prefixLengthToKeep += suffixLengthToKeep - croppedParms.SuffixText.Length;
                    suffixLengthToKeep = croppedParms.SuffixText.Length;
                }
                else if (prefixLengthToKeep > croppedParms.PrefixText.Length)
                {
                    // Prefix is not long enough. Add more text to suffix
                    suffixLengthToKeep += prefixLengthToKeep - croppedParms.PrefixText.Length;
                    prefixLengthToKeep = croppedParms.PrefixText.Length;
                }
                Debug.Assert(prefixLengthToKeep + suffixLengthToKeep == settings.MaxPromptCharacters);

                // Crop text
                OutputPaneHandler.Instance.Log($"Prompt cropped to {settings.MaxPromptCharacters} chars", LogLevel.Debug);
                croppedParms.PrefixText = croppedParms.PrefixText.Substring(croppedParms.PrefixText.Length - prefixLengthToKeep);
                croppedParms.SuffixText = croppedParms.SuffixText.Substring(0, suffixLengthToKeep);

                return croppedParms;
            }
            else
                return this;
        }
    }
}
