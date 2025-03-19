using Microsoft.Extensions.AI;
using OllamaSharp.Models;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.VisualStudio.Text.Editor;
using System.Threading;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Generates an autocompletions. Only a single generation in any view is run at same time
	/// </summary>
	internal class OLlamaGeneration
	{

		static private OLlamaGeneration _Instance;

		/// <summary>
		/// The autocompletion generation instance. It will be null until the package is initialized
		/// </summary>
		static public OLlamaGeneration Instance
		{
			get
			{
				if (_Instance == null)
				{
					if(AutocompleteVsPackage.Instance == null)
					{
						// Package not initialized yet
						return null;
					}
					_Instance = new OLlamaGeneration();
				}
				return _Instance;
			}
		}

		/// <summary>
		/// Handles concurrency to have a single generation at same time
		/// </summary>
		private SemaphoreSlim Semaphore = new SemaphoreSlim(1);

		private OllamaApiClient OLlamaClient;

        /// <summary>
        /// Current generation task. It is null if no generation is running
        /// </summary>
        private Task CurrentAutocompletion;

		private GenerationParameters NextAutocompletionParameters;

        /// <summary>
        /// cancellation token source for the current autocompletion generation
        /// </summary>
        private CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Package settings
        /// </summary>
        Settings Settings;

        /// <summary>
        /// Initializes the ollama client
        /// </summary>
        private OLlamaGeneration()
		{
			ApplySettings(false);
		}

        /// <summary>
        /// Create a new autocompletion client with the current settings
        /// </summary>
        /// <param name="cancelCurrentAutocompletion">True is current autocompletion should be cancelled</param>
        public void ApplySettings(bool cancelCurrentAutocompletion)
		{
			if (cancelCurrentAutocompletion)
				CancelCurrentGeneration();

            // TODO: This is no thread safe, but an error should be very unusual
			if(OLlamaClient != null)
			{
				// Dispose previous client
				try 
				{ 
					OLlamaClient.Dispose();  
				}
				catch(Exception ex)
				{
                    // TODO: Log exception
                    Debug.WriteLine(ex.ToString());
                }
            }

            // Set up the new client
            Settings = AutocompleteVsPackage.Instance.Settings;
			var uri = new Uri(Settings.OllamaUrl);
			OLlamaClient = new OllamaApiClient(uri);
			OLlamaClient.SelectedModel = Settings.ModelName;
        }

		public void CancelCurrentGeneration()
		{
            Semaphore.Wait();
			try
			{
				CancellationTokenSource?.Cancel();
			}
            finally
            {
                Semaphore.Release();
            }
        }

		public void StartAutocompletion(GenerationParameters parameters)
		{
            // Wait until the semaphore is available
            Semaphore.Wait();

			try
			{
				if(CurrentAutocompletion != null)
				{
                    // There is a running generation. Cancel it
                    CancellationTokenSource?.Cancel();

					// Queue the new generation. Override any previously queued generation
					NextAutocompletionParameters = parameters;
					return;
                }

                // No generation is running. Start a new one, and do not wait for it to finish
                CancellationTokenSource = new CancellationTokenSource();
                CurrentAutocompletion = GetAutocompletionInternalAsync(parameters);
				_ = RunQueuedAutocompletionsAsync();

            }
            finally
            {
                Semaphore.Release();
            }
        }

		async private Task RunQueuedAutocompletionsAsync()
		{
			while (true)
			{
				await CurrentAutocompletion;

				// Check if there is a new generation queued
				await Semaphore.WaitAsync();
				try
				{
                    CancellationTokenSource.Dispose();
                    CancellationTokenSource = null;

                    if (NextAutocompletionParameters == null)
					{
						// No new generation queued
						CurrentAutocompletion = null;
                        return;
					}

                    // Prepare the new generation
                    CancellationTokenSource = new CancellationTokenSource();
                    CurrentAutocompletion = GetAutocompletionInternalAsync(NextAutocompletionParameters);
                    NextAutocompletionParameters = null;
				}
				finally
				{
					Semaphore.Release();
				}
			}
        }

		async private Task GetAutocompletionInternalAsync(GenerationParameters parameters)
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

                request.Prompt = parameters.PrefixText;
                request.Suffix = parameters.SuffixText;

                // TODO: Currently, there is no need to get the response as a stream
                string autocompleteText = "";
                GenerateResponseStream lastResponse = null;
                using (new ExecutionTime($"Autocompletion generation, prefix chars: {parameters.PrefixText.Length}, " +
                    $"suffix chars: {parameters.SuffixText.Length}"))
                {
                    // Debug.WriteLine("---------------");
                    var enumerator = OLlamaClient.GenerateAsync(request, CancellationTokenSource.Token).GetAsyncEnumerator();
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

                CancellationTokenSource.Token.ThrowIfCancellationRequested();

                PrintResponseStats(autocompleteText, lastResponse);

                // Notify the view the autocompletion has finished.
                // Run it in the UI thread. Otherwise it will trhow an excepcion
                await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                parameters.View.AutocompletionGenerationFinished(autocompleteText);
            }
            catch (TaskCanceledException)
			{
				Debug.WriteLine("Suggestion cancelled");
			}
			catch (Exception ex)
			{
                // Somethimes is giving me IOException inside "await enumerator.MoveNextAsync()" instead a TaskCanceledException
                // So check if process was canceled
                bool isCanceled = CancellationTokenSource?.IsCancellationRequested ?? true;
                if (!isCanceled)
				{
					// TODO: Log excepcion somewhere
					Debug.WriteLine(ex.ToString());
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
