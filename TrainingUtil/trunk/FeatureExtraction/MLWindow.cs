using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FeatureExtraction.FeaturePoints;
using FeatureExtraction.Extractors;
using FeatureExtraction.ML;
using FeatureExtraction.Faces;

namespace FeatureExtraction
{
    public partial class MLWindow : Form
    {
        public IExtractor Extractor { get; protected set; }
        public IEnumerable<string> UsingLabels { get; protected set; }

        public MLWindow()
        {
            InitializeComponent();
            if (TrainingDataTable.Items.Count == 0)
                throw new InvalidOperationException("Face data is not loaded");
            if (!TrainingPointTable.Loaded)
                throw new InvalidOperationException("Extractor is not defined");
            if (TrainingPointTable.UsingLabels.Count == 0)
                throw new InvalidOperationException("You must select at least one label for classification");

        }

        public FaceDataTable TrainingDataTable
        {
            get
            {
                return FaceDataTable.GetInstance("training");
            }
        }

        public FaceDataTable TestDataTable
        {
            get
            {
                return FaceDataTable.GetInstance("test");
            }
        }

        public FeaturePointDataTable TrainingPointTable
        {
            get
            {
                return FeaturePointDataTable.GetInstance("training");
            }
        }

        public FeaturePointDataTable TestPointTable
        {
            get
            {
                return FeaturePointDataTable.GetInstance("test");
            }
        }

        private void MLWindow_Load(object sender, EventArgs e)
        {
            comboBox_MLAlgorithm.Items.Clear();
            foreach (var method in MLMethodTable.GetAll())
            {
                method.Initialize(TrainingPointTable.UsingLabels, TrainingDataTable.LabelDefinitions);
                comboBox_MLAlgorithm.Items.Add(new ComboBoxItemMLAlgorithm
                {
                    Name = method.Name,
                    Method = method,
                });
            }
            if (comboBox_MLAlgorithm.Items.Count > 0)
            {
                comboBox_MLAlgorithm.SelectedIndex = 0;
            }
        }

        private class ComboBoxItemMLAlgorithm
        {
            public string Name { get; set; }
            public IMLMethod Method { get; set; }
            public override string ToString()
            {
                return this.Name;
            }
        }

        private IMLMethod SelectedMLMethod
        {
            get
            {
                return (comboBox_MLAlgorithm.Items[comboBox_MLAlgorithm.SelectedIndex] as ComboBoxItemMLAlgorithm).Method;
            }
        }

