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
    public partial class LoadingWindow : Form
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }

        private void LoadingWindow_Load(object sender, EventArgs e)
        {
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = 1000;
        }

        public void SetTitle(string title)
        {
            if (this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(() => this.Text = title));
                }
                else
                {
                    this.Text = title;
                }
            }
        }

        public void SetMessage(string msg)
        {
            if (this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(() => this.label1.Text = msg));
                }
                else
                {
                    this.label1.Text = msg;
                }
            }
        }

        public void SetProgressBarStyle(ProgressBarStyle style)
        {
            if (this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(() => this.progressBar1.Style = style));
                }
                else
                {
                    this.progressBar1.Style = style;
                }
            }
        }

        public void SetProgress(double progress)
        {
            if (this.IsHandleCreated)
            {
                if (this.InvokeRequired)
                {
                    this.BeginInvoke(new MethodInvoker(() => this.progressBar1.Value = (int)(progress * 1000)));
                }
                else
                {
                    this.progressBar1.Value = (int)(progress * 1000);
                }
            }
        }

        private void LoadingWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
