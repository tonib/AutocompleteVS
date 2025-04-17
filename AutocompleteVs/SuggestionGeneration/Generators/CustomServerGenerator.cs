using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration.Generators
{
    class CustomServerGenerator : IGenerator
    {
        private Settings Settings;

        public CustomServerGenerator(Settings settings)
        {
            Settings = settings;
        }

        public void Dispose()
        {
            
        }

        async public Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken)
        {
            if(parameters.Document == null)
            {
                return;
            }

            // TODO: If is not a C# document (ex Visual Studio) then do nothing

            CompletionService completionService = CompletionService.GetService(parameters.Document);

            // https://www.strathweb.com/2018/12/using-roslyn-c-completion-service-programmatically/
            CompletionList completions = await completionService
                .GetCompletionsAsync(parameters.Document, parameters.OriginalPrompt.PrefixText.Length);
            if (completions == null)
            {
                return;
            }

            foreach (CompletionItem completionItem in completions.Items)
            {
                var x = completionItem;
                // Debug.WriteLine(completionItem.DisplayText);
            }
            // List<string> textItems = completions.Items.Select(i => i.DisplayText).ToList();
        }
    }
}
