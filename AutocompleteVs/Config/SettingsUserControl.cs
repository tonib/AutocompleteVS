using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AutocompleteVs.Config
{
    /// <summary>
    /// User control to edit extension settings in VS > Tools > Settings
    /// </summary>
    public partial class SettingsUserControl : UserControl
    {
        private Settings _settings;
        private bool _initializing = false;

        public SettingsUserControl()
        {
            InitializeComponent();
            
            // Load LogLevel enum values into combo box
            cmbLogLevel.Items.Clear();
            foreach (LogLevel logLevel in Enum.GetValues(typeof(LogLevel)))
            {
                cmbLogLevel.Items.Add(logLevel.ToString());
            }
        }

        public void Initialize(Settings settings)
        {
            _initializing = true;
            _settings = settings;
            LoadSettings();
            
            UpdateModelList();

            UpdateConfigsList();
            UpdateAutocompleteConfigCombo();

            _initializing = false;
        }

        private void LoadSettings()
        {
            if (_settings == null) return;

            chkAutomaticSuggestions.Checked = _settings.AutomaticSuggestions;
            cmbLogLevel.SelectedIndex = (int)_settings.LogLevel;
            nudNumTokensProgress.Value = _settings.NumTokensProgress;
        }

        private void UpdateSettings()
        {
            if (_settings == null || _initializing) return;

            _settings.AutomaticSuggestions = chkAutomaticSuggestions.Checked;
            _settings.LogLevel = (LogLevel)cmbLogLevel.SelectedIndex;
            _settings.NumTokensProgress = (int)nudNumTokensProgress.Value;

            if (cmbAutocompleteConfigId.SelectedItem is AutcocompleteConfigDisplayItem selectedModel)
            {
                _settings.AutocompleteConfigId = selectedModel.Id;
            }
        }

        // Event handlers that will be connected in the designer
        private void chkAutomaticSuggestions_CheckedChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbLogLevel_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudNumTokensProgress_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbAutocompleteModelId_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();

        #region Models CRUD

        private void UpdateModelList()
        {
            lstModels.Items.Clear();
            foreach (var model in _settings.Models)
            {
                lstModels.Items.Add(model);
            }
        }

        private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditModel.Enabled = lstModels.SelectedItem != null;
            btnDeleteModel.Enabled = lstModels.SelectedItem != null;
        }

        private void btnAddOllama_Click(object sender, EventArgs e)
        {
            using (var dialog = new ModelSettingsDialog(new OllamaModelConfig { Id = "New Ollama Model" }, false))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _settings.Models.Add(dialog.Model);
                    UpdateModelList();
                }
            }
        }

        private void btnAddOpenAI_Click(object sender, EventArgs e)
        {
            using (var dialog = new ModelSettingsDialog(new OpenAIModelConfig { Id = "New OpenAI Model" }, false))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _settings.Models.Add(dialog.Model);
                    UpdateModelList();
                }
            }
        }

        private void btnEditModel_Click(object sender, EventArgs e)
        {
            IModelConfig model = lstModels.SelectedItem as IModelConfig;
            if (model == null)
            {
                return;
            }

            using (var dialog = new ModelSettingsDialog(model, true))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;
                
                var index = _settings.Models.IndexOf(model);
                if (index >= 0)
                {
                    _settings.Models[index] = dialog.Model;
                    UpdateModelList();

                    // Update model id in autocomplete configs
                    foreach (AutocompleteConfig c in _settings.AutocompletionConfigurations.Where(a => a.ModelConfigId == model.Id))
                    {
                        c.ModelConfigId = dialog.Model.Id;
                    }
                    UpdateConfigsList();
                }
            }
            
        }

        private void lstModels_DoubleClick(object sender, EventArgs e) =>
            btnEditModel_Click(sender, e);
        
        private void btnDeleteModel_Click(object sender, EventArgs e)
        {
            IModelConfig model = lstModels.SelectedItem as IModelConfig;
            if (model == null)
                return;

            var cfg = _settings.AutocompletionConfigurations
                .Where(a => a.ModelConfigId == model.Id)
                .FirstOrDefault();
            if(cfg != null)
            {
                MessageBox.Show($"Can't delete the model because it is used in '{cfg.Id}' " +
                    $"autocompletion configuration", "Validation Error", MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete the model " +
                $"'{model.Id}'?", 
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                _settings.Models.Remove(model);
                UpdateModelList();
            }
        }

        #endregion

        #region Autocomplete settings CRUD

        private void UpdateConfigsList()
        {
            lstConfigs.Items.Clear();
            foreach (AutocompleteConfig cfg in _settings.AutocompletionConfigurations)
            {
                lstConfigs.Items.Add(cfg);
            }
        }

        private void UpdateAutocompleteConfigCombo()
        {
            cmbAutocompleteConfigId.Items.Clear();
            var none = new AutcocompleteConfigDisplayItem(null);
            cmbAutocompleteConfigId.Items.Add(none); // None option

            foreach (var cfg in _settings.AutocompletionConfigurations)
            {
                cmbAutocompleteConfigId.Items.Add(new AutcocompleteConfigDisplayItem(cfg));
            }

            // Select current configuration
            AutcocompleteConfigDisplayItem item = cmbAutocompleteConfigId.Items
                .Cast<AutcocompleteConfigDisplayItem>()
                .FirstOrDefault(i => i.Id == _settings.AutocompleteConfigId);
            if (item == null)
                item = none;
            cmbAutocompleteConfigId.SelectedItem = item;
        }

        private class AutcocompleteConfigDisplayItem
        {
            public AutocompleteConfig AutocompleteConfig { get; }
            public string Id => AutocompleteConfig?.Id ?? "";

            public override string ToString() => AutocompleteConfig == null ? 
                "(None)" : AutocompleteConfig.ToString();

            public AutcocompleteConfigDisplayItem(AutocompleteConfig config)
            {
                AutocompleteConfig = config;
            }
        }

        private void btnAddConfig_Click(object sender, EventArgs e)
        {
            var cfg = new AutocompleteConfig() { Id = "New autocompletion configuration" };
            if(_settings.Models.Count > 0)
                cfg.ModelConfigId = _settings.Models[0].Id;

            using (var dialog = new AutocompleteConfigDialog(cfg, false))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                _settings.AutocompletionConfigurations.Add(dialog.Config);
                UpdateConfigsList();
                UpdateAutocompleteConfigCombo();
            }
        }

        private void btnEditConfig_Click(object sender, EventArgs e)
        {
            var cfg = lstConfigs.SelectedItem as AutocompleteConfig;
            if (cfg == null)
                return;

            using (var dialog = new AutocompleteConfigDialog(cfg, true))
            {
                if (dialog.ShowDialog(this) != DialogResult.OK)
                    return;

                var index = _settings.AutocompletionConfigurations.IndexOf(cfg);
                if(index >= 0)
                {
                    _settings.AutocompletionConfigurations[index] = dialog.Config;
                    UpdateConfigsList();

                    // If current selected autocomplete config was this, udpate it
                    if(_settings.AutocompleteConfigId == cfg.Id)
                        _settings.AutocompleteConfigId = dialog.Config.Id;

                    UpdateAutocompleteConfigCombo();
                }
            }
        }

        private void lstConfigs_DoubleClick(object sender, EventArgs e) =>
            btnEditConfig_Click(sender, e);

        private void btnDeleteConfig_Click(object sender, EventArgs e)
        {
            var cfg = lstConfigs.SelectedItem as AutocompleteConfig;
            if (cfg == null)
                return;

            var result = MessageBox.Show($"Are you sure you want to delete the configuration " +
                $"'{cfg.Id}'?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _settings.AutocompletionConfigurations.Remove(cfg);
                UpdateConfigsList();
                UpdateAutocompleteConfigCombo();
            }
        }

        private void lstConfigs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedItem = lstConfigs.SelectedItem;
            btnDeleteConfig.Enabled = btnEditConfig.Enabled =
                selectedItem != null;

            btnMoveUp.Enabled = lstConfigs.SelectedIndex >= 1;
            btnMoveDown.Enabled = selectedItem != null &&
                lstConfigs.SelectedIndex < (lstModels.Items.Count - 1);
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (lstConfigs.SelectedIndex < 1)
                return;

            int selIndex = lstConfigs.SelectedIndex;
            var item = _settings.AutocompletionConfigurations[selIndex];
            _settings.AutocompletionConfigurations[selIndex] =
                _settings.AutocompletionConfigurations[selIndex - 1];
            _settings.AutocompletionConfigurations[selIndex - 1] = item;
            UpdateConfigsList();
            lstConfigs.SelectedItem = item;
        }

        private void btnMoveDown_Click(object sender, EventArgs e)
        {
            if (lstConfigs.SelectedIndex > (lstConfigs.Items.Count - 1))
                return;

            int selIndex = lstConfigs.SelectedIndex;
            var item = _settings.AutocompletionConfigurations[selIndex];
            _settings.AutocompletionConfigurations[selIndex] =
                _settings.AutocompletionConfigurations[selIndex + 1];
            _settings.AutocompletionConfigurations[selIndex + 1] = item;
            UpdateConfigsList();
            lstConfigs.SelectedItem = item;
        }

        #endregion

    }
}