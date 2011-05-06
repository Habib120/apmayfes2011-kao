using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace FeatureExtraction.Extractors.Concrete
{
    class LaplacianHist : AssemblyExtractor
    {
        public LaplacianHist()
        {
            this.Normalizer = new NormalizerNone();
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            Image<Gray, double> difx_int = img.Resize(50, 50, INTER.CV_INTER_CUBIC).Laplace(3).Integral();
            List<double> ret = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                if (i > 0)
                    ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 5 - 1].Intensity - difx_int[difx_int.Width - 1, i * 5 - 1].Intensity) / (difx_int.Width * 3));
                else
                    ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 5 - 1].Intensity) / (difx_int.Width * 3));
            };
            return ret;
        }

        public override string Help
        {
            get { return "横方向に分割した顔領域における、ラプラシアンの平均値"; }
        }
        public override string Name
        {
            get { return "ラプラシアン(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }
}
