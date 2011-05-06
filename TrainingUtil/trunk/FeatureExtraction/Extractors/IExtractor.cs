using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// 特徴抽出器を表すインターフェース
    /// </summary>
    public interface IExtractor
    {

        /// <summary>
        /// 画像ファイルから特徴を抽出する
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        IEnumerable<double> Extract(Faces.FaceData data);
        

        /// <summary>
        /// この特徴抽出器が抽出する特徴についての簡単な説明
        /// </summary>
        string Help { get;}

        /// <summary>
        /// この特徴抽出器の名前
        /// </summary>
        string Name { get; }

        /// <summary>
        /// この特徴抽出器を作成した人
        /// </summary>
        string Author { get; }
    }

}