using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Config
{
    /// <summary>
    /// Configuration for OpenAI models
    /// </summary>
    internal class OpenAIModelConfig : IModelConfig
    {
        /// <summary>
        /// Model name
        /// </summary>
        public string Id { get; set; }

        public ModelType Type => ModelType.OpenAi;

        public string OpenAiKey { get; set; }

        /// <summary>
        /// OpenAI model name
        /// </summary>
        public string ModelName { get; set; } = "gpt-4o-mini";

        public bool IsInfillModel => false;

    }
}
