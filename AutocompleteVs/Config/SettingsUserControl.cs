using AutocompleteVs.Logging;
using AutocompleteVs.SuggestionGeneration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace AutocompleteVs.Config
{
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
            UpdateAutocompleteModelCombo();
            _initializing = false;
        }

        private void LoadSettings()
        {
            if (_settings == null) return;

            chkAutomaticSuggestions.Checked = _settings.AutomaticSuggestions;
            nudMaxPromptCharacters.Value = _settings.MaxPromptCharacters ?? 2048;
            nudInfillPrefixPercentage.Value = (decimal)(_settings.InfillPrefixPercentage ?? 75.0);
            cmbLogLevel.SelectedIndex = (int)_settings.LogLevel;
            nudNumTokensProgress.Value = _settings.NumTokensProgress;
        }

        private void UpdateSettings()
        {
            if (_settings == null || _initializing) return;

            _settings.AutomaticSuggestions = chkAutomaticSuggestions.Checked;
            _settings.MaxPromptCharacters = (int)nudMaxPromptCharacters.Value;
            _settings.InfillPrefixPercentage = (double)nudInfillPrefixPercentage.Value;
            _settings.LogLevel = (LogLevel)cmbLogLevel.SelectedIndex;
            _settings.NumTokensProgress = (int)nudNumTokensProgress.Value;

            if (cmbAutocompleteModelId.SelectedItem is ModelDisplayItem selectedModel)
            {
                _settings.AutocompleteModelId = selectedModel.Id;
            }
        }

        private void UpdateModelList()
        {
            lstModels.Items.Clear();
            foreach (var model in _settings.Models)
            {
                lstModels.Items.Add(new ModelDisplayItem(model));
            }
        }

        private void UpdateAutocompleteModelCombo()
        {
            cmbAutocompleteModelId.Items.Clear();
            var none = new ModelDisplayItem(null);
            cmbAutocompleteModelId.Items.Add(none); // None option

            foreach (var model in _settings.Models)
            {
                cmbAutocompleteModelId.Items.Add(new ModelDisplayItem(model));
            }

            // Select current model
            bool found = false;
            for (int i = 0; i < cmbAutocompleteModelId.Items.Count; i++)
            {
                if (cmbAutocompleteModelId.Items[i] is ModelDisplayItem item && 
                    item.Id == _settings.AutocompleteModelId)
                {
                    cmbAutocompleteModelId.SelectedIndex = i;
                    found = true;
                    break;
                }
            }
            if(!found)
            {
                cmbAutocompleteModelId.SelectedItem = none;
            }
        }


        // Event handlers that will be connected in the designer
        private void chkAutomaticSuggestions_CheckedChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudMaxPromptCharacters_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudInfillPrefixPercentage_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbLogLevel_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudNumTokensProgress_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbAutocompleteModelId_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();

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
                    if (ValidateModelId(dialog.Model.Id, false))
                    {
                        _settings.Models.Add(dialog.Model);
                        UpdateModelList();
                        UpdateAutocompleteModelCombo();
                    }
                }
            }
        }

        private void btnAddOpenAI_Click(object sender, EventArgs e)
        {
            using (var dialog = new ModelSettingsDialog(new OpenAIModelConfig { Id = "New OpenAI Model" }, false))
            {
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    if (ValidateModelId(dialog.Model.Id, false))
                    {
                        _settings.Models.Add(dialog.Model);
                        UpdateModelList();
                        UpdateAutocompleteModelCombo();
                    }
                }
            }
        }

        private void btnEditModel_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem is ModelDisplayItem selectedItem && selectedItem.Model != null)
            {
                bool isCurrentAutocompletionModel = 
                    selectedItem.Model.Id == _settings.AutocompleteModelId;
                using (var dialog = new ModelSettingsDialog(selectedItem.Model, true))
                {
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (ValidateModelId(dialog.Model.Id, true, selectedItem.Model))
                        {
                            var index = _settings.Models.IndexOf(selectedItem.Model);
                            if (index >= 0)
                            {
                                if(isCurrentAutocompletionModel)
                                {
                                    // Be sure selected model id is right
                                    _settings.AutocompleteModelId = dialog.Model.Id;
                                }
                                _settings.Models[index] = dialog.Model;
                                UpdateModelList();
                                UpdateAutocompleteModelCombo();
                            }
                        }
                    }
                }
            }
        }

        private void lstModels_DoubleClick(object sender, EventArgs e) =>
            btnEditModel_Click(sender, e);
        
        private void btnDeleteModel_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem is ModelDisplayItem selectedItem && selectedItem.Model != null)
            {
                var result = MessageBox.Show($"Are you sure you want to delete the model '{selectedItem.Model.Id}'?", 
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
                if (result == DialogResult.Yes)
                {
                    _settings.Models.Remove(selectedItem.Model);
                    UpdateModelList();
                    UpdateAutocompleteModelCombo();
                }
            }
        }


        private bool ValidateModelId(string modelId, bool isEditing, IModelConfig existingModel = null)
        {
            if (string.IsNullOrWhiteSpace(modelId))
            {
                MessageBox.Show("Model ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Check for duplicate IDs (except when editing the same model)
            if (!isEditing || (existingModel != null && existingModel.Id != modelId))
            {
                if (_settings.Models.Any(m => m.Id == modelId))
                {
                    MessageBox.Show("A model with this ID already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        private class ModelDisplayItem
        {
            public IModelConfig Model { get; }
            public string Id => Model?.Id ?? "";

            public override string ToString() => Model == null ? "(None)" : Model.ToString();

            public ModelDisplayItem(IModelConfig model)
            {
                Model = model;
            }
        }

    }
}