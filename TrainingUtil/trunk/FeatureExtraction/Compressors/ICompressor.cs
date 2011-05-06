using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureExtraction.FeaturePoints;

namespace FeatureExtraction.Compressors
{
    /// <summary>
    /// 特徴量の圧縮を行うクラスの仕様
    /// </summary>
    public interface ICompressor
    {
        /// <summary>
        /// 圧縮器の名前
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 圧縮器の説明
        /// </summary>
        string Help { get; }
        /// <summary>
        /// 圧縮器の作成者
        /// </summary>
        string Author { get; }

        /// <summary>
        /// 必要なパラメータが読み込まれているか
        /// </summary>
        bool Loaded { get; set; }

        /// <summary>
        /// データを読み込み、圧縮に必要なパラメータを求める。
        /// </summary>
        /// <param name="data"></param>
        void Load(IEnumerable<FeaturePointData> items);

        /// <summary>
        /// 既存のファイルから、パラメータを読み込み
        /// </summary>
        /// <param name="filename"></param>
        void Load(string filename);

        /// <summary>
        /// 現在のパラメータをファイルに書き出す
        /// </summary>
        /// <param name="filename"></param>
        void Save(string filename);

        /// <summary>
        /// 現在のパラメータに基づき、特徴量を指定された次元まで圧縮する。
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        void Compress(IEnumerable<FeaturePointData> items, int dim);
    }
}
