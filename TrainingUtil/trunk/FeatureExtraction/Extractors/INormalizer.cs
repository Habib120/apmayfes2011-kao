using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// 値を正規化する仕方を定める
    /// </summary>
    public interface INormalizer
    {
        /// <summary>
        /// 値を適切な方法で正規化する
        /// </summary>
        /// <param name="values"></param>
        void Normalize(List<double> values);
    }
}
