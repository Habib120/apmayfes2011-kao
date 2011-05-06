using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace DataVariation
{
    /// <summary>
    /// バリエーターの配列を管理するクラス
    /// </summary>
    public class DataVariatorSchema
    {
        public List<IDataVariator> Variators{ get; protected set;}

        public DataVariatorSchema()
        {
            Variators = new List<IDataVariator>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variator"></param>
        public void Add(IDataVariator variator)
        {
            var same_classes = FindByClassName(variator.GetType().Name);
            var variator_name = variator.Name;
            if (same_classes.Count() > 0)
            {
                foreach (var item in same_classes.Select((v, i) => new {v, i}))
                {
                    item.v.ID =  variator_name + (item.i + 1).ToString("D2");
                }
                variator.ID = variator_name + (same_classes.Count() + 1).ToString("D2");
            }
            else
            {
                variator.ID = variator_name;
            }
            Variators.Add(variator);
        }

        public IEnumerable<IDataVariator> FindByClassName(string name)
        {
            var results = Variators.Where((v) => v.GetType().Name == name);
            return results;
        }

        public void Remove(IDataVariator variator)
        {
            Variators.Remove(variator);
            var same_classes = FindByClassName(variator.GetType().Name);
            var variator_name = variator.Name;
            if (same_classes.Count() == 1)
            {
                same_classes.ElementAt(0).ID = variator_name;
            }
            else
            {
                foreach (var item in same_classes.Select((v, i) => new { v, i }))
                {
                    item.v.ID = variator_name + (item.i + 1).ToString("D2");
                }
            }
        }

        public static IEnumerable<IDataVariator> GetDefinedVariators()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes().Where((t) => t.BaseType == Type.GetType("DataVariation.DataVariatorBase"));
            
            return types.Select((t) => (IDataVariator)asm.CreateInstance(t.ToString()));
        }

        public static IDataVariator CreateInstance(string name)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            return (IDataVariator)asm.CreateInstance(name);
        }

        public IEnumerable<VariatorForm> GetForms()
        {
            foreach (var v in Variators)
            {
                yield return v.GetForm();
            }
        }
    }
}
