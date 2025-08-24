using OllamaSharp.Models;
using System;
using System.Linq;
using System.Windows.Forms;

namespace AutocompleteVs.Config
{
    public partial class ModelSettingsDialog : Form
    {
        private IModelConfig _model;
        private bool _isEditingExistingModel = false;

        public IModelConfig Model => _model;

        public ModelSettingsDialog()
        {
            InitializeComponent();
        }

        public ModelSettingsDialog(IModelConfig model, bool isEditing = false) : this()
        {
            _isEditingExistingModel = isEditing;
            LoadModel(model);
        }

        private void LoadModel(IModelConfig model)
        {
            _model = model;
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
        }

        private void SetValue(NumericUpDown field, float? value)
        {
            if (value != null)
                field.Value = (decimal)value;
            else
                field.Text = "";
        }

        private void LoadOllamaModel(OllamaModelConfig model)
        {
            txtOllamaUrl.Text = model.OllamaUrl ?? "";
            txtOllamaModelName.Text = model.ModelName ?? "";

            SetValue(nudTopK, model.TopK);
            SetValue(nudTopP, model.TopP);
            SetValue(nudTemperature, model.Temperature);
            SetValue(nudNumCtx, model.NumCtx);
            SetValue(numSeed, model.Seed);
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
                TopK = nudTopK.Text == "" ? null : (int?)nudTopK.Value,
                TopP = nudTopP.Text == "" ? null : (float?)nudTopP.Value,
                Temperature = nudTemperature.Text == "" ? null : (float?)nudTemperature.Value,
                NumCtx = nudNumCtx.Text == "" ? null : (int?)nudNumCtx.Value,
                KeepAlive = string.IsNullOrWhiteSpace(txtKeepAlive.Text) ? null : txtKeepAlive.Text,
                IsInfillModel = chkIsInfillModel.Checked,
                Seed = numSeed.Text == "" ? null : (int?)numSeed.Value
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtModelId.Text))
            {
                MessageBox.Show("Model ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string errorMsg = OllamaValidation() ?? OpenAIValidation();
            if (errorMsg != null)
            {
                MessageBox.Show(errorMsg, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (pnlOllamaSettings.Visible)
            {
                _model = SaveOllamaModel();
            }
            else if (pnlOpenAISettings.Visible)
            {
                _model = SaveOpenAIModel();
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private string OpenAIValidation()
        {
            if (!(_model is OpenAIModelConfig))
            {
                return null;
            }
            if(string.IsNullOrWhiteSpace(txtOpenAiKey.Text))
            {
                return "OpenAI API key is required.";
            }
            if(string.IsNullOrWhiteSpace(txtOpenAiModelName.Text))
            {
                return "Model name is required.";
            }
            return null;
        }

        private string OllamaValidation()
        {
            if (!(_model is OllamaModelConfig))
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(txtOllamaModelName.Text))
            {
                return "Model name is required.";
            }
            if (string.IsNullOrWhiteSpace(txtOllamaUrl.Text))
            {
                return "Ollama URL is required.";
            }

            return null;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}