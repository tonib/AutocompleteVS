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

namespace AutocompleteVs
{
	/// <summary>
	/// Generates an autocompletions. Only a single generation in any view is run at same time
	/// </summary>
	class AutocompletionGeneration
	{
		static private AutocompletionGeneration _Instance;

		/// <summary>
		/// The autocompletion generation instance. It will be null until the package is initialized
		/// </summary>
		static public AutocompletionGeneration Instance
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
					_Instance = new AutocompletionGeneration();
				}
				return _Instance;
			}
		}

		private OllamaApiClient OLlamaClient;

        /// <summary>
        /// The current autocompletion generation task. It is null if no generation is running
        /// </summary>
        Task CurrentAutocompletion;

        /// <summary>
        /// cancellation token source for the current autocompletion generation
        /// </summary>
        CancellationTokenSource CancellationTokenSource;

        /// <summary>
        /// Initializes the ollama client
        /// </summary>
        private AutocompletionGeneration()
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
				_ = CancelCurrentGenerationAsync();

            // set up the client
            Settings settings = AutocompleteVsPackage.Instance.Settings;
            var uri = new Uri(settings.OllamaUrl);
            OLlamaClient = new OllamaApiClient(uri);
            // select a model which should be used for further operations
            OLlamaClient.SelectedModel = settings.ModelName;
        }

		async public Task CancelCurrentGenerationAsync()
		{
			CancellationTokenSource?.Cancel();
			if (CurrentAutocompletion != null)
			{
				Debug.WriteLine("Waiting current completion to finish (cancelled)");
				await CurrentAutocompletion;
			}
		}

		public async Task StartAutocompletionAsync(ViewAutocompleteHandler view, string prefixText, string suffixText)
		{
			await CancelCurrentGenerationAsync();
			CurrentAutocompletion = GetAutocompletionInternalAsync(view, prefixText, suffixText);
			await CurrentAutocompletion;
		}

		async private Task GetAutocompletionInternalAsync(ViewAutocompleteHandler view, string prefixText, string suffixText)
		{
			try
			{
				Debug.WriteLine("Starting new suggestion");

				// Cancel current generation, if there is some
				CancellationTokenSource = new CancellationTokenSource();

				var request = new GenerateRequest();
				request.Prompt = prefixText;
				request.Suffix = suffixText;

                string autocompleteText = "";
                using (new ExecutionTime($"Autocompletion generation, prefix chars: {prefixText.Length}, suffix chars: {suffixText.Length}"))
				{
					// Debug.WriteLine("---------------");
					var enumerator = OLlamaClient.GenerateAsync(request, CancellationTokenSource.Token).GetAsyncEnumerator();
					// ConfigureAwait(false) is required to avoid to get this task running to the UI thread
					while (await enumerator.MoveNextAsync().ConfigureAwait(false))
					{
						string newToken = enumerator.Current.Response;
						// Debug.Write(newToken);
						autocompleteText += newToken;
					}
                    // Debug.WriteLine("---------------");
                }

                if (CancellationTokenSource.IsCancellationRequested)
					return;

				// qwen2.5-coder:1.5b-base adds unwanted spaces. NO
				// autocompleteText = autocompleteText.Trim();
				Debug.WriteLine("Suggestion finished: " + autocompleteText);

				// Notify the view the autocompletion has finished.
				// Run it in the UI thread. Otherwise it will trhow an excepcion
				await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
				view.AutocompletionGenerationFinished(autocompleteText);
			}
            catch(TaskCanceledException)
			{
				Debug.WriteLine("Suggestion cancelled");
			}
			catch (Exception ex)
			{
				// Somethimes is giving me IOException inside "await enumerator.MoveNextAsync()" instead a TaskCanceledException
				// So check if process was canceled
				if (!CancellationTokenSource.IsCancellationRequested)
				{
					// TODO: Log excepcion somewhere
					Debug.WriteLine(ex.ToString());
				}
				else
				{
					Debug.WriteLine("Suggestion cancelled");
				}
			}
			finally
			{
				CancellationTokenSource = null;
				CurrentAutocompletion = null;
			}
		}
	}
}
