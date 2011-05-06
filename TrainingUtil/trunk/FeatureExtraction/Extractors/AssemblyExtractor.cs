using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureExtraction.Faces;

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
