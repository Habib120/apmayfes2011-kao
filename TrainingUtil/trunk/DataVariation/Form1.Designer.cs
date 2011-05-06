namespace DataVariation
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button_Add = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button_AddVariation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.panel_Variations = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // button_Add
            // 
            this.button_Add.Location = new System.Drawing.Point(341, 52);
            this.button_Add.Name = "button_Add";
            this.button_Add.Size = new System.Drawing.Size(61, 28);
            this.button_Add.TabIndex = 0;
            this.button_Add.Text = "Add";
            this.button_Add.UseVisualStyleBackColor = true;
            this.button_Add.Click += new System.EventHandler(this.button_Add_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(10, 57);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(321, 20);
            this.comboBox1.TabIndex = 1;
            // 
            // button_AddVariation
            // 
            this.button_AddVariation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_AddVariation.Location = new System.Drawing.Point(153, 311);
            this.button_AddVariation.Name = "button_AddVariation";
            this.button_AddVariation.Size = new System.Drawing.Size(131, 46);
            this.button_AddVariation.TabIndex = 2;
            this.button_AddVariation.Text = "Add Variation";
            this.button_AddVariation.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Data Directory";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(9, 24);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(322, 19);
            this.textBox1.TabIndex = 4;
            // 
            // button_Browse
            // 
            this.button_Browse.Location = new System.Drawing.Point(341, 21);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(60, 25);
            this.button_Browse.TabIndex = 5;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // panel_Variations
            // 
            this.panel_Variations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_Variations.AutoScroll = true;
            this.panel_Variations.Location = new System.Drawing.Point(4, 86);
            this.panel_Variations.Name = "panel_Variations";
            this.panel_Variations.Size = new System.Drawing.Size(428, 216);
            this.panel_Variations.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 360);
            this.Controls.Add(this.panel_Variations);
            this.Controls.Add(this.button_Browse);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_AddVariation);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button_Add);
            this.Name = "Form1";
            this.Text = "Add variation to face data";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_Add;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button_AddVariation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.Panel panel_Variations;
    }
}

