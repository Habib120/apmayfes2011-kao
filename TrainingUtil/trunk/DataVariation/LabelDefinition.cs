using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DataVariation
{
    public class LabelDefinition : Dictionary<string, string[]>
    {
        public LabelDefinition(XElement xml)
        {
            foreach (var def in xml.Elements("def"))
            {
                var key = def.Element("name").Value;
                var values = def.Element("values").Elements("item").Select((e) => e.Value).ToArray();
                this.Add(key, values);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public XDocument AsXml()
        {
            throw new NotImplementedException();
        }
    }
}
