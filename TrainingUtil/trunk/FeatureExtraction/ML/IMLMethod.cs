using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FeatureExtraction.FeaturePoints;
using FeatureExtraction;

namespace FeatureExtraction.ML
{
    public interface IMLMethod
    {
        string Name {get;}
        string Help {get;}
        string Author {get;}

        void Initialize(IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs);

        IEnumerable<string> Train(string dirname, IEnumerable<FeaturePointData> data);

        IEnumerable<string> Predict(string filename, IEnumerable<FeaturePointData> data, out IEnumerable<string> results);
    }
}
