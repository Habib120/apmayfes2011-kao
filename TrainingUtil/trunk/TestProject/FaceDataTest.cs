using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataVariation;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Windows.Forms;

namespace TestProject
{
    [TestClass]
    public class FaceDataTest
    {
        [TestMethod]
        public void ReadTest()
        {
            var reader = new FaceDataSchemaReader();
            var schema = reader.Read(@"C:\Users\t2ladmin\Documents\smiles\faces");
            var labeldef = schema.LabelDefinition;

            //ラベルのテスト
            Assert.IsTrue(labeldef.Keys.Contains("is_smiling"));
            Assert.IsTrue(labeldef["is_smiling"].Contains("smiling"));
            Assert.IsTrue(labeldef["is_smiling"].Contains("not_smiling"));
            Assert.IsTrue(labeldef.Keys.Contains("gender"));
            Assert.IsTrue(labeldef.Keys.Contains("glasses"));

            //画像読み込みのテスト
            Assert.IsTrue(schema.Count() > 0);
            MessageBox.Show("画像読み込みのテストを開始します");

            CvInvoke.cvNamedWindow("dataschema_read");
            CvInvoke.cvShowImage("dataschema_read", schema.First().Image);

            var r = MessageBox.Show("画像読み込みは正しく行われていますか？",
                "test",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            Assert.IsTrue(r == DialogResult.Yes);
        }

        [TestMethod]
        public void WriteTest()
        {
        }
    }
}
