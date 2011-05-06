using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using FeatureExtraction.Faces;

namespace FeatureExtraction.Extractors.Concrete
{
    public class BrightnessHist : AssemblyExtractor
    {
        public BrightnessHist()
        {
            this.Normalizer = (INormalizer)new NormalizerSimple(0, 255);
        }

        protected override List<double> doExtract(FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            Image<Gray, double> difx_int = img.Resize(50, 50, INTER.CV_INTER_CUBIC).Integral();
            List<double> ret = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                if (i > 0)
                    ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 5 - 1].Intensity - difx_int[difx_int.Width - 1, i * 5 - 1].Intensity) / (difx_int.Width * 3));
                else
                    ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 5 - 1].Intensity) / (difx_int.Width * 3));
            };
            face.UnloadImage();
            return ret;
        }

        public override string Help
        {
            get { return "横方向に分割した顔領域における、明度の平均値"; }
        }
        public override string Name
        {
            get { return "明度平均(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }

    public class BrightnessHistHSV : AssemblyExtractor
    {
        public BrightnessHistHSV()
        {
            this.Normalizer = new NormalizerSimple(0, 255) as INormalizer;
        }

        protected override List<double> doExtract(FaceData face)
        {
            var img = new Image<Hsv, byte>(face.ImagePath);
            Image<Hsv, double> int_hsv = img.Resize(50, 50, INTER.CV_INTER_CUBIC).Integral();
            var int_channels = int_hsv.Split();
            List<double> ret = new List<double>();
            foreach (var channel in int_channels)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (i > 0)
                        ret.Add((channel[channel.Width - 1, (i + 1) * 5 - 1].Intensity - channel[channel.Width - 1, i * 5 - 1].Intensity) / (channel.Width * 3));
                    else
                        ret.Add((channel[channel.Width - 1, (i + 1) * 5 - 1].Intensity) / (channel.Width * 3));
                };
            }
            return ret;
        }
        public override string Help
        {
            get { return "横方向に分割した顔領域における、HSV各成分ごとの平均値"; }
        }
        public override string Name
        {
            get { return "３チャンネルHSV平均(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }
}
