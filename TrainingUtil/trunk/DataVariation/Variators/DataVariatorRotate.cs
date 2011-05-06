using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace DataVariation
{
    public class DataVariatorRotate : DataVariatorBase
    {

        public override string Name
        {
            get { return "回転"; }
        }
        public override string Description
        {
            get { return "もと画像を回転させたデータを作成します"; }
        }
        public override string PropertyName
        {
            get { return "角度(degree)"; }
        }

        public override IEnumerable<FaceData> GetVariation(FaceData src)
        {
            var src_img = src.Image;
            RotationMatrix2D<double> rotmat_l = new RotationMatrix2D<double>(new System.Drawing.PointF(src_img.Width / 2, src_img.Height / 2), value, 1);
            RotationMatrix2D<double> rotmat_r = new RotationMatrix2D<double>(new System.Drawing.PointF(src_img.Width / 2, src_img.Height / 2), -value, 1);
            //var rot_l = src_img.WarpAffine(rotmat_l, INTER.CV_INTER_CUBIC, WARP.CV_WARP_FILL_OUTLIERS, new Bgr(System.Drawing.Color.Transparent));
            var rot_l = new Image<Bgr, byte>(src_img.Size);
            var rot_r = new Image<Bgr, byte>(src_img.Size);
            rotmat_l[0, 2] = src_img.Width / 2;
            rotmat_l[1, 2] = src_img.Height / 2;
            rotmat_r[0, 2] = src_img.Width / 2;
            rotmat_r[1, 2] = src_img.Height / 2;
            CvInvoke.cvGetQuadrangleSubPix(src_img, rot_l, rotmat_l);
            CvInvoke.cvGetQuadrangleSubPix(src_img, rot_r, rotmat_r);
            


            var variation_l = src.Clone();
            variation_l.Image = rot_l;
            var variation_r = src.Clone();
            variation_r.Image = rot_r;
            return new List<FaceData> { variation_l, variation_r, src };
        }
    }
}