        private void button_Train_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fDialog = new FolderBrowserDialog())
            {
                fDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadingWindow lWindow = new LoadingWindow();
                    lWindow.StartPosition = FormStartPosition.Manual;
                    lWindow.Left = this.Left + (this.Width - lWindow.Width) / 2;
                    lWindow.Top = this.Top + (this.Height - lWindow.Height) / 2;
                    lWindow.Owner = this;
                    lWindow.Show();
                    lWindow.SetProgressBarStyle(ProgressBarStyle.Marquee);
                    lWindow.SetMessage("学習しています...");
                    lWindow.SetTitle(SelectedMLMethod.Name);
                    BackgroundWorker bw = new BackgroundWorker();
                    var method = SelectedMLMethod;
                    var dir = fDialog.SelectedPath;
                    bw.DoWork += (s, ev) =>
                        {
                            try
                            {
                                long tick1 = Environment.TickCount;
                                //デバッグ用にテストデータに対して特徴量を計算し、結果を保存する
                                TrainingPointTable.Extractor.ExtractForDebug(dir + @"\featurepoints.dump", FaceData.GetTestData());
                                //学習
                                var errors = method.Train(dir, TrainingPointTable.Items);
                                long tick2 = Environment.TickCount;
                                if (errors.Count() > 0)
                                {
                                    AlertdDialog.Error("学習中にエラーが発生しました。", errors);
                                }
                                else
                                {
                                    double processtimems = (tick2 - tick1) / 1000.0;
                                    this.Invoke(new MethodInvoker(() =>
                                        {
                                            label_Message.Text = String.Format("Training completed. Process time : {0:F2}ms", processtimems);
                                        }));
                                }

                            }
                            catch (Exception ex)
                            {
                                AlertdDialog.Error("学習中にエラーが発生しました。", ex);
                            }
                        };
                    bw.RunWorkerCompleted += (s, ev) =>
                        {
                            lWindow.Dispose();
                            this.Enabled = true;
                            this.Focus();
                            AlertdDialog.Notify("学習が完了しました.");
                        };
                    bw.RunWorkerAsync();
                    this.Enabled = false;
                }
                
            }
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fDialog = new FolderBrowserDialog())
            {
                try
                {
                    fDialog.RootFolder = Environment.SpecialFolder.MyDocuments;
                    if (fDialog.ShowDialog() == DialogResult.OK)
                    {
                        TestDataTable.Load(fDialog.SelectedPath);
                        TestPointTable.Load(TestDataTable.Items, true);
                        TestPointTable.SetExtractor(TrainingPointTable.Extractor);
                        TestPointTable.SetUsingLabels(TrainingPointTable.UsingLabels);
                        TestPointTable.SetCompressor(TrainingPointTable.Compressor);
                        TestPointTable.SetCompressionDim(TrainingPointTable.CompressionDim);
                        TestPointTable.SetCompressionEnabled(TrainingPointTable.CompressionEnabled);
                        TestPointTable.Compressor.Loaded = true;
                        textBox_TestDataDir.Text = fDialog.SelectedPath;
                        foreach (var item in TestDataTable.Items)
                        {
                            dataGridView_Results.DataSource = TestPointTable.Items.Select((i) => new DataGridViewItemPredictionResults
                            {
                                FileName = i.Data.ImageName,
                                TrueLabel = GetLabel(i, TestPointTable.UsingLabels),
                                PredictedLabel = "",
                                IsCorrect = false,
                                Data = i.Data,
                            }).ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AlertdDialog.Error("テストデータの読み込み中にエラーが発生しました。", ex);
                }
            }
        }

        /// <summary>
        /// 現在選択されているラベル定義に基づいて
        /// 項目をラベル付けする。
        /// ex : is_smiling と has_glassesが選択されている場合
        ///    smiling x no_glasses
        /// </summary>
        private string GetLabel(FeaturePointData data, IEnumerable<string> using_labels)
        {
            var lbls = data.Data.Labels.Where((item) => using_labels.Contains(item.Name)).Select((item) => item.Value);
            return String.Join(" x ", lbls.ToArray());
        }

        private string GetLabel(int index, IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs)
        {
            IEnumerable<string> all_labels = label_defs[using_labels.ElementAt(0)];
            for (int i = 1; i < using_labels.Count(); i++) 
            {
                all_labels = Conbination(all_labels, label_defs[using_labels.ElementAt(1)]);
            }
            all_labels = all_labels.OrderBy((label) => label);
            return all_labels.ElementAt(index);
        }

        private IEnumerable<string> Conbination(IEnumerable<string> c1, IEnumerable<string> c2)
        {
            return from i1 in c1
                   from i2 in c2
                   select i1 + " x " + i2;
        }

        private void button_Predict_Click(object sender, EventArgs e)
        {
            if (!TestDataTable.Loaded)
                return;
            LoadingWindow lWindow = new LoadingWindow();
            lWindow.StartPosition = FormStartPosition.Manual;
            lWindow.Left = this.Left + (this.Width - lWindow.Width) / 2;
            lWindow.Top = this.Top + (this.Height - lWindow.Height) / 2;
            lWindow.Owner = this;
            lWindow.Show();

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (s, ev) =>
            {
                if (!TestPointTable.FeatureCalculated)
                {
                    lWindow.SetMessage("特徴量を計算しています...");
                    TestPointTable.RecalcAll(lWindow.SetMessage, lWindow.SetProgress);
                    lWindow.SetMessage("特徴量の計算が完了しました。");
                }
                IEnumerable<string> results;
                lWindow.SetProgressBarStyle(ProgressBarStyle.Marquee);
                lWindow.SetMessage("予測を実行しています...");
                long tick1 = Environment.TickCount;
                var selected_method = (IMLMethod)this.Invoke(new Func<IMLMethod>(() => SelectedMLMethod));
                var errors = selected_method.Predict(textBox_TestDataDir.Text, TestPointTable.Items, out results);
                long tick2 = Environment.TickCount;

                if (errors.Count() > 0 && results.Count() == 0)
                {
                    throw new Exception(String.Join(Environment.NewLine, errors.ToArray()));
                }
                else if (errors.Count() > 0)
                {
                    AlertdDialog.Error("予測中、以下の警告が表示されました", errors);
                }

                lWindow.SetMessage("予測結果を反映しています...");
                this.Invoke(new MethodInvoker(() =>
                    {
                        var source = dataGridView_Results.DataSource as IEnumerable<DataGridViewItemPredictionResults>;
                        int count = 0;
                        foreach (var item in results.Select((result, i) => new { result, i }))
                        {
                            var target = source.ElementAt(item.i);
                            target.PredictedLabel = item.result;
                            target.IsCorrect = target.PredictedLabel == target.TrueLabel;
                            if (target.IsCorrect)
                                count++;
                        }
                        var groups = source.GroupBy((t) => t.TrueLabel);
                        var correctness_by_label = new Dictionary<string, double>();
                        foreach (var group in groups)
                        {
                            correctness_by_label[group.First().TrueLabel] = group.Where((t) => t.IsCorrect).Count() / (double)group.Count() * 100;
                        }
                        var by_label_str = String.Join(", ", correctness_by_label.Select((kvp) => String.Format("{0} : {1:F2}%", kvp.Key, kvp.Value)).ToArray());

                        dataGridView_Results.Refresh();

                        double correctness = count / (double)results.Count() * 100;
                        double e_ms = (tick2 - tick1) / 1000.0;
                        string message = String.Format("Calcuration time : {0:F2}s, Correctness : {1:F2}% ({2}) ", e_ms, correctness, by_label_str);
                        label_Message.Text = message;
                    }));
            };
            bw.RunWorkerCompleted += (s, ev) =>
            {
                lWindow.Dispose();
                if (ev.Cancelled)
                {
                    AlertdDialog.Notify("キャンセルされました");
                }
                else if (ev.Error != null)
                {
                    AlertdDialog.Error("予測に失敗しました。", ev.Error);
                }
                else
                {
                    AlertdDialog.Notify("予測が完了しました.");
                }
                
                this.Enabled = true;
                this.Focus();
            };
            bw.RunWorkerAsync();
            this.Enabled = false;
            
        }

        private class DataGridViewItemPredictionResults
        {
            public string FileName {get; set;}
            public string TrueLabel { get; set;}
            public string PredictedLabel {get; set;}
            public bool IsCorrect {get; set;}
            public FaceData Data;
        }

    }
}
