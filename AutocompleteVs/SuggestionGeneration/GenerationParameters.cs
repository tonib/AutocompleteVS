using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
    /// <summary>
    /// Contains the parameters for an autocompletion generation request
    /// </summary>
    internal class GenerationParameters
    {


        /// <summary>
        /// The view where the autocompletion request is being made
        /// </summary>
        public ViewAutocompleteHandler View;

        /// <summary>
        /// The original prompt
        /// </summary>
        public Prompt OriginalPrompt;

        /// <summary>
        /// The prompt to feed to the model. It may be cropped by a maximum length
        /// </summary>
        public Prompt ModelPrompt;

        public GenerationParameters(ViewAutocompleteHandler view, Prompt originalPrompt, Prompt modelPrompt)
        {
            this.View = view;
            this.OriginalPrompt = originalPrompt;
            this.ModelPrompt = modelPrompt;
        }

    }
}
