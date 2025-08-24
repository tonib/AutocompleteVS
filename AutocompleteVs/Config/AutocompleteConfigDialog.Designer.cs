namespace AutocompleteVs.Config
{
    partial class AutocompleteConfigDialog
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
            this.cmbAutocompleteModelId = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.nudInfillPrefixPercentage = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudMaxPromptCharacters = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtId = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbAutocompleteModelId
            // 
            this.cmbAutocompleteModelId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAutocompleteModelId.FormattingEnabled = true;
            this.cmbAutocompleteModelId.Location = new System.Drawing.Point(182, 46);
            this.cmbAutocompleteModelId.Name = "cmbAutocompleteModelId";
            this.cmbAutocompleteModelId.Size = new System.Drawing.Size(281, 21);
            this.cmbAutocompleteModelId.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Model";
            // 
            // nudInfillPrefixPercentage
            // 
            this.nudInfillPrefixPercentage.DecimalPlaces = 1;
            this.nudInfillPrefixPercentage.Location = new System.Drawing.Point(182, 99);
            this.nudInfillPrefixPercentage.Name = "nudInfillPrefixPercentage";
            this.nudInfillPrefixPercentage.Size = new System.Drawing.Size(100, 20);
            this.nudInfillPrefixPercentage.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 101);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Prefix % when max reached:";
            // 
            // nudMaxPromptCharacters
            // 
            this.nudMaxPromptCharacters.Location = new System.Drawing.Point(182, 73);
            this.nudMaxPromptCharacters.Name = "nudMaxPromptCharacters";
            this.nudMaxPromptCharacters.Size = new System.Drawing.Size(100, 20);
            this.nudMaxPromptCharacters.TabIndex = 5;
            this.nudMaxPromptCharacters.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Max. characters in prompt:";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(26, 136);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(108, 136);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Settings identifier";
            // 
            // txtId
            // 
            this.txtId.Location = new System.Drawing.Point(182, 20);
            this.txtId.Name = "txtId";
            this.txtId.Size = new System.Drawing.Size(280, 20);
            this.txtId.TabIndex = 1;
            // 
            // AutocompleteConfigDialog
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(474, 174);
            this.Controls.Add(this.txtId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.cmbAutocompleteModelId);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudInfillPrefixPercentage);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudMaxPromptCharacters);
            this.Controls.Add(this.label1);
            this.Name = "AutocompleteConfigDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Autocompletion settings";
            ((System.ComponentModel.ISupportInitialize)(this.nudInfillPrefixPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxPromptCharacters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAutocompleteModelId;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown nudInfillPrefixPercentage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudMaxPromptCharacters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtId;
    }
}