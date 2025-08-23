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
            this.txtCustomServerUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
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
            this.grpModelEditor = new System.Windows.Forms.GroupBox();
            this.btnCancelModelEdit = new System.Windows.Forms.Button();
            this.btnSaveModel = new System.Windows.Forms.Button();
            this.pnlOpenAISettings = new System.Windows.Forms.Panel();
            this.txtOpenAiModelName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOpenAiKey = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pnlOllamaSettings = new System.Windows.Forms.Panel();
            this.chkIsInfillModel = new System.Windows.Forms.CheckBox();
            this.txtKeepAlive = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.nudNumCtx = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.nudTemperature = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.nudTopP = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.nudTopK = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.txtOllamaModelName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtOllamaUrl = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtModelId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.grpGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).BeginInit();
            this.grpModels.SuspendLayout();
            this.grpModelEditor.SuspendLayout();
            this.pnlOpenAISettings.SuspendLayout();
            this.pnlOllamaSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCtx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopK)).BeginInit();
            this.SuspendLayout();
            // 
            // grpGeneral
            // 
            this.grpGeneral.Controls.Add(this.cmbAutocompleteModelId);
            this.grpGeneral.Controls.Add(this.label6);
            this.grpGeneral.Controls.Add(this.txtCustomServerUrl);
            this.grpGeneral.Controls.Add(this.label5);
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
            this.grpGeneral.Size = new System.Drawing.Size(580, 200);
            this.grpGeneral.TabIndex = 0;
            this.grpGeneral.TabStop = false;
            this.grpGeneral.Text = "General Settings";
            // 
            // cmbAutocompleteModelId
            // 
            this.cmbAutocompleteModelId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutocompleteModelId.FormattingEnabled = true;
            this.cmbAutocompleteModelId.Location = new System.Drawing.Point(300, 135);
            this.cmbAutocompleteModelId.Name = "cmbAutocompleteModelId";
            this.cmbAutocompleteModelId.Size = new System.Drawing.Size(250, 21);
            this.cmbAutocompleteModelId.TabIndex = 12;
            this.cmbAutocompleteModelId.SelectedIndexChanged += new System.EventHandler(this.cmbAutocompleteModelId_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(300, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Autocomplete model:";
            // 
            // txtCustomServerUrl
            // 
            this.txtCustomServerUrl.Location = new System.Drawing.Point(300, 75);
            this.txtCustomServerUrl.Name = "txtCustomServerUrl";
            this.txtCustomServerUrl.Size = new System.Drawing.Size(250, 20);
            this.txtCustomServerUrl.TabIndex = 10;
            this.txtCustomServerUrl.TextChanged += new System.EventHandler(this.txtCustomServerUrl_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Custom server URL:";
            // 
            // nudNumTokensProgress
            // 
            this.nudNumTokensProgress.Location = new System.Drawing.Point(160, 145);
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
            this.label4.Location = new System.Drawing.Point(10, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Progress each # tokens:";
            // 
            // cmbLogLevel
            // 
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Location = new System.Drawing.Point(100, 115);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(120, 21);
            this.cmbLogLevel.TabIndex = 6;
            this.cmbLogLevel.SelectedIndexChanged += new System.EventHandler(this.cmbLogLevel_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Log level:";
            // 
            // nudInfillPrefixPercentage
            // 
            this.nudInfillPrefixPercentage.DecimalPlaces = 1;
            this.nudInfillPrefixPercentage.Location = new System.Drawing.Point(170, 85);
            this.nudInfillPrefixPercentage.Name = "nudInfillPrefixPercentage";
            this.nudInfillPrefixPercentage.Size = new System.Drawing.Size(100, 20);
            this.nudInfillPrefixPercentage.TabIndex = 4;
            this.nudInfillPrefixPercentage.ValueChanged += new System.EventHandler(this.nudInfillPrefixPercentage_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 85);
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
            this.grpModels.Controls.Add(this.btnDeleteModel);
            this.grpModels.Controls.Add(this.btnEditModel);
            this.grpModels.Controls.Add(this.btnAddOpenAI);
            this.grpModels.Controls.Add(this.btnAddOllama);
            this.grpModels.Controls.Add(this.lstModels);
            this.grpModels.Location = new System.Drawing.Point(10, 220);
            this.grpModels.Name = "grpModels";
            this.grpModels.Size = new System.Drawing.Size(280, 270);
            this.grpModels.TabIndex = 1;
            this.grpModels.TabStop = false;
            this.grpModels.Text = "Model Management";
            // 
            // btnDeleteModel
            // 
            this.btnDeleteModel.Enabled = false;
            this.btnDeleteModel.Location = new System.Drawing.Point(75, 215);
            this.btnDeleteModel.Name = "btnDeleteModel";
            this.btnDeleteModel.Size = new System.Drawing.Size(60, 25);
            this.btnDeleteModel.TabIndex = 4;
            this.btnDeleteModel.Text = "Delete";
            this.btnDeleteModel.UseVisualStyleBackColor = true;
            this.btnDeleteModel.Click += new System.EventHandler(this.btnDeleteModel_Click);
            // 
            // btnEditModel
            // 
            this.btnEditModel.Enabled = false;
            this.btnEditModel.Location = new System.Drawing.Point(10, 215);
            this.btnEditModel.Name = "btnEditModel";
            this.btnEditModel.Size = new System.Drawing.Size(60, 25);
            this.btnEditModel.TabIndex = 3;
            this.btnEditModel.Text = "Edit";
            this.btnEditModel.UseVisualStyleBackColor = true;
            this.btnEditModel.Click += new System.EventHandler(this.btnEditModel_Click);
            // 
            // btnAddOpenAI
            // 
            this.btnAddOpenAI.Location = new System.Drawing.Point(95, 185);
            this.btnAddOpenAI.Name = "btnAddOpenAI";
            this.btnAddOpenAI.Size = new System.Drawing.Size(80, 25);
            this.btnAddOpenAI.TabIndex = 2;
            this.btnAddOpenAI.Text = "Add OpenAI";
            this.btnAddOpenAI.UseVisualStyleBackColor = true;
            this.btnAddOpenAI.Click += new System.EventHandler(this.btnAddOpenAI_Click);
            // 
            // btnAddOllama
            // 
            this.btnAddOllama.Location = new System.Drawing.Point(10, 185);
            this.btnAddOllama.Name = "btnAddOllama";
            this.btnAddOllama.Size = new System.Drawing.Size(80, 25);
            this.btnAddOllama.TabIndex = 1;
            this.btnAddOllama.Text = "Add Ollama";
            this.btnAddOllama.UseVisualStyleBackColor = true;
            this.btnAddOllama.Click += new System.EventHandler(this.btnAddOllama_Click);
            // 
            // lstModels
            // 
            this.lstModels.DisplayMember = "DisplayText";
            this.lstModels.FormattingEnabled = true;
            this.lstModels.Location = new System.Drawing.Point(10, 25);
            this.lstModels.Name = "lstModels";
            this.lstModels.Size = new System.Drawing.Size(260, 147);
            this.lstModels.TabIndex = 0;
            this.lstModels.SelectedIndexChanged += new System.EventHandler(this.lstModels_SelectedIndexChanged);
            // 
            // grpModelEditor
            // 
            this.grpModelEditor.Controls.Add(this.btnCancelModelEdit);
            this.grpModelEditor.Controls.Add(this.btnSaveModel);
            this.grpModelEditor.Controls.Add(this.txtModelId);
            this.grpModelEditor.Controls.Add(this.label7);
            this.grpModelEditor.Controls.Add(this.pnlOpenAISettings);
            this.grpModelEditor.Controls.Add(this.pnlOllamaSettings);
            this.grpModelEditor.Location = new System.Drawing.Point(300, 220);
            this.grpModelEditor.Name = "grpModelEditor";
            this.grpModelEditor.Size = new System.Drawing.Size(290, 270);
            this.grpModelEditor.TabIndex = 2;
            this.grpModelEditor.TabStop = false;
            this.grpModelEditor.Text = "Model Editor";
            this.grpModelEditor.Visible = false;
            // 
            // btnCancelModelEdit
            // 
            this.btnCancelModelEdit.Location = new System.Drawing.Point(210, 235);
            this.btnCancelModelEdit.Name = "btnCancelModelEdit";
            this.btnCancelModelEdit.Size = new System.Drawing.Size(70, 25);
            this.btnCancelModelEdit.TabIndex = 5;
            this.btnCancelModelEdit.Text = "Cancel";
            this.btnCancelModelEdit.UseVisualStyleBackColor = true;
            this.btnCancelModelEdit.Click += new System.EventHandler(this.btnCancelModelEdit_Click);
            // 
            // btnSaveModel
            // 
            this.btnSaveModel.Location = new System.Drawing.Point(130, 235);
            this.btnSaveModel.Name = "btnSaveModel";
            this.btnSaveModel.Size = new System.Drawing.Size(70, 25);
            this.btnSaveModel.TabIndex = 4;
            this.btnSaveModel.Text = "Save";
            this.btnSaveModel.UseVisualStyleBackColor = true;
            this.btnSaveModel.Click += new System.EventHandler(this.btnSaveModel_Click);
            // 
            // pnlOpenAISettings
            // 
            this.pnlOpenAISettings.Controls.Add(this.txtOpenAiModelName);
            this.pnlOpenAISettings.Controls.Add(this.label9);
            this.pnlOpenAISettings.Controls.Add(this.txtOpenAiKey);
            this.pnlOpenAISettings.Controls.Add(this.label8);
            this.pnlOpenAISettings.Location = new System.Drawing.Point(10, 45);
            this.pnlOpenAISettings.Name = "pnlOpenAISettings";
            this.pnlOpenAISettings.Size = new System.Drawing.Size(270, 180);
            this.pnlOpenAISettings.TabIndex = 3;
            this.pnlOpenAISettings.Visible = false;
            // 
            // txtOpenAiModelName
            // 
            this.txtOpenAiModelName.Location = new System.Drawing.Point(90, 40);
            this.txtOpenAiModelName.Name = "txtOpenAiModelName";
            this.txtOpenAiModelName.Size = new System.Drawing.Size(170, 20);
            this.txtOpenAiModelName.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 40);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Model Name:";
            // 
            // txtOpenAiKey
            // 
            this.txtOpenAiKey.Location = new System.Drawing.Point(70, 10);
            this.txtOpenAiKey.Name = "txtOpenAiKey";
            this.txtOpenAiKey.Size = new System.Drawing.Size(190, 20);
            this.txtOpenAiKey.TabIndex = 1;
            this.txtOpenAiKey.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "API Key:";
            // 
            // pnlOllamaSettings
            // 
            this.pnlOllamaSettings.Controls.Add(this.chkIsInfillModel);
            this.pnlOllamaSettings.Controls.Add(this.txtKeepAlive);
            this.pnlOllamaSettings.Controls.Add(this.label16);
            this.pnlOllamaSettings.Controls.Add(this.nudNumCtx);
            this.pnlOllamaSettings.Controls.Add(this.label15);
            this.pnlOllamaSettings.Controls.Add(this.nudTemperature);
            this.pnlOllamaSettings.Controls.Add(this.label14);
            this.pnlOllamaSettings.Controls.Add(this.nudTopP);
            this.pnlOllamaSettings.Controls.Add(this.label13);
            this.pnlOllamaSettings.Controls.Add(this.nudTopK);
            this.pnlOllamaSettings.Controls.Add(this.label12);
            this.pnlOllamaSettings.Controls.Add(this.txtOllamaModelName);
            this.pnlOllamaSettings.Controls.Add(this.label11);
            this.pnlOllamaSettings.Controls.Add(this.txtOllamaUrl);
            this.pnlOllamaSettings.Controls.Add(this.label10);
            this.pnlOllamaSettings.Location = new System.Drawing.Point(10, 45);
            this.pnlOllamaSettings.Name = "pnlOllamaSettings";
            this.pnlOllamaSettings.Size = new System.Drawing.Size(270, 180);
            this.pnlOllamaSettings.TabIndex = 2;
            this.pnlOllamaSettings.Visible = false;
            // 
            // chkIsInfillModel
            // 
            this.chkIsInfillModel.AutoSize = true;
            this.chkIsInfillModel.Location = new System.Drawing.Point(200, 160);
            this.chkIsInfillModel.Name = "chkIsInfillModel";
            this.chkIsInfillModel.Size = new System.Drawing.Size(87, 17);
            this.chkIsInfillModel.TabIndex = 14;
            this.chkIsInfillModel.Text = "Is Infill Model";
            this.chkIsInfillModel.UseVisualStyleBackColor = true;
            // 
            // txtKeepAlive
            // 
            this.txtKeepAlive.Location = new System.Drawing.Point(90, 160);
            this.txtKeepAlive.Name = "txtKeepAlive";
            this.txtKeepAlive.Size = new System.Drawing.Size(100, 20);
            this.txtKeepAlive.TabIndex = 13;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 160);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "Keep Alive:";
            // 
            // nudNumCtx
            // 
            this.nudNumCtx.Location = new System.Drawing.Point(90, 130);
            this.nudNumCtx.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.nudNumCtx.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nudNumCtx.Name = "nudNumCtx";
            this.nudNumCtx.Size = new System.Drawing.Size(80, 20);
            this.nudNumCtx.TabIndex = 11;
            this.nudNumCtx.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 130);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(69, 13);
            this.label15.TabIndex = 10;
            this.label15.Text = "Context Size:";
            // 
            // nudTemperature
            // 
            this.nudTemperature.DecimalPlaces = 2;
            this.nudTemperature.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudTemperature.Location = new System.Drawing.Point(90, 100);
            this.nudTemperature.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudTemperature.Name = "nudTemperature";
            this.nudTemperature.Size = new System.Drawing.Size(70, 20);
            this.nudTemperature.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 100);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(70, 13);
            this.label14.TabIndex = 8;
            this.label14.Text = "Temperature:";
            // 
            // nudTopP
            // 
            this.nudTopP.DecimalPlaces = 2;
            this.nudTopP.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudTopP.Location = new System.Drawing.Point(190, 70);
            this.nudTopP.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopP.Name = "nudTopP";
            this.nudTopP.Size = new System.Drawing.Size(70, 20);
            this.nudTopP.TabIndex = 7;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(135, 70);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Top P:";
            // 
            // nudTopK
            // 
            this.nudTopK.Location = new System.Drawing.Point(60, 70);
            this.nudTopK.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopK.Name = "nudTopK";
            this.nudTopK.Size = new System.Drawing.Size(60, 20);
            this.nudTopK.TabIndex = 5;
            this.nudTopK.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(5, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Top K:";
            // 
            // txtOllamaModelName
            // 
            this.txtOllamaModelName.Location = new System.Drawing.Point(90, 40);
            this.txtOllamaModelName.Name = "txtOllamaModelName";
            this.txtOllamaModelName.Size = new System.Drawing.Size(170, 20);
            this.txtOllamaModelName.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 40);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Model Name:";
            // 
            // txtOllamaUrl
            // 
            this.txtOllamaUrl.Location = new System.Drawing.Point(90, 10);
            this.txtOllamaUrl.Name = "txtOllamaUrl";
            this.txtOllamaUrl.Size = new System.Drawing.Size(170, 20);
            this.txtOllamaUrl.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Ollama URL:";
            // 
            // txtModelId
            // 
            this.txtModelId.Location = new System.Drawing.Point(90, 19);
            this.txtModelId.Name = "txtModelId";
            this.txtModelId.Size = new System.Drawing.Size(180, 20);
            this.txtModelId.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Model ID:";
            // 
            // SettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.grpModelEditor);
            this.Controls.Add(this.grpModels);
            this.Controls.Add(this.grpGeneral);
            this.Name = "SettingsUserControl";
            this.Size = new System.Drawing.Size(600, 500);
            this.grpGeneral.ResumeLayout(false);
            this.grpGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumTokensProgress)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).EndInit();
            this.grpModels.ResumeLayout(false);
            this.grpModelEditor.ResumeLayout(false);
            this.grpModelEditor.PerformLayout();
            this.pnlOpenAISettings.ResumeLayout(false);
            this.pnlOpenAISettings.PerformLayout();
            this.pnlOllamaSettings.ResumeLayout(false);
            this.pnlOllamaSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCtx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopK)).EndInit();
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
        private System.Windows.Forms.TextBox txtCustomServerUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbAutocompleteModelId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox grpModels;
        private System.Windows.Forms.ListBox lstModels;
        private System.Windows.Forms.Button btnAddOllama;
        private System.Windows.Forms.Button btnAddOpenAI;
        private System.Windows.Forms.Button btnEditModel;
        private System.Windows.Forms.Button btnDeleteModel;
        private System.Windows.Forms.GroupBox grpModelEditor;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtModelId;
        private System.Windows.Forms.Panel pnlOllamaSettings;
        private System.Windows.Forms.Panel pnlOpenAISettings;
        private System.Windows.Forms.Button btnSaveModel;
        private System.Windows.Forms.Button btnCancelModelEdit;
        private System.Windows.Forms.TextBox txtOllamaUrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtOllamaModelName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nudTopK;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudTopP;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudTemperature;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudNumCtx;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtKeepAlive;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.CheckBox chkIsInfillModel;
        private System.Windows.Forms.TextBox txtOpenAiKey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtOpenAiModelName;
        private System.Windows.Forms.Label label9;
    }
}