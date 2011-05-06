using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace FeatureExtraction.ML
{
    public class MLMethodTable
    {
        public static IEnumerable<IMLMethod> GetAll()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var matches = asm.GetTypes().Where((t) => t.GetInterfaces().Contains(typeof(IMLMethod)));
            List<IMLMethod> extractors = new List<IMLMethod>();
            foreach (var type in matches)
            {
                extractors.Add((IMLMethod)asm.CreateInstance(type.ToString()));
            }
            return extractors;
        }
    }
}
