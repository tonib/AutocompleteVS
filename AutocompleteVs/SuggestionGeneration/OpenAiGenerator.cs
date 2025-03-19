using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Suggestions generation with OpenAI API
	/// </summary>
	class OpenAiGenerator : IGenerator
	{

		public OpenAiGenerator(Settings settings)
		{
			
		}

		public void Dispose()
		{

		}

		public Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken)
		{
			throw new NotImplementedException();
		}
	}
}
