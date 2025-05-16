using AutocompleteVs.Logging;
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

    // TODO: Organize this in categories, it's a mess

    /// <summary>
    /// Package settings
    /// </summary>
    class Settings : DialogPage
    {
        /// <summary>
        /// VS settings page category name
        /// </summary>
        public const string PageCategory = "AutocompleteVs";

        // Properties categories
        private const string GeneralCategory = "General";
        private const string OllamaCategory = "Ollama";
        private const string OpenAiCategory = "OpenAI";
        private const string CustomServerCategory = "CustomServer";

        #region General

        [Category(GeneralCategory)]
        [DisplayName("Automatic suggestions")]
        [Description("If true, suggestions will be generated automatically when typing or moving the caret in editor")]
        public bool AutomaticSuggestions { get; set; } = true;

        [Category(GeneralCategory)]
        [DisplayName("Top K")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower " +
            "value (e.g., 0.5) will generate more focused and conservative text. Empty == use default")]
        public int? TopK { get; set; } = 1;

        [Category(GeneralCategory)]
        [DisplayName("Top P")]
        [Description("Works together with top-k. A higher value (e.g., 0.95) will lead to more diverse text, while a lower value " +
            "(e.g., 0.5) will generate more focused and conservative text. Empty == use default")]
        public float? TopP { get; set; }

        [Category(GeneralCategory)]
        [DisplayName("Temperature")]
        [Description("The temperature of the model. Increasing the temperature will make the model answer more creatively" +
            ". Empty == use default")]
        public float? Temperature { get; set; }

        [Category(GeneralCategory)]
        [DisplayName("Seed")]
        [Description("Sets the random number seed to use for generation. Setting this to a specific number will make the model " +
            "generate the same text for the same prompt. Empty == use default")]
        public int? Seed { get; set; }

        [Category(GeneralCategory)]
        [DisplayName("Suggestion max. tokens number")]
        [Description("Maximum number of tokens to predict when generating text. Empty == use default")]
        public int? NumPredict { get; set; }

        [Category(GeneralCategory)]
        [DisplayName("Max. number of characters in prompt")]
        [Description("Maximum number of characters to send as prompt. Empty == send all file")]
        public int? MaxPromptCharacters { get; set; } = 2048;

        [Category(GeneralCategory)]
        [DisplayName("Prefix % when max. characters is reached")]
        [Description("Only applies the current file has a size lager than the max. number of characters in prompt. " +
            "As only that maximum number of characters will be sent, this is the % of characters to get from before the caret position." +
            "The remaining % will be characters after caret position. Between 0 and 100")]
        public double? InfillPrefixPercentage { get; set; } = 75.0;

        [Browsable(false)]
        public double? InfillSuffixPercentage => 100.0 - InfillPrefixPercentage;

        [Category(GeneralCategory)]
        [DisplayName("Generator type")]
        public GeneratorType GeneratorType { get; set; } = GeneratorType.Ollama;

        [Category(GeneralCategory)]
        [DisplayName("Log level")]
        [Description("Log level for this extension messages. Log is written to Output > AutocompleteVS pane")]
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// If > 0, a status bar message will be displayed, when each this number of tokens is generated
        /// </summary>
        [Category(GeneralCategory)]
        [DisplayName("Generation progress each # tokens")]
        [Description("If > 0, a message will be displayed in status bar when genenerating suggestions, updating progress each this " +
            "token number multiple. If <= 0, no status bar message will be displayed")]
        public int NumTokensProgress { get; set; } = 100;

        #endregion

        #region Ollama

        [Category(OllamaCategory)]
        [DisplayName("Context size")]
        [Description("Number of tokens in context size. Empty == use default")]
        public int? NumCtx { get; set; } = 2048;

        [Category(OllamaCategory)]
        [DisplayName("Ollama URL")]
        [Description("Ollama URL")]
        public string OllamaUrl { get; set; } = "http://localhost:11434";

        [Category(OllamaCategory)]
        [DisplayName("Model name")]
        [Description("The Ollama model name to use for autocompletion")]
        public string ModelName { get; set; } = "qwen2.5-coder:1.5b-base";

        [Category(OllamaCategory)]
        [DisplayName("Time to keep alive the model in ollama")]
        [Description("how long the model will stay loaded into memory following the request. Ex. 60m, 1h, 3600. " +
            "Empty == use ollama default")]
        public string KeepAlive { get; set; } = "1h";

        [Category(OllamaCategory)]
        [DisplayName("Is infill model?")]
        [Description("True if model accepts cursor position, and text before and after cursor are feeded separately as prompt." +
            "False if model only accepts text before cursor")]
        public bool IsInfillModel { get; set; } = true;

        #endregion

        [Category(OpenAiCategory)]
        [DisplayName("OpenAI key")]
        public string OpenAiKey { get; set; }

        [Category(CustomServerCategory)]
        [DisplayName("Custorm server URL")]
        [Description("URL for custom AutocompleteVs server")]
        public string CustomServerUrl { get; set; } = "http://localhost:5118/InferenceHub";

        protected override void OnApply(PageApplyEventArgs e)
        {
            if (InfillPrefixPercentage != null)
            {
                if (InfillPrefixPercentage < 0 || InfillPrefixPercentage > 100)
                {
                    e.ApplyBehavior = ApplyKind.Cancel;
                    AutocompleteVsPackage.Instance.MessageBox(
                        "'Prefix % when max. characters is reached' must to be between 0 and 100",
                        "Error",
                        Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_CRITICAL);
                    return;
                }
            }

            base.OnApply(e);


            // Update the ollama client
            AutocompletionsGenerator.Instance.ApplySettings(true);
            OutputPaneHandler.Instance.LogLevel = AutocompleteVsPackage.Instance.Settings.LogLevel;
        }

    }
}
