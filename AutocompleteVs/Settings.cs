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

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);

            // Update the ollama client
            AutocompletionGeneration.Instance.ApplySettings(true);
        }

    }
}
