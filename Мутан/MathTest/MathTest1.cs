using Microsoft.VisualStudio.TestTools.UnitTesting;
using Мутан;

namespace MathTest
{
    [TestClass]
    public class MathTest1
    {
        #region Singleton для тестируемого объекта

        private static Form1 single_object; // единственный объект класса

        public static Form1 Instance // свойство
        {
            get
            {
                if (single_object == null)
                {
                    single_object = new Form1();
                    single_object.listArray = new double[] { 123.3, 133.3, 77.8, 92.2 };
                    // типа input
                }
                return single_object;
            }
        }

        #endregion

        // Нда мало что нормально выйдет если архитектура такая не гибкая изначально

        [TestMethod]
        public void Test_quickSort()
        {
            Instance.quickSort(Instance.listArray, 0, Instance.listArray.Length - 1);

            double[] expected = { 77.8, 92.2, 123.3, 133.3 }; // result
            CollectionAssert.AreEqual(expected, single_object.listArray);
        }
        
        [TestMethod]
        public void Test_min()
        {
            Instance.min = Instance.GetMin(Instance.listArray);
            
            double expected = 77.8;
            Assert.AreEqual(expected, Instance.min);
        }
        
        [TestMethod]
        public void Test_max()
        {
            Instance.max = Instance.GetMax(Instance.listArray);
            
            double expected = 133.3;
            Assert.AreEqual(expected, Instance.max);
        }
        
        [TestMethod]
        public void Test_CalcK()
        {
            Instance.CalcK();
            
            double expected = 4;
            Assert.AreEqual(expected, Instance.K);
        }
        
        [TestMethod]
        public void Test_CalcR()
        {
            Instance.CalcR();
            
            double expected = 55.5;
            Assert.AreEqual(expected, Instance.R);
        }
        
        [TestMethod]
        public void Test_CalcStep()
        {
            Instance.CalcStep();
            
            double expected = 13.9;
            Assert.AreEqual(expected, Instance.step_H);
        }
        
        [TestMethod]
        public void Test_CalcD()
        {
            Instance.quickSort(Instance.listArray, 0, Instance.listArray.Length - 1);
            Instance.min = Instance.GetMin(Instance.listArray);
            Instance.max = Instance.GetMax(Instance.listArray);
            Instance.CalcK();
            Instance.CalcR();
            Instance.CalcStep();

            Instance.CalcD();
            
            double expected = 0.1;
            Assert.AreEqual(expected, Instance.D);
        }
        
        [TestMethod]
        public void Test_CalcD2()
        {
            Instance.CalcD2();
            
            double expected = 0;
            Assert.AreEqual(expected, Instance.D2);
        }
        
        [TestMethod]
        public void Test_Calc_X̅()
        {
            
            Instance.Calc_X̅();
            
            double expected = 106.65;
            Assert.AreEqual(expected, Instance.X_Sr);
        }
        
        [TestMethod]
        public void Test_Calc_S()
        {
            
            Instance.Calc_S();
            
            double expected = 0;
            Assert.AreEqual(expected, Instance.S);
        }

    }
}
