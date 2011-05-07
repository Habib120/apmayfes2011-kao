using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using Emgu.CV;

namespace FeatureExtraction.Faces
{
    public class FaceData
    {
        public string LastUpdate;
        public string ImagePath;
        public string ImageName
        {
            get
            {
                if (ImagePath == null)
                {
                    return "";
                }
                else
                {
                    return System.IO.Path.GetFileName(ImagePath);
                }
            }
        }
        private Image<Bgr, byte> _image;
        public Image<Bgr, byte> Image
        {
            get
            {
                if (_image == null)
                {
                    _image = new Image<Bgr, byte>(this.ImagePath);
                }
                return _image;
            }
        }

        public void UnloadImage()
        {
            if (_image != null)
            {
                _image.Dispose();
                _image = null;
            }
        }
        public List<Label> Labels = new List<Label>();

        public static FaceData GetTestData()
        {
            var ret = new FaceData();
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();

            ret.ImagePath = System.IO.Path.GetDirectoryName(asm.Location) + "/Faces/test/face_0000.jpg";
            ret.Labels = new List<Label>
            {
                new Label
                {
                    Name   = "is_smiling",
                    Value  = "smiling",
                    Values = new string[]
                    {
                        "smiling",
                        "not_smiling",
                    }
                },
                new Label
                {
                    Name   = "glasses",
                    Value  = "no_glasses",
                    Values = new string[]
                    {
                        "glasses",
                        "no_glasses",
                    }
                },
                new Label
                {
                    Name   = "gender",
                    Value  = "male",
                    Values = new string[]
                    {
                        "male",
                        "female",
                    }
                }
            };
            return ret;
        }
    }
}
