using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using FeatureExtraction.Faces;

namespace FeatureExtraction.Extractors
{
    /// <summary>
    /// EXEからの特徴抽出を行うクラス
    /// 実行ファイルは以下のオプションを実装している必要がある
    ///     --help : 特徴抽出器の簡単な説明を出力
    ///     --name : 特徴抽出器の名前を出力
    ///     --author : 特徴抽出器の作成者の名前を出力
    ///     --extract -f /path/to/file : 指定した画像ファイルから特徴を抽出
    /// </summary>
    class BinaryExtractor : IExtractor
    {
        private string _binaryFilename;

        public BinaryExtractor(string binary_filename)
        {
            if (!File.Exists(binary_filename) || Path.GetExtension(binary_filename) != ".exe")
            {
                throw new ArgumentException("Argument 'filename' must be a valid path to the executable!");    
            }
            this._binaryFilename = binary_filename;

            try
            {
                var name = this.Name;
                var help = this.Help;
                var author = this.Author;
                if (name == "")
                {
                    throw new ArgumentException("binary extractor must have --name option, and return not-null value");
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Specified binary file is not valid extractor format." + Environment.NewLine + ex.Message);
            }

        }

        /// <summary>
        /// ファイルからの特徴抽出を行う
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public IEnumerable<double> Extract(FaceData face)
        {
            if(!File.Exists(face.ImagePath))
                throw new ArgumentException("Invalid filepath!");

            //取りあえずカレントディレクトリをバイナリのあるディレクトリに移動
            var current_directory_old = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetDirectoryName(this._binaryFilename));

            try
            {
                string arguments = String.Format("--extract -f {0}", face.ImagePath);
                Process p = StartExecutable(arguments);
                p.WaitForExit();
                string error_msg = p.StandardError.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(error_msg))
                {
                    throw new IOException("プロセスがエラーを返しました。" + Environment.NewLine
                        + error_msg);
                }
                else
                {
                    List<double> feature_vec = new List<double>();
                    try
                    {
                        foreach (var token in p.StandardOutput.ReadToEnd().Split(' ', '\0', '\n', '\t'))
                        {
                            if (String.IsNullOrWhiteSpace(token))
                            {
                                continue;
                            }
                            feature_vec.Add(Double.Parse(token));
                        }
                        return feature_vec;
                    }
                    catch (FormatException e)
                    {
                        throw e;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Directory.SetCurrentDirectory(current_directory_old);
            }
        }

        public void ExtractForDebug(string filename, FaceData face)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 実行形式ファイルを指定した引数で実行する
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private Process StartExecutable(string arguments)
        {
            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.FileName = this._binaryFilename;
            pinfo.Arguments = arguments;
            pinfo.CreateNoWindow = true;
            pinfo.UseShellExecute = false;
            pinfo.RedirectStandardOutput = true;
            pinfo.RedirectStandardError = true;

            return Process.Start(pinfo);
        }

        /// <summary>
        /// 特徴抽出器についての簡単な説明
        /// > path/to/program/program.exe --help
        /// をコールし、標準出力を返す。
        /// </summary>
        public string Help
        {
            get
            {
                Process p = StartExecutable("--help");
                p.WaitForExit();
                string error_msg = p.StandardError.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(error_msg))
                {
                    throw new IOException("プロセスがエラーを返しました。" + Environment.NewLine
                        + error_msg);
                }
                return p.StandardOutput.ReadToEnd();
            }
        }

        /// <summary>
        /// 特徴抽出器の作者
        /// > path/to/program/program.exe --author
        /// をコールし、標準出力を返す。
        /// </summary>
        public string Author
        {
            get
            {
                Process p = StartExecutable("--author");
                p.WaitForExit();
                string error_msg = p.StandardError.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(error_msg))
                {
                    throw new IOException("プロセスがエラーを返しました。" + Environment.NewLine
                        + error_msg);
                }
                return p.StandardOutput.ReadToEnd().Replace(@"\n", "");
            }
        }

        /// <summary>
        /// 特徴抽出器の名前
        /// > path/to/program/program.exe --name
        /// をコールし、標準出力を返す。
        /// </summary>
        public string Name
        {
            get
            {
                Process p = StartExecutable("--name");
                p.WaitForExit();
                string error_msg = p.StandardError.ReadToEnd();
                if (!String.IsNullOrWhiteSpace(error_msg))
                {
                    throw new IOException("プロセスがエラーを返しました。" + Environment.NewLine
                        + error_msg);
                }
                return p.StandardOutput.ReadToEnd().Replace(@"\n", "");
            }
        }

    }
}
