namespace FeatureExtraction
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_OpenDir = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItem_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView_Labels = new System.Windows.Forms.DataGridView();
            this.gridColumn_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gridColumn_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_LastUpdate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ItemFilename = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button_Prev = new System.Windows.Forms.Button();
            this.button_Next = new System.Windows.Forms.Button();
            this.imageBox_Face = new Emgu.CV.UI.ImageBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBox_CompressionMethods = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numericUpDown_PCADim = new System.Windows.Forms.NumericUpDown();
            this.checkBox_UseCompression = new System.Windows.Forms.CheckBox();
            this.button_LoadExtractor = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox_ShowIntraClassMean = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.checkedListBox_Labels = new System.Windows.Forms.CheckedListBox();
            this.button_StartML = new System.Windows.Forms.Button();
            this.checkedListBox_Extractors = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.openDataDirDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Labels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Face)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PCADim)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1039, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルToolStripMenuItem
            // 
            this.ファイルToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItem_OpenDir,
            this.menuItem_Quit});
            this.ファイルToolStripMenuItem.Name = "ファイルToolStripMenuItem";
            this.ファイルToolStripMenuItem.Size = new System.Drawing.Size(68, 22);
            this.ファイルToolStripMenuItem.Text = "ファイル";
            // 
            // menuItem_OpenDir
            // 
            this.menuItem_OpenDir.Name = "menuItem_OpenDir";
            this.menuItem_OpenDir.Size = new System.Drawing.Size(220, 22);
            this.menuItem_OpenDir.Text = "顔画像ディレクトリを開く";
            this.menuItem_OpenDir.Click += new System.EventHandler(this.menuItem_OpenDir_Click);
            // 
            // menuItem_Quit
            // 
            this.menuItem_Quit.Name = "menuItem_Quit";
            this.menuItem_Quit.Size = new System.Drawing.Size(220, 22);
            this.menuItem_Quit.Text = "終了";
            this.menuItem_Quit.Click += new System.EventHandler(this.menuItem_Quit_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.listBox1);
            this.panel1.Controls.Add(this.button_Prev);
            this.panel1.Controls.Add(this.button_Next);
            this.panel1.Controls.Add(this.imageBox_Face);
            this.panel1.Location = new System.Drawing.Point(2, 29);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1037, 252);
            this.panel1.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView_Labels);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBox_LastUpdate);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBox_ItemFilename);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(334, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 234);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Data Profile";
            // 
            // dataGridView_Labels
            // 
            this.dataGridView_Labels.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            this.dataGridView_Labels.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Labels.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView_Labels.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_Labels.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.gridColumn_Name,
            this.gridColumn_Value});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView_Labels.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView_Labels.Location = new System.Drawing.Point(18, 94);
            this.dataGridView_Labels.Name = "dataGridView_Labels";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView_Labels.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView_Labels.RowTemplate.Height = 21;
            this.dataGridView_Labels.Size = new System.Drawing.Size(271, 134);
            this.dataGridView_Labels.TabIndex = 5;
            // 
            // gridColumn_Name
            // 
            this.gridColumn_Name.DataPropertyName = "Name";
            this.gridColumn_Name.HeaderText = "Name";
            this.gridColumn_Name.Name = "gridColumn_Name";
            this.gridColumn_Name.ReadOnly = true;
            // 
            // gridColumn_Value
            // 
            this.gridColumn_Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.gridColumn_Value.DataPropertyName = "Value";
            this.gridColumn_Value.HeaderText = "Value";
            this.gridColumn_Value.Name = "gridColumn_Value";
            this.gridColumn_Value.ReadOnly = true;
            this.gridColumn_Value.Width = 130;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Labels:";
            // 
            // textBox_LastUpdate
            // 
            this.textBox_LastUpdate.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox_LastUpdate.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_LastUpdate.Location = new System.Drawing.Point(104, 51);
            this.textBox_LastUpdate.Name = "textBox_LastUpdate";
            this.textBox_LastUpdate.ReadOnly = true;
            this.textBox_LastUpdate.Size = new System.Drawing.Size(186, 12);
            this.textBox_LastUpdate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Last Update:";
            // 
            // textBox_ItemFilename
            // 
            this.textBox_ItemFilename.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBox_ItemFilename.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox_ItemFilename.Location = new System.Drawing.Point(104, 26);
            this.textBox_ItemFilename.Name = "textBox_ItemFilename";
            this.textBox_ItemFilename.ReadOnly = true;
            this.textBox_ItemFilename.Size = new System.Drawing.Size(186, 12);
            this.textBox_ItemFilename.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Filename:";
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(662, 59);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(363, 184);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button_Prev
            // 
            this.button_Prev.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Prev.Image = global::FeatureExtraction.Properties.Resources.leftarrow321;
            this.button_Prev.Location = new System.Drawing.Point(903, 0);
            this.button_Prev.Name = "button_Prev";
            this.button_Prev.Size = new System.Drawing.Size(58, 53);
            this.button_Prev.TabIndex = 4;
            this.button_Prev.UseVisualStyleBackColor = true;
            this.button_Prev.Click += new System.EventHandler(this.button_Prev_Click);
            // 
            // button_Next
            // 
            this.button_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Next.Image = global::FeatureExtraction.Properties.Resources.arrowright32;
            this.button_Next.Location = new System.Drawing.Point(967, 0);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(58, 53);
            this.button_Next.TabIndex = 3;
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // imageBox_Face
            // 
            this.imageBox_Face.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.imageBox_Face.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            this.imageBox_Face.Location = new System.Drawing.Point(43, 7);
            this.imageBox_Face.Name = "imageBox_Face";
            this.imageBox_Face.Size = new System.Drawing.Size(248, 235);
            this.imageBox_Face.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageBox_Face.TabIndex = 2;
            this.imageBox_Face.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.button_LoadExtractor);
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.checkBox_ShowIntraClassMean);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.checkedListBox_Labels);
            this.panel2.Controls.Add(this.button_StartML);
            this.panel2.Controls.Add(this.checkedListBox_Extractors);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.chart1);
            this.panel2.Location = new System.Drawing.Point(2, 287);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1035, 355);
            this.panel2.TabIndex = 4;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.comboBox_CompressionMethods);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numericUpDown_PCADim);
            this.groupBox2.Controls.Add(this.checkBox_UseCompression);
            this.groupBox2.Location = new System.Drawing.Point(667, 202);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(362, 64);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "圧縮オプション";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 41);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "メソッド：";
            // 
            // comboBox_CompressionMethods
            // 
            this.comboBox_CompressionMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_CompressionMethods.FormattingEnabled = true;
            this.comboBox_CompressionMethods.Location = new System.Drawing.Point(79, 38);
            this.comboBox_CompressionMethods.Name = "comboBox_CompressionMethods";
            this.comboBox_CompressionMethods.Size = new System.Drawing.Size(106, 20);
            this.comboBox_CompressionMethods.TabIndex = 16;
            this.comboBox_CompressionMethods.SelectedIndexChanged += new System.EventHandler(this.comboBox_CompressionMethods_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(300, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 15;
            this.label6.Text = "次元";
            // 
            // numericUpDown_PCADim
            // 
            this.numericUpDown_PCADim.Location = new System.Drawing.Point(215, 38);
            this.numericUpDown_PCADim.Name = "numericUpDown_PCADim";
            this.numericUpDown_PCADim.Size = new System.Drawing.Size(79, 19);
            this.numericUpDown_PCADim.TabIndex = 14;
            this.numericUpDown_PCADim.ValueChanged += new System.EventHandler(this.numericUpDown_PCADim_ValueChanged);
            this.numericUpDown_PCADim.Leave += new System.EventHandler(this.numericUpDown_PCADim_Leave);
            // 
            // checkBox_UseCompression
            // 
            this.checkBox_UseCompression.AutoSize = true;
            this.checkBox_UseCompression.Location = new System.Drawing.Point(10, 18);
            this.checkBox_UseCompression.Name = "checkBox_UseCompression";
            this.checkBox_UseCompression.Size = new System.Drawing.Size(146, 16);
            this.checkBox_UseCompression.TabIndex = 13;
            this.checkBox_UseCompression.Text = "特徴量の次元圧縮を行う";
            this.checkBox_UseCompression.UseVisualStyleBackColor = true;
            this.checkBox_UseCompression.CheckedChanged += new System.EventHandler(this.checkBox_UseCompression_CheckedChanged);
            // 
            // button_LoadExtractor
            // 
            this.button_LoadExtractor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_LoadExtractor.Location = new System.Drawing.Point(966, 278);
            this.button_LoadExtractor.Name = "button_LoadExtractor";
            this.button_LoadExtractor.Size = new System.Drawing.Size(63, 24);
            this.button_LoadExtractor.TabIndex = 12;
            this.button_LoadExtractor.Text = "Load";
            this.button_LoadExtractor.UseVisualStyleBackColor = true;
            this.button_LoadExtractor.Click += new System.EventHandler(this.button_LoadExtractor_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox2.Location = new System.Drawing.Point(823, 272);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(145, 24);
            this.checkBox2.TabIndex = 11;
            this.checkBox2.Text = "分散をグラフに表示";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox_ShowIntraClassMean
            // 
            this.checkBox_ShowIntraClassMean.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox_ShowIntraClassMean.AutoSize = true;
            this.checkBox_ShowIntraClassMean.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.checkBox_ShowIntraClassMean.Location = new System.Drawing.Point(677, 272);
            this.checkBox_ShowIntraClassMean.Name = "checkBox_ShowIntraClassMean";
            this.checkBox_ShowIntraClassMean.Size = new System.Drawing.Size(145, 24);
            this.checkBox_ShowIntraClassMean.TabIndex = 6;
            this.checkBox_ShowIntraClassMean.Text = "平均をグラフに表示";
            this.checkBox_ShowIntraClassMean.UseVisualStyleBackColor = true;
            this.checkBox_ShowIntraClassMean.CheckedChanged += new System.EventHandler(this.checkBox_ShowIntraClassMean_CheckedChanged);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(673, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(310, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "学習に使用するラベルを選択してください。";
            // 
            // checkedListBox_Labels
            // 
            this.checkedListBox_Labels.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox_Labels.FormattingEnabled = true;
            this.checkedListBox_Labels.Location = new System.Drawing.Point(667, 136);
            this.checkedListBox_Labels.Name = "checkedListBox_Labels";
            this.checkedListBox_Labels.Size = new System.Drawing.Size(362, 60);
            this.checkedListBox_Labels.TabIndex = 9;
            this.checkedListBox_Labels.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_Labels_ItemCheck);
            // 
            // button_StartML
            // 
            this.button_StartML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_StartML.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_StartML.Image = global::FeatureExtraction.Properties.Resources.agent_32;
            this.button_StartML.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_StartML.Location = new System.Drawing.Point(920, 308);
            this.button_StartML.Name = "button_StartML";
            this.button_StartML.Size = new System.Drawing.Size(111, 42);
            this.button_StartML.TabIndex = 8;
            this.button_StartML.Text = "Start ML";
            this.button_StartML.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button_StartML.UseVisualStyleBackColor = true;
            this.button_StartML.Click += new System.EventHandler(this.button_StartML_Click);
            // 
            // checkedListBox_Extractors
            // 
            this.checkedListBox_Extractors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBox_Extractors.FormattingEnabled = true;
            this.checkedListBox_Extractors.Location = new System.Drawing.Point(667, 31);
            this.checkedListBox_Extractors.Name = "checkedListBox_Extractors";
            this.checkedListBox_Extractors.Size = new System.Drawing.Size(362, 74);
            this.checkedListBox_Extractors.TabIndex = 5;
            this.checkedListBox_Extractors.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox_Extractors_ItemCheck);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(673, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(250, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "特徴量の種類を選択してください。";
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.chart1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.chart1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chart1.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chart1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.chart1.BorderlineColor = System.Drawing.Color.Transparent;
            this.chart1.BorderSkin.BackColor = System.Drawing.Color.Green;
            this.chart1.BorderSkin.BackImageTransparentColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chart1.BorderSkin.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.chart1.BorderSkin.PageColor = System.Drawing.Color.Transparent;
            chartArea1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            chartArea1.BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.TopBottom;
            chartArea1.BackSecondaryColor = System.Drawing.Color.White;
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(14, 16);
            this.chart1.Name = "chart1";
            series1.BackSecondaryColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            series1.BorderWidth = 4;
            series1.ChartArea = "ChartArea1";
            series1.CustomProperties = "DrawingStyle=LightToDark, PointWidth=0.4";
            series1.Legend = "Legend1";
            series1.MarkerBorderColor = System.Drawing.Color.Transparent;
            series1.MarkerBorderWidth = 2;
            series1.MarkerColor = System.Drawing.Color.DarkOrange;
            series1.MarkerSize = 10;
            series1.Name = "Series1";
            series1.ShadowColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            series1.ShadowOffset = 1;
            series1.SmartLabelStyle.CalloutLineWidth = 6;
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(647, 330);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            title1.Name = "明度ヒストグラム＋勾配ヒストグラム";
            this.chart1.Titles.Add(title1);
            // 
            // openDataDirDialog
            // 
            this.openDataDirDialog.SelectedPath = "C:\\Users\\t2ladmin\\Documents\\smiles";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1039, 643);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(840, 680);
            this.Name = "MainWindow";
            this.Text = "Feature Extraction";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_Labels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.imageBox_Face)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_PCADim)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItem_OpenDir;
        private System.Windows.Forms.ToolStripMenuItem menuItem_Quit;
        private System.Windows.Forms.Panel panel1;
        private Emgu.CV.UI.ImageBox imageBox_Face;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button_Prev;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_StartML;
        private System.Windows.Forms.CheckBox checkBox_ShowIntraClassMean;
        private System.Windows.Forms.CheckedListBox checkedListBox_Extractors;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox_LastUpdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ItemFilename;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FolderBrowserDialog openDataDirDialog;
        private System.Windows.Forms.DataGridView dataGridView_Labels;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColumn_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn gridColumn_Value;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckedListBox checkedListBox_Labels;
        private System.Windows.Forms.Button button_LoadExtractor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBox_CompressionMethods;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numericUpDown_PCADim;
        private System.Windows.Forms.CheckBox checkBox_UseCompression;
    }
}

