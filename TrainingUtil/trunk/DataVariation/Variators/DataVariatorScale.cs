using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace DataVariation
{
    public class DataVariatorScale : DataVariatorBase
    {
        public override string Name
        {
            get { return "スケール"; }
        }
        public override string Description
        {
            get { return "もとの画像を縮小して解像度を下げます。"; }
        }
        public override string PropertyName
        {
            get { return "比率"; }
        }

        public override IEnumerable<FaceData> GetVariation(FaceData src)
        {
            Image<Bgr, byte> scaled_img = src.Image.Resize(value);
            FaceData scaled = src.Clone();
            scaled.Image = scaled_img;

            return new List<FaceData>{scaled, src};
        }
    }
}
