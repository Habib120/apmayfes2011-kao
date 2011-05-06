using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;

namespace DataVariation
{
    public class DataVariatorWarpAffine : DataVariatorBase
    {
        public override string Name
        {
            get { return "ワープ"; }
        }
        public override string Description
        {
            get { return "画像の下端・上端を指定された比率でゆがめます"; }
        }
        public override string PropertyName
        {
            get { return "圧縮比"; }
        }

        public override IEnumerable<FaceData> GetVariation(FaceData src)
        {
            var src_img = src.Image;
            Matrix<float> warp_mat1 = new Matrix<float>(3, 3);
            Matrix<float> warp_mat2 = new Matrix<float>(3, 3);
            PointF[] base_pts = new PointF[]
            {
                new PointF(0,0),
                new PointF(src_img.Width, 0),
                new PointF(0, src_img.Height),
                new PointF(src_img.Width, src_img.Height),
            };
            PointF[] dst_pts1 = new PointF[]
            {
                new PointF(0,0),
                new PointF(src_img.Width, 0),
                new PointF((float)(1.0/2.0 - 1.0/(2.0*value)) * src_img.Width, src_img.Height),
                new PointF((float)(1.0/2.0 + 1.0/(2.0*value)) * src_img.Width, src_img.Height),
            };
            PointF[] dst_pts2 = new PointF[]
            {
                new PointF((float)(1.0/2.0 - 1.0/(2.0*value)) * src_img.Width,0),
                new PointF((float)(1.0/2.0 + 1.0/(2.0*value)) * src_img.Width, 0),
                new PointF(0, src_img.Height),
                new PointF(src_img.Width, src_img.Height),
            };
            CvInvoke.cvGetPerspectiveTransform(base_pts, dst_pts1, warp_mat1);
            CvInvoke.cvGetPerspectiveTransform(base_pts, dst_pts2, warp_mat2);
           
            var rot_l = src_img.WarpPerspective(warp_mat1, INTER.CV_INTER_CUBIC, WARP.CV_WARP_DEFAULT, new Bgr(System.Drawing.Color.White));
            var rot_r = src_img.WarpPerspective(warp_mat2, INTER.CV_INTER_CUBIC, WARP.CV_WARP_DEFAULT, new Bgr(System.Drawing.Color.White));

            var variation_l = src.Clone();
            variation_l.Image = rot_l;
            var variation_r = src.Clone();
            variation_r.Image = rot_r;
            return new List<FaceData> { variation_l, variation_r, src };
        }
    }
}
