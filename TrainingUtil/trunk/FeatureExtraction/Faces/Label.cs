using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeatureExtraction.Faces
{
    public class Label
    {
        public string Name { get; set; }
        public string Value { get; set; }
        private string[] _values;
        public string[] Values
        {
            get { return _values; }
            set
            {
                this._values = value.Distinct().ToArray();
            }
        }
    }
}
