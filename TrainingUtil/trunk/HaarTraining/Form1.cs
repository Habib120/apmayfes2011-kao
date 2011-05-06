using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using Emgu.Util;
using Emgu.CV.UI;
using System.Threading;

namespace HaarTraining
{
    public partial class Form1 : Form
    {
        private string _save_dir = "";
        private Capture capture;
        private Image<Bgr, byte> currentFrame;
        private int cp_count = 0;
        private bool quitcap = false;

        private Object lockObject = new Object();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button_Capture.Click += (seed, ev) =>
                {
                    if (!String.IsNullOrEmpty(_save_dir))
                    {
                        SaveCurrentFrame();
                    }
                    else
                    {
                        OpenSaveDirDialog();
                    }
                };

            try
            {
                capture = new Capture();   
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Application.Exit();
            }
            backgroundWorker1.DoWork += (seed, ev) => CaptureThreadProc();
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        public void ConfirmSaveDir()
        {
            if (MessageBox.Show(_save_dir + Environment.NewLine +
                "にキャプチャ画像を保存します。よろしいですか？", "保存先の確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
            {
                folderBrowserDialog1.SelectedPath = _save_dir;
                if (folderBrowserDialog1.ShowDialog() != DialogResult.Cancel)
                {
                    this._save_dir = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        public void SaveCurrentFrame()
        {
            Image<Bgr, byte> copyFrame;
            lock (lockObject)
            {
                copyFrame = currentFrame.Copy();
            }
            string filename = "";
            //すでにある学習データを上書きしないように。
            while (true)
            {
                filename = String.Format("face_{0:D4}.jpg", cp_count);
                if (File.Exists(filename))
                    cp_count++;
                else
                    break;
            }
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += (seed, ev) =>
                {
                    cp_count++;
                    this.Invoke(new MethodInvoker(() => {label1.Text = filename + "を保存しています。";}));
                    copyFrame.Save(this._save_dir + @"\" + filename);
                };
            bw.RunWorkerCompleted += (seed, ev) =>
                {
                    if (ev.Cancelled)
                    {
                        cp_count--;
                        return;
                    }
                    if (ev.Error != null)
                    {
                        this.Invoke(new MethodInvoker(() => { 
                            label1.Text = filename + "の保存中にエラーが発生しました。"; 
                            listBox1.Items.Add("[Error]" + filename);
                        }));
                        return;
                    }
                    this.Invoke(new MethodInvoker(() => { 
                        label1.Text = filename + "を保存しました。";
                        listBox1.Items.Add(filename);
                    }));
                };
            bw.RunWorkerAsync();
        }

        /// <summary>
        /// キャプチャーを終了し、キャプチャスレッドが終了するまで待機する。
        /// </summary>
        public void DisposeCapturing()
        {
            quitcap = true;
            while (true)
            {
                if (backgroundWorker1.IsBusy)
                {
                    Application.DoEvents();
                    Thread.Sleep(60);
                }
                else
                {
                    break;
                }
            }
            capture.Dispose();
        }

        public void CaptureThreadProc()
        {
            while (!quitcap)
            {
                long tc = Environment.TickCount;
                lock (lockObject)
                {
                    currentFrame = capture.QueryFrame();
                }
                this.Invoke(new MethodInvoker(() =>
                {   
                    imageBox1.Image = currentFrame;
                    imageBox1.Refresh();
                }));
                long tc2 = Environment.TickCount;
                if (tc2 < tc + 30)
                {
                    Thread.Sleep((int)(30 - (tc2 - tc)));
                }
            }
            return;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DisposeCapturing();
        }

        private void button_Capture_Click(object sender, EventArgs e)
        {

        }

        private void OpenSaveDirDialog()
        {
            using (FolderBrowserDialog fDialog = new FolderBrowserDialog())
            {
                fDialog.RootFolder = System.Environment.SpecialFolder.MyDocuments;
                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    this._save_dir = fDialog.SelectedPath;
                    if (Directory.GetFiles(_save_dir).Count() > 0 &&
                        MessageBox.Show("フォルダ内にファイルが存在します。削除しますか？", "Capture", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        DirectoryInfo di = new DirectoryInfo(_save_dir);
                        foreach (var f in di.GetFiles())
                        {
                            f.Delete();
                        }
                    }
                }
            }
        }

        private void menuItem_OpenSaveDir_Click(object sender, EventArgs e)
        {
            OpenSaveDirDialog();
        }

        private void menuItem_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }



    }
}
