using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Мутан;

namespace MathTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            Form1 form1_test = new Form1();
            form1_test.
        }

        public void TestMethod1_0_0()
        {
            //arange
            double x = 0;
            double expected = 0;
            //act
            WindowsFormsApp2_1.Form1 c = new WindowsFormsApp2_1.Form1();
            double actual = c.Method1(x);
            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
