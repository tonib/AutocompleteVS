using AutocompleteVs.SuggestionGeneration;
using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocompleteVs
{
    // https://learn.microsoft.com/en-us/visualstudio/extensibility/creating-an-options-page?view=vs-2022

    /// <summary>
    /// Package settings
    /// </summary>
    class Settings : DialogPage
    {
        /// <summary>
        /// Category for the settings
        /// </summary>
        public const string Category = "AutocompleteVs";

        [Category(Category)]
        [DisplayName("Ollama URL")]
        [Description("Ollama URL")]
        public string OllamaUrl { get; set; } = "http://localhost:11434";

        [Category(Category)]
        [DisplayName("Model name")]
        [Description("The Ollama model to user for autocompletion")]
        public string ModelName { get; set; } = "qwen2.5-coder:1.5b-base";

        [Category(Category)]
        [DisplayName("Automatic suggestions")]
        [Description("If true, suggestions will be generated automatically when typing or moving the caret in editor")]
        public bool AutomaticSuggestions { get; set; } = true;

        [Category(Category)]
        [DisplayName("Context size")]
        [Description("Number of tokens in context size. Empty == use ollama default")]
        public int? NumCtx { get; set; } = 2048;

        [Category(Category)]
        [DisplayName("Top K")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower " +
            "value (e.g., 0.5) will generate more focused and conservative text. Empty == use ollama default")]
        public int? TopK { get; set; } = 1;

        [Category(Category)]
        [DisplayName("Top P")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value " +
            "(e.g., 0.5) will generate more focused and conservative text. Empty == use ollama default")]
        public float? TopP { get; set; }

        [Category(Category)]
        [DisplayName("Temperature")]
        [Description("The temperature of the model. Increasing the temperature will make the model answer more creatively" +
            ". Empty == use ollama default")]
        public float? Temperature { get; set; }

        [Category(Category)]
        [DisplayName("Seed")]
        [Description("Sets the random number seed to use for generation. Setting this to a specific number will make the model " +
            "generate the same text for the same prompt. Empty == use ollama default")]
        public int? Seed { get; set; }

        [Category(Category)]
        [DisplayName("Suggestion max. tokens number")]
        [Description("Maximum number of tokens to predict when generating text. Empty == use ollama default")]
        public int? NumPredict { get; set; }

        [Category(Category)]
        [DisplayName("Time to keep alive the model in ollama")]
        [Description("how long the model will stay loaded into memory following the request. Ex. 60m, 1h, 3600. " +
            "Empty == use ollama default")]
        public string KeepAlive { get; set; } = "1h";

        [Category(Category)]
        [DisplayName("Max. number of characters in prompt")]
        [Description("Maximum number of characters to send as prompt. Empty == send all file")]
        public int? MaxPromptCharacters { get; set; } = 2048;

        [Category(Category)]
        [DisplayName("Prefix % when max. characters is reached")]
        [Description("Only applies the current file has a size lager than the max. number of characters in prompt. " +
            "As only that maximum number of characters will be sent, this is the % of characters to get from before the caret position." +
            "The remaining % will be characters after caret position. Between 0 and 100")]
        public double? InfillPrefixPercentage { get; set; } = 75.0;

        [Browsable(false)]
        public double? InfillSuffixPercentage => 100.0 - InfillPrefixPercentage;

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            // Update the ollama client
            AutocompletionGeneration.Instance.ApplySettings(true);
        }

    }
}
