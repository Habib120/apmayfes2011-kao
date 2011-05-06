using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Labeling
{
    public partial class ProgressWindow : Form
    {
        public ProgressWindow()
        {
            InitializeComponent();
        }

        private void ProgressWindow_Load(object sender, EventArgs e)
        {
            this.progressBar1.Maximum = 100;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Style = ProgressBarStyle.Continuous;
        }

        public void SetLabel(string msg)
        {
            this.Invoke(new MethodInvoker(() => {
               this.label1.Text = msg;
                this.Refresh();
            }));
        }

        public void SetProgress(double progress)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                this.progressBar1.Value = (int)progress;
                this.Refresh();
            }));
        }
    }
}
