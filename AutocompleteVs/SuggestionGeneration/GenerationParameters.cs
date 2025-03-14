using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.SuggestionGeneration
{
    internal class GenerationParameters
    {
        public ViewAutocompleteHandler View;
        public string PrefixText;
        public string SuffixText;

        public GenerationParameters(ViewAutocompleteHandler view, string prefixText, string suffixText)
        {
            View = view;
            PrefixText = prefixText;
            SuffixText = suffixText;
        }
    }
}
