using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// 何も正規化を実行しない
    /// </summary>
    class NormalizerNone : INormalizer
    {
        public void Normalize(List<double> values)
        {
        }
    }
}
