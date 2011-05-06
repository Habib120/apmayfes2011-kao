namespace Labeling
{
    partial class Labelling
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Labelling));
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_OpenDir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_SaveNew = new System.Windows.Forms.ToolStripMenuItem();
            this.エクスポートToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_ExtractFaces = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_ExtractForML = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.編集ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_LabelAllItems = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_LabelNewItems = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker2 = new System.ComponentModel.BackgroundWorker();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkBox_Smile = new System.Windows.Forms.CheckBox();
            this.checkBox_Glasses = new System.Windows.Forms.CheckBox();
            this.checkBox_Women = new System.Windows.Forms.CheckBox();
            this.checkBox_Man = new System.Windows.Forms.CheckBox();
            this.button_Delete = new System.Windows.Forms.Button();
            this.button_AutoDetect = new System.Windows.Forms.Button();
            this.button_Next = new System.Windows.Forms.Button();
            this.button_Prev = new System.Windows.Forms.Button();
            this.imageBox1 = new Emgu.CV.UI.ImageBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(658, 260);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(152, 292);
            this.listBox1.TabIndex = 5;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(12, 519);
            this.trackBar1.Maximum = 1800;
            this.trackBar1.Minimum = -1800;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(640, 45);
            this.trackBar1.TabIndex = 8;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem,
            this.編集ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(810, 26);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_OpenDir,
            this.menuItem_Save,
            this.menuItem_SaveNew,
            this.エクスポートToolStripMenuItem,
            this.menuItem_Exit});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // menuItem_OpenDir
            // 
            this.menuItem_OpenDir.Name = "menuItem_OpenDir";
            this.menuItem_OpenDir.Size = new System.Drawing.Size(184, 22);
            this.menuItem_OpenDir.Text = "ディレクトリを開く";
            this.menuItem_OpenDir.Click += new System.EventHandler(this.openDir_Click);
            // 
            // menuItem_Save
            // 
            this.menuItem_Save.Name = "menuItem_Save";
            this.menuItem_Save.Size = new System.Drawing.Size(184, 22);
            this.menuItem_Save.Text = "保存";
            this.menuItem_Save.Click += new System.EventHandler(this.menuItem_Save_Click);
            // 
            // menuItem_SaveNew
            // 
            this.menuItem_SaveNew.Name = "menuItem_SaveNew";
            this.menuItem_SaveNew.Size = new System.Drawing.Size(184, 22);
            this.menuItem_SaveNew.Text = "名前を付けて保存";
            this.menuItem_SaveNew.Click += new System.EventHandler(this.menuItem_SaveNew_Click);
            // 
            // エクスポートToolStripMenuItem
            // 
            this.エクスポートToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_ExtractFaces,
            this.menuItem_ExtractForML});
            this.エクスポートToolStripMenuItem.Name = "エクスポートToolStripMenuItem";
            this.エクスポートToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.エクスポートToolStripMenuItem.Text = "エクスポート";
            // 
            // menuItem_ExtractFaces
            // 
            this.menuItem_ExtractFaces.Name = "menuItem_ExtractFaces";
            this.menuItem_ExtractFaces.Size = new System.Drawing.Size(220, 22);
            this.menuItem_ExtractFaces.Text = "顔切り出し";
            this.menuItem_ExtractFaces.Click += new System.EventHandler(this.menuItem_ExtractFaces_Click);
            // 
            // menuItem_ExtractForML
            // 
            this.menuItem_ExtractForML.Name = "menuItem_ExtractForML";
            this.menuItem_ExtractForML.Size = new System.Drawing.Size(220, 22);
            this.menuItem_ExtractForML.Text = "機械学習用にエクスポート";
            this.menuItem_ExtractForML.Click += new System.EventHandler(this.menuItem_ExtractForML_Click);
            // 
            // menuItem_Exit
            // 
            this.menuItem_Exit.Name = "menuItem_Exit";
            this.menuItem_Exit.Size = new System.Drawing.Size(184, 22);
            this.menuItem_Exit.Text = "終了";
            this.menuItem_Exit.Click += new System.EventHandler(this.menuItem_Exit_Click);
            // 
            // 編集ToolStripMenuItem
            // 
            this.編集ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_LabelAllItems,
            this.menuItem_LabelNewItems});
            this.編集ToolStripMenuItem.Name = "編集ToolStripMenuItem";
            this.編集ToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.編集ToolStripMenuItem.Text = "編集";
            // 
            // menuItem_LabelAllItems
            // 
            this.menuItem_LabelAllItems.Name = "menuItem_LabelAllItems";
            this.menuItem_LabelAllItems.Size = new System.Drawing.Size(352, 22);
            this.menuItem_LabelAllItems.Text = "全ての項目を自動的にラベル付け";
            this.menuItem_LabelAllItems.Click += new System.EventHandler(this.menuItem_LabelAllItems_Click);
            // 
            // menuItem_LabelNewItems
            // 
            this.menuItem_LabelNewItems.Name = "menuItem_LabelNewItems";
            this.menuItem_LabelNewItems.Size = new System.Drawing.Size(352, 22);
            this.menuItem_LabelNewItems.Text = "ラベル付けされていない項目を自動的にラベル付け";
            this.menuItem_LabelNewItems.Click += new System.EventHandler(this.menuItem_LabelNewItems_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // checkBox_Smile
            // 
            this.checkBox_Smile.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Smile.AutoSize = true;
            this.checkBox_Smile.Image = global::Labeling.Properties.Resources.smile32;
            this.checkBox_Smile.Location = new System.Drawing.Point(735, 205);
            this.checkBox_Smile.Name = "checkBox_Smile";
            this.checkBox_Smile.Size = new System.Drawing.Size(59, 38);
            this.checkBox_Smile.TabIndex = 15;
            this.checkBox_Smile.Text = "           ";
            this.checkBox_Smile.UseVisualStyleBackColor = true;
            this.checkBox_Smile.CheckedChanged += new System.EventHandler(this.checkBox_Smile_CheckedChanged);
            // 
            // checkBox_Glasses
            // 
            this.checkBox_Glasses.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Glasses.AutoSize = true;
            this.checkBox_Glasses.Image = global::Labeling.Properties.Resources.glasses;
            this.checkBox_Glasses.Location = new System.Drawing.Point(670, 201);
            this.checkBox_Glasses.Name = "checkBox_Glasses";
            this.checkBox_Glasses.Size = new System.Drawing.Size(59, 46);
            this.checkBox_Glasses.TabIndex = 14;
            this.checkBox_Glasses.Text = "           ";
            this.checkBox_Glasses.UseVisualStyleBackColor = true;
            this.checkBox_Glasses.CheckedChanged += new System.EventHandler(this.checkBox_Glasses_CheckedChanged);
            // 
            // checkBox_Women
            // 
            this.checkBox_Women.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Women.AutoSize = true;
            this.checkBox_Women.Image = global::Labeling.Properties.Resources.woman;
            this.checkBox_Women.Location = new System.Drawing.Point(735, 157);
            this.checkBox_Women.Name = "checkBox_Women";
            this.checkBox_Women.Size = new System.Drawing.Size(59, 38);
            this.checkBox_Women.TabIndex = 13;
            this.checkBox_Women.Text = "           ";
            this.checkBox_Women.UseVisualStyleBackColor = true;
            this.checkBox_Women.CheckedChanged += new System.EventHandler(this.checkBox_Women_CheckedChanged);
            // 
            // checkBox_Man
            // 
            this.checkBox_Man.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox_Man.AutoSize = true;
            this.checkBox_Man.Image = global::Labeling.Properties.Resources.man;
            this.checkBox_Man.Location = new System.Drawing.Point(670, 157);
            this.checkBox_Man.Name = "checkBox_Man";
            this.checkBox_Man.Size = new System.Drawing.Size(59, 38);
            this.checkBox_Man.TabIndex = 12;
            this.checkBox_Man.Text = "           ";
            this.checkBox_Man.UseVisualStyleBackColor = true;
            this.checkBox_Man.CheckedChanged += new System.EventHandler(this.checkBox_Man_CheckedChanged);
            // 
            // button_Delete
            // 
            this.button_Delete.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_Delete.Image = global::Labeling.Properties.Resources.stop32;
            this.button_Delete.Location = new System.Drawing.Point(735, 95);
            this.button_Delete.Name = "button_Delete";
            this.button_Delete.Size = new System.Drawing.Size(59, 56);
            this.button_Delete.TabIndex = 11;
            this.button_Delete.UseVisualStyleBackColor = true;
            this.button_Delete.Click += new System.EventHandler(this.button_Delete_Click);
            // 
            // button_AutoDetect
            // 
            this.button_AutoDetect.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_AutoDetect.Image = global::Labeling.Properties.Resources.wand32;
            this.button_AutoDetect.Location = new System.Drawing.Point(670, 95);
            this.button_AutoDetect.Name = "button_AutoDetect";
            this.button_AutoDetect.Size = new System.Drawing.Size(59, 56);
            this.button_AutoDetect.TabIndex = 10;
            this.button_AutoDetect.UseVisualStyleBackColor = true;
            this.button_AutoDetect.Click += new System.EventHandler(this.button_AutoDetect_Click);
            // 
            // button_Next
            // 
            this.button_Next.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_Next.Image = global::Labeling.Properties.Resources.arrowright32;
            this.button_Next.Location = new System.Drawing.Point(735, 33);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(59, 56);
            this.button_Next.TabIndex = 4;
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // button_Prev
            // 
            this.button_Prev.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button_Prev.Image = global::Labeling.Properties.Resources.leftarrow32;
            this.button_Prev.Location = new System.Drawing.Point(670, 33);
            this.button_Prev.Name = "button_Prev";
            this.button_Prev.Size = new System.Drawing.Size(59, 56);
            this.button_Prev.TabIndex = 3;
            this.button_Prev.UseVisualStyleBackColor = true;
            this.button_Prev.Click += new System.EventHandler(this.button_Prev_Click);
            // 
            // imageBox1
            // 
            this.imageBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.imageBox1.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox1.Location = new System.Drawing.Point(12, 33);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(640, 480);
            this.imageBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox1.TabIndex = 2;
            this.imageBox1.TabStop = false;
            this.imageBox1.Click += new System.EventHandler(this.imageBox1_Click);
            this.imageBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseDown);
            this.imageBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseMove);
            this.imageBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imageBox1_MouseUp);
            // 
            // Labelling
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(810, 560);
            this.Controls.Add(this.checkBox_Smile);
            this.Controls.Add(this.checkBox_Glasses);
            this.Controls.Add(this.checkBox_Women);
            this.Controls.Add(this.checkBox_Man);
            this.Controls.Add(this.button_Delete);
            this.Controls.Add(this.button_AutoDetect);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button_Next);
            this.Controls.Add(this.button_Prev);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Labelling";
            this.Text = "Labelling";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LabelingWindow_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox imageBox1;
        private System.Windows.Forms.Button button_Prev;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_OpenDir;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Save;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button button_Delete;
        private System.Windows.Forms.Button button_AutoDetect;
        private System.Windows.Forms.CheckBox checkBox_Man;
        private System.Windows.Forms.CheckBox checkBox_Women;
        private System.Windows.Forms.CheckBox checkBox_Glasses;
        private System.Windows.Forms.CheckBox checkBox_Smile;
        private System.Windows.Forms.ToolStripMenuItem 編集ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LabelAllItems;
        private System.Windows.Forms.ToolStripMenuItem menuItem_LabelNewItems;
        private System.Windows.Forms.ToolStripMenuItem menuItem_SaveNew;
        private System.Windows.Forms.ToolStripMenuItem エクスポートToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_ExtractFaces;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Exit;
        private System.ComponentModel.BackgroundWorker backgroundWorker2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem menuItem_ExtractForML;
    }
}