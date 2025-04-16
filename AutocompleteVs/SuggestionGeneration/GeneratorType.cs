using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
	public enum GeneratorType
	{
		Ollama = 0,
		OpenAi = 1,
		CustomServer = 2
	}
}
