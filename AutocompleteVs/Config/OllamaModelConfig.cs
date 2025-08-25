using AutocompleteVs.SuggestionGeneration;
using EnvDTE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs.Config
{
    /// <summary>
    /// Configuration for Ollama models
    /// </summary>
    internal class OllamaModelConfig : IModelConfig
    {
        public string Id { get; set; }

        public ModelType Type => ModelType.Ollama;

        [DisplayName("Top K")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower " +
            "value (e.g., 0.5) will generate more focused and conservative text. Empty == use default")]
        public int? TopK { get; set; } = 1;

        [DisplayName("Top P")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value " +
            "(e.g., 0.5) will generate more focused and conservative text. Empty == use default")]
        public float? TopP { get; set; }

        [DisplayName("Temperature")]
        [Description("The temperature of the model. Increasing the temperature will make the model answer more creatively" +
            ". Empty == use default")]
        public float? Temperature { get; set; }

        [DisplayName("Seed")]
        [Description("Sets the random number seed to use for generation. Setting this to a specific number will make the model " +
            "generate the same text for the same prompt. Empty == use default")]
        public int? Seed { get; set; }

        [DisplayName("Generator type")]
        public GeneratorType GeneratorType { get; set; } = GeneratorType.Ollama;

        [DisplayName("Context size")]
        [Description("Number of tokens in context size. Empty == use default")]
        public int? NumCtx { get; set; } = 2048;

        [DisplayName("Ollama URL")]
        [Description("Ollama URL")]
        public string OllamaUrl { get; set; } = "http://localhost:11434";

        [DisplayName("Model name")]
        [Description("The Ollama model name to use for autocompletion")]
        public string ModelName { get; set; } = "qwen2.5-coder:1.5b-base";

        [DisplayName("Time to keep alive the model in ollama")]
        [Description("how long the model will stay loaded into memory following the request. Ex. 60m, 1h, 3600. " +
            "Empty == use ollama default")]
        public string KeepAlive { get; set; } = "1h";

        // TODO: Replace this by enum with: Base / Infill / Chat or Instruct
        [DisplayName("Is infill model?")]
        [Description("True if model accepts cursor position, and text before and after cursor are feeded separately as prompt." +
            "False if model only accepts text before cursor")]
        public bool IsInfillModel { get; set; } = true;

        [DisplayName("Suggestion max. tokens number")]
        [Description("Maximum number of tokens to predict when generating text. Empty == use default")]
        public int? NumPredict { get; set; } = 200;

        public override string ToString() => Id + " (OLlama)";

        public override bool Equals(object obj)
            => (obj as IModelConfig)?.Id == Id;

        override public int GetHashCode() => Id.GetHashCode();
    }
}
