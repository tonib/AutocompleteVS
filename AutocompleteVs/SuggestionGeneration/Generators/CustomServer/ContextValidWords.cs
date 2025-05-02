using AutocompleteVs.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;
using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration.Generators.CustomServer
{
    /// <summary>
    /// Get valid works in a C# file position
    /// </summary>
    class ContextValidWords
    {
        private readonly GenerationParameters Parameters;
        private readonly SuggestionStringBuilder StringBuilder;
        private readonly CompletionService CompletionService;

        public ContextValidWords(GenerationParameters parameters, SuggestionStringBuilder stringBuilder)
        {
            Parameters = parameters;
            StringBuilder = stringBuilder;

            // TODO: If is not a C# document (ex Visual Studio) then do nothing. Probably this will return null, check it
            if (parameters.Document != null)
                CompletionService = CompletionService.GetService(parameters.Document);
        }

        async public Task<string[]> GetValidWordsAsync()
        {
            try
            {
                // return null;

                if (CompletionService == null)
                {
                    return null;
                }

                string prefixText = Parameters.OriginalPrompt.PrefixText + StringBuilder.ToString();
                string currentDocText = prefixText + Parameters.OriginalPrompt.SuffixText;

                Document editableDoc = Parameters.Document.Project.AddDocument("test.cs", SourceText.From(currentDocText));
                CompletionList completions = await CompletionService
                    .GetCompletionsAsync(editableDoc, prefixText.Length);

                // TODO: This, for sure, will be wrong
                return completions.Items.Select(c => c.DisplayText).ToArray();
            }

            catch (Exception ex)
            {
                await OutputPaneHandler.Instance.LogAsync(ex);
                return null;
            }
        }
    }
}
