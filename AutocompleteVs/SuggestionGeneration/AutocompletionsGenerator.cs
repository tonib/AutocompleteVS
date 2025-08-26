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
	/// Generates autocompletions with a given IGenerator
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
		/// Suggestions generators cache. Key = AutocompleteConfig.Id, Value = Generator instance
		/// for that configuration
		/// </summary>
		private Dictionary<string, IGenerator> Generators = new Dictionary<string, IGenerator>();

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
            Settings = AutocompleteVsPackage.Instance.Settings;

            if (cancelCurrentAutocompletion)
				CancelCurrentGeneration();

            // TODO: This is no thread safe, but an error should be very unusual
            foreach (IGenerator generator in Generators.Values)
            {
				// Dispose previous client
				try 
				{
                    generator.Dispose();  
				}
				catch(Exception ex)
				{
					OutputPaneHandler.Instance.Log(ex);
                }
            }
            Generators.Clear();
            
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

            IGenerator generator = GetGenerator(parameters);

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
                CurrentAutocompletion = generator.GetAutocompletionInternalAsync(parameters, CancellationTokenSource.Token);
				_ = RunQueuedAutocompletionsAsync();

            }
            finally
            {
                Semaphore.Release();
            }
        }

		/// <summary>
		/// Get the autocompletions generation for the requested autocompletion configuration
		/// </summary>
		/// <param name="parameters">Requested generation parameters</param>
		/// <returns>The generator to use</returns>
		/// <exception cref="Exception"></exception>
        private IGenerator GetGenerator(GenerationParameters parameters)
        {
			// Try to get the generator from the cache
            if(!Generators.TryGetValue(parameters.AutocompleteConfigId, out IGenerator generator))
			{
				// Create the generator for this configuration

				AutocompleteConfig config = this.Settings.AutocompletionConfigurations
					.FirstOrDefault(cfg => cfg.Id == parameters.AutocompleteConfigId);
				if(config == null)
					throw new Exception($"Unknown {parameters.AutocompleteConfigId} autocomplete configuration");								
				var modelConfig = config.ModelConfig;
				if(modelConfig == null)
					throw new Exception($"Unknown model {config.ModelConfigId} in {config.Id} autocomplete configuration");

				OutputPaneHandler.Instance.Log($"Creating new generator for {config.Id} autocomplete configuration",
					LogLevel.Debug);

				switch (modelConfig.Type)
				{
					case ModelType.Ollama:
                        generator = new OllamaGenerator(Settings);
						break;
					case ModelType.OpenAi:
                        generator = new OpenAiGenerator(Settings);
						break;
					//case GeneratorType.CustomServer:
					//	Generator = new CustomServerGenerator(Settings);
					//	break;
					default:
						throw new Exception($"Unknown {modelConfig.Type} autocomplete model in {config.Id} autocomplete configuration");
				}
				Generators[parameters.AutocompleteConfigId] = generator;
            }

            return generator;
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
                    IGenerator generator = GetGenerator(NextAutocompletionParameters);
                    CurrentAutocompletion = generator.GetAutocompletionInternalAsync(NextAutocompletionParameters, 
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
