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
            this.cmbAutocompleteConfigId = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudNumTokensProgress = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkAutomaticSuggestions = new System.Windows.Forms.CheckBox();
            this.grpModels = new System.Windows.Forms.GroupBox();
            this.btnDeleteModel = new System.Windows.Forms.Button();
            this.btnEditModel = new System.Windows.Forms.Button();
            this.btnAddOpenAI = new System.Windows.Forms.Button();
            this.btnAddOllama = new System.Windows.Forms.Button();
            this.lstModels = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstConfigs = new System.Windows.Forms.ListBox();
            this.btnDeleteConfig = new System.Windows.Forms.Button();
            this.btnEditConfig = new System.Windows.Forms.Button();
            this.btnAddConfig = new System.Windows.Forms.Button();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).BeginInit();
            this.grpModels.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpGeneral.Controls.Add(this.cmbAutocompleteConfigId);
            this.grpGeneral.Controls.Add(this.label6);
            this.grpGeneral.Controls.Add(this.nudNumTokensProgress);
            this.grpGeneral.Controls.Add(this.label4);
            this.grpGeneral.Controls.Add(this.cmbLogLevel);
            this.grpGeneral.Controls.Add(this.label3);
            this.grpGeneral.Controls.Add(this.chkAutomaticSuggestions);
            this.grpGeneral.Location = new System.Drawing.Point(10, 10);
            this.grpGeneral.Name = "grpGeneral";
            this.grpGeneral.Size = new System.Drawing.Size(518, 120);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Settings";
            // 
            // cmbAutocompleteConfigId
            // 
            this.cmbAutocompleteConfigId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAutocompleteConfigId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutocompleteConfigId.FormattingEnabled = true;
            this.cmbAutocompleteConfigId.Location = new System.Drawing.Point(188, 36);
            this.cmbAutocompleteConfigId.Name = "cmbAutocompleteConfigId";
            this.cmbAutocompleteConfigId.Size = new System.Drawing.Size(324, 21);
            this.cmbAutocompleteConfigId.TabIndex = 2;
            this.cmbAutocompleteConfigId.SelectedIndexChanged += new System.EventHandler(this.cmbAutocompleteModelId_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(171, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Autocomplete config 1 (Ctrl + M, A)";
            // 
            // nudNumTokensProgress
            // 
            this.nudNumTokensProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudNumTokensProgress.Location = new System.Drawing.Point(188, 90);
            this.nudNumTokensProgress.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudNumTokensProgress.Name = "nudNumTokensProgress";
            this.nudNumTokensProgress.Size = new System.Drawing.Size(324, 20);
            this.nudNumTokensProgress.TabIndex = 6;
            this.nudNumTokensProgress.ValueChanged += new System.EventHandler(this.nudNumTokensProgress_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(120, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Progress each # tokens";
            // 
            // cmbLogLevel
            // 
            this.cmbLogLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Location = new System.Drawing.Point(188, 63);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(324, 21);
            this.cmbLogLevel.TabIndex = 4;
            this.cmbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLogLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Log level";
            // 
            // chkAutomaticSuggestions
            // 
            this.chkAutomaticSuggestions.AutoSize = true;
            this.chkAutomaticSuggestions.Location = new System.Drawing.Point(10, 19);
            this.chkAutomaticSuggestions.Name = "chkAutomaticSuggestions";
            this.chkAutomaticSuggestions.Size = new System.Drawing.Size(132, 17);
            this.chkAutomaticSuggestions.TabIndex = 0;
            this.chkAutomaticSuggestions.Text = "Automatic suggestions";
            this.chkAutomaticSuggestions.UseVisualStyleBackColor = true;
            this.chkAutomaticSuggestions.CheckedChanged += new System.EventHandler(this.chkAutomaticSuggestions_CheckedChanged);
            // 
            // grpModels
            // 
            this.grpModels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpModels.Controls.Add(this.btnDeleteModel);
            this.grpModels.Controls.Add(this.btnEditModel);
            this.grpModels.Controls.Add(this.btnAddOpenAI);
            this.grpModels.Controls.Add(this.btnAddOllama);
            this.grpModels.Controls.Add(this.lstModels);
            this.grpModels.Location = new System.Drawing.Point(10, 318);
            this.grpModels.Name = "grpModels";
            this.grpModels.Size = new System.Drawing.Size(518, 154);
            this.grpModels.TabIndex = 2;
            this.grpModels.TabStop = false;
            this.grpModels.Text = "Models";
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDeleteModel.Enabled = false;
            this.btnDeleteModel.Location = new System.Drawing.Point(246, 120);
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
            this.btnEditModel.Location = new System.Drawing.Point(181, 120);
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
            this.btnAddOpenAI.Location = new System.Drawing.Point(95, 120);
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
            this.btnAddOllama.Location = new System.Drawing.Point(10, 120);
            this.btnAddOllama.Name = "btnAddOllama";
            this.btnAddOllama.Size = new System.Drawing.Size(80, 25);
            this.btnAddOllama.TabIndex = 1;
            this.btnAddOllama.Text = "Add Ollama";
            this.btnAddOllama.UseVisualStyleBackColor = true;
            this.btnAddOllama.Click += new System.EventHandler(this.btnAddOllama_Click);
            // 
            // lstModels
            // 
            this.lstModels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstModels.DisplayMember = "DisplayText";
            this.lstModels.FormattingEnabled = true;
            this.lstModels.Location = new System.Drawing.Point(14, 19);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(498, 95);
            this.lstModels.TabIndex = 0;
            this.lstModels.SelectedIndexChanged += new System.EventHandler(this.lstModels_SelectedIndexChanged);
            this.lstModels.DoubleClick += new System.EventHandler(this.lstModels_DoubleClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.lstConfigs);
            this.groupBox1.Controls.Add(this.btnDeleteConfig);
            this.groupBox1.Controls.Add(this.btnEditConfig);
            this.groupBox1.Controls.Add(this.btnAddConfig);
            this.groupBox1.Location = new System.Drawing.Point(10, 136);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 176);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Autocompletion configurations";
            // 
            // lstConfigs
            // 
            this.lstConfigs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstConfigs.FormattingEnabled = true;
            this.lstConfigs.Location = new System.Drawing.Point(14, 20);
            this.lstConfigs.Name = "lstConfigs";
            this.lstConfigs.Size = new System.Drawing.Size(498, 121);
            this.lstConfigs.TabIndex = 0;
            this.lstConfigs.SelectedIndexChanged += new System.EventHandler(this.lstConfigs_SelectedIndexChanged);
            this.lstConfigs.DoubleClick += new System.EventHandler(this.lstConfigs_DoubleClick);
            // 
            // btnDeleteConfig
            // 
            this.btnDeleteConfig.Enabled = false;
            this.btnDeleteConfig.Location = new System.Drawing.Point(179, 147);
            this.btnDeleteConfig.Name = "btnDeleteConfig";
            this.btnDeleteConfig.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteConfig.TabIndex = 3;
            this.btnDeleteConfig.Text = "Delete";
            this.btnDeleteConfig.UseVisualStyleBackColor = true;
            this.btnDeleteConfig.Click += new System.EventHandler(this.btnDeleteConfig_Click);
            // 
            // btnEditConfig
            // 
            this.btnEditConfig.Enabled = false;
            this.btnEditConfig.Location = new System.Drawing.Point(96, 147);
            this.btnEditConfig.Name = "btnEditConfig";
            this.btnEditConfig.Size = new System.Drawing.Size(75, 23);
            this.btnEditConfig.TabIndex = 2;
            this.btnEditConfig.Text = "Edit";
            this.btnEditConfig.UseVisualStyleBackColor = true;
            this.btnEditConfig.Click += new System.EventHandler(this.btnEditConfig_Click);
            // 
            // btnAddConfig
            // 
            this.btnAddConfig.Location = new System.Drawing.Point(14, 147);
            this.btnAddConfig.Name = "btnAddConfig";
            this.btnAddConfig.Size = new System.Drawing.Size(75, 23);
            this.btnAddConfig.TabIndex = 1;
            this.btnAddConfig.Text = "Add";
            this.btnAddConfig.UseVisualStyleBackColor = true;
            this.btnAddConfig.Click += new System.EventHandler(this.btnAddConfig_Click);
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpModels);
            this.Controls.Add(this.grpGeneral);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(539, 486);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).EndInit();
            this.grpModels.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpGeneral;
        private System.Windows.Forms.CheckBox chkAutomaticSuggestions;
        private System.Windows.Forms.ComboBox cmbLogLevel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudNumTokensProgress;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbAutocompleteConfigId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpModels;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Button btnAddOllama;
        private System.Windows.Forms.Button btnAddOpenAI;
        private System.Windows.Forms.Button btnEditModel;
        private System.Windows.Forms.Button btnDeleteModel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEditConfig;
        private System.Windows.Forms.Button btnAddConfig;
        private System.Windows.Forms.Button btnDeleteConfig;
        private System.Windows.Forms.ListBox lstConfigs;
    }
}