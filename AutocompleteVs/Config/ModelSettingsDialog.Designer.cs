namespace AutocompleteVs.Config
{
    partial class ModelSettingsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtModelId = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pnlOllamaSettings = new System.Windows.Forms.Panel();
            this.numSeed = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
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
            this.pnlOpenAISettings = new System.Windows.Forms.Panel();
            this.txtOpenAiModelName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtOpenAiKey = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlOllamaSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCtx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperature)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopK)).BeginInit();
            this.pnlOpenAISettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtModelId
            // 
            this.txtModelId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtModelId.Location = new System.Drawing.Point(110, 20);
            this.txtModelId.Name = "txtModelId";
            this.txtModelId.Size = new System.Drawing.Size(406, 20);
            this.txtModelId.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(25, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Model ID:";
            // 
            // pnlOllamaSettings
            // 
            this.pnlOllamaSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOllamaSettings.Controls.Add(this.numSeed);
            this.pnlOllamaSettings.Controls.Add(this.label1);
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
            this.pnlOllamaSettings.Location = new System.Drawing.Point(20, 46);
            this.pnlOllamaSettings.Name = "pnlOllamaSettings";
            this.pnlOllamaSettings.Size = new System.Drawing.Size(496, 196);
            this.pnlOllamaSettings.TabIndex = 2;
            this.pnlOllamaSettings.Visible = false;
            // 
            // numSeed
            // 
            this.numSeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numSeed.Location = new System.Drawing.Point(241, 98);
            this.numSeed.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSeed.Name = "numSeed";
            this.numSeed.Size = new System.Drawing.Size(70, 20);
            this.numSeed.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(196, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Seed:";
            // 
            // chkIsInfillModel
            // 
            this.chkIsInfillModel.AutoSize = true;
            this.chkIsInfillModel.Location = new System.Drawing.Point(241, 150);
            this.chkIsInfillModel.Name = "chkIsInfillModel";
            this.chkIsInfillModel.Size = new System.Drawing.Size(87, 17);
            this.chkIsInfillModel.TabIndex = 14;
            this.chkIsInfillModel.Text = "Is Infill Model";
            this.chkIsInfillModel.UseVisualStyleBackColor = true;
            // 
            // txtKeepAlive
            // 
            this.txtKeepAlive.Location = new System.Drawing.Point(90, 150);
            this.txtKeepAlive.Name = "txtKeepAlive";
            this.txtKeepAlive.Size = new System.Drawing.Size(100, 20);
            this.txtKeepAlive.TabIndex = 13;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(5, 153);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(61, 13);
            this.label16.TabIndex = 12;
            this.label16.Text = "Keep Alive:";
            // 
            // nudNumCtx
            // 
            this.nudNumCtx.Location = new System.Drawing.Point(90, 124);
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
            this.label15.Location = new System.Drawing.Point(5, 126);
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
            this.nudTemperature.Location = new System.Drawing.Point(90, 98);
            this.nudTemperature.Maximum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudTemperature.Name = "nudTemperature";
            this.nudTemperature.Size = new System.Drawing.Size(80, 20);
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
            this.nudTopP.Location = new System.Drawing.Point(241, 72);
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
            this.label13.Location = new System.Drawing.Point(196, 74);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Top P:";
            // 
            // nudTopK
            // 
            this.nudTopK.Location = new System.Drawing.Point(90, 72);
            this.nudTopK.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudTopK.Name = "nudTopK";
            this.nudTopK.Size = new System.Drawing.Size(80, 20);
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
            this.label12.Location = new System.Drawing.Point(5, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(39, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "Top K:";
            // 
            // txtOllamaModelName
            // 
            this.txtOllamaModelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOllamaModelName.Location = new System.Drawing.Point(90, 46);
            this.txtOllamaModelName.Name = "txtOllamaModelName";
            this.txtOllamaModelName.Size = new System.Drawing.Size(393, 20);
            this.txtOllamaModelName.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(70, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Model Name:";
            // 
            // txtOllamaUrl
            // 
            this.txtOllamaUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOllamaUrl.Location = new System.Drawing.Point(90, 20);
            this.txtOllamaUrl.Name = "txtOllamaUrl";
            this.txtOllamaUrl.Size = new System.Drawing.Size(393, 20);
            this.txtOllamaUrl.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 23);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Ollama URL:";
            // 
            // pnlOpenAISettings
            // 
            this.pnlOpenAISettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOpenAISettings.Controls.Add(this.txtOpenAiModelName);
            this.pnlOpenAISettings.Controls.Add(this.label9);
            this.pnlOpenAISettings.Controls.Add(this.txtOpenAiKey);
            this.pnlOpenAISettings.Controls.Add(this.label8);
            this.pnlOpenAISettings.Location = new System.Drawing.Point(20, 46);
            this.pnlOpenAISettings.Name = "pnlOpenAISettings";
            this.pnlOpenAISettings.Size = new System.Drawing.Size(493, 196);
            this.pnlOpenAISettings.TabIndex = 3;
            this.pnlOpenAISettings.Visible = false;
            // 
            // txtOpenAiModelName
            // 
            this.txtOpenAiModelName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOpenAiModelName.Location = new System.Drawing.Point(90, 50);
            this.txtOpenAiModelName.Name = "txtOpenAiModelName";
            this.txtOpenAiModelName.Size = new System.Drawing.Size(393, 20);
            this.txtOpenAiModelName.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(70, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Model Name:";
            // 
            // txtOpenAiKey
            // 
            this.txtOpenAiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOpenAiKey.Location = new System.Drawing.Point(97, 20);
            this.txtOpenAiKey.Name = "txtOpenAiKey";
            this.txtOpenAiKey.PasswordChar = '*';
            this.txtOpenAiKey.Size = new System.Drawing.Size(386, 20);
            this.txtOpenAiKey.TabIndex = 1;
            this.txtOpenAiKey.UseSystemPasswordChar = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 23);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "API Key:";
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(23, 248);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(104, 248);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // ModelSettingsDialog
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(528, 283);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtModelId);
            this.Controls.Add(this.pnlOllamaSettings);
            this.Controls.Add(this.pnlOpenAISettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ModelSettingsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Model Settings";
            this.pnlOllamaSettings.ResumeLayout(false);
            this.pnlOllamaSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumCtx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTemperature)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTopK)).EndInit();
            this.pnlOpenAISettings.ResumeLayout(false);
            this.pnlOpenAISettings.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtModelId;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel pnlOllamaSettings;
        private System.Windows.Forms.CheckBox chkIsInfillModel;
        private System.Windows.Forms.TextBox txtKeepAlive;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown nudNumCtx;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudTemperature;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown nudTopP;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown nudTopK;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtOllamaModelName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtOllamaUrl;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel pnlOpenAISettings;
        private System.Windows.Forms.TextBox txtOpenAiModelName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtOpenAiKey;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numSeed;
        private System.Windows.Forms.Label label1;
    }
}