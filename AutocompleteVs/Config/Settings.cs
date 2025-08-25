using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration;
using Microsoft.VisualStudio.Shell;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocompleteVs.Config
{
    // https://learn.microsoft.com/en-us/visualstudio/extensibility/creating-an-options-page?view=vs-2022

    // TODO: Organize this in categories, it's a mess
    // TODO: Create a custom settings page: https://learn.microsoft.com/en-us/visualstudio/extensibility/creating-an-options-page?view=vs-2022#create-a-tools-options-custom-page

    /// <summary>
    /// Package settings with custom UI
    /// </summary>
    public class Settings : DialogPage
    {
        /// <summary>
        /// VS settings page category name
        /// </summary>
        public const string PageCategory = "AutocompleteVs";

        private SettingsUserControl _control;

        #region General

        [DisplayName("Automatic suggestions")]
        [Description("If true, suggestions will be generated automatically when typing or moving the caret in editor")]
        public bool AutomaticSuggestions { get; set; } = true;

        [DisplayName("Log level")]
        [Description("Log level for this extension messages. Log is written to Output > AutocompleteVS pane")]
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;

        /// <summary>
        /// If > 0, a status bar message will be displayed, when each this number of tokens is generated
        /// </summary>
        [DisplayName("Generation progress each # tokens")]
        [Description("If > 0, a message will be displayed in status bar when genenerating suggestions, updating progress each this " +
            "token number multiple. If <= 0, no status bar message will be displayed")]
        public int NumTokensProgress { get; set; } = 100;

        #endregion

        // TODO: Still not in use
        [DisplayName("Custorm server URL")]
        [Description("URL for custom AutocompleteVs server")]
        public string CustomServerUrl { get; set; } = "http://localhost:5118/InferenceHub";

        /// <summary>
        /// Configured models (not persisted directly)
        /// </summary>
        [Browsable(false)]
        public List<IModelConfig> Models { get; set; } = new List<IModelConfig>();

        /// <summary>
        /// JSON serialization of models for persistence
        /// </summary>
        [DisplayName("Models JSON")]
        [Description("JSON representation of configured models (internal use)")]
        [Browsable(false)]
        public string ModelsJson
        {
            get
            {
                try
                {
                    var serializedModels = Models.Select(m => new
                    {
                        Type = m.Type.ToString(),
                        Data = m
                    }).ToList();
                    return JsonConvert.SerializeObject(serializedModels, Formatting.Indented);
                }
                catch
                {
                    return "[]";
                }
            }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        Models = new List<IModelConfig>();
                        return;
                    }

                    var serializedModels = JsonConvert.DeserializeObject<dynamic[]>(value);
                    Models = new List<IModelConfig>();
                    
                    foreach (var item in serializedModels)
                    {
                        string type = item.Type;
                        var data = item.Data;
                        
                        if (type == "Ollama")
                        {
                            var model = JsonConvert.DeserializeObject<OllamaModelConfig>(data.ToString());
                            Models.Add(model);
                        }
                        else if (type == "OpenAi")
                        {
                            var model = JsonConvert.DeserializeObject<OpenAIModelConfig>(data.ToString());
                            Models.Add(model);
                        }
                    }
                }
                catch
                {
                    Models = new List<IModelConfig>();
                }
            }
        }

        /// <summary>
        /// Id of configuration to use for autocompletion
        /// </summary>
        public string AutocompleteConfigId { get; set; } = null;

        /// <summary>
        /// Autocompletion configurations
        /// </summary>
        public List<AutocompleteConfig> AutocompletionConfigurations = new List<AutocompleteConfig>();

        /// <summary>
        /// Serialization for AutocompletionConfigurations. This is what is stored 
        /// in VS settings
        /// </summary>
        public string AutocompletionConfigurationsJson
        {
            get => JsonConvert.SerializeObject(AutocompletionConfigurations, Formatting.Indented);
            set => AutocompletionConfigurations = JsonConvert.DeserializeObject<List<AutocompleteConfig>>(value);
        }

        /// <summary>
        /// Model to use for autocomplete. It can be null if was not specified
        /// </summary>
        [Browsable(false)]
        public AutocompleteConfig AutocompleteConfig =>
            AutocompletionConfigurations.FirstOrDefault(a => a.Id == AutocompleteConfigId);

        /// <summary>
        /// Gets the Windows Forms control that hosts the custom options UI
        /// </summary>
        protected override IWin32Window Window
        {
            get
            {
                if (_control == null)
                {
                    _control = new SettingsUserControl();
                    _control.Initialize(this);
                }
                return _control;
            }
        }

        /// <summary>
        /// Called when the options page loads its state
        /// </summary>
        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();
            if (_control != null)
            {
                _control.Initialize(this);
            }
        }

        protected override void OnApply(PageApplyEventArgs e)
        {
            //if (InfillPrefixPercentage != null)
            //{
            //    if (InfillPrefixPercentage < 0 || InfillPrefixPercentage > 100)
            //    {
            //        e.ApplyBehavior = ApplyKind.Cancel;
            //        AutocompleteVsPackage.Instance.MessageBox(
            //            "'Prefix % when max. characters is reached' must to be between 0 and 100",
            //            "Error",
            //            Microsoft.VisualStudio.Shell.Interop.OLEMSGICON.OLEMSGICON_CRITICAL);
            //        return;
            //    }
            //}

            base.OnApply(e);

            // Update the ollama client
            AutocompletionsGenerator.Instance.ApplySettings(true);
            OutputPaneHandler.Instance.LogLevel = AutocompleteVsPackage.Instance.Settings.LogLevel;
        }

    }
}
