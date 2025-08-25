using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocompleteVs.Config
{
    public partial class AutocompleteConfigDialog : Form
    {
        public AutocompleteConfig Config;

        protected AutocompleteConfigDialog()
        {
            InitializeComponent();

            var models = AutocompleteVsPackage.Instance.Settings.Models;
            foreach (var model in models)
            {
                cmbAutocompleteModelId.Items.Add(model);
            }

            nudMaxPromptCharacters.Maximum = decimal.MaxValue;
        }

        public AutocompleteConfigDialog(AutocompleteConfig config, bool update) : this()
        {
            Config = config;

            txtId.Text = config.Id;
            cmbAutocompleteModelId.SelectedItem = config.ModelConfig;

            ModelSettingsDialog.SetValue(nudMaxPromptCharacters, config.MaxPromptCharacters);
            ModelSettingsDialog.SetValue(nudInfillPrefixPercentage, config.InfillPrefixPercentage);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string id = txtId.Text;
            if (string.IsNullOrWhiteSpace(id))
            {
                MessageBox.Show("Model ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            IModelConfig selectedModel = cmbAutocompleteModelId.SelectedItem as IModelConfig;
            if (selectedModel == null)
            {
                MessageBox.Show("Model is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (AutocompleteVsPackage.Instance.Settings.AutocompletionConfigurations
                .Any(c => c.Id == id && c != Config))
            {
                MessageBox.Show("A configuration with this ID already exists.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Config = new AutocompleteConfig()
            {
                Id = txtId.Text,
                ModelConfigId = selectedModel.Id,
                InfillPrefixPercentage = string.IsNullOrWhiteSpace(nudInfillPrefixPercentage.Text) ? null : (float?)nudInfillPrefixPercentage.Value,
                MaxPromptCharacters = string.IsNullOrWhiteSpace(nudMaxPromptCharacters.Text) ? null : (int?)nudMaxPromptCharacters.Value
            };

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
