using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
	/// <summary>
	/// Interface for suggestions generators
	/// </summary>
	internal interface IGenerator: IDisposable
	{
		Task GetAutocompletionInternalAsync(GenerationParameters parameters, CancellationToken cancellationToken);
	}
}
