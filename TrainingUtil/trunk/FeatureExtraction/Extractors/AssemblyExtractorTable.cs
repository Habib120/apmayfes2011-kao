using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FeatureExtraction.Extractors
{
    public class AssemblyExtractorTable
    {
        public static IEnumerable<IExtractor> GetAll()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var matches = asm.GetTypes().Where((t) => t.BaseType == typeof(AssemblyExtractor));
            List<IExtractor> extractors = new List<IExtractor>();
            foreach (var type in matches)
            {
                extractors.Add((IExtractor)asm.CreateInstance(type.ToString()));
            }
            return extractors;
        }
    }
}
