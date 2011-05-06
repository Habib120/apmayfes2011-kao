using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataVariation
{
    public class LabelInfo
    {
        public Dictionary<string, string> Labels;

        public LabelInfo()
        {
            Labels = new Dictionary<string, string>();
        }


        /// <summary>
        /// Xmlからデータを読み込む
        /// 具体的には/items/item/labelsノードを受けて
        /// </summary>
        /// <param name="elem"></param>
        public LabelInfo(XElement elem)
        {
            Labels = new Dictionary<string, string>();
            foreach (var item_xml in elem.Elements("item"))
            {
                string name = item_xml.Element("name").Value;
                string value = item_xml.Element("value").Value;
                Labels.Add(name, value);
            }
        }

        public XElement AsXml()
        {
            XElement elem = new XElement("labels");
            foreach (var kvp in Labels)
            {
                XElement item_elem = new XElement("item");
                item_elem.Add(new XElement("name", kvp.Key));
                item_elem.Add(new XElement("value", kvp.Value));
                elem.Add(item_elem);
            }
            return elem;
        }

        public string this[string key]
        {
            get
            {
                return Labels[key];
            }
            set
            {
                Labels[key] = value;
            }
        }

        public LabelInfo Clone()
        {
            var ret = new LabelInfo();
            foreach (var kvp in Labels)
            {
                ret.Labels.Add(kvp.Key, kvp.Value);
            }
            return ret;
        }
    }
}
