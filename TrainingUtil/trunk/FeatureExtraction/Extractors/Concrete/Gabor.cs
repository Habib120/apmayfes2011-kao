using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Diagnostics;

namespace FeatureExtraction.Extractors.Concrete
{
    public class Gabor : AssemblyExtractor
    {
        public override string Help
        {
            get { return "GaborFilterによる8方向x4スケールの特定周波数領域抽出画像を、ベクトルにして返します。"; }
        }
        public override string Name
        {
            get { return "Gabor特徴量"; }
        }
        public override string Author
        {
            get { return "ムラシタ"; }
        }

        //何件ものデータを処理するのでカーネルの計算は一か所にまとめる
        protected static List<GaborFilter> Filters { get; set; }

        public Gabor()
        {
            this.Normalizer = new NormalizerNone() as INormalizer;

            if (Filters != null)
                return;
            Filters = new List<GaborFilter>();
            double f = Math.Sqrt(2);
            double Sigma = Math.Sqrt(2 * Math.PI);
            double k_max = Math.PI;
            for (int u = 0; u < 5; u++)
            {
                for (int v = 0; v < 8; v++)
                {
                    double K = k_max / Math.Pow(f, v);
                    double Phi = Math.PI * u / 8;
                    var filter = new GaborFilter(K, Sigma, Phi);
                    Filters.Add(filter);
                }
            }
        }

        protected override List<double> doExtract(Faces.FaceData face)
        {
            var img = new Image<Gray, byte>(face.ImagePath);
            int width = 128;
            Image<Gray, double> resized = img.Resize(width, width, INTER.CV_INTER_CUBIC).Convert<Gray, double>();

            //王さんのレポートにしたがって特徴量を計算する。
            var dev_images = new List<Image<Gray, float>>();
            var ret = new List<double>();
            foreach (var item in Filters.Select((filter, i) => new {filter, i}))
            {
                var u = item.i / 8;
                var v = item.i / 5;
                var tmp = item.filter.ConvertImage(resized, GaborType.Mag).Resize(20, 20, INTER.CV_INTER_LINEAR);
               
                foreach (float d in  tmp.Data.Cast<float>())
                {
                    ret.Add((double)d);
                }   
                /*/ For Debug
                if (face.ImageName == "face_0000.jpg"){
                    double[] mins, maxs;
                    System.Drawing.Point[] minpts, maxpts;
                    tmp.MinMax(out mins, out maxs, out minpts, out maxpts);
                    //スケーリング
                    dev_images.Add(tmp.ConvertScale<float>(255 / (maxs[0] - mins[0]),0));
                }
                //*/
                tmp.Dispose();
            }
            /*/ For Debug
            if (face.ImageName == "face_0000.jpg")
            {
                CvInvoke.cvNamedWindow("test");
                var dev_img = new Image<Gray, float>(width * 5, width * 8);
                for (int i = 0; i < 40; i++) {
                    var u = i / 8;
                    var v = i % 8;
                    dev_img.ROI = new System.Drawing.Rectangle(u * width, v* width, width, width);
                    dev_images.ElementAt(i).CopyTo(dev_img);
                }
                dev_img.ROI = System.Drawing.Rectangle.Empty;
                dev_img.Convert<Gray, double>().Save(@"dev.png");
            }
            //*/
            return ret;
        }
    }

    /// <summary>
    /// ガボールフィルタのタイプ
    /// </summary>
    public enum GaborType { Real, Imag, Mag }

    /// <summary>
    /// ガボールフィルタ
    /// ネット上に落ちてたC++版の実装を参考にしながら作成
    /// </summary>
    /// <see cref="http://www.hackchina.com/en/r/119574/cvgabor.cpp__html"/>
    public class GaborFilter : IDisposable
    {
        public double K { get; protected set; }
        public double Sigma { get; protected set; }
        public double Phi { get; protected set; }
        public int Width { get; protected set; }


        //カーネルの実部
        public Matrix<float> Real { get; protected set; }
        //カーネルの虚部
        public Matrix<float> Imag { get; protected set; }

