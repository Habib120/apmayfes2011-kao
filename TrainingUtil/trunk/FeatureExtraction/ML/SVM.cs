using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using FeatureExtraction.FeaturePoints;

namespace FeatureExtraction.ML
{
    class SVM : IMLMethod
    {
        public string Name
        {
            get { return "SVM"; }
        }
        public string Author
        {
            get { return "Yadapo"; }
        }
        public string Help
        {
            get { return "サポートベクタマシンによる学習を行います。"; }
        }

        private IEnumerable<string> _using_labels;
        private Dictionary<string, string[]> _label_defs;

        public void Initialize(IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs)
        {
            this._using_labels = using_labels;
            this._label_defs = label_defs;
        }

        public IEnumerable<string> Train(string dirname, IEnumerable<FeaturePointData> items)
        {
            if (!Directory.Exists(dirname))
                return new List<string>{"specified directory does not exist"};
            if (_using_labels.Count() == 0) 
                return new List<string>{"you must specify at least one label"};
            List<string> errors = new List<string>();
            
            //ラベルごとに学習データファイルを作成
            var groups = from item in items
                         let label = _getLabel(item, _using_labels)
                         orderby label ascending
                         group item by label into g
                         select g;
            var filepaths = new List<string>();
            foreach (var group in groups)
            {
                var label = _getLabel(group.ElementAt(0), _using_labels);
                var filepath = String.Format(@"{0}\{1}.txt", dirname, label);
                File.Delete(filepath);
                filepaths.Add(filepath);
                try {
                    using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                    using (StreamWriter sr = new StreamWriter(fs))
                    {
                        foreach (var item in group.Select((data, i) => new {data, i}))
                        {
                            var line = String.Join(" ", item.data.FeaturePoints);
                            sr.WriteLine(line);
                        }
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(String.Format(ex.GetType().ToString() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace));
                }
            }
            if (errors.Count() > 0)
                return errors;

            //学習開始
            Assembly asm = Assembly.GetEntryAssembly();
            var app_dir = Path.GetDirectoryName(asm.Location);
            Directory.SetCurrentDirectory(app_dir + @"\ML\bin");

            var arguments = String.Join(" ", filepaths.ToArray());
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.RedirectStandardError = true;
            pinfo.RedirectStandardOutput = true;
            pinfo.UseShellExecute = false;
            pinfo.CreateNoWindow = true;
            pinfo.FileName = @"SVM.exe";
            pinfo.Arguments = arguments;
            var p = Process.Start(pinfo);
            p.WaitForExit();

            Directory.SetCurrentDirectory(app_dir);
            var error = p.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(error)) {
                errors.Add(error);
            }
            return errors;
        }



        public IEnumerable<string> Predict(string dirname, IEnumerable<FeaturePointData> items, out IEnumerable<string> results)
        {
            if (!Directory.Exists(dirname))
            {
                results = new List<string>();
                return new List<string> { "specified directory does not exist" };
            }
            List<string> errors = new List<string>();

            var filepath = String.Format(@"{0}\test.txt", dirname);
            File.Delete(filepath);
            try
            {
                using (FileStream fs = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.Write))
                using (StreamWriter sr = new StreamWriter(fs))
                {
                    foreach (var item in items.Select((data, i) => new {data, i}))
                    {
                        var line = String.Join(" ", item.data.FeaturePoints);
                        sr.WriteLine(line);
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(String.Format(ex.GetType().ToString() + Environment.NewLine + ex.Message + Environment.NewLine + ex.StackTrace));
            }
            if (errors.Count() > 0) {
                results = new List<string>();
                return errors;
            }

            //予測
            Assembly asm = Assembly.GetEntryAssembly();
            var app_dir = Path.GetDirectoryName(asm.Location);
            Directory.SetCurrentDirectory(app_dir + @"\ML\bin");

            var arguments = "-p " + filepath;
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.RedirectStandardError = true;
            pinfo.RedirectStandardOutput = true;
            pinfo.UseShellExecute = false;
            pinfo.CreateNoWindow = true;
            pinfo.FileName = @"SVM.exe";
            pinfo.Arguments = arguments;
            var p = Process.Start(pinfo);
            p.WaitForExit();

            var error = p.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(error))
            {
                if (error.IndexOf("Warning") != 0) 
                {   
                    errors.Add(error);
                    results = new List<string>();
                    return errors;
                }
            }

            var ret = new List<string>();
            var all_labels = _getAllLabels(_using_labels, _label_defs);
            using (FileStream fs = new FileStream("output.txt", FileMode.Open, FileAccess.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (true)
                {
                    var line = sr.ReadLine();
                    if (line == null)
                        break;
                    ret.Add(all_labels.ElementAt(int.Parse(line) - 1));
                }
            }
            results = ret;
            Directory.SetCurrentDirectory(app_dir);
            return new List<string>();
        }
        private IEnumerable<string> _getAllLabels(IEnumerable<string> using_labels, Dictionary<string, string[]> label_defs)
        {
            IEnumerable<string> all_labels = label_defs[using_labels.ElementAt(0)];
            for (int i = 1; i < using_labels.Count(); i++)
            {
                all_labels = _combination(all_labels, label_defs[using_labels.ElementAt(1)]);
            }
            all_labels = all_labels.OrderBy((l) => l);
            return all_labels;
        }

        private int _indexOf(string label, IEnumerable<string> all_labels)
        {
            var result = all_labels.Select((l, i) => new { l, i }).Where((item) => item.l == label).Select((item) => item.i);
            if (result.Count() > 0)
            {
                return result.ElementAt(0);
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// 現在選択されているラベル定義に基づいて
        /// 項目をラベル付けする。
        /// ex : is_smiling と has_glassesが選択されている場合
        ///    smiling x no_glasses
        /// </summary>
        private string _getLabel(FeaturePointData data, IEnumerable<string> using_labels)
        {
            var lbls = data.Data.Labels.Where((item) => using_labels.Contains(item.Name)).Select((item) => item.Value);
            return String.Join("_x_", lbls.ToArray());
        }

        private IEnumerable<string> _combination(IEnumerable<string> c1, IEnumerable<string> c2)
        {
            return from i1 in c1
                   from i2 in c2
                   select i1 + " x " + i2;
        }
    }
}
