namespace AutocompleteVs.Config
{
    partial class SettingsUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpGeneral = new System.Windows.Forms.GroupBox();
            this.cmbAutocompleteModelId = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudNumTokensProgress = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nudInfillPrefixPercentage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMaxPromptCharacters = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.chkAutomaticSuggestions = new System.Windows.Forms.CheckBox();
            this.grpModels = new System.Windows.Forms.GroupBox();
            this.btnDeleteModel = new System.Windows.Forms.Button();
            this.btnEditModel = new System.Windows.Forms.Button();
            this.btnAddOpenAI = new System.Windows.Forms.Button();
            this.btnAddOllama = new System.Windows.Forms.Button();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).BeginInit();
            this.grpModels.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.cmbAutocompleteModelId);
            this.grpGeneral.Controls.Add(this.label6);
            this.grpGeneral.Controls.Add(this.nudNumTokensProgress);
            this.grpGeneral.Controls.Add(this.label4);
            this.grpGeneral.Controls.Add(this.cmbLogLevel);
            this.grpGeneral.Controls.Add(this.label3);
            this.grpGeneral.Controls.Add(this.nudInfillPrefixPercentage);
            this.grpGeneral.Controls.Add(this.label2);
            this.grpGeneral.Controls.Add(this.nudMaxPromptCharacters);
            this.grpGeneral.Controls.Add(this.label1);
            this.grpGeneral.Controls.Add(this.chkAutomaticSuggestions);
            this.grpGeneral.Location = new System.Drawing.Point(10, 10);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(541, 163);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Settings";
            // 
            // cmbAutocompleteModelId
            // 
            this.cmbAutocompleteModelId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAutocompleteModelId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutocompleteModelId.FormattingEnabled = true;
            this.cmbAutocompleteModelId.Location = new System.Drawing.Point(295, 49);
            this.cmbAutocompleteModelId.Name = "cmbAutocompleteModelId";
            this.cmbAutocompleteModelId.Size = new System.Drawing.Size(236, 21);
            this.cmbAutocompleteModelId.TabIndex = 12;
            this.cmbAutocompleteModelId.SelectedIndexChanged += new System.EventHandler(this.cmbAutocompleteModelId_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(295, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Model for autocompletion:";
            // 
            // nudNumTokensProgress
            // 
            this.nudNumTokensProgress.Location = new System.Drawing.Point(170, 134);
            this.nudNumTokensProgress.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudNumTokensProgress.Name = "nudNumTokensProgress";
            this.nudNumTokensProgress.Size = new System.Drawing.Size(100, 20);
            this.nudNumTokensProgress.TabIndex = 8;
            this.nudNumTokensProgress.ValueChanged += new System.EventHandler(this.nudNumTokensProgress_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Progress each # tokens:";
            // 
            // cmbLogLevel
            // 
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Location = new System.Drawing.Point(170, 107);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(197, 21);
            this.cmbLogLevel.TabIndex = 6;
            this.cmbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLogLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Log level:";
            // 
            // nudInfillPrefixPercentage
            // 
            this.nudInfillPrefixPercentage.DecimalPlaces = 1;
            this.nudInfillPrefixPercentage.Location = new System.Drawing.Point(170, 81);
            this.nudInfillPrefixPercentage.Name = "nudInfillPrefixPercentage";
            this.nudInfillPrefixPercentage.Size = new System.Drawing.Size(100, 20);
            this.nudInfillPrefixPercentage.TabIndex = 4;
            this.nudInfillPrefixPercentage.ValueChanged += new System.EventHandler(this.nudInfillPrefixPercentage_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Prefix % when max reached:";
            // 
            // nudMaxPromptCharacters
            // 
            this.nudMaxPromptCharacters.Location = new System.Drawing.Point(170, 55);
            this.nudMaxPromptCharacters.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.nudMaxPromptCharacters.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudMaxPromptCharacters.Name = "nudMaxPromptCharacters";
            this.nudMaxPromptCharacters.Size = new System.Drawing.Size(100, 20);
            this.nudMaxPromptCharacters.TabIndex = 2;
            this.nudMaxPromptCharacters.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudMaxPromptCharacters.ValueChanged += new System.EventHandler(this.nudMaxPromptCharacters_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Max. characters in prompt:";
            // 
            // chkAutomaticSuggestions
            // 
            this.chkAutomaticSuggestions.AutoSize = true;
            this.chkAutomaticSuggestions.Location = new System.Drawing.Point(10, 25);
            this.chkAutomaticSuggestions.Name = "chkAutomaticSuggestions";
            this.chkAutomaticSuggestions.Size = new System.Drawing.Size(132, 17);
            this.chkAutomaticSuggestions.TabIndex = 0;
            this.chkAutomaticSuggestions.Text = "Automatic suggestions";
            this.chkAutomaticSuggestions.UseVisualStyleBackColor = true;
            this.chkAutomaticSuggestions.CheckedChanged += new System.EventHandler(this.chkAutomaticSuggestions_CheckedChanged);
            // 
            // grpModels
            // 
            this.grpModels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpModels.Controls.Add(this.btnDeleteModel);
            this.grpModels.Controls.Add(this.btnEditModel);
            this.grpModels.Controls.Add(this.btnAddOpenAI);
            this.grpModels.Controls.Add(this.btnAddOllama);
            this.grpModels.Controls.Add(this.lstModels);
            this.grpModels.Location = new System.Drawing.Point(10, 179);
            this.grpModels.Name = "grpModels";
            this.grpModels.Size = new System.Drawing.Size(541, 228);
            this.grpModels.TabIndex = 1;
            this.grpModels.TabStop = false;
            this.grpModels.Text = "Models";
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteModel.Enabled = false;
            this.btnDeleteModel.Location = new System.Drawing.Point(246, 194);
            this.btnDeleteModel.Name = "btnDeleteModel";
            this.btnDeleteModel.Size = new System.Drawing.Size(60, 25);
            this.btnDeleteModel.TabIndex = 4;
            this.btnDeleteModel.Text = "Delete";
            this.btnDeleteModel.UseVisualStyleBackColor = true;
            this.btnDeleteModel.Click += new System.EventHandler(this.btnDeleteModel_Click);
            // 
            // btnEditModel
            // 
            this.btnEditModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEditModel.Enabled = false;
            this.btnEditModel.Location = new System.Drawing.Point(181, 194);
            this.btnEditModel.Name = "btnEditModel";
            this.btnEditModel.Size = new System.Drawing.Size(60, 25);
            this.btnEditModel.TabIndex = 3;
            this.btnEditModel.Text = "Edit";
            this.btnEditModel.UseVisualStyleBackColor = true;
            this.btnEditModel.Click += new System.EventHandler(this.btnEditModel_Click);
            // 
            // btnAddOpenAI
            // 
            this.btnAddOpenAI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddOpenAI.Location = new System.Drawing.Point(95, 194);
            this.btnAddOpenAI.Name = "btnAddOpenAI";
            this.btnAddOpenAI.Size = new System.Drawing.Size(80, 25);
            this.btnAddOpenAI.TabIndex = 2;
            this.btnAddOpenAI.Text = "Add OpenAI";
            this.btnAddOpenAI.UseVisualStyleBackColor = true;
            this.btnAddOpenAI.Click += new System.EventHandler(this.btnAddOpenAI_Click);
            // 
            // btnAddOllama
            // 
            this.btnAddOllama.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddOllama.Location = new System.Drawing.Point(10, 194);
            this.btnAddOllama.Name = "btnAddOllama";
            this.btnAddOllama.Size = new System.Drawing.Size(80, 25);
            this.btnAddOllama.TabIndex = 1;
            this.btnAddOllama.Text = "Add Ollama";
            this.btnAddOllama.UseVisualStyleBackColor = true;
            this.btnAddOllama.Click += new System.EventHandler(this.btnAddOllama_Click);
            // 
            // lstModels
            // 
            this.lstModels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstModels.DisplayMember = "DisplayText";
            this.lstModels.FormattingEnabled = true;
            this.lstModels.Location = new System.Drawing.Point(10, 25);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(510, 160);
            this.lstModels.TabIndex = 0;
            this.lstModels.SelectedIndexChanged += new System.EventHandler(this.lstModels_SelectedIndexChanged);
            this.lstModels.DoubleClick += new System.EventHandler(this.lstModels_DoubleClick);
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.grpModels);
            this.Controls.Add(this.grpGeneral);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(562, 414);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).EndInit();
            this.grpModels.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.CheckBox chkAutomaticSuggestions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudMaxPromptCharacters;
        private System.Windows.Forms.NumericUpDown nudInfillPrefixPercentage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbLogLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudNumTokensProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbAutocompleteModelId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpModels;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Button btnAddOllama;
        private System.Windows.Forms.Button btnAddOpenAI;
        private System.Windows.Forms.Button btnEditModel;
        private System.Windows.Forms.Button btnDeleteModel;
    }
}