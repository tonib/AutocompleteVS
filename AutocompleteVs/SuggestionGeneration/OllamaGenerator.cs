using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
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
                Debug.WriteLine("Starting new suggestion");

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

                // TODO: Currently, there is no need to get the response as a stream
                string autocompleteText = "";
                GenerateResponseStream lastResponse = null;
                using (new ExecutionTime($"Autocompletion generation, prefix chars: {parameters.ModelPrompt.PrefixText.Length}, " +
                    $"suffix chars: {parameters.ModelPrompt.SuffixText.Length}"))
                {
                    // Debug.WriteLine("---------------");
                    var enumerator = OLlamaClient.GenerateAsync(request, cancellationToken).GetAsyncEnumerator();
                    // ConfigureAwait(false) is required to avoid to get this task running to the UI thread
                    while (await enumerator.MoveNextAsync().ConfigureAwait(false))
                    {
                        lastResponse = enumerator.Current;
                        string newToken = lastResponse.Response;
                        // Debug.Write(newToken);
                        autocompleteText += newToken;
                    }
                    // Debug.WriteLine("---------------");
                }

                cancellationToken.ThrowIfCancellationRequested();

                PrintResponseStats(autocompleteText, lastResponse);

                // Notify the view the autocompletion has finished.
                // Run it in the UI thread. Otherwise it will trhow an excepcion
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                parameters.View.AutocompletionGenerationFinished(new Autocompletion(autocompleteText, parameters));
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Suggestion cancelled");
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
                    Debug.WriteLine("Suggestion cancelled");
                }
            }
        }

        private static string NanoToMiliseconds(long ns) => (ns / 1000000.0).ToString("0.00") + " ms";

        private static void PrintResponseStats(string autocompleteText, GenerateResponseStream lastResponse)
        {
            Debug.WriteLine($"Suggestion finished, {autocompleteText.Length} chars.");
            // Debug.WriteLine($"Suggestion: {autocompleteText}");
            if (lastResponse is GenerateDoneResponseStream doneResponse)
            {
                Debug.WriteLine($"Total duration: {NanoToMiliseconds(doneResponse.TotalDuration)}, " +
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
