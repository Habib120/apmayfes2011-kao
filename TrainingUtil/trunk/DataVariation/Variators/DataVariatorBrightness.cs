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
    public class DataVariatorBrightness : DataVariatorBase
    {

        public override string Name
        {
            get { return "明暗"; }
        }
        public override string Description
        {
            get { return "指定された強度で明るさを変化させます"; }
        }

        public override string PropertyName
        {
            get { return "強度"; }
        }

        public override IEnumerable<FaceData> GetVariation(FaceData src)
        {
            Image<Gray, byte>[] channels = src.Image.Split();
            Image<Gray, byte>[] brighter_channels = new Image<Gray, byte>[3];
            Image<Gray, byte>[] darker_channels = new Image<Gray, byte>[3];
            for (int i = 0; i < 3; i++)
            {
                brighter_channels[i] = channels[i].Convert(new Converter<byte, byte>((b) =>
                    {
                        return (byte)Math.Min(b + (255 - b) * value, 255);
                    }));
                darker_channels[i] = channels[i].Convert(new Converter<byte, byte>((b) =>
                    {
                        return (byte)Math.Max(b - b * value, 0);
                    }));
            }
            Image<Bgr, byte> brighter_img = new Image<Bgr, byte>(brighter_channels);
            Image<Bgr, byte> darker_img = new Image<Bgr, byte>(darker_channels);

            FaceData brighter = src.Clone();
            FaceData darker = src.Clone();
            brighter.Image = brighter_img;
            darker.Image = darker_img;

            return new List<FaceData> {brighter, darker};
        }
    }
}
