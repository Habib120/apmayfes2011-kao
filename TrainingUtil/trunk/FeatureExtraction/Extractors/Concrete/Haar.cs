using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace FeatureExtraction.Extractors.Concrete
{
    public class Haar : AssemblyExtractor
    {
        public Haar()
        {
            this.Normalizer = new NormalizerNone() as INormalizer;
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            Image<Gray, double> int_img = img.Resize(30, 30, INTER.CV_INTER_CUBIC).Integral();
            List<double> ret = new List<double>();
            //6種類のHaar-likeなパッチを使用
            HaarPatchLine p1 = new HaarPatchLine
            {
                X = 0,
                Y = 0,
                W = 3,
                H = 1,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 1,
                scalePitchY = 1,
                TP1 = 0.33,
                TP2 = 0.66,
                Direction = HaarPatchLine.PatchDirection.Horizontal,
            };
            HaarPatchLine p2 = new HaarPatchLine
            {
                X = 0,
                Y = 0,
                W = 4,
                H = 1,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 4,
                scalePitchY = 4,
                TP1 = 0.33,
                TP2 = 0.66,
                Direction = HaarPatchLine.PatchDirection.Horizontal,
            };
            HaarPatchLine p3 = new HaarPatchLine
            {
                X = 0,
                Y = 0,
                W = 1,
                H = 3,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 1,
                scalePitchY = 1,
                TP1 = 0.33,
                TP2 = 0.66,
                Direction = HaarPatchLine.PatchDirection.Vertical,
            };
            HaarPatchLine p4 = new HaarPatchLine
            {
                X = 0,
                Y = 0,
                W = 1,
                H = 4,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 1,
                scalePitchY = 1,
                TP1 = 0.33,
                TP2 = 0.66,
                Direction = HaarPatchLine.PatchDirection.Vertical,
            };
            HaarPatchDif p5 = new HaarPatchDif
            {
                X = 0,
                Y = 0,
                W = 2,
                H = 1,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 1,
                scalePitchY = 1,
                TP = 0.5,
                Direction = HaarPatchDif.PatchDirection.Vertical,
            };
            HaarPatchDif p6 = new HaarPatchDif
            {
                X = 0,
                Y = 0,
                W = 1,
                H = 2,
                PitchX = 1,
                PitchY = 1,
                scalePitchX = 1,
                scalePitchY = 1,
                TP = 0.5,
                Direction = HaarPatchDif.PatchDirection.Vertical,
            };
            List<HaarPatch> patches = new List<HaarPatch>();
            patches.AddRange(new HaarPatch[] { p1, p2, p3, p4 });

            foreach (var p in patches)
            {
                for (; p.W < int_img.Width; p.W += p.scalePitchX)
                {
                    for (; p.H < int_img.Height; p.H += p.scalePitchY)
                    {
                        for (; p.X + p.W < int_img.Width; p.X += p.PitchX)
                        {
                            for (; p.Y + p.H < int_img.Height; p.Y += p.PitchY)
                            {
                                ret.Add(p.Calculate(int_img));
                            }

                        }
                    }
                }
            }

            return ret;
        }
        private static void Normalize(ref List<double> hist)
        {
            double max = hist.Max();
            double min = hist.Min();
            for (int i = 0; i < hist.Count(); i++)
            {
                hist[i] = (hist[i] - min) / (max - min);
            }
        }

        public override string Help
        {
            get { return "Haar-likeな特徴量"; }
        }
        public override string Name
        {
            get { return "Haar特徴量"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }
    }


    abstract class HaarPatch
    {
        public int W { get; set; }
        public int H { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int scalePitchX { get; set; }
        public int scalePitchY { get; set; }
        public int PitchX { get; set; }
        public int PitchY { get; set; }
        abstract public double Calculate(Image<Gray, double> integral_image);

        public static double GetAreaSum(Point lt, Point rb, Image<Gray, double> integral_image)
        {
            double whole = integral_image[rb.X, rb.Y].Intensity;
            double rt_sum = _get_intensity(rb.X, lt.Y - 1, integral_image);
            double lb_sum = _get_intensity(lt.X - 1, rb.Y, integral_image);
            double tt_sum = _get_intensity(lt.X - 1, lt.Y - 1, integral_image);

            return whole - rt_sum - lb_sum + tt_sum;
        }

        private static double _get_intensity(int x, int y, Image<Gray, double> integral_image)
        {
            if (x < 0 || y < 0)
            {
                return 0;
            }
            else
            {
                return integral_image[x, y].Intensity;
            }
        }
    }

    /// <summary>
    /// 線素に反応するパッチ
    /// </summary>
    class HaarPatchLine : HaarPatch
    {
        public double TP1 { get; set; }
        public double TP2 { get; set; }
        public enum PatchDirection { Horizontal, Vertical };
        public PatchDirection Direction;

        public override double Calculate(Image<Gray, double> integral_image)
        {
            List<double> ret = new List<double>();
            double whole_black = GetAreaSum(new Point(X, Y), new Point(X + W, Y + H), integral_image);
            Point lt, rb;
            if (this.Direction == PatchDirection.Horizontal)
            {
                lt = new Point
                {
                    X = (int)Math.Round(W * TP1),
                    Y = this.Y,
                };
                rb = new Point
                {
                    X = (int)Math.Round(W * TP2),
                    Y = this.Y + this.H,
                };
            }
            else
            {
                lt = new Point
                {
                    Y = (int)Math.Round(H * TP1),
                    X = this.X,
                };
                rb = new Point
                {
                    Y = (int)Math.Round(H * TP2),
                    X = this.X + this.W,
                };
            }
            double white = GetAreaSum(lt, rb, integral_image);

            return 2 * white - whole_black;
        }
    }

    /// <summary>
    /// 微分パッチ
    /// </summary>
    class HaarPatchDif : HaarPatch
    {
        public double TP { get; set; }
        public enum PatchDirection { Horizontal, Vertical };
        public PatchDirection Direction;

        public override double Calculate(Image<Gray, double> integral_image)
        {
            List<double> ret = new List<double>();
            double whole_black = GetAreaSum(new Point(X, Y), new Point(X + W, Y + H), integral_image);
            Point lt, rb;
            if (this.Direction == PatchDirection.Horizontal)
            {
                lt = new Point
                {
                    X = this.X,
                    Y = this.Y,
                };
                rb = new Point
                {
                    X = (int)Math.Round(W * TP),
                    Y = this.Y + this.H,
                };
            }
            else
            {
                lt = new Point
                {
                    Y = this.Y,
                    X = this.X,
                };
                rb = new Point
                {
                    Y = (int)Math.Round(H * TP),
                    X = this.X + this.W,
                };
            }
            double white = GetAreaSum(lt, rb, integral_image);

            return 2 * white - whole_black;
        }
    }
}
