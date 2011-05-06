using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using FeatureExtraction.Extractors;
using FeatureExtraction.Faces;
using FeatureExtraction.FeaturePoints;
using FeatureExtraction.Compressors;

namespace FeatureExtraction
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private FeaturePointDataTable PointTable
        {
            get
            {
                return FeaturePointDataTable.GetInstance("training");
            }
        }

        private FaceDataTable DataTable
        {
            get
            {
                return FaceDataTable.GetInstance("training");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDefinedExtractors();
            LoadDefinedCompressors();
            InitializeGraph();
        }

        private void InitializeGraph()
        {
            // Enable range selection and zooming end user interface
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = true;
        }

        private void LoadDefinedExtractors()
        {
            //登録されている特徴抽出器を読み込む
            IEnumerable<IExtractor> definedExtractors = AssemblyExtractorTable.GetAll();
            definedExtractors = definedExtractors.Concat(BinaryExtractorTable.GetAll());
            checkedListBox_Extractors.Items.Clear();
            checkedListBox_Extractors.Items.AddRange(
                definedExtractors.Select((extractor)
                => new CheckedListBoxExtractorItem
                {
                    Name = extractor.Name,
                    Description = extractor.Help + Environment.NewLine + "作成者:" + extractor.Author,
                    Extractor = extractor
                }).ToArray());

            checkedListBox_Extractors.MouseClick += (sender, e) =>
                {
                    var target = (CheckedListBoxExtractorItem)checkedListBox_Extractors.Items[checkedListBox_Extractors.IndexFromPoint(e.Location)];
                    toolTip1.SetToolTip(checkedListBox_Extractors, target.Description);
                };
            checkedListBox_Extractors.MouseLeave += (sender, ev) =>
                {
                    toolTip1.Hide(this);
                };
        }


        private void LoadDefinedCompressors()
        {
            //登録されている特徴抽出器を読み込む
            IEnumerable<ICompressor> definedCompressors = CompressorTable.GetAll();
            comboBox_CompressionMethods.Items.Clear();
            comboBox_CompressionMethods.Items.AddRange(
                definedCompressors.Select((compressor)
                => new ComboBoxCompressorItem
                {
                    Name = compressor.Name,
                    Description = compressor.Help + Environment.NewLine + "作成者:" + compressor.Author,
                    Compressor = compressor
                }).ToArray());
            comboBox_CompressionMethods.MouseHover += (sender, e) =>
                {
                    var target = (ComboBoxCompressorItem)comboBox_CompressionMethods.SelectedItem;
                    toolTip1.SetToolTip(comboBox_CompressionMethods, target.Description);
                };
            comboBox_CompressionMethods.MouseLeave += (sender, ev) =>
                {
                    toolTip1.Hide(this);
                };
            comboBox_CompressionMethods.SelectedIndex = 0;
            numericUpDown_PCADim.Value = 20;
        }

        public class ComboBoxCompressorItem
        {
            public string Name;
            public string Description;
            public ICompressor Compressor;
            public override string ToString()
            {   
                return this.Name;
            }
        }

        public class CheckedListBoxExtractorItem
        {
            public string Name;
            public string Description;
            public IExtractor Extractor;
            public override string ToString()
            {
                return this.Name;
            }
        }


        #region 使用する特徴量の取得

        /// <summary>
        /// 現在の選択に基づいたExtractorSchema
        /// </summary>
        private IExtractor CurrentExtractor
        {
            get
            {
                return new ExtractorSchema(_pickupExtractors(checkedListBox_Extractors.CheckedIndices.Cast<int>()));
            }
        }

        /// <summary>
        /// checkedListBoxのItemCheckに連動して動作。
        /// checkedListBoxにはチェック完了時を表すイベントが存在しないため、
        /// 選択状況の変化の情報をもとにチェック完了時のExtractorの選択状況を取得し、
        /// その選択に基づいたExtractorSchemaを返す。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private IExtractor GetNextExtractor(int index, CheckState oldValue, CheckState newValue)
        {
            List<int> nextCheckedIndices = checkedListBox_Extractors.CheckedIndices.Cast<int>().ToList();
            if (nextCheckedIndices.Contains(index) && newValue != CheckState.Checked)
            {
                nextCheckedIndices.Remove(index);
            }
            else if (!nextCheckedIndices.Contains(index) && newValue == CheckState.Checked)
            {
                nextCheckedIndices.Add(index);
            }
            return new ExtractorSchema(_pickupExtractors(nextCheckedIndices));
        }

        /// <summary>
        /// 全Extractorから指定されたインデックスのものを取得して返す。
        /// </summary>
        /// <param name="indices"></param>
        /// <returns></returns>
        private IEnumerable<IExtractor> _pickupExtractors(IEnumerable<int> indices)
        {
            foreach (var i in indices)
            {
                yield return ((CheckedListBoxExtractorItem)checkedListBox_Extractors.Items[(int)i]).Extractor;
            }
        }
        #endregion

        #region 学習に使用するラベルの取得

        /// <summary>
        /// checkedListBoxのItemCheckに連動して動作。
        /// checkedListBoxにはチェック完了時を表すイベントが存在しないため、
        /// 選択状況の変化の情報をもとにチェック完了時のExtractorの選択状況を取得し、
        /// その選択に基づいたExtractorSchemaを返す。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private IEnumerable<string> GetNextLabels(int index, CheckState oldValue, CheckState newValue)
        {
            List<int> nextCheckedIndices = checkedListBox_Labels.CheckedIndices.Cast<int>().ToList();
            if (nextCheckedIndices.Contains(index) && newValue != CheckState.Checked)
            {
                nextCheckedIndices.Remove(index);
            }
            else if (!nextCheckedIndices.Contains(index) && newValue == CheckState.Checked)
            {
                nextCheckedIndices.Add(index);
            }
            foreach (int idx in nextCheckedIndices)
            {
                yield return checkedListBox_Labels.Items[idx].ToString();
            }
        }

        /// <summary>
        /// 現在選択されているラベルのリスト
        /// </summary>
        private IEnumerable<string> SelectedLabels
        {
            get
            {
                return from item in checkedListBox_Labels.Items.Cast<string>().Select((name, i) => new { name, i })
                       where checkedListBox_Labels.GetItemCheckState(item.i) == CheckState.Checked
                       select item.name;
            }
        }
        #endregion

        private void menuItem_OpenDir_Click(object sender, EventArgs e)
        {
            try
            {
                openDataDirDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openDataDirDialog.ShowDialog() == DialogResult.OK)
                {
                    string datadir = openDataDirDialog.SelectedPath;
                    DataTable.Load(datadir);
                    ShowDirectoryInfo();
                    this.PointTable.Load(DataTable.Items);
                    this.PointTable.SetExtractor(CurrentExtractor);
                    this.PointTable.SetUsingLabels(this.SelectedLabels);
                    listBox1.SelectedIndex = 0;
                }
                
            }
            catch(Exception ex)
            {
                AlertdDialog.Error("データのロードに失敗しました。", ex);
            }
        }

        /// <summary>
        /// 読み込んだ顔ディレクトリについての情報表示を読み込み
        /// 　・登録されている顔画像のリスト
        /// 　・定義されているラベル情報
        /// を表示する 
        /// </summary>
        private void ShowDirectoryInfo()
        {
            var items = DataTable.Items;
            var label_defs = DataTable.LabelDefinitions;
            this.SuspendLayout();
            if (items.Count() == 0)
            {
                return;
            }
            listBox1.Items.Clear();
            checkedListBox_Labels.Items.Clear();
            foreach (var item in items)
            {
                listBox1.Items.Add(item.ImageName);
            }
            foreach (var item in label_defs)
            {
                checkedListBox_Labels.Items.Add(item.Key);
            }
            ShowFaceData(items.ElementAt(0));
            this.ResumeLayout();
        }

        /// <summary>
        /// 顔画像についての情報を読み込んで表示
        /// </summary>
        /// <param name="item"></param>
        private void ShowFaceData(FaceData item)
        {
            
            //画像を設定
            var center = imageBox_Face.Location + new Size(imageBox_Face.Width / 2, imageBox_Face.Height / 2);
            var img_size = item.Image.Size;
            img_size.Width = img_size.Width > 250 ? 250 : img_size.Width;
            img_size.Height = img_size.Height > 235 ? 235 : img_size.Height;
            imageBox_Face.Location = new Point(center.X - img_size.Width / 2, center.Y - img_size.Height / 2);
            imageBox_Face.Size = img_size;
            imageBox_Face.Image = item.Image;
            this.Refresh();

            //情報を設定
            textBox_ItemFilename.Text = item.ImagePath;
            textBox_LastUpdate.Text = item.LastUpdate;
            var griditems = item.Labels.Select((l) => new DataGridViewLabelsItem
            {
                Name = l.Name,
                Value = l.Value,
            }).ToArray();
            this.Refresh();
            dataGridView_Labels.DataSource = griditems;
        }

        /// <summary>
        /// 特徴量をグラフに表示する。
        /// </summary>
        /// <param name="item"></param>
        private void UpdateChart(int index)
        {
            var table = this.PointTable;
            chart1.Series.Clear();
            if (index < 0)
                return;
            if (String.IsNullOrWhiteSpace(table.Extractor.Name))
                return;
            Series mainPlot = new Series(table.Extractor.Name);
            mainPlot.ChartType = SeriesChartType.FastLine;
            mainPlot.BorderWidth = 4;
            mainPlot.SetCustomProperty("DrawingStyle", "LightToDark");
            chart1.Series.Add(mainPlot);
            foreach (var point in table.Items[index].FeaturePoints)
            {
                chart1.Series[table.Extractor.Name].Points.AddY(point);
            }
            if (checkBox_ShowIntraClassMean.Checked)
            {
                var means = table.IntraClassMeans;
                foreach (var item in means)
                {
                    if (item.Key == "")
                        break;
                    chart1.Series.Add(item.Key);
                    chart1.Series[item.Key].Points.Clear();
                    chart1.Series[item.Key].ChartType = SeriesChartType.FastLine;
                    chart1.Series[item.Key].SetCustomProperty("DrawingStyle", "LightToDark");
                    foreach (var point in item.Value)
                    {
                        chart1.Series[item.Key].Points.AddY(point);
                    }
                }
            }
        }

        private void UpdateSelectedChart()
        {
            UpdateChart(listBox1.SelectedIndex);
        }


        #region ファイルリスト操作
        private class DataGridViewLabelsItem
        {
            public string Name {get; set;}
            public string Value {get; set;}
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ShowFaceData(DataTable.Items.ElementAt(listBox1.SelectedIndex));
            DataTable.Items.ElementAt((listBox1.SelectedIndex + listBox1.Items.Count - 1) % listBox1.Items.Count).UnloadImage();
            DataTable.Items.ElementAt((listBox1.SelectedIndex + 1) % listBox1.Items.Count).UnloadImage();
            UpdateSelectedChart();
        }

        private void button_Prev_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0) 
                listBox1.SelectedIndex = (listBox1.SelectedIndex + listBox1.Items.Count - 1) % listBox1.Items.Count;
        }

        private void button_Next_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = (listBox1.SelectedIndex + 1) % listBox1.Items.Count;
        }
        #endregion

        private void checkedListBox_Labels_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!DataTable.Loaded)
                return;
            else if (!this.PointTable.Loaded)
                return;
            PointTable.SetUsingLabels(GetNextLabels(e.Index, e.CurrentValue, e.NewValue));
            PointTable.RecalcStatisticalItems();
            UpdateSelectedChart();
        }


        private void checkedListBox_Extractors_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (!DataTable.Loaded)
                return;
            PointTable.SetExtractor(GetNextExtractor(e.Index, e.CurrentValue, e.NewValue));
        }

        private void checkBox_ShowIntraClassMean_CheckedChanged(object sender, EventArgs e)
        {
            if (this.DataTable.Items.Count() == 0)
                return;
            else if (!this.PointTable.Loaded)
                return;
            UpdateSelectedChart();
        }


        private void button_StartML_Click(object sender, EventArgs e)
        {
            if (!this.DataTable.Loaded || !this.PointTable.Loaded)
            {
                return;
            }
            if (this.SelectedLabels.Count() == 0)
            {
                AlertdDialog.Notify("少なくとも一つラベルを選択してください！");
                return;
            }

            if (this.CurrentExtractor.Name == "")
            {
                AlertdDialog.Notify("少なくとも一つ特徴量を選択してください！");
                return;
            }
            if (!this.PointTable.FeatureCalculated)
            {
                if (AlertdDialog.Question("前回計算されてから特徴量が変更されているため、特徴量を再計算する必要があります。よろしいですか？") == DialogResult.Yes)
                {
                    CalculateFeatures(() =>
                    {
                        using (MLWindow mWindow = new MLWindow())
                        {
                            mWindow.ShowDialog();
                        }
                    });
                    return;
                }
            }
            using (MLWindow mWindow = new MLWindow())
            {
                mWindow.ShowDialog();
            }
        }

        private void menuItem_Quit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_LoadExtractor_Click(object sender, EventArgs e)
        {
            CalculateFeatures();
        }

        private void CalculateFeatures(Action action = null)
        {
            LoadingWindow lWindow = new LoadingWindow();
            lWindow.StartPosition = FormStartPosition.Manual;
            lWindow.Left = this.Left + (this.Width - lWindow.Width) / 2;
            lWindow.Top = this.Top + (this.Height - lWindow.Height) / 2;
            lWindow.Owner = this;
            lWindow.Show();
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, ev) =>
            {
                PointTable.RecalcAll(lWindow.SetMessage, lWindow.SetProgress);
            };
            bw.RunWorkerCompleted += (s, ev) =>
            {
                if (ev.Cancelled)
                {
                    AlertdDialog.Notify("キャンセルされました");
                }
                if (ev.Error != null)
                {
                    AlertdDialog.Error("特徴量のロードに失敗しました。", ev.Error);
                }
                ShowFaceData(DataTable.Items.ElementAt(listBox1.SelectedIndex));
                UpdateSelectedChart();
                this.Focus();
                this.Enabled = true;
                lWindow.Dispose();
                if (action != null)
                    action();
            };
            bw.RunWorkerAsync();
            this.Enabled = false;
        }

        private void comboBox_CompressionMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            var target = (ComboBoxCompressorItem)comboBox_CompressionMethods.SelectedItem;
            PointTable.SetCompressor(target.Compressor);
        }

        private void checkBox_UseCompression_CheckedChanged(object sender, EventArgs e)
        {
            PointTable.SetCompressionEnabled(checkBox_UseCompression.Checked);
        }

        private void numericUpDown_PCADim_ValueChanged(object sender, EventArgs e)
        {
            PointTable.SetCompressionDim((int)numericUpDown_PCADim.Value);
        }

        private void numericUpDown_PCADim_Leave(object sender, EventArgs e)
        {
            PointTable.SetCompressionDim((int)numericUpDown_PCADim.Value);
        }

        

    }
}