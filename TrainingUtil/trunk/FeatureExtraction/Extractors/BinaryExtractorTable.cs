using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace FeatureExtraction.Extractors
{
    public class BinaryExtractorTable
    {
        public static IEnumerable<IExtractor> GetAll()
        {
            var ret = new List<IExtractor>();
            Assembly asm = Assembly.GetEntryAssembly();
            var app_base_dir = Path.GetDirectoryName(asm.Location);
            var base_dir = new DirectoryInfo(Properties.Settings.Default.BINARY_EXTRACTOR_DIRECTORY.Replace("%APP_BASE_DIR%", app_base_dir));
            if (!base_dir.Exists)
                return ret;
            var dirs = base_dir.GetDirectories().ToList();
            dirs.Add(base_dir);
            foreach (var dir in dirs)
            {
                var binary_files = from f in dir.GetFiles()
                                   where f.Extension == ".exe"
                                   select f;
                foreach (var f in binary_files)
                {
                    try
                    {
                        var extractor = new BinaryExtractor(f.FullName);
                        ret.Add(extractor);
                    }
                    catch (Exception ex)
                    {
                        AlertdDialog.Error("特徴抽出器の読み込みに失敗しました", ex);
                        if (AlertdDialog.Question(String.Format("ファイル'{0}'を削除しますか？", f.FullName)) == System.Windows.Forms.DialogResult.Yes)
                        {
                            f.Delete();
                        }
                    }
                }
            }
            return ret;
        }
    }
}
