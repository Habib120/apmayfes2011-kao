using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// 配列の最小値と最大値が
    /// 指定された区間の中に収まるように線形にスケーリングする
    /// </summary>
    public class NormalizerSimple : INormalizer
    {
        protected double range_min;
        protected double range_max;

        public NormalizerSimple(double min, double max)
        {
            this.range_min = min;
            this.range_max = max;
        }

        /// <summary>
        /// 値を正規化する
        /// </summary>
        /// <param name="values"></param>
        public void Normalize(List<double> values)
        {
            double max = values.Max();
            double min = values.Min();
            for (int i = 0; i < values.Count(); i++)
            {
                values[i] = (range_max - range_min) * (values[i] - min) / (max - min) + range_min;
            }
        }
    }
}
