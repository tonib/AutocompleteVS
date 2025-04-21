using AutocompleteVs.Client;
using AutocompleteVs.Logging;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Completion;
using Microsoft.CodeAnalysis.Text;
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
			try
			{
                await OutputPaneHandler.Instance.LogAsync("Starting new suggestion");
                InferenceClient client = null;
                try
				{
                    client = new InferenceClient(Settings.CustomServerUrl);
                    await client.ConnectAsync();

                }
                finally
				{
                    await client.DisposeAsync();
                    client = null;
				}
            }
            catch (TaskCanceledException)
            {
                await OutputPaneHandler.Instance.LogAsync("Suggestion cancelled");
            }
            catch (Exception ex)
            {
                // Somethimes is giving me IOException inside "await enumerator.MoveNextAsync()" instead a TaskCanceledException
                // So check if process was canceled
                bool isCanceled = cancellationToken.IsCancellationRequested;
                if (!isCanceled)
                {
                    await OutputPaneHandler.Instance.LogAsync(ex);
                }
                else
                {
                    await OutputPaneHandler.Instance.LogAsync("Suggestion cancelled");
                }
            }
        }

        async public Task TestRoslynAutocompletionsAsync(GenerationParameters parameters, CancellationToken cancellationToken)
        {
            if(parameters.Document == null)
            {
                return;
            };

			// TODO: If is not a C# document (ex Visual Studio) then do nothing
			Document editableDoc = parameters.Document.Project
                .AddDocument("test.cs", SourceText.From(parameters.OriginalPrompt.FullText));

            CompletionService completionService = CompletionService.GetService(parameters.Document);

            // https://www.strathweb.com/2018/12/using-roslyn-c-completion-service-programmatically/
            CompletionList completions = await completionService
                .GetCompletionsAsync(editableDoc, parameters.OriginalPrompt.PrefixText.Length);
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
