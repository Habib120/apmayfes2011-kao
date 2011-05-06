using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeatureExtraction.FeaturePoints;
using System.Diagnostics;
using System.Reflection;
using System.IO;

namespace FeatureExtraction.Compressors
{
    public class PCADecomposer : ICompressor
    {
        private string binaryFilename;
        private string binaryDir;
        private string paramFilePath;
        private string trainDataFilePath;
        private string dataFilePath;
        private string resultFilePath;

        public PCADecomposer()
        {
            Assembly asm = Assembly.GetEntryAssembly();
            binaryFilename = "PCA.exe";
            binaryDir = Path.GetDirectoryName(asm.Location) + @"\Compressors\bin";
            paramFilePath = binaryDir + @"\vector.txt";
            trainDataFilePath = binaryDir + @"\train_data.txt";
            dataFilePath = binaryDir + @"\data.txt";
            resultFilePath = binaryDir + @"\PCAdata.txt";
        }

        public string Name
        {
            get { return "PCA"; }
        }

        public string Help
        {
            get { return "特徴ベクトルの共分散行列を計算し、その固有ベクトルのうち固有値が大きいものn本で貼られる部分空間への射影を考えます。"; }
        }

        public string Author
        {
            get { return "yadapo"; }
        }

        public bool Loaded { get; set; }

        public void Load(string filename)
        {
            if (File.Exists(filename))
            {
                File.Move(filename, paramFilePath);
            }
        }

        public void Load(IEnumerable<FeaturePointData> data)
        {
            File.Delete(trainDataFilePath);
            using (FileStream fs = new FileStream(trainDataFilePath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in data)
                {
                    sw.WriteLine(String.Join(" ", item.FeaturePoints));
                }
            }

            int nsamples = data.Count();
            int nvars = data.ElementAt(0).FeaturePoints.Count();

            var oldCurDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = binaryDir;

            string arguments = String.Format(" -svd train_data.txt {0:d} {1:d}", nsamples, nvars);
            Process p = StartExecutable(arguments);
            p.WaitForExit();
            Environment.CurrentDirectory = oldCurDir;
            var errors = "";// p.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(errors))
            {
                throw new Exception("分解に失敗しました。" + Environment.NewLine + errors);
            }
            Loaded = true;
        }

        public void Compress(IEnumerable<FeaturePointData> data, int dim)
        {
            if (!Loaded)
                throw new InvalidOperationException("Compressor must be loaded before saving");

            int nsamples = data.Count();
            int nvars = data.ElementAt(0).FeaturePoints.Count();
            if (dim > nvars)
            {
                throw new InvalidOperationException("The 'dim' parameter must be lower than the original variable counts");
            }
            
            int num_of_vectors = 0;
            using (FileStream fs = new FileStream(paramFilePath, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                while (sr.ReadLine() != null)
                    num_of_vectors ++;
            }

            File.Delete(dataFilePath);
            using (FileStream fs = new FileStream(dataFilePath, FileMode.OpenOrCreate))
            using (StreamWriter sw = new StreamWriter(fs))
            {
                foreach (var item in data)
                {
                    sw.WriteLine(String.Join(" ", item.FeaturePoints));
                }
            }

            string arguments = String.Format(
                    " -proj vector.txt data.txt {0:d} {1:d} {2:d}"
                    , nsamples
                    , nvars
                    , dim
                );

            var oldCurDir = Environment.CurrentDirectory;
            Environment.CurrentDirectory = binaryDir;
            Process p = StartExecutable(arguments);
            p.WaitForExit();
            Environment.CurrentDirectory = oldCurDir;
            var errors = "";// p.StandardError.ReadToEnd();
            if (!String.IsNullOrWhiteSpace(errors))
            {
                throw new Exception("次元の削減に失敗しました。" + Environment.NewLine + errors);
            }

            List<FeaturePoints.FeaturePoints> new_values = new List<FeaturePoints.FeaturePoints>();
            using (FileStream fs = new FileStream(resultFilePath, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            {
                var line = sr.ReadLine();
                while (line != null)
                {
                    var tokens = line.Split(' ');
                    FeaturePoints.FeaturePoints item = new FeaturePoints.FeaturePoints();
                    int cnt = 0;
                    foreach (var token in tokens)
                    {
                        if (!String.IsNullOrWhiteSpace(token))
                        {
                            cnt++;
                            item.Add(double.Parse(token));
                            if (cnt >= dim)
                                break;
                        }
                    }
                    new_values.Add(item);

                    line = sr.ReadLine();
                }
            }

            for (int i = 0; i < data.Count(); i++)
            {
                data.ElementAt(i).FeaturePoints = new_values[i];
            }
        }

        public void Save(string filename)
        {
            if (!Loaded)
                throw new InvalidOperationException("Compressor must be loaded before saving");
            File.Move(paramFilePath, filename);
        }


        /// <summary>
        /// 実行形式ファイルを指定した引数で実行する
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private Process StartExecutable(string arguments)
        {

            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.FileName = this.binaryFilename;
            pinfo.Arguments = arguments;
            pinfo.CreateNoWindow = true;
            pinfo.UseShellExecute = false;
            pinfo.RedirectStandardOutput = false;
            pinfo.RedirectStandardError = true;

            return Process.Start(pinfo);
        }

    }
}
