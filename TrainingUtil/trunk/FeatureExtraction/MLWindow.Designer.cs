namespace FeatureExtraction
{
    partial class MLWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MLWindow));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label_Message = new System.Windows.Forms.Label();
            this.button_Predict = new System.Windows.Forms.Button();
            this.button_Browse = new System.Windows.Forms.Button();
            this.textBox_TestDataDir = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_Train = new System.Windows.Forms.Button();
            this.comboBox_MLAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView_Results = new System.Windows.Forms.DataGridView();
            this.gridViewColumn_Filename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridViewColumn_TrueLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridViewColumn_PredictedLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridViewColumn_IsCorrect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Results)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.label_Message);
            this.panel1.Controls.Add(this.button_Predict);
            this.panel1.Controls.Add(this.button_Browse);
            this.panel1.Controls.Add(this.textBox_TestDataDir);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button_Train);
            this.panel1.Controls.Add(this.comboBox_MLAlgorithm);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(586, 112);
            this.panel1.TabIndex = 1;
            // 
            // label_Message
            // 
            this.label_Message.AutoSize = true;
            this.label_Message.Location = new System.Drawing.Point(3, 97);
            this.label_Message.Name = "label_Message";
            this.label_Message.Size = new System.Drawing.Size(11, 12);
            this.label_Message.TabIndex = 7;
            this.label_Message.Text = "-";
            // 
            // button_Predict
            // 
            this.button_Predict.Location = new System.Drawing.Point(510, 51);
            this.button_Predict.Name = "button_Predict";
            this.button_Predict.Size = new System.Drawing.Size(70, 37);
            this.button_Predict.TabIndex = 6;
            this.button_Predict.Text = "Predict";
            this.button_Predict.UseVisualStyleBackColor = true;
            this.button_Predict.Click += new System.EventHandler(this.button_Predict_Click);
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(434, 51);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(70, 37);
            this.button_Browse.TabIndex = 5;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBox_TestDataDir
            // 
            this.textBox_TestDataDir.Location = new System.Drawing.Point(150, 60);
            this.textBox_TestDataDir.Name = "textBox_TestDataDir";
            this.textBox_TestDataDir.ReadOnly = true;
            this.textBox_TestDataDir.Size = new System.Drawing.Size(265, 19);
            this.textBox_TestDataDir.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(21, 63);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Test Data Directory";
            // 
            // button_Train
            // 
            this.button_Train.Location = new System.Drawing.Point(434, 12);
            this.button_Train.Name = "button_Train";
            this.button_Train.Size = new System.Drawing.Size(70, 37);
            this.button_Train.TabIndex = 2;
            this.button_Train.Text = "Train";
            this.button_Train.UseVisualStyleBackColor = true;
            this.button_Train.Click += new System.EventHandler(this.button_Train_Click);
            // 
            // comboBox_MLAlgorithm
            // 
            this.comboBox_MLAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_MLAlgorithm.FormattingEnabled = true;
            this.comboBox_MLAlgorithm.Location = new System.Drawing.Point(101, 21);
            this.comboBox_MLAlgorithm.Name = "comboBox_MLAlgorithm";
            this.comboBox_MLAlgorithm.Size = new System.Drawing.Size(314, 20);
            this.comboBox_MLAlgorithm.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(21, 21);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(74, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Algorithm";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.dataGridView_Results);
            this.panel2.Location = new System.Drawing.Point(0, 112);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(586, 408);
            this.panel2.TabIndex = 2;
            // 
            // dataGridView_Results
            // 
            this.dataGridView_Results.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Results.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridViewColumn_Filename,
            this.gridViewColumn_TrueLabel,
            this.gridViewColumn_PredictedLabel,
            this.gridViewColumn_IsCorrect});
            this.dataGridView_Results.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_Results.Location = new System.Drawing.Point(0, 0);
            this.dataGridView_Results.Name = "dataGridView_Results";
            this.dataGridView_Results.RowTemplate.Height = 21;
            this.dataGridView_Results.Size = new System.Drawing.Size(586, 408);
            this.dataGridView_Results.TabIndex = 0;
            // 
            // gridViewColumn_Filename
            // 
            this.gridViewColumn_Filename.DataPropertyName = "FileName";
            this.gridViewColumn_Filename.HeaderText = "FileName";
            this.gridViewColumn_Filename.Name = "gridViewColumn_Filename";
            this.gridViewColumn_Filename.ReadOnly = true;
            this.gridViewColumn_Filename.Width = 130;
            // 
            // gridViewColumn_TrueLabel
            // 
            this.gridViewColumn_TrueLabel.DataPropertyName = "TrueLabel";
            this.gridViewColumn_TrueLabel.HeaderText = "True Label";
            this.gridViewColumn_TrueLabel.Name = "gridViewColumn_TrueLabel";
            this.gridViewColumn_TrueLabel.ReadOnly = true;
            this.gridViewColumn_TrueLabel.Width = 130;
            // 
            // gridViewColumn_PredictedLabel
            // 
            this.gridViewColumn_PredictedLabel.DataPropertyName = "PredictedLabel";
            this.gridViewColumn_PredictedLabel.HeaderText = "Predicted Label";
            this.gridViewColumn_PredictedLabel.Name = "gridViewColumn_PredictedLabel";
            this.gridViewColumn_PredictedLabel.ReadOnly = true;
            this.gridViewColumn_PredictedLabel.Width = 130;
            // 
            // gridViewColumn_IsCorrect
            // 
            this.gridViewColumn_IsCorrect.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.gridViewColumn_IsCorrect.DataPropertyName = "IsCorrect";
            this.gridViewColumn_IsCorrect.HeaderText = "Correctness";
            this.gridViewColumn_IsCorrect.Name = "gridViewColumn_IsCorrect";
            // 
            // MLWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 519);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MLWindow";
            this.Text = "MLWindow";
            this.Load += new System.EventHandler(this.MLWindow_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Results)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_Train;
        private System.Windows.Forms.ComboBox comboBox_MLAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView_Results;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox textBox_TestDataDir;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_Predict;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewColumn_Filename;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewColumn_TrueLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewColumn_PredictedLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridViewColumn_IsCorrect;
        private System.Windows.Forms.Label label_Message;
    }
}