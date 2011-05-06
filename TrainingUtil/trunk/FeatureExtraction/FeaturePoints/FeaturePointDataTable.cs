using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FeatureExtraction.Faces;
using FeatureExtraction.Extractors;
using FeatureExtraction.Compressors;

namespace FeatureExtraction.FeaturePoints
{
    public class FeaturePointDataTable
    {
        public bool Loaded
        {
            get
            {
                return (Items != null && Items.Count > 0) && !IsBusy;
            }
        }
        static Dictionary<string, FeaturePointDataTable> instances = new Dictionary<string,FeaturePointDataTable>();
        public List<FeaturePointData> Items {get; protected set;}
        public List<string> UsingLabels {get; protected set;}
        public IExtractor Extractor { get; protected set; }
        public ICompressor Compressor { get; protected set; }
        public bool CompressionEnabled { get; protected set; }
        public int CompressionDim { get; protected set; }
        public Dictionary<string, FeaturePoints> IntraClassMeans { get; protected set; }
        public Dictionary<string, FeaturePoints> IntraClassVariances { get; protected set; }
        public bool IsBusy { get; protected set; }
        public bool IsTest { get; protected set; }

        public bool FeatureCalculated { get; protected set; }
        public bool StatsCalculated { get; protected set; }

        public static FeaturePointDataTable GetInstance(string key)
        {
            if (!instances.ContainsKey(key)){
                instances[key] = new FeaturePointDataTable();
            }
            return instances[key];
        }

        public void Load(IEnumerable<FaceData> faces, bool is_test = false)
        {
            Items = new List<FeaturePointData>();
            foreach (var face in faces){
                Items.Add(new FeaturePointData(
                    face,
                    new FeaturePoints()
                ));
            }
            IsTest = is_test;
            FeatureCalculated = false;
        }

        public void SetExtractor(IExtractor extractor)
        {
            this.Extractor = extractor;
            FeatureCalculated = false;
        }

        public void SetCompressor(ICompressor compressor)
        {
            this.Compressor = compressor;
            FeatureCalculated = false;
        }

        public void SetCompressionDim(int dim)
        {
            this.CompressionDim = dim;
            FeatureCalculated = false;
        }

        public void SetCompressionEnabled(bool enabled)
        {
            this.CompressionEnabled = enabled;
            FeatureCalculated = false;
        }

        public void SetUsingLabels(IEnumerable<string> collection)
        {
            this.UsingLabels = collection.ToList();
            FeatureCalculated = false;
        }

        public void RecalcAll(Action<string> msgCallback = null, Action<double> progressCallback = null)
        {
            if (!Loaded)
                return;
            try
            {
                IsBusy = true;
                FeaturePointLoader.Counter = 0;
                int items_to_load = Items.Count();
                foreach (var item in Items)
                {
                    var worker = new FeaturePointLoader(Extractor, item);
                    ThreadPool.QueueUserWorkItem(new WaitCallback(worker.Load));
                }
                while (true)
                {
                    lock (FeaturePointLoader.LockObject)
                    {
                        if (FeaturePointLoader.Counter < items_to_load)
                        {
                            if (msgCallback != null)
                                msgCallback(String.Format("特徴量を計算しています... {0} / {1} 完了", FeaturePointLoader.Counter, items_to_load));
                            if (progressCallback != null)
                                progressCallback(FeaturePointLoader.Counter / (double)items_to_load);
                        }
                        else
                        {
                            break;
                        }
                    }
                    Thread.Sleep(60);
                }
                if (CompressionEnabled)
                {
                    if (msgCallback != null)
                        msgCallback("次元を圧縮中...圧縮に必要な情報を集めています。");
                    if (!IsTest) 
                        this.Compressor.Load(this.Items);
                    if (msgCallback != null)
                        msgCallback("次元を圧縮中...特徴量を圧縮しています。");
                    this.Compressor.Compress(this.Items, CompressionDim);
                    if (msgCallback != null)
                        msgCallback("特徴量の圧縮が完了しました。");
                }
                if (msgCallback != null)
                    msgCallback("各統計量を計算しています...");
                IsBusy = false;
                RecalcStatisticalItems();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                IsBusy = false;
                FeatureCalculated = true;
            }
        }

        /// <summary>
        /// 統計関係の情報のみを更新
        /// </summary>
        public void RecalcStatisticalItems()
        {
            if (!Loaded)
                return;
            this.IntraClassMeans = this.CalcIntraClassMean();
            //this.IntraClassVariances = this.CalcIntraClassVariance();
        }

        /// <summary>
        /// クラス内平均を計算
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, FeaturePoints> CalcIntraClassMean()
        {
            Dictionary<string, FeaturePoints> ret;
            var means = from item in Items
                        let label = GetLabel(item)
                        group new { item, label } by label into g
                        where g.Count() > 0
                        select new
                        {
                            Label = g.ElementAt(0).label,
                            Mean = Mean(from gg in g
                                        select gg.item.FeaturePoints),
                        };
            ret = new Dictionary<string, FeaturePoints>();
            foreach (var m in means)
            {
                ret.Add(m.Label, m.Mean);
            }
            return ret;
        }

        private FeaturePoints Mean(IEnumerable<FeaturePoints> items)
        {
            if (items.Count() == 0)
                return new FeaturePoints(0);
            FeaturePoints ret = new FeaturePoints(items.ElementAt(0).Count());
            foreach (var item in items)
            {
                ret = ret + item;
            }
            ret = ret / items.Count();
            return ret;
        }

        /// <summary>
        /// クラス内分散を計算
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, FeaturePoints> CalcIntraClassVariance()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 現在選択されているラベル定義に基づいて
        /// 項目をラベル付けする。
        /// ex : is_smiling と has_glassesが選択されている場合
        ///    smiling x no_glasses
        /// </summary>
        private string GetLabel(FeaturePointData data)
        {
            var lbls = data.Data.Labels.Where((item) => UsingLabels.Contains(item.Name)).Select((item) => item.Value);
            return String.Join(" x ", lbls.ToArray());
        }
    }

    public class FeaturePointLoader
    {
        public IExtractor Extractor { get; set; }
        public FeaturePointData Item { get; set; }
        public static int Counter { get; set; }
        public static Object LockObject = new Object();


        public FeaturePointLoader(IExtractor extractor, FeaturePointData item)
        {
            this.Extractor = extractor;
            this.Item = item;
        }

        public void Load(Object obj)
        {
            lock (Item)
            {
                Item.FeaturePoints.Clear();
                Item.FeaturePoints.AddRange(Extractor.Extract(Item.Data));
            }
            lock (LockObject)
            {
                Counter++;
            }
        }
    }
}
