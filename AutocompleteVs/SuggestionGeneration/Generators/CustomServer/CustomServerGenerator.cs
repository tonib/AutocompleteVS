// using AutocompleteVs.Client;
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

namespace AutocompleteVs.SuggestionGeneration.Generators.CustomServer
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
        { }

        /*
        async public Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken)
		{
            // Removed reference to custom server: Problems with VS 2019, with Microsoft.Bcl.AsyncInterfaces AGAIN...
			try
			{
                await OutputPaneHandler.Instance.LogAsync("Starting new suggestion");
                InferenceClient client = null;
                try
				{
                    var sb = new SuggestionStringBuilder(parameters);

                    using (var exeTime = new ExecutionTime($"Autocompletion generation, " +
                        $"prefix chars: {parameters.ModelPrompt.PrefixText.Length}, " +
                        $"suffix chars: {parameters.ModelPrompt.SuffixText.Length}", false))
                    {
                        client = new InferenceClient(Settings.CustomServerUrl);
                        await client.ConnectAsync();

                        var request = new InferenceRequest
                        {
                            Prompt = parameters.ModelPrompt.PrefixText,
                            Suffix = parameters.ModelPrompt.SuffixText
                        };

                        // TODO: Change client to support cancelation token
                        // TODO: Store model to use in settings
                        ContextValidWords ctxWords = new ContextValidWords(parameters, sb);
                        string[] validWords = await ctxWords.GetValidWordsAsync();

                        // Generate first token
                        string token = await client.StartInferenceAsync("qwen2.5-coder-1.5b-q8_0.gguf", request, null);
                        while (token != null)
					    {
                            cancellationToken.ThrowIfCancellationRequested();
                            sb.Add(token);
                            Debug.WriteLine(token);
                            if (sb.StopGeneration)
                                break;

                            // Generate next token
                            validWords = await ctxWords.GetValidWordsAsync();
                            token = await client.ContinueInferenceAsync(null);
					    }

                        await exeTime.WriteElapsedTimeAsync();
                    }

                    cancellationToken.ThrowIfCancellationRequested();

                    // Notify the view the autocompletion has finished.
                    // Run it in the UI thread. Otherwise it will trhow an excepcion
                    await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    parameters.View.AutocompletionGenerationFinished(new Autocompletion(sb.ToString(), parameters));
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
        */
    }
}
