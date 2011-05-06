using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace FeatureExtraction.Extractors.Concrete
{
    public class GradientHistX : AssemblyExtractor
    {
        public GradientHistX()
        {
            this.Normalizer = new NormalizerNone();
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            Image<Gray, double> difx_int = img.Resize(30, 30, INTER.CV_INTER_CUBIC).Sobel(1, 0, 3).Integral();
            List<double> ret = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 3 - 1].Intensity - difx_int[difx_int.Width - 1, i * 3].Intensity) / (difx_int.Width * 3));
            };
            return ret;
        }

        public override string Help
        {
            get { return "横方向に分割した顔領域における、X方向勾配の平均"; }
        }
        public override string Name
        {
            get { return "平均勾配X(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }

    public class GradientHistY :AssemblyExtractor
    {
        public GradientHistY()
        {
            this.Normalizer = new NormalizerNone();
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            Image<Gray, double> difx_int = img.Resize(30, 30, INTER.CV_INTER_CUBIC).Sobel(0, 1, 3).Integral();
            List<double> ret = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                ret.Add((difx_int[difx_int.Width - 1, (i + 1) * 3 - 1].Intensity - difx_int[difx_int.Width - 1, i * 3].Intensity )/ (difx_int.Width * 3));
            };
            return ret;
        }

        public override string Help
        {
            get { return "横方向に分割した顔領域における、Y方向勾配の平均"; }
        }
        public override string Name
        {
            get { return "平均勾配Y(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }


    public class GradientHistXHSV : AssemblyExtractor
    {
        public GradientHistXHSV()
        {
            this.Normalizer = new NormalizerNone() as INormalizer;
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Hsv, byte>(face.ImagePath);
            Image<Hsv, double> int_hsv = img.Resize(50, 50, INTER.CV_INTER_CUBIC).Sobel(1, 0, 3).Integral();
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
            get { return "横方向に分割した顔領域における、HSV各成分ごとのX方向勾配の平均"; }
        }
        public override string Name
        {
            get { return "３チャンネル平均勾配X(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }

    public class GradientHistYHSV : AssemblyExtractor
    {

        public GradientHistYHSV()
        {
            this.Normalizer = new NormalizerNone() as INormalizer;
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Hsv, byte>(face.ImagePath);
            Image<Hsv, double> int_hsv =  img.Resize(50, 50, INTER.CV_INTER_CUBIC).Sobel(0, 1, 3).Integral();
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
            get { return "横方向に分割した顔領域における、HSV各成分ごとのY方向勾配の絶対値平均"; }
        }
        public override string Name
        {
            get { return "３チャンネル平均勾配Y(横分割)"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }
}
