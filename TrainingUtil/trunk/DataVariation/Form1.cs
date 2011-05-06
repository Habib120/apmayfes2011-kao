using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataVariation
{
    public partial class Form1 : Form
    {
        string dirname = "";
        DataVariatorSchema schema;

        List<VariatorForm> forms;

        public Form1()
        {
            InitializeComponent();
            forms = new List<VariatorForm>();
            schema = new DataVariatorSchema();
        }

        private IDataVariator SelectedVariator
        {
            get
            {
                var item = (ComboBoxVariatorItem)comboBox1.SelectedItem;
                return item.Variator;
            }
        }

        private void button_Browse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fDialog = new FolderBrowserDialog())
            {
                fDialog.RootFolder = Environment.SpecialFolder.MyComputer;
                fDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (fDialog.ShowDialog() == DialogResult.OK)
                {
                    dirname = fDialog.SelectedPath;
                    textBox1.Text = dirname;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var variators = DataVariatorSchema.GetDefinedVariators();
            comboBox1.Items.AddRange(variators.Select((v) => new ComboBoxVariatorItem
                {
                    Variator = v
                }).ToArray());
            comboBox1.SelectedIndex = 0;
        }


        private void button_Add_Click(object sender, EventArgs e)
        {
            var variator = DataVariatorSchema.CreateInstance(SelectedVariator.ToString());
            schema.Add(variator);
            reloadVariatorForms();
        }

        public void reloadVariatorForms()
        {
            panel_Variations.SuspendLayout();
            var deleted_forms = panel_Variations.Controls.Cast<VariatorForm>()
                .Where((f) => !schema.GetForms().Contains(f));
            foreach (var deleted_form in deleted_forms)
            {
                panel_Variations.Controls.Remove(deleted_form);
            }
            foreach (var item in schema.GetForms().Select((form, i) => new {form, i}))
            {
                item.form.Top = item.i * item.form.Height;
                if (item.form.OnDelete == null)
                {
                    item.form.OnDelete += new VariationFormDeleteEventHandler(variatorForm_Delete);
                }
                if (!panel_Variations.Contains(item.form))
                {
                    panel_Variations.Controls.Add(item.form);
                }
            }
            panel_Variations.ResumeLayout();
        }

        public void variatorForm_Delete(object sender)
        {
            var form = (VariatorForm)sender;
            schema.Remove(form.Variator);
            reloadVariatorForms();
        }
    }

    public class ComboBoxVariatorItem
    {
        public IDataVariator Variator { get; set; }

        public override string ToString()
        {
            return Variator.Name;
        }
    }
}

