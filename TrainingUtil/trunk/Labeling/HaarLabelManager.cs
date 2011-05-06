using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.Util;
using System.Windows.Forms;
using System.Drawing;
using System.Xml.Linq;
using System.Xml;
using System.ComponentModel;
using System.Threading;

namespace Labeling
{
    class HaarLabelManager
    {
        protected bool Dragging = false;
        protected Point DragBasePoint = new Point();
        protected enum Draggable {None, ET, EB, EL, ER, PRT, PRB, PLT, PLB, Region};
        protected Draggable DraggingItem = Draggable.None;
        protected FaceRestAPI API;

        public List<HaarLabel> Labels { get; protected set; }
        public Form Parent{ get; protected set; }
        protected int selected_index;
        public int SelectedIndex
        {
            get
            {
                return selected_index;
            }
            set
            {
                var vi = GetValidIndex(value);
                if (vi != selected_index)
                {
                    SelectedLabel.UnloadImage();
                    selected_index = vi;
                }
            }
        }


        public HaarLabelManager(Form parent, string directory_path, string label_file_path)
        {
            try
            {
                this.Parent = parent;
                InitializeLabels(directory_path, label_file_path);
                API = new FaceRestAPI(
                        "cfd89d59c0681a7d9e26573c223a46c9",
                        "1bb0b5a9dee6a4030dd159909df7b274",
                        null,
                        false,
                        null,
                        null,
                        null
                    );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 現在編集中のラベル
        /// </summary>
        public HaarLabel SelectedLabel
        {
            get
            {
                return Labels[SelectedIndex];
            }
        }

        public IEnumerable<HaarLabel> NewLabels
        {
            get
            {
                return from l in Labels
                       where l.IsNew
                       select l;
            }
        }

        /// <summary>
        /// ラベルを初期化する
        /// </summary>
        /// <param name="directory_path">画像ファイルのあるディレクトリ</param>
        /// <param name="label_file_path">過去のラベル付けを引き継ぐ場合の、ラベルファイルのパス</param>
        protected void InitializeLabels(string directory_path, string label_file_path)
        {
            if (!Directory.Exists(directory_path))
                throw new InvalidOperationException("ディレクトリが存在しません。");

            var di = new DirectoryInfo(directory_path);
            var imgexts = new string[] { ".png", ".bmp", ".jpg", ".gif" };
            var lbls = from f in di.GetFiles()
                       where imgexts.Contains(f.Extension)
                       select new HaarLabel
                       {
                           ImageFileInfo = f,
                           Rect = Rectangle.Empty,
                           Gender = HaarLabel.Genders.Male,
                           HasGlasses = false,
                           Smiling = false,
                       };
            if (lbls.Count() == 0)
                throw new InvalidOperationException("ディレクトリに画像ファイルがありません。");

            if (File.Exists(label_file_path))
            {

                Func<string, string> getfilename =
                    (src) =>
                    {
                        var tokens = src.Split(new char[] { ' ' });
                        return Path.GetFileName(tokens[0]);
                    };
                Func<HaarLabel, IEnumerable<string>, HaarLabel> update =
                    (src, lines) =>
                    {
                        var matches = from l in lines
                                      where getfilename(l) == src.ImageFileInfo.Name
                                      select l;
                        if (matches.Count() == 0)
                        {
                            return src;
                        }
                        else
                        {
                            var tokens = matches.ElementAt(0).Split(new char[] { ' ' });
                            src.Rect = new Rectangle(
                                    int.Parse(tokens[1]),
                                    int.Parse(tokens[2]),
                                    int.Parse(tokens[3]),
                                    int.Parse(tokens[4])
                                );
                            src.Rotation = double.Parse(tokens[5]);
                            src.Gender = tokens[6] == "male" ? HaarLabel.Genders.Male : HaarLabel.Genders.Female;
                            src.Smiling = tokens[7] == "smiling";
                            src.HasGlasses = tokens[8] == "glasses";

                            return src;
                        }
                    };
                var ls = readLabelFile(label_file_path);
                this.Labels = (from l in lbls
                               select update(l, ls)).ToList();
            }
            else
            {
                this.Labels = lbls.ToList();
            }
        }

        protected IEnumerable<string> readLabelFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// 現在のラべリングをファイルに保存する
        /// </summary>
        /// <param name="filename">書き込み先</param>
        public void Save(string filename)
        {
            using (StreamWriter sr = new StreamWriter(filename, false))
            {
                foreach (var label in this.Labels.Where((l) => !l.IsNew))
                {
                    List<string> attributes = new List<string>();
                    attributes.Add(label.ImageFileInfo.Name);
                    attributes.Add(label.Rect.X.ToString());
                    attributes.Add(label.Rect.Y.ToString());
                    attributes.Add(label.Rect.Width.ToString());
                    attributes.Add(label.Rect.Height.ToString());
                    attributes.Add(label.Rotation.ToString("f2"));
                    attributes.Add(label.Gender == HaarLabel.Genders.Male ? "male" : "female");
                    attributes.Add(label.Smiling ? "smiling": "not_smiling");
                    attributes.Add(label.HasGlasses ? "glasses": "no_glasses");

                    sr.WriteLine(String.Join(" ", attributes.ToArray()));
                }
            }
        }

        public void ExtractFacesForML(string dirname, bool clear_directory = true, Action<string> messageCallback = null, Action<double> progressCallback = null)
        {
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
            else
            {
                if (clear_directory)
                {
                    if (messageCallback != null)
                        messageCallback("ファイルを削除しています...");
                    if (progressCallback != null)
                        progressCallback(0);
                    foreach (var f in Directory.GetFiles(dirname))
                    {
                        new FileInfo(f).Delete();
                    }
                }
            }
            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            if (messageCallback != null)
                messageCallback("Xmlファイルを出力しています");
            if (progressCallback != null)
                progressCallback(20);
            using (XmlWriter xw = XmlWriter.Create(dirname + @"\labels.xml", xws))
            {
                XDocument xdoc = new XDocument();

                //ラベル定義
                XElement defs = new XElement("definitions");
                Dictionary<string, string[]> definitions = new Dictionary<string, string[]>
                {
                    {"is_smiling", new string[]{"smiling", "not_smiling"}},
                    {"glasses", new string[]{"glasses", "no_glasses"}},
                    {"gender", new string[]{"male", "female"}},
                };
                foreach (var item in definitions)
                {
                    XElement def = new XElement("def");
                    def.Add(new XElement("name", item.Key));
                    XElement values = new XElement("values");
                    foreach (var iitem in item.Value)
                    {
                        values.Add(new XElement("item", iitem));
                    }
                    def.Add(values);
                    defs.Add(def);
                }

                //顔画像のラベル情報
                XElement data = new XElement("data");
                foreach (var labelitem in this.Labels.Where((l)=>!l.IsNew).Select((label, i) => new { i, label }))
                {
                    XElement item = new XElement("item");
                    item.Add(new XElement("filename", String.Format("face_{0:D4}{1}", labelitem.i, labelitem.label.ImageFileInfo.Extension))
                    );
                    XElement lbls = new XElement("labels");

                    XElement is_smiling = new XElement("label");
                    is_smiling.Add(new XElement("name", "is_smiling"));
                    is_smiling.Add(new XElement("value", labelitem.label.Smiling ? definitions["is_smiling"][0] : definitions["is_smiling"][1]));
                    lbls.Add(is_smiling);

                    XElement glasses = new XElement("label");
                    glasses.Add(new XElement("name", "glasses"));
                    glasses.Add(new XElement("value", labelitem.label.HasGlasses ? definitions["glasses"][0] : definitions["glasses"][1]));
                    lbls.Add(glasses);

                    XElement gender = new XElement("label");
                    gender.Add(new XElement("name", "gender"));
                    gender.Add(new XElement("value", labelitem.label.Gender == HaarLabel.Genders.Male ? definitions["gender"][0] : definitions["gender"][1]));
                    lbls.Add(gender);

                    item.Add(lbls);
                    data.Add(item);
                }

                XElement root = new XElement("root");
                root.Add(defs);
                root.Add(data);
                xdoc.Add(root);
                xdoc.WriteTo(xw);
            }

            if (messageCallback != null)
                messageCallback("顔画像を切り出しています...");
            if (progressCallback != null)
                progressCallback(40);

            var valid_labels = Labels.Where((l) => !l.IsNew);
            foreach (var item in valid_labels.Select((l, i) => new { l, i }))
            {

                if (messageCallback != null)
                    messageCallback("顔画像を切り出しています..." + item.l.ImageFileInfo.Name);
                if (progressCallback != null)
                    progressCallback(40 + ((item.i + 1) * 60) / valid_labels.Count());

                string filename = String.Format("face_{0:D4}{1}", item.i, item.l.ImageFileInfo.Extension);
                var img = item.l.Image.Copy();
                var rot_mat = new Emgu.CV.RotationMatrix2D<double>(new PointF(img.Width / 2, img.Height / 2), item.l.Rotation, 1);
                img = img.WarpAffine(rot_mat, INTER.CV_INTER_CUBIC,
                WARP.CV_WARP_DEFAULT, new Bgr(Color.White));
                img.ROI = item.l.Rect;
                img.Save(dirname + @"\" + filename);
                img.Dispose();
                item.l.UnloadImage();
            }
        }


        /// <summary>
        /// 指定したディレクトリに顔画像を書き出す
        /// </summary>
        /// <param name="dirname">書き込み先</param>
        /// <param name="clear_directory">書き込み前にディレクトリ内のファイルを消去する</param>
        public void ExtractFaces(string dirname, string format = null, bool clear_directory = true, Action<string> messageCallback = null, Action<double> progressCallback = null)
        {
            if (!Directory.Exists(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
            else
            {
                if (clear_directory)
                {
                    if (messageCallback != null)
                        messageCallback("ファイルを削除しています...");
                    if (progressCallback != null)
                        progressCallback(0);
                    foreach (var f in Directory.GetFiles(dirname))
                    {
                        new FileInfo(f).Delete();
                    }
                }
            }

            if (messageCallback != null)
                messageCallback("顔画像を切り出しています...");
            if (progressCallback != null)
                progressCallback(20);

            if (format == null)
            {
                format = "%gender%_%smiling%_%glasses%_%index%.%ext%";
            }
            foreach (var item in Labels.Where((l) => !l.IsNew).Select((l,i) => new {l,i}))
            {

                if (messageCallback != null)
                    messageCallback("顔画像を切り出しています..." + item.l.ImageFileInfo.Name);
                if (progressCallback != null)
                    progressCallback(20 + ((item.i + 1) * 80) / Labels.Count());

                string filename = format.Replace("%gender%", item.l.Gender == HaarLabel.Genders.Male ? "male" : "female")
                    .Replace("%smiling%", item.l.Smiling ? "smiling" : "notsmiling")
                    .Replace("%glasses%", item.l.HasGlasses ? "glasses" : "noglasses")
                    .Replace("%index%", String.Format("{0:D4}", item.i))
                    .Replace("%original_filename%", item.l.ImageFileInfo.Name)
                    .Replace("%ext%", item.l.ImageFileInfo.Extension);
                var img = item.l.Image.Copy();
                img = img.WarpAffine(new Emgu.CV.RotationMatrix2D<double>(new PointF(img.Width / 2, img.Height / 2), item.l.Rotation, 1), INTER.CV_INTER_CUBIC,
                WARP.CV_WARP_DEFAULT, new Bgr(Color.White));
                img.ROI = item.l.Rect;
                img.Save(dirname + @"\" + filename);
            }
        }


        /// <summary>
        /// ラベルファイルを削除する
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            if (SelectedIndex > index)
            {
                SelectedIndex--;
            }
            File.Delete(Labels[index].ImageFileInfo.FullName);
            Labels.RemoveAt(index);
        }

        /// <summary>
        /// modを取って、有効なインデックスに変換
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected int GetValidIndex(int index)
        {
            if (Labels.Count() > 0)
                return (index + Labels.Count()) % Labels.Count();
            else
                return 0;
        }


        #region 自動検出
        public IEnumerable<string> LabelItemAuto(HaarLabel lb)
        {
            var result = API.faces_detect(null, lb.ImageFileInfo.FullName, null, null);
            var photo = result.photos[0];
            var tmp = from t in photo.tags
                      orderby t.attributes["face"].confidence descending
                      select t;
            if (tmp.Count() == 0)
            {
                return new List<string>{"no faces are found"};
            }
            var tag = tmp.ElementAt(0);

            lb.Rotation = Double.Parse(tag.roll);

            var rotmat = new Emgu.CV.RotationMatrix2D<double>(new PointF(lb.Image.Width/2, lb.Image.Height/2), -lb.Rotation, 1.0);
            var p = new PointF(tag.center.x * lb.Image.Width / 100, tag.center.y * lb.Image.Height / 100);
            var le = new PointF(tag.eye_left.x * lb.Image.Width / 100, tag.eye_left.y * lb.Image.Height / 100);
            var re = new PointF(tag.eye_right.x * lb.Image.Width / 100, tag.eye_right.y * lb.Image.Height / 100);
            var nose = new PointF(tag.nose.x * lb.Image.Width / 100, tag.nose.y * lb.Image.Height / 100);
            rotmat.RotatePoints(new PointF[] { p, le, re, nose});


            int width = (int)Math.Round(tag.width * lb.Image.Width / 100);
            int height = (int)Math.Round(tag.height * lb.Image.Height/100);
            var center = new PointF((le.X + re.X) / 2, nose.Y);
            lb.Rect = new Rectangle(
                    (int)Math.Round(center.X) - width/2,
                    (int)Math.Round(center.Y) - height/2,
                    width,
                    height
                );
            lb.HasGlasses = tag.attributes["glasses"].value == "true";
            lb.Smiling = tag.attributes["smiling"].value == "true";
            lb.Gender = tag.attributes["gender"].value == "male" ? HaarLabel.Genders.Male : HaarLabel.Genders.Female;
            return new List<string>();
        }

        public IEnumerable<string> LabelItemsAuto(IEnumerable<HaarLabel> labels, Action<string> msgCallback = null, Action<double> progressCallback = null)
        {
            List<string> errors = new List<string>();
            if (msgCallback != null)
                msgCallback("認識中...");
            int count = 0;
            List<BackgroundWorker> workers = new List<BackgroundWorker>();
            for (int i = 0; i < Labels.Count; i++ )
            {
                try
                {
                    Application.DoEvents();
                    var result = LabelItemAuto(Labels[i]);
                    errors.AddRange(result);
                    count++;
                    if (progressCallback != null && labels.Count() > 0)
                    {
                        progressCallback(count / (double)labels.Count() * 100);
                    }
                    if (i != SelectedIndex)
                    {
                        Labels[i].UnloadImage();
                    }
                }
                catch(Exception ex)
                {
                    errors.Add(ex.Message);
                    continue;
                }
            }
            if (msgCallback != null) {
                msgCallback("全ての項目の認識が完了しました!");
            }
            return errors;
        }


        #endregion

        #region イベント時のラベル操作関数群

        /// <summary>
        /// マウス押下時のラベル操作
        /// </summary>
        /// <param name="p"></param>
        /// <param name="canvasSize"></param>
        public void MouseDown(Point p, Size canvasSize)
        {
            Rectangle rect = SelectedLabel.Rect;
            Point rp = new Point
            {
                X = p.X * SelectedLabel.Image.Width / canvasSize.Width,
                Y = p.Y * SelectedLabel.Image.Height / canvasSize.Height
            };
            if (rect == Rectangle.Empty)
            {
                SelectedLabel.Rect = new Rectangle(rp, new Size(1, 1));
                this.DraggingItem = Draggable.PRB;
                this.Dragging = true;
                return;
            }
            this.DraggingItem = GetDraggablePoint(p, canvasSize);
            this.DragBasePoint = p;
            this.Dragging = true;
        }

        /// <summary>
        /// マウス押上時のラベル操作
        /// </summary>
        /// <param name="p"></param>
        /// <param name="canvasSize"></param>
        public void MouseUp(Point p, Size canvasSize)
        {
            this.DraggingItem = Draggable.None;
            this.Dragging = false;
        }

        /// <summary>
        /// マウス移動時のラベル操作
        /// </summary>
        /// <param name="p"></param>
        /// <param name="canvasSize"></param>
        public void MouseMove(Point p, Size canvasSize)
        {
            Rectangle rect = SelectedLabel.Rect;
            Point rp = new Point
            {
                X = p.X * SelectedLabel.Image.Width / canvasSize.Width,
                Y = p.Y * SelectedLabel.Image.Height / canvasSize.Height
            };
            Point dbrp = new Point
            {
                X = DragBasePoint.X * SelectedLabel.Image.Width / canvasSize.Width,
                Y = DragBasePoint.Y * SelectedLabel.Image.Height / canvasSize.Height
            };
            Func<int, int, int> min = (x, y) => x < y ? x:y;
            Func<int, int> abs = (x) => x > 0 ? x : -x;
            Func<Rectangle, Rectangle> resizehl = (rectangle) =>
                {
                    var location = new Point
                    {
                        X = min(rp.X, rectangle.X + rectangle.Width),
                        Y = rectangle.Y,
                    };
                    var size = new Size
                    {
                        Width = abs(rectangle.X + rectangle.Width - rp.X),
                        Height = rectangle.Height,
                    };
                    return new Rectangle(location, size);
                };
            Func<Rectangle, Rectangle> resizehr = (rectangle) =>
            {
                var location = new Point
                {
                    X = min(rectangle.X, rp.X),
                    Y = rectangle.Y,
                };
                var size = new Size
                {
                    Width = abs(rp.X - rectangle.X),
                    Height = rectangle.Height,
                };
                return new Rectangle(location, size);
            };
            Func<Rectangle,Rectangle> resizevt = (rectangle) =>
            {
                var location = new Point
                {
                    Y = min(rp.Y, rectangle.Y + rectangle.Height),
                    X = rectangle.X,
                };
                var size = new Size
                {
                    Height = abs(rectangle.Y + rectangle.Height - rp.Y),
                    Width = rectangle.Width,
                };
                return new Rectangle(location, size);
            };
            Func<Rectangle,Rectangle> resizevb = (rectangle) =>
            {
                var location = new Point
                {
                    Y = min(rectangle.Y, rp.Y),
                    X = rectangle.X,
                };
                var size = new Size
                {
                    Height = abs(rp.Y - rectangle.Y),
                    Width = rectangle.Width,
                };
                return new Rectangle(location, size);
            };
            Func<Rectangle> shift = () =>
                {
                    var newloc = rect.Location + new Size(rp.X - dbrp.X, rp.Y - dbrp.Y);
                    DragBasePoint = p;
                    return new Rectangle(newloc, rect.Size);
                };

            switch (DraggingItem)
            {
                case Draggable.EB:
                    SelectedLabel.Rect = resizevb(rect);
                    break;
                case Draggable.ET:
                    SelectedLabel.Rect = resizevt(rect);
                    break;
                case Draggable.EL:
                    SelectedLabel.Rect = resizehl(rect);
                    break;
                case Draggable.ER:
                    SelectedLabel.Rect = resizehr(rect);
                    break;
                case Draggable.PRT:
                    SelectedLabel.Rect = resizevt(resizehr(rect));
                    break;
                case Draggable.PRB:
                    SelectedLabel.Rect = resizevb(resizehr(rect));
                    break;
                case Draggable.PLT:
                    SelectedLabel.Rect = resizevt(resizehl(rect));
                    break;
                case Draggable.PLB:
                    SelectedLabel.Rect = resizevb(resizehl(rect));
                    break;
                case Draggable.Region:
                    SelectedLabel.Rect = shift();
                    break;
            }
        }

        /// <summary>
        /// 画像の回転を反映
        /// </summary>
        /// <param name="angle"></param>
        public void Rotate(double angle)
        {
            ((HaarLabel)Labels[this.SelectedIndex]).Rotation = angle;
        }


        /// <summary>
        /// クリック位置がラベル矩形のどこに対応するか判定
        /// </summary>
        /// <param name="p">マウスのクリック位置(ImageBox上の座標)</param>
        /// <param name="canvasSize">表示されるImageBoxのサイズ</param>
        /// <returns></returns>
        protected Draggable GetDraggablePoint(Point p, Size canvasSize)
        {
            Rectangle rect = SelectedLabel.Rect;
            Point rp = new Point
            {
                X = p.X * SelectedLabel.Image.Width / canvasSize.Width,
                Y = p.Y * SelectedLabel.Image.Height / canvasSize.Height
            };
            Size csize = new Size(8, 8);
            var candidates = new Dictionary<Draggable, Rectangle>
            {
                {Draggable.PLT, new Rectangle(rect.Location + new Size(-4, -4), csize)},
                {Draggable.PRT, new Rectangle(rect.Location + new Size(rect.Width -4, -4), csize)},
                {Draggable.PLB, new Rectangle(rect.Location + new Size(-4, rect.Height- 4), csize)},
                {Draggable.PRB, new Rectangle(rect.Location + rect.Size + new Size(-4, -4), csize)},
                {Draggable.EL ,new Rectangle(rect.Location + new Size(-4, 4), new Size(8, rect.Height - 8))},
                {Draggable.ER ,new Rectangle(rect.Location + new Size(rect.Width-2, 2), new Size(8, rect.Height-8))},
                {Draggable.ET ,new Rectangle(rect.Location + new Size(4, -4), new Size(rect.Width -8 , 8))},
                {Draggable.EB ,new Rectangle(rect.Location + new Size(4, rect.Height - 4), new Size(rect.Width-8 , 8))},
            };
            Func<Point, Dictionary<Draggable, Rectangle>, Draggable> search = (point, dict) =>
            {
                var results = from entry in dict
                              where entry.Value.Contains(point)
                              select entry.Key;
                if (results.Count() == 0)
                {
                    return Draggable.None;
                }
                else
                {
                    return results.ElementAt(0);
                }
            };
            var r = search(rp, candidates);
            if (r == Draggable.None)
            {
                if (rect.Contains(rp))
                {
                    return Draggable.Region;
                }
            }
            return r;
        }

        #endregion

        #region 描画
        /// <summary>
        /// 現在のラべリングを画像に描画する
        /// </summary>
        /// <param name="canvasSize"></param>
        /// <returns></returns>
        public Emgu.CV.Image<Bgr, byte> drawImage(Size canvasSize)
        {
            var image = SelectedLabel.Image;
            var rect = SelectedLabel.Rect;
            var canvas = image.Copy();
            canvas = canvas.WarpAffine(new Emgu.CV.RotationMatrix2D<double>(new PointF(image.Width/2, image.Height/2), SelectedLabel.Rotation, 1), INTER.CV_INTER_CUBIC,
                WARP.CV_WARP_DEFAULT, new Bgr(Color.White));
            if (rect != Rectangle.Empty)
                canvas.Draw(rect, new Bgr(Color.Purple), 2);
            return canvas.Resize(canvasSize.Width,canvasSize.Height, INTER.CV_INTER_CUBIC, true);
        }
        #endregion
    }

    public class HaarLabel
    {
        public double Scale {get; set;}
        public double Rotation { get; set; }
        public Rectangle Rect { get; set; }
        private Emgu.CV.Image<Bgr, byte> _image;
        public Emgu.CV.Image<Bgr, byte> Image {
            get
            {
                if (_image == null)
                    this._image = new Emgu.CV.Image<Bgr, byte>(ImageFileInfo.FullName);
                return this._image;
            }
        }
        public void UnloadImage()
        {
            if (_image != null)
            {
                this._image.Dispose();
                this._image = null;
            }
                
        }
        public FileInfo ImageFileInfo { get; set; }
        public enum Genders { Male, Female }
        public Genders Gender {get; set;}
        public bool HasGlasses { get; set; }
        public bool Smiling { get; set; }
        /// <summary>
        /// 学習ソフトでテスト結果の集計に使うグループ情報
        /// </summary>
        public string Group { get; set; }

        public bool IsNew
        {
            get { return Rect == Rectangle.Empty; }
        }
    }
}