        public GaborFilter(double pK, double pSigma, double pPhi)
        {
            K = pK / 2;
            Sigma = pSigma;
            Phi = pPhi;
            _initialize();
        }

        public void Dispose()
        {
            if (Real != null)
                Real.Dispose();
            if (Imag != null)
                Imag.Dispose();
        }

        public Image<Gray, byte> getKernelImage()
        {
            Image<Gray, float> image = new Image<Gray,float>(Imag.Size);
            CvInvoke.cvPow(Real * Real + Imag * Imag, image, 0.5);
            double[] mins, maxs;
            System.Drawing.Point[] minpts, maxpts;
            image.MinMax(out mins, out maxs, out minpts, out maxpts);
            //スケーリング
            CvInvoke.cvNormalize(image, image, 255, 0, NORM_TYPE.CV_MINMAX, IntPtr.Zero);
            return image.Convert<Gray, byte>();
        }

        /// <summary>
        /// 指定されたパラメータでのガボールフィルタを作成する。
        /// </summary>
        /// <param name="pK">スケール -5 to \infty</param>
        /// <param name="pSigma">ガウス分布の分散：通常2πらしい</param>
        /// <param name="pPhi">ガボールフィルタの方向(degree)</param>
        private void _initialize()
        {
            Width = _getWidth(K, Sigma, Phi);
            _createKernel();
        }

        private static int _getWidth(double pK, double pSigma, double pPhi)
        {
            double dModSigma = pSigma / pK;
            int dWidth = (int)(dModSigma * 6 + 1);
            if (dWidth % 2 == 0)
                dWidth++;
            return dWidth;
        }

        private void _createKernel()
        {
            var mReal = new Matrix<float>(Width, Width);
            var mImag = new Matrix<float>(Width, Width);

            int x, y;
            double dReal;
            double dImag;
            double dTemp1, dTemp2, dTemp3;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    x = i - (Width - 1) / 2;
                    y = j - (Width - 1) / 2;
                    dTemp1 = (Math.Pow(K, 2) / Math.Pow(Sigma, 2)) * Math.Exp(-(Math.Pow((double)x, 2) + Math.Pow((double)y, 2)) * Math.Pow(K, 2) / (2 * Math.Pow(Sigma, 2)));
                    dTemp2 = Math.Cos(K * Math.Cos(Phi) * x + K * Math.Sin(Phi) * y) - Math.Exp(-Math.Pow(Sigma, 2) / 2);
                    dTemp3 = Math.Sin(K * Math.Cos(Phi) * x + K * Math.Sin(Phi) * y);
                    dReal = dTemp1 * dTemp2;
                    dImag = dTemp1 * dTemp3;
                    mReal[i, j] = (float)dReal;
                    mImag[i, j] = (float)dImag;
                }
            }

            this.Real = mReal;
            this.Imag = mImag;
        }

        public Image<Gray, float> ConvertImage(Image<Gray, double> src, GaborType type)
        {
            Image<Gray, float> mat;
            Image<Gray, float> imat;
            Image<Gray, float> rmat;
            switch (type)
            {
                case GaborType.Real:
                    mat = src.Convolution(new ConvolutionKernelF(this.Real, new System.Drawing.Point((Width-1)/2, (Width-1)/2)));
                    break;
                case GaborType.Imag:
                    mat = src.Convolution(new ConvolutionKernelF(this.Imag, new System.Drawing.Point((Width - 1) / 2, (Width - 1) / 2)));
                    break;
                case GaborType.Mag:
                    rmat = src.Convolution(new ConvolutionKernelF(this.Real, new System.Drawing.Point((Width - 1) / 2, (Width - 1) / 2)));
                    imat = src.Convolution(new ConvolutionKernelF(this.Imag, new System.Drawing.Point((Width - 1) / 2, (Width - 1) / 2)));
                    mat = (rmat.Pow(2) + imat.Pow(2)).Pow(0.5);
                    break;
                default:
                    throw new ArgumentException();
            }

            return mat;
        }

    }


}
