﻿using Microsoft.Extensions.AI;
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

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Generates an autocompletions. Only a single generation in any view is run at same time
	/// </summary>
	internal class AutocompletionGeneration
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
			// TODO: Make this thread safe
			if (cancelCurrentAutocompletion)
				CancelCurrentGeneration();

			// set up the client
			Settings settings = AutocompleteVsPackage.Instance.Settings;
			var uri = new Uri(settings.OllamaUrl);
			OLlamaClient = new OllamaApiClient(uri);
			// select a model which should be used for further operations
			OLlamaClient.SelectedModel = settings.ModelName;
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
				request.Prompt = parameters.PrefixText;
				request.Suffix = parameters.SuffixText;

                string autocompleteText = "";
                using (new ExecutionTime($"Autocompletion generation, prefix chars: {parameters.PrefixText.Length}, " +
					$"suffix chars: {parameters.SuffixText.Length}"))
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

                CancellationTokenSource.Token.ThrowIfCancellationRequested();

				// qwen2.5-coder:1.5b-base adds unwanted spaces. NO
				// autocompleteText = autocompleteText.Trim();
				Debug.WriteLine("Suggestion finished: " + autocompleteText);

				// Notify the view the autocompletion has finished.
				// Run it in the UI thread. Otherwise it will trhow an excepcion
				await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                parameters.View.AutocompletionGenerationFinished(autocompleteText);
			}
            catch(TaskCanceledException)
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
	}
}
