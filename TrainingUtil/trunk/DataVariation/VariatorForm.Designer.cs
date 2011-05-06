namespace DataVariation
{
    partial class VariatorForm
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

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.label_Title = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label_PropertyName = new System.Windows.Forms.Label();
            this.button_Delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label_Title
            // 
            this.label_Title.AutoSize = true;
            this.label_Title.Font = new System.Drawing.Font("ＭＳ ゴシック", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label_Title.Location = new System.Drawing.Point(3, 8);
            this.label_Title.Name = "label_Title";
            this.label_Title.Size = new System.Drawing.Size(111, 13);
            this.label_Title.TabIndex = 0;
            this.label_Title.Text = "XXX Title XXX";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(207, 5);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(116, 19);
            this.textBox1.TabIndex = 1;
            // 
            // label_PropertyName
            // 
            this.label_PropertyName.AutoSize = true;
            this.label_PropertyName.Location = new System.Drawing.Point(120, 8);
            this.label_PropertyName.Name = "label_PropertyName";
            this.label_PropertyName.Size = new System.Drawing.Size(81, 12);
            this.label_PropertyName.TabIndex = 2;
            this.label_PropertyName.Text = "Property Name";
            // 
            // button_Delete
            // 
            this.button_Delete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Delete.Location = new System.Drawing.Point(352, 3);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(58, 24);
            this.button_Delete.TabIndex = 3;
            this.button_Delete.Text = "削除";
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // VariatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.label_PropertyName);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label_Title);
            this.Name = "VariatorForm";
            this.Size = new System.Drawing.Size(413, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_Title;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label_PropertyName;
        private System.Windows.Forms.Button button_Delete;
    }
}
