using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace DataVariation
{
    /// <summary>
    /// 認識に必要な顔データを表現するクラス
    /// 以下の4つの要素からなる
    /// 　画像データ
    /// 　画像データを保存するファイルパス
    /// 　ラベル
    /// 　画像のグループ
    /// </summary>
    public class FaceData
    {
        public Image<Bgr, byte> Image;
        public string Filename;
        public string Group;
        public LabelInfo LabelInfo;

        public FaceData()
        {
            
        }

        public FaceData(XElement elem)
        {
            if (elem.Element("labels") == null || elem.Element("filename") == null)
            {
                throw new Exception("Invalid xml file");
            }
            else
            {
                this.LabelInfo = new LabelInfo(elem.Element("labels"));
            }

            if (elem.Element("group") != null)
            {
                this.Group = elem.Element("group").Value;
            }



        }

        /// <summary>
        /// FaceDataをXml形式で出力する
        /// </summary>
        /// <returns></returns>
        public XElement AsXml()
        {
            XElement elem = new XElement("item");
            elem.Add(LabelInfo.AsXml());
            return elem;
        }

        /// <summary>
        /// 画像データをファイルに保存する
        /// </summary>
        public void Save()
        {
            checkValid();
            this.Image.Save(this.Filename);
        }

        /// <summary>
        /// 有効なオブジェクトか
        /// </summary>
        protected void checkValid()
        {
            if (Image == null)
            {
                throw new InvalidOperationException("イメージが設定されていません");
            }
            if (Filename == null)
            {
                throw new InvalidOperationException("イメージ名が設定されていません");
            }
        }

        public FaceData Clone()
        {
            var ret = (FaceData)this.MemberwiseClone();
            if (this.LabelInfo != null)
            {
                ret.LabelInfo = this.LabelInfo.Clone();
            }
            return ret;
        }
    }
}
