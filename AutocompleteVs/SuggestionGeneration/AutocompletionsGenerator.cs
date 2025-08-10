using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration.Generators;
using AutocompleteVs.SuggestionGeneration.Generators.CustomServer;
using AutocompleteVs.Config;

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Generates autocompletions with OLlama
	/// </summary>
	internal class AutocompletionsGenerator
    {

		static private AutocompletionsGenerator _Instance;

		/// <summary>
		/// The autocompletion generation instance. It will be null until the package is initialized
		/// </summary>
		static public AutocompletionsGenerator Instance
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
					_Instance = new AutocompletionsGenerator();
				}
				return _Instance;
			}
		}

		/// <summary>
		/// Handles concurrency to have a single generation at same time
		/// </summary>
		private SemaphoreSlim Semaphore = new SemaphoreSlim(1);

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
        private Settings Settings;

        /// <summary>
        /// Suggestions generator
        /// </summary>
        private IGenerator Generator;

        /// <summary>
        /// Initializes the ollama client
        /// </summary>
        private AutocompletionsGenerator()
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
			if(Generator != null)
			{
				// Dispose previous client
				try 
				{
                    Generator.Dispose();  
				}
				catch(Exception ex)
				{
					OutputPaneHandler.Instance.Log(ex);
                }
            }

            // Set up the new client
            Settings = AutocompleteVsPackage.Instance.Settings;
			switch(Settings.GeneratorType)
			{
				case GeneratorType.Ollama:
					Generator = new OllamaGenerator(Settings);
					break;
				case GeneratorType.OpenAi:
					Generator = new OpenAiGenerator(Settings);
					break;
				case GeneratorType.CustomServer:
					Generator = new CustomServerGenerator(Settings);
					break;
				default:
					throw new Exception($"Unknown {Settings.GeneratorType} suggestions generator");
			}
            
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
                CurrentAutocompletion = Generator.GetAutocompletionInternalAsync(parameters, CancellationTokenSource.Token);
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
                    CurrentAutocompletion = Generator.GetAutocompletionInternalAsync(NextAutocompletionParameters, 
                        CancellationTokenSource.Token);
                    NextAutocompletionParameters = null;
				}
				finally
				{
					Semaphore.Release();
				}
			}
        }
    }
}
