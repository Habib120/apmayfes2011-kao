using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using System.IO;

namespace FeatureExtraction.Faces
{
    public class FaceDataTable
    {
        static Dictionary<string, FaceDataTable> instances = new Dictionary<string, FaceDataTable>();

        private List<FaceData> _items = new List<FaceData>();
        private Dictionary<string, string[]> _label_defs = new Dictionary<string, string[]>();

        public static FaceDataTable GetInstance(string key)
        {
            if (!instances.ContainsKey(key) || instances[key] == null)
            {
                instances[key] = new FaceDataTable();
            }
            return instances[key];
        }

        public bool Loaded {
            get {
                return Items.Count > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<FaceData> Items
        {
            get
            {
                return _items;
            }
        }

        public Dictionary<string, string[]> LabelDefinitions
        {
            get
            {
                return _label_defs;
            }
        }

        /// <summary>
        /// 指定されたディレクトリから、顔とそのラベルのリストを読み込む
        /// </summary>
        /// <param name="dirname">データディレクトリ</param>
        /// <exception cref="InvalidOperationException">label.xmlが存在しないか、無効なフォーマットである。</exception>
        /// <exception cref="ArgumentException">指定したディレクトリのパスが無効</exception>
        public void Load(string dirname)
        {
            if (!Directory.Exists(dirname))
            {
                throw new ArgumentException("Directory does not exist.");
            }
            DirectoryInfo di = new DirectoryInfo(dirname);
            var results = di.GetFiles().Where((f) => f.Name == "labels.xml");
            if (results.Count() == 0){
                throw new InvalidOperationException("Data directory must contain a label file(labels.xml).");
            }
            var lblfile = results.ElementAt(0);

            Unload();

            try
            {

                XDocument xdoc = XDocument.Load(lblfile.FullName);
                XElement root = xdoc.Element("root");
                //ラベル定義の読み込み
                XElement defs = root.Element("definitions");
                foreach (var def in defs.Elements("def"))
                {
                    var values = from item in def.Element("values").Elements("item")
                                 select item.Value;
                    var name = def.Element("name").Value;
                    _label_defs.Add(name, values.ToArray());
                }

                //顔データの読み込み
                XElement data = root.Element("data");
                int count = data.Elements("item").Count();
                int limit = 0;
                using (NumberInputWindow nWindow = new NumberInputWindow())
                {
                    nWindow.Message = String.Format("{0}件のレコードが見つかりました。使用するレコードの数を選択してください.", count);
                    nWindow.Value = count;
                    nWindow.ShowDialog();
                    limit = nWindow.Value;
                }
                foreach (var item in data.Elements("item").Take(limit))
                {
                    FileInfo f = new FileInfo(dirname + @"\" + Path.GetFileName(item.Element("filename").Value));
                    if (!f.Exists)
                    {
                        throw new IOException(String.Format("file: '{0}' does not exist", f.FullName));
                    }
                    var labels = from l in item.Element("labels").Elements("label")
                                 select new Label {
                                    Name = l.Element("name").Value,
                                    Value = l.Element("value").Value,
                                 };
                    _items.Add(new FaceData
                    {
                        ImagePath = f.FullName,
                        LastUpdate = f.LastWriteTime.ToString("yyyy/MM/dd hh:mm:ss"),
                        Labels = labels.ToList(),
                    });
                }
            }
            catch (Exception ex)
            {
                _label_defs.Clear();
                _items.Clear();
                throw ex;
            }
        }

        /// <summary>
        /// 読み込んだラベルリストをクリアする
        /// </summary>
        public void Unload()
        {
            _items.Clear();
            _label_defs.Clear();
        }
    }
}
