using Microsoft.CodeAnalysis;
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

        /// <summary>
        /// Whether the generation should be limited to a single line
        /// </summary>
        public bool GenerateSingleLine;

        /// <summary>
        /// The document where the autocompletion request is being made. Null if is not a C# document
        /// </summary>
        public Document Document;

        /// <summary>
        /// The location of the caret in the document when suggestion generation started
        /// </summary>
        public int CaretBufferLocation;

        /// <summary>
        /// The id of the autocompletion settings for this autocompletion. 
        /// Refrences to <see cref="AutocompleteConfig.Id" />
        /// </summary>
        public readonly string AutocompleteConfigId;

        /// <summary>
        /// Creates a new instance of the <see cref="GenerationParameters"/> class
        /// </summary>
        /// <param name="view">The VS view where the autocompletion request is being made</param>
        /// <param name="originalPrompt">The original prompt (the entire file)</param>
        /// <param name="modelPrompt">The prompt to feed to the model. It may be cropped by a maximum length</param>
        /// <param name="singleLine">Whether the generation should be limited to a single line</param>
        /// <param name="caretBufferLocation">Caret position in document buffer when autocompletion was
        /// requested</param>
        /// <param name="autocompleteConfigId">Id settings for this autocompletion</param>
        public GenerationParameters(ViewAutocompleteHandler view, Prompt originalPrompt, 
            Prompt modelPrompt, bool singleLine,
            Document document, int caretBufferLocation, string autocompleteConfigId)
        {
            this.Document = document;
            this.View = view;
            this.OriginalPrompt = originalPrompt;
            this.ModelPrompt = modelPrompt;
            this.GenerateSingleLine = singleLine;
            this.CaretBufferLocation = caretBufferLocation;
            this.AutocompleteConfigId = autocompleteConfigId;
        }

    }
}
