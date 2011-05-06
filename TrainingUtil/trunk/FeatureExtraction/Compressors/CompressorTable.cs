using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FeatureExtraction.Compressors
{
    public class CompressorTable
    {
        public static IEnumerable<ICompressor> GetAll()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var matches = asm.GetTypes().Where((t) => t.GetInterfaces().Contains(typeof(ICompressor)));
            List<ICompressor> compressors = new List<ICompressor>();
            foreach (var type in matches)
            {
                compressors.Add((ICompressor)asm.CreateInstance(type.ToString()));
            }
            return compressors;
        }
    }
}
