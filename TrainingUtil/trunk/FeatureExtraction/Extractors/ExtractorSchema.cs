using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using FeatureExtraction.Faces;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// 複数の抽出器を組み合わせた新しい抽出器を表すクラス
    /// </summary>
    class ExtractorSchema :  IExtractor
    {
        private List<IExtractor> _extractors = new List<IExtractor>();

        public ExtractorSchema()
        {
        }

        public ExtractorSchema(IEnumerable<IExtractor> extractors)
        {
            this._extractors.AddRange(extractors);
        }

        public IExtractor this[int i]
        {
            get {
                return this._extractors[i];
            }
            set {
                _extractors[i] = value;
            }
        }

        public IEnumerable<double> Extract(FaceData face)
        {
            List<double> ret = new List<double>();
            foreach (var extractor in _extractors)
            {
                ret.AddRange(extractor.Extract(face));
            }
            return ret;
        }

        public string Name
        {
            get
            {
                if (this._extractors.Count() == 0)
                    return "";
                else if (this._extractors.Count() == 1)
                    return this._extractors.ElementAt(0).Name;
                else
                    return "カスタム";
            }
        }

        public string Author
        {
            get
            {
                var authors = from extractor in _extractors
                            select extractor.Author;
                return String.Join(" + ", authors.ToArray());
            }
        }

        public string Help
        {
            get
            {
                return "";
            }
        }
    }
}
