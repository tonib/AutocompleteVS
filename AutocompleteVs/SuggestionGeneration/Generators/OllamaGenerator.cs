using AutocompleteVs.Logging;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration.Generators
{
	internal class OllamaGenerator : IGenerator
	{
		private OllamaApiClient OLlamaClient;

        /// <summary>
        /// Package settings
        /// </summary>
        Settings Settings;

        public OllamaGenerator(Settings settings)
		{
            Settings = settings;

            var uri = new Uri(Settings.OllamaUrl);
            OLlamaClient = new OllamaApiClient(uri);
            OLlamaClient.SelectedModel = Settings.ModelName;
        }

        public void Dispose() => OLlamaClient?.Dispose();

        async public Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken)
        {
            try
            {
                await OutputPaneHandler.Instance.LogAsync("Starting new suggestion");

                var request = new GenerateRequest();

                // Request options
                if (!string.IsNullOrEmpty(Settings.KeepAlive))
                    request.KeepAlive = Settings.KeepAlive;
                request.Options = new RequestOptions()
                {
                    TopK = Settings.TopK,
                    TopP = Settings.TopP,
                    Temperature = Settings.Temperature,
                    Seed = Settings.Seed,
                    NumPredict = Settings.NumPredict,
                    NumCtx = Settings.NumCtx
                };

                request.Prompt = parameters.ModelPrompt.PrefixText;
                request.Suffix = parameters.ModelPrompt.SuffixText;

                // Debug prompt
                await OutputPaneHandler.Instance.LogAsync("Prefix:", LogLevel.Debug);
                await OutputPaneHandler.Instance.LogAsync(parameters.ModelPrompt.PrefixText, LogLevel.Debug);
                await OutputPaneHandler.Instance.LogAsync("Suffix:", LogLevel.Debug);
                await OutputPaneHandler.Instance.LogAsync(parameters.ModelPrompt.SuffixText, LogLevel.Debug);

                var sb = new SuggestionStringBuilder(parameters);
                GenerateResponseStream lastResponse = null;
                using (var exeTime = new ExecutionTime($"Autocompletion generation, " +
                    $"prefix chars: {parameters.ModelPrompt.PrefixText.Length}, " +
                    $"suffix chars: {parameters.ModelPrompt.SuffixText.Length}", false))
                {

                    IAsyncEnumerator<GenerateResponseStream> enumerator = null;
                    try
                    {
                        // Start generating
                        enumerator = OLlamaClient
                            .GenerateAsync(request, cancellationToken)
                            .GetAsyncEnumerator();

                        // ConfigureAwait(false) is required to avoid to get this task running in the UI thread
                        while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                        {
                            lastResponse = enumerator.Current;
                            sb.Add(lastResponse.Response);
                            if (sb.StopGeneration)
                                break;
                        }
                    }
                    finally
                    {
                        if(enumerator != null)
                            await enumerator.DisposeAsync();
                    }

                    await exeTime.WriteElapsedTimeAsync();
                }

                cancellationToken.ThrowIfCancellationRequested();

                await PrintResponseStatsAsync(sb.ToString(), lastResponse);

                // Notify the view the autocompletion has finished.
                // Run it in the UI thread. Otherwise it will trhow an excepcion
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                parameters.View.AutocompletionGenerationFinished(new Autocompletion(sb.ToString(), parameters));
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

        private static string NanoToMiliseconds(long ns) => (ns / 1000000.0).ToString("0.00") + " ms";

        async private static Task PrintResponseStatsAsync(string autocompleteText, GenerateResponseStream lastResponse)
        {
            await OutputPaneHandler.Instance.LogAsync($"Suggestion finished, {autocompleteText.Length} chars.");
            if (lastResponse is GenerateDoneResponseStream doneResponse)
            {
                await OutputPaneHandler.Instance.LogAsync(
                    $"Total duration: {NanoToMiliseconds(doneResponse.TotalDuration)}, " +
                    $"n. tokens prompt: {doneResponse.PromptEvalCount}, " +
                    $"n. tokens response: {doneResponse.EvalCount}, " +
                    $"Load duration: {NanoToMiliseconds(doneResponse.LoadDuration)}, " +
                    $"PromptEval duration: {NanoToMiliseconds(doneResponse.PromptEvalDuration)}, " +
                    $"Eval. duration: {NanoToMiliseconds(doneResponse.EvalDuration)}"
                );
            }
        }
    }
}
