using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataVariation
{
    public delegate void VariationFormDeleteEventHandler(object sender);

    public partial class VariatorForm : UserControl
    {
        public VariationFormDeleteEventHandler OnDelete;

        public VariatorForm()
        {
            InitializeComponent();
        }

        public IDataVariator Variator;

        public string Value
        {
            get
            {
                return textBox1.Text;
            }
            set
            {
                textBox1.Text = value;
            }
        }

        public string Title
        {
            get
            {
                return label_Title.Text;
            }
            set
            {
                label_Title.Text = value;
            }
        }

        public string PropertyName
        {
            get
            {
                return label_PropertyName.Text;
            }
            set
            {
                label_PropertyName.Text = value;
            }
        }

        private string _error;
        
        /// <summary>
        /// 入力のバリデーション
        /// </summary>
        public string Error
        {
            get
            {
                return _error;
            }
            set
            {
                _error = value;
                if (String.IsNullOrEmpty(_error))
                {
                    label_PropertyName.ForeColor = Color.Black;
                }
                else
                {
                    label_PropertyName.BackColor = Color.Red;
                }
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (OnDelete != null)
            {
                OnDelete(this);
            }
        }
    }
}
