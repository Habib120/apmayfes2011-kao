using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Labeling
{
    public partial class Labelling : Form
    {
        HaarLabelManager manager;
        string LabelFilename;
        string CurrentDir;
        Thread DrawThread;
        bool StopPainting = false;

        public Labelling()
        {
            InitializeComponent();
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void imageBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (manager != null)
            {
                manager.MouseDown(e.Location, imageBox1.Size);
            }
        }

        private void imageBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (manager != null)
            {
                manager.MouseUp(e.Location, imageBox1.Size);
            }
        }

        private void imageBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (manager != null)
            {
                manager.MouseMove(e.Location, imageBox1.Size);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (manager != null)
            {
                manager.Rotate(trackBar1.Value / (double)trackBar1.Maximum * 180);
            }
        }

        private void openDir_Click(object sender, EventArgs e)
        {
            if (DrawThread != null && DrawThread.ThreadState != ThreadState.Stopped)
            {
                StopPainting = true;
                while (true)
                {
                    if (DrawThread.ThreadState == ThreadState.Stopped)
                    {
                        break;
                    }
                    Application.DoEvents();
                    Thread.Sleep(60);
                }
            }
            folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyDocuments;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentDir = folderBrowserDialog1.SelectedPath;
                var di = new DirectoryInfo(CurrentDir);
                var idxfiles = from f in di.GetFiles()
                               where f.Extension == ".lbl"
                               select f;
                if (idxfiles.Count() > 0)
                {
                    var idxfile = idxfiles.ElementAt(0);
                    var msg = String.Format("ラベルファイル\"{0}\"が見つかりました。このファイルを読み込んでラべリングを行いますか？", idxfile.Name);
                    var result = MessageBox.Show(msg, "Labelling Manager", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            this.LabelFilename = idxfile.FullName;
                            manager = new HaarLabelManager(this, CurrentDir, idxfile.FullName);
                            imageBox1.Image = manager.drawImage(imageBox1.Size);
                            ReloadListView();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Labelling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else
                {
                    try {
                        manager = new HaarLabelManager(this, CurrentDir, "");
                        imageBox1.Image = manager.drawImage(imageBox1.Size);
                        ReloadListView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Labelling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                StopPainting = false;
                DrawThread = new Thread(new ThreadStart(DrawThreadProc));
                DrawThread.Start();
            }
        }

        private void DrawThreadProc()
        {
            while (!StopPainting)
            {
                    var tickbefore = Environment.TickCount;
                    if (this.IsHandleCreated)
                    {
                        this.Invoke(new MethodInvoker(() =>
                        {
                            if (manager != null)
                            {
                                try
                                {
                                    imageBox1.Image = manager.drawImage(imageBox1.Size);
                                }
                                catch
                                {
                                }
                            }
                        }));
                    }
                    var tickafter = Environment.TickCount;
                    if (60 - (tickafter - tickbefore) > 0)
                    {
                        Thread.Sleep((int)(60 - (tickafter - tickbefore)));
                    }
            }
        }

        private void ReloadListView()
        {
            if (manager != null)
            {
                listBox1.Items.Clear();
                foreach (var item in manager.Labels.Select( (v, i) => new {v, i}))
                {
                    listBox1.Items.Add(item.v.ImageFileInfo.Name);
                }
                SyncSelected();
            }
        }

        private void SyncSelected()
        {
            if (manager != null)
            {
                try
                {
                    listBox1.SelectedIndex = manager.SelectedIndex;
                }
                catch
                {
                    ReloadListView();
                }
            }
        }

        private void menuItem_LoadLabelFile_Click(object sender, EventArgs e)
        {

        }

        private void button_Next_Click(object sender, EventArgs e)
        {
            if (manager != null && manager.Labels.Count() > 0)
            {
                manager.SelectedIndex++;
                SyncSelected();
            }
        }

        private void button_Prev_Click(object sender, EventArgs e)
        {
            if (manager != null && manager.Labels.Count() > 0)
            {
                manager.SelectedIndex--;
                SyncSelected();
            }
        }

        private void button_AutoDetect_Click(object sender, EventArgs e)
        {
            if (manager != null && manager.Labels.Count() > 0)
            {
                manager.LabelItemAuto(manager.SelectedLabel);
                ShowLabelInfo(manager.SelectedLabel);
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (manager != null && manager.Labels.Count() > 0)
            {
                int index = manager.SelectedIndex;
                manager.RemoveAt(index);
                listBox1.Items.RemoveAt(index);
                listBox1.SelectedIndex = index;
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (manager != null && manager.Labels.Count() > 0)
            {
                manager.SelectedIndex = listBox1.SelectedIndex;
                ShowLabelInfo(manager.SelectedLabel);
            }
            this.Refresh();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            
            if (e.Index != listBox1.SelectedIndex)
            {
                //ラベル付けされたものをマークアップする。
                if (manager != null && !manager.Labels[e.Index].IsNew)
                {
                    e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
                }
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), (RectangleF)e.Bounds);
            }
            else
            {
                if (e.Index >= 0)
                {
                    e.Graphics.FillRectangle(Brushes.Blue, e.Bounds);
                    e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), (RectangleF)e.Bounds);
                }
            }
        }

        public void ShowLabelInfo(HaarLabel l)
        {
            checkBox_Man.Checked = l.Gender == HaarLabel.Genders.Male;
            checkBox_Women.Checked = l.Gender == HaarLabel.Genders.Female;
            checkBox_Smile.Checked = l.Smiling;
            checkBox_Glasses.Checked = l.HasGlasses;
            trackBar1.Value = (int)Math.Floor(trackBar1.Maximum * l.Rotation / 180);
        }

        private void checkBox_Man_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Women.Checked = !checkBox_Man.Checked;
            manager.SelectedLabel.Gender = checkBox_Man.Checked ? HaarLabel.Genders.Male : HaarLabel.Genders.Female;
        }

        private void checkBox_Women_CheckedChanged(object sender, EventArgs e)
        {
            checkBox_Man.Checked = !checkBox_Women.Checked;
            manager.SelectedLabel.Gender = checkBox_Man.Checked ? HaarLabel.Genders.Male : HaarLabel.Genders.Female;
        }

        private void menuItem_LabelAllItems_Click(object sender, EventArgs e)
        {
            ProgressWindow pwindow = new ProgressWindow();
            pwindow.Show();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += (seed, args) =>
                {
                        manager.LabelItemsAuto(manager.Labels, (msg) => pwindow.SetLabel(msg),
                            (progress) => pwindow.SetProgress(progress));
                };
            backgroundWorker1.RunWorkerCompleted += (seed, args) =>
                {
                    pwindow.Close();
                };
            backgroundWorker1.RunWorkerAsync();
        }

        private void menuItem_LabelNewItems_Click(object sender, EventArgs e)
        {
            ProgressWindow pwindow = new ProgressWindow();
            pwindow.Show();
            backgroundWorker1 = new BackgroundWorker();
            backgroundWorker1.DoWork += (seed, args) =>
                {
                    manager.LabelItemsAuto(manager.NewLabels, (msg) => pwindow.SetLabel(msg),
                        (progress) => pwindow.SetProgress(progress));
                };
            backgroundWorker1.RunWorkerCompleted += (seed, args) =>
                {
                    pwindow.Close();
                };
            backgroundWorker1.RunWorkerAsync();
        }

        private void menuItem_SaveNew_Click(object sender, EventArgs e)
        {
            saveNew();
        }

        private void saveNew()
        {
            if (manager == null) {
                return;
            }
            saveFileDialog1.Filter = "ラベルファイル(*.lbl)|*.lbl";
            saveFileDialog1.InitialDirectory = CurrentDir;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                manager.Save(saveFileDialog1.FileName);
            }
            this.LabelFilename = saveFileDialog1.FileName;
        }

        private void menuItem_Save_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.LabelFilename))
            {
                saveNew();
            }
            else
            {
                manager.Save(LabelFilename);
            }
        }

        private void menuItem_ExtractFaces_Click(object sender, EventArgs e)
        {
            if (manager == null || manager.Labels.Count() == 0) 
                return;

            folderBrowserDialog1.SelectedPath = CurrentDir;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath == CurrentDir)
                {
                    MessageBox.Show("読み込み先フォルダと同じディレクトリにファイルをエクスポートすることはできません。", "Labelling",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var result = MessageBox.Show("古いファイルをすべて消去しますか？", "Labelling", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                ProgressWindow pwindow = new ProgressWindow();
                pwindow.Show();
                pwindow.SetLabel("エクスポートしています...");

                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (seed, args) =>
                {
                    if (result == DialogResult.Yes)
                    {
                        manager.ExtractFaces(folderBrowserDialog1.SelectedPath, null, true, (msg) => pwindow.SetLabel(msg), (progress) => pwindow.SetProgress(progress));
                    }
                    else if (result == DialogResult.No)
                    {
                        manager.ExtractFaces(folderBrowserDialog1.SelectedPath, null, false, (msg) => pwindow.SetLabel(msg), (progress) => pwindow.SetProgress(progress));
                    }
                    else
                    {
                        return;
                    }
                };
                bw.RunWorkerCompleted += (seed, args) =>
                {
                    pwindow.Close();
                    MessageBox.Show("エクスポートを完了しました。", "Labelling", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                };
                bw.RunWorkerAsync();
            }

        }

        private void menuItem_ExtractForML_Click(object sender, EventArgs e)
        {
            if (manager == null || manager.Labels.Count() == 0) return;
            folderBrowserDialog1.SelectedPath = CurrentDir;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (folderBrowserDialog1.SelectedPath == CurrentDir)
                {
                    MessageBox.Show("読み込み先フォルダと同じディレクトリにファイルをエクスポートすることはできません。", "Labelling",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var result = MessageBox.Show("古いファイルをすべて消去しますか？", "Labelling", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                ProgressWindow pwindow = new ProgressWindow();
                pwindow.Show();
                pwindow.SetLabel("エクスポートしています...");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (seed, args) =>
                {
                    if (result == DialogResult.Yes)
                    {
                        manager.ExtractFacesForML(folderBrowserDialog1.SelectedPath, true, (msg) => pwindow.SetLabel(msg), (progress) => pwindow.SetProgress(progress));
                    }
                    else if (result == DialogResult.No)
                    {
                        manager.ExtractFacesForML(folderBrowserDialog1.SelectedPath, false, (msg) => pwindow.SetLabel(msg), (progress) => pwindow.SetProgress(progress));
                    }
                    else
                    {
                        return;
                    }
                };
                bw.RunWorkerCompleted += (seed, args) =>
                    {
                        pwindow.Close();
                        MessageBox.Show("エクスポートを完了しました。", "Labelling", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    };
                bw.RunWorkerAsync();
            }
        }

        private void LabelingWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DrawThread != null && DrawThread.ThreadState != ThreadState.Stopped)
            {
                StopPainting = true;
                while (true)
                {
                    if (DrawThread.ThreadState == ThreadState.Stopped)
                    {
                        break;
                    }
                    Application.DoEvents();
                    Thread.Sleep(60);
                }
            }
        }

        private void menuItem_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox_Glasses_CheckedChanged(object sender, EventArgs e)
        {
            manager.SelectedLabel.HasGlasses = checkBox_Glasses.Checked;
        }

        private void checkBox_Smile_CheckedChanged(object sender, EventArgs e)
        {
            manager.SelectedLabel.Smiling = checkBox_Smile.Checked;
        }
    }
}
