using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureExtraction.Faces;
using System.IO;

namespace FeatureExtraction.Extractors
{
    public abstract class AssemblyExtractor : IExtractor
    {
        public abstract string Name { get;}
        public abstract string Author { get;}
        public abstract string Help { get;}

        protected INormalizer Normalizer;

        public IEnumerable<double> Extract(FaceData face)
        {
            preExtract(face);
            var ret = doExtract(face);
            postExtract(face, ret);
            return ret;
        }

        public void ExtractForDebug(string filename, FaceData face)
        {
            var result = this.Extract(face);
            File.Delete(filename);
            using (FileStream fs = new FileStream(filename, FileMode.Create))
            using (StreamWriter sr = new StreamWriter(fs))
            {
                var result_str = String.Join(" ", result.Select((d) => d.ToString()).ToArray());
                sr.WriteLine(result_str);
            }
        }

        protected virtual void preExtract(FaceData face)
        {
        }

        protected abstract List<double> doExtract(FaceData face);

        protected virtual void postExtract(FaceData face, List<double> ret)
        {
            if (this.Normalizer != null)
            {
                Normalizer.Normalize(ret);
            }
        }


    }
}
