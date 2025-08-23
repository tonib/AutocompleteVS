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
        private IModelConfig _currentEditingModel;
        private bool _isEditingExistingModel = false;

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
            txtCustomServerUrl.Text = _settings.CustomServerUrl ?? "";
        }

        private void UpdateSettings()
        {
            if (_settings == null || _initializing) return;

            _settings.AutomaticSuggestions = chkAutomaticSuggestions.Checked;
            _settings.MaxPromptCharacters = (int)nudMaxPromptCharacters.Value;
            _settings.InfillPrefixPercentage = (double)nudInfillPrefixPercentage.Value;
            _settings.LogLevel = (LogLevel)cmbLogLevel.SelectedIndex;
            _settings.NumTokensProgress = (int)nudNumTokensProgress.Value;
            _settings.CustomServerUrl = txtCustomServerUrl.Text;

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
            cmbAutocompleteModelId.Items.Add(new ModelDisplayItem(null)); // None option

            foreach (var model in _settings.Models)
            {
                cmbAutocompleteModelId.Items.Add(new ModelDisplayItem(model));
            }

            // Select current model
            for (int i = 0; i < cmbAutocompleteModelId.Items.Count; i++)
            {
                if (cmbAutocompleteModelId.Items[i] is ModelDisplayItem item && 
                    item.Id == _settings.AutocompleteModelId)
                {
                    cmbAutocompleteModelId.SelectedIndex = i;
                    break;
                }
            }
        }

        private void ShowModelEditor(IModelConfig model, bool isEditing = false)
        {
            _currentEditingModel = model;
            _isEditingExistingModel = isEditing;
            
            txtModelId.Text = model.Id;
            
            if (model is OllamaModelConfig ollamaModel)
            {
                LoadOllamaModel(ollamaModel);
                pnlOllamaSettings.Visible = true;
                pnlOpenAISettings.Visible = false;
            }
            else if (model is OpenAIModelConfig openAiModel)
            {
                LoadOpenAIModel(openAiModel);
                pnlOllamaSettings.Visible = false;
                pnlOpenAISettings.Visible = true;
            }

            grpModelEditor.Visible = true;
        }

        private void LoadOllamaModel(OllamaModelConfig model)
        {
            txtOllamaUrl.Text = model.OllamaUrl ?? "";
            txtOllamaModelName.Text = model.ModelName ?? "";
            nudTopK.Value = model.TopK ?? 1;
            nudTopP.Value = (decimal)(model.TopP ?? 0.9);
            nudTemperature.Value = (decimal)(model.Temperature ?? 0.7);
            nudNumCtx.Value = model.NumCtx ?? 2048;
            txtKeepAlive.Text = model.KeepAlive ?? "";
            chkIsInfillModel.Checked = model.IsInfillModel;
        }

        private void LoadOpenAIModel(OpenAIModelConfig model)
        {
            txtOpenAiKey.Text = model.OpenAiKey ?? "";
            txtOpenAiModelName.Text = model.ModelName ?? "";
        }

        private OllamaModelConfig SaveOllamaModel()
        {
            return new OllamaModelConfig
            {
                Id = txtModelId.Text,
                OllamaUrl = txtOllamaUrl.Text,
                ModelName = txtOllamaModelName.Text,
                TopK = (int?)nudTopK.Value,
                TopP = (float?)nudTopP.Value,
                Temperature = (float?)nudTemperature.Value,
                NumCtx = (int?)nudNumCtx.Value,
                KeepAlive = txtKeepAlive.Text,
                IsInfillModel = chkIsInfillModel.Checked
            };
        }

        private OpenAIModelConfig SaveOpenAIModel()
        {
            return new OpenAIModelConfig
            {
                Id = txtModelId.Text,
                OpenAiKey = txtOpenAiKey.Text,
                ModelName = txtOpenAiModelName.Text
            };
        }

        // Event handlers that will be connected in the designer
        private void chkAutomaticSuggestions_CheckedChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudMaxPromptCharacters_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudInfillPrefixPercentage_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbLogLevel_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();
        private void nudNumTokensProgress_ValueChanged(object sender, EventArgs e) => UpdateSettings();
        private void txtCustomServerUrl_TextChanged(object sender, EventArgs e) => UpdateSettings();
        private void cmbAutocompleteModelId_SelectedIndexChanged(object sender, EventArgs e) => UpdateSettings();

        private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnEditModel.Enabled = lstModels.SelectedItem != null;
            btnDeleteModel.Enabled = lstModels.SelectedItem != null;
        }

        private void btnAddOllama_Click(object sender, EventArgs e)
        {
            ShowModelEditor(new OllamaModelConfig { Id = "New Ollama Model" });
        }

        private void btnAddOpenAI_Click(object sender, EventArgs e)
        {
            ShowModelEditor(new OpenAIModelConfig { Id = "New OpenAI Model" });
        }

        private void btnEditModel_Click(object sender, EventArgs e)
        {
            if (lstModels.SelectedItem is ModelDisplayItem selectedItem && selectedItem.Model != null)
            {
                ShowModelEditor(selectedItem.Model, true);
            }
        }

        private void btnSaveModel_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModelId.Text))
            {
                MessageBox.Show("Model ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check for duplicate IDs (except when editing the same model)
            if (!_isEditingExistingModel && _settings.Models.Any(m => m.Id == txtModelId.Text))
            {
                MessageBox.Show("A model with this ID already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            IModelConfig newModel;
            
            if (pnlOllamaSettings.Visible)
            {
                newModel = SaveOllamaModel();
            }
            else if (pnlOpenAISettings.Visible)
            {
                newModel = SaveOpenAIModel();
            }
            else
            {
                return;
            }

            if (_isEditingExistingModel)
            {
                var index = _settings.Models.IndexOf(_currentEditingModel);
                if (index >= 0)
                {
                    _settings.Models[index] = newModel;
                }
            }
            else
            {
                _settings.Models.Add(newModel);
            }

            UpdateModelList();
            UpdateAutocompleteModelCombo();
            grpModelEditor.Visible = false;
        }

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

        private void btnCancelModelEdit_Click(object sender, EventArgs e)
        {
            grpModelEditor.Visible = false;
        }

        private class ModelDisplayItem
        {
            public IModelConfig Model { get; }
            public string Id => Model?.Id ?? "";
            public string DisplayText => Model == null ? "(None)" : $"{Model.Id} ({Model.Type})";

            public ModelDisplayItem(IModelConfig model)
            {
                Model = model;
            }
        }
    }
}