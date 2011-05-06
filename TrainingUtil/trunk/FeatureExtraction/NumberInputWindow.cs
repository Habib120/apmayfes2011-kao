using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FeatureExtraction
{
    public partial class NumberInputWindow : Form
    {
        public NumberInputWindow()
        {
            InitializeComponent();
        }

        public string Message
        {
            get
            {
                return label1.Text;
            }
            set
            {
                label1.Text = value;
            }
        }

        public int Value {
            get
            {
                return (int)numericUpDown1.Value;
            }
            set
            {
                numericUpDown1.Value = value;
            }
        }

        public int Max
        {
            get
            {
                return (int)numericUpDown1.Maximum;
            }
            set
            {
                numericUpDown1.Maximum = (int)value;
            }
        }

        public int Min
        {
            get
            {
                return (int)numericUpDown1.Minimum;
            }
            set
            {
                numericUpDown1.Minimum = (int)value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void numericUpDown1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                return;
            }
            e.Handled = false;
            
        }

    }
}
