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

		static public AutocompletionGeneration Instance
		{
			get
			{
				if (_Instance == null)
					_Instance = new AutocompletionGeneration();
				return _Instance;
			}
		}

		private OllamaApiClient OLlamaClient;

		private AutocompletionGeneration()
		{
			// set up the client. TODO: Load this from a configuration
			var uri = new Uri("http://localhost:11434");
			OLlamaClient = new OllamaApiClient(uri);
			// select a model which should be used for further operations
			OLlamaClient.SelectedModel = "qwen2.5-coder:1.5b-base";
		}

		Task CurrentAutocompletion;
		CancellationTokenSource CancellationTokenSource;

		async public Task CancelCurrentGenerationAsync()
		{
			CancellationTokenSource?.Cancel();
			if (CurrentAutocompletion != null)
			{
				Debug.WriteLine("Waiting current completion to finish (cancelled)");
				await CurrentAutocompletion;
			}
		}

		public async Task GetAutocompletionAsync(ViewAutocompleteHandler view, string prefixText, string suffixText)
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

				// Debug.WriteLine("---------------");
				string autocompleteText = "";
				var enumerator = OLlamaClient.GenerateAsync(request, CancellationTokenSource.Token).GetAsyncEnumerator();
				// ConfigureAwait(false) is required to avoid to get this task running to the UI thread
				while (await enumerator.MoveNextAsync().ConfigureAwait(false))
				{
					string newToken = enumerator.Current.Response;
					// Debug.Write(newToken);
					autocompleteText += newToken;
				}
				// Debug.WriteLine("---------------");

				if (CancellationTokenSource.IsCancellationRequested)
					return;

				// qwen2.5-coder:1.5b-base adds unwanted spaces
				autocompleteText = autocompleteText.Trim();
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
				// TODO: If is exception due to cancelled task, do not log anything
				// TODO: Log excepcion somewhere
				Debug.WriteLine(ex.ToString());
			}
			finally
			{
				CancellationTokenSource = null;
				CurrentAutocompletion = null;
			}
		}
	}
}
