using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataVariation;

namespace TestProject
{
    [TestClass]
    public class VariatorSchemaTest
    {
        [TestMethod]
        public void AddTest()
        {
            DataVariatorSchema schema = new DataVariatorSchema();

            DataVariatorRotate rotator1 = new DataVariatorRotate();
            DataVariatorRotate rotator2 = new DataVariatorRotate();

            schema.Add(rotator1);
            //同じクラスがいっけんの時には添え字なし
            Assert.IsTrue(rotator1.ID == "回転");
            schema.Add(rotator2);

            Assert.IsTrue(schema.Variators.Count() == 2);

            //2件以上で添え字が登場
            Assert.IsTrue(rotator1.ID == "回転01");
            Assert.IsTrue(rotator2.ID == "回転02");

            //他のクラスを追加しても添え字は影響を受けない
            DataVariatorScale scalor1 = new DataVariatorScale();
            schema.Add(scalor1);
            Assert.IsTrue(scalor1.ID == "スケール");
        }

        [TestMethod]
        public void RemoveTest()
        {
            DataVariatorSchema schema = new DataVariatorSchema();

            DataVariatorRotate rotator1 = new DataVariatorRotate();
            DataVariatorRotate rotator2 = new DataVariatorRotate();
            schema.Add(rotator1);
            schema.Add(rotator2);

            Assert.IsTrue(rotator1.ID == "回転01");
            Assert.IsTrue(rotator2.ID == "回転02");

            schema.Remove(rotator1);

            Assert.IsFalse(schema.Variators.Contains(rotator1));
            //削除したら添え字がもとに戻っている
            Assert.IsTrue(rotator2.ID == "回転");
        }


        [TestMethod]
        public void ReflectionTest()
        {
            var variators = DataVariatorSchema.GetDefinedVariators();
            var names = variators.Select((v) => v.GetType().Name);
            Assert.IsTrue(names.Count() == 4);
            Assert.IsTrue(names.Contains("DataVariatorWarpAffine"));
            Assert.IsTrue(names.Contains("DataVariatorScale"));
            Assert.IsTrue(names.Contains("DataVariatorRotate"));
            Assert.IsTrue(names.Contains("DataVariatorBrightness"));

            var variator = DataVariatorSchema.CreateInstance(variators.First().ToString());
            Assert.IsTrue(variator.ToString() == variators.First().ToString());
        }

        [TestMethod]
        public void MultipleFormSyncTest()
        {
            DataVariatorSchema schema = new DataVariatorSchema();

            DataVariatorRotate rotator1 = new DataVariatorRotate();
            DataVariatorRotate rotator2 = new DataVariatorRotate();
            
            schema.Add(rotator1);
            var forms = schema.GetForms();
            Assert.IsTrue(forms.First().Title == "回転");

            schema.Add(rotator2);
            forms = schema.GetForms();

            Assert.IsTrue(forms.Count() == 2);
            Assert.IsTrue(forms.First().Title == "回転01");
            Assert.IsTrue(forms.Last().Title == "回転02");
        }

    }
}
