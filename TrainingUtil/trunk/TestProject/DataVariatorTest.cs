using DataVariation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace TestProject
{
    
    
    /// <summary>
    ///DataVariatorScaleTest のテスト クラスです。すべての
    ///DataVariatorScaleTest 単体テストをここに含めます
    ///</summary>
    [TestClass()]
    public class DataVariatorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///現在のテストの実行についての情報および機能を
        ///提供するテスト コンテキストを取得または設定します。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        
        private FaceData GetFaceData()
        {
            FaceData data = new FaceData();
            var asm = System.Reflection.Assembly.GetExecutingAssembly();
            Console.WriteLine(asm.Location);
            data.Image = new Image<Bgr, byte>(System.IO.Path.GetDirectoryName(asm.Location) + @"/Test/images/face_0000.jpg");
            data.Filename = "face_0000.jpg";
            return data;
        }

        #region 追加のテスト属性
        // 
        //テストを作成するときに、次の追加属性を使用することができます:
        //
        //クラスの最初のテストを実行する前にコードを実行するには、ClassInitialize を使用
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //クラスのすべてのテストを実行した後にコードを実行するには、ClassCleanup を使用
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //各テストを実行する前にコードを実行するには、TestInitialize を使用
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //各テストを実行した後にコードを実行するには、TestCleanup を使用
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        private void TestVariatorResult(string title, IDataVariator variator)
        {
            FaceData data = GetFaceData();
            var results = variator.GetVariation(data);
            CvInvoke.cvNamedWindow("test_scale");
            MessageBox.Show(String.Format("{0}のテストを開始します", title));
            foreach (var result in results)
            {
                CvInvoke.cvShowImage("test_scale", result.Image);
                CvInvoke.cvWaitKey(0);
            }
            CvInvoke.cvDestroyWindow("test_scale");
            var r = MessageBox.Show(String.Format("{0}のテスト。結果は正しいですか？", title),
                    "テスト",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
            Assert.IsTrue(r == DialogResult.Yes);
        }


        [TestMethod()]
        public void ScalingTest()
        {
            var variator = new DataVariatorScale();
            var form = new VariatorForm();
            form.Value = "0.5";
            variator.Bind(form);
            TestVariatorResult("スケーリング", (IDataVariator)variator);
        }

        [TestMethod()]
        public void AffineTest()
        {
            var variator = new DataVariatorWarpAffine();
            var form = new VariatorForm();
            form.Value = "0.7";
            variator.Bind(form);
            TestVariatorResult("アフィン変換", (IDataVariator)variator);
        }

        [TestMethod()]
        public void BrightnessTest()
        {
            var variator = new DataVariatorBrightness();
            var form = new VariatorForm();
            form.Value = "0.5";
            variator.Bind(form);
            TestVariatorResult("明るさ変動", (IDataVariator)variator);
        }

        [TestMethod()]
        public void RotateTest()
        {
            var variator = new DataVariatorRotate();
            var form = new VariatorForm();
            form.Value = "30";
            variator.Bind(form);
            TestVariatorResult("回転", (IDataVariator)variator);
        }

    }
}
