using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;
using FeatureExtraction.FeaturePoints;
using Emgu.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.ML;
using Emgu.CV.ML.Structure;

namespace FeatureExtraction.ML
{
    class AdaBoost : IMLMethod
    {
        public Boost _boost;
        private IEnumerable<string> _using_labels;
        private Dictionary<string, string[]> _label_defs;
        


        public AdaBoost()
        {
            _boost = new Boost();
        }

        public void Initialize(IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs)
        {
            this._using_labels = using_labels;
            this._label_defs = label_defs;
        }


        public string Name
        {
            get { return "AdaBoost"; }
        }
        public string Author
        {
            get { return "ムラシタ"; }
        }
        public string Help
        {
            get { return "AdaBoostによる学習を行います。"; }
        }

        public IEnumerable<string> Train(string dirname, IEnumerable<FeaturePointData> data)
        {
            List<string> errors = new List<string>();

            try
            {
                //学習用データを配列にコピーする。
                Matrix<float> train_data = new Matrix<float>(data.Count(), data.ElementAt(0).FeaturePoints.Count());
                for (int i = 0; i < data.Count(); i++)
                {
                    for (int j = 0; j < data.ElementAt(0).FeaturePoints.Count(); j++)
                    {
                        train_data[i, j] = (float)data.ElementAt(i).FeaturePoints[j];
                    }
                }
                //ラベルデータをコピーする
                Matrix<float> responses = new Matrix<float>(data.Count(), 1);
                IEnumerable<string> all_labels = _getAllLabels(_using_labels, _label_defs);
                for (int i = 0; i < data.Count(); i++)
                {
                    var t = _indexOf(_getLabel(data.ElementAt(i), _using_labels), all_labels);
                    if (t < 0)
                    {
                        throw new Exception("label not found... BUG!");
                    }
                    responses[i, 0] = t;
                }
                //データタイプの指定
                Matrix<byte> var_type = new Matrix<byte>(data.ElementAt(0).FeaturePoints.Count(), 1);
                var_type.SetValue(0, null);
                var_type[var_type.Rows - 1, 0] = 1;
                //仕様する特徴量 = 全て
                Matrix<byte> var_idx = new Matrix<byte>(data.ElementAt(0).FeaturePoints.Count(), 1);
                var_idx.SetValue(1);
                var_idx[0, 0] = 0;
                //使用するサンプル = 全て
                Matrix<byte> sample_idx = new Matrix<byte>(1, data.Count());
                sample_idx.SetValue(1);
                sample_idx[0, 0] = 0;
                Matrix<byte> missing_mask = new Matrix<byte>(train_data.Size);
                missing_mask.SetValue(0);

                //学習条件の設定
                var boost_params = new MCvBoostParams();
                boost_params.boostType = Emgu.CV.ML.MlEnum.BOOST_TYPE.REAL;
                boost_params.weightTrimRate = 0.95;
                boost_params.weakCount = 100;
                boost_params.maxDepth = 5;
                boost_params.minSampleCount = 10;
                boost_params.regressionAccuracy = 0;
                boost_params.maxCategories = 15;
                boost_params.useSurrogates = false;
                //トレーニング開始


                _boost.Train(train_data, Emgu.CV.ML.MlEnum.DATA_LAYOUT_TYPE.ROW_SAMPLE, responses, var_idx, sample_idx, var_type, missing_mask, boost_params, false);
                return new List<string>();
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message + Environment.NewLine + ex.StackTrace);
                return errors;
            }
        }

        public IEnumerable<string> Predict(string filename, IEnumerable<FeaturePointData> data, out IEnumerable<string> results)
        {
            try
            {
                var ret = new List<string>();
                var all_labels = _getAllLabels(_using_labels, _label_defs);
                foreach (var item in data)
                {
                    Matrix<float> sample = new Matrix<float>(item.FeaturePoints.Count(), 1);
                    for (int i = 0; i < sample.Rows; i++)
                    {
                        sample[i, 0] = (float)item.FeaturePoints[i];
                    }
                    var t = _boost.Predict(sample, null, null, MCvSlice.WholeSeq, false);
                    ret.Add(all_labels.ElementAt((int)t));
                }
                results = ret;
                return new List<string>();
            }
            catch (Exception ex)
            {
                results = new List<string>();
                return new List<string> { ex.Message };
            }
        }

        private IEnumerable<string> _getAllLabels(IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs)
        {
            IEnumerable<string> all_labels = label_defs[using_labels.ElementAt(0)];
            for (int i = 1; i < using_labels.Count(); i++)
            {
                all_labels = _combination(all_labels, label_defs[using_labels.ElementAt(1)]);
            }
            all_labels = all_labels.OrderBy((l) => l);
            return all_labels;
        }

        private int _indexOf(string label, IEnumerable<string> all_labels)
        {
            var result = all_labels.Select((l, i) => new { l, i }).Where((item) => item.l == label).Select((item) => item.i);
            if (result.Count() > 0)
            {
                return result.ElementAt(0);
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 現在選択されているラベル定義に基づいて
        /// 項目をラベル付けする。
        /// ex : is_smiling と has_glassesが選択されている場合
        ///    smiling x no_glasses
        /// </summary>
        private string _getLabel(FeaturePointData data, IEnumerable<string> using_labels)
        {
            var lbls = data.Data.Labels.Where((item) => using_labels.Contains(item.Name)).Select((item) => item.Value);
            return String.Join(" x ", lbls.ToArray());
        }

        private IEnumerable<string> _combination(IEnumerable<string> c1, IEnumerable<string> c2)
        {
            return from i1 in c1
                   from i2 in c2
                   select i1 + " x " + i2;
        }
    }
}
