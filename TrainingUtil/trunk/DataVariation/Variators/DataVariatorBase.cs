using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataVariation
{
    public abstract class DataVariatorBase : IDataVariator
    {
        protected float value;
        protected VariatorForm form;

        public abstract string Name{ get; }
        public abstract string Description { get; }
        public abstract string PropertyName { get; }
        public string ID { get; set; }

        public VariatorForm GetForm()
        {
            if (form != null)
            {
                form.Title = this.ID;
                return form;
            }
            form = new VariatorForm();
            form.Title = this.ID;
            form.PropertyName = this.PropertyName;
            form.Value = value.ToString();
            form.Variator = this;
            return form;
        }

        public void Bind(VariatorForm form)
        {
            try
            {
                this.value = float.Parse(form.Value);
            }
            catch
            {
                throw new Exception("数値を入力してください");
            }
        }

        public abstract IEnumerable<FaceData> GetVariation(FaceData src);
        
    }
}
