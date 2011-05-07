using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

namespace DataVariation
{
    public class FaceDataSchemaReader
    {
        public FaceDataSchema Read(string dirname)
        {
            DirectoryInfo di = new DirectoryInfo(dirname);
            FileInfo xmlfile;
            try
            {
                xmlfile = di.GetFiles().Where((f) => f.Name == "labels.xml").Single();
            }
            catch
            {
                throw new Exception("labels.xmlが見つかりませんでした。");
            }
            XElement root = XDocument.Load(xmlfile.FullName).Root;

            LabelDefinition def = new LabelDefinition(root.Element("definitions"));


            FaceDataSchema ret = new FaceDataSchema();
            ret.LabelDefinition = def;

            foreach (var item in root.Element("data").Elements("item"))
            {
                try
                {
                    FaceData data = new FaceData(item);
                    ret.Add(data);
                }
                catch
                {
                    continue; //エラーが出ても無視する
                }
            }

            return ret;
        }
    }
}
