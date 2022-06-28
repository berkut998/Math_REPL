using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using REPL;
namespace REPL_test
{
    [TestClass]
    public class MathEvaluator_test
    {
        [TestMethod]
        public void sum()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(36, mathEvaluator.evaluate("1+2+3 +4 +5 +6 +7+ 8"));

            MathEvaluator mathEvaluator1 = new MathEvaluator();
            Assert.AreEqual(1000000, mathEvaluator1.evaluate("999999+1"));
        }

        [TestMethod]
        public void substraction()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(-36, mathEvaluator.evaluate("-1-2-3 -4 -5 -6 -7- 8"));

            MathEvaluator mathEvaluator1 = new MathEvaluator();
            Assert.AreEqual(999998, mathEvaluator1.evaluate("999999-1"));
        }

        [TestMethod]
        public void multiply()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(-40320, mathEvaluator.evaluate("-1*2*3 *4 *5 *6 *7* 8"));

            MathEvaluator mathEvaluator1 = new MathEvaluator();
            Assert.AreEqual(999999, mathEvaluator1.evaluate("999999*1"));
        }


        [TestMethod]
        public void divide()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(-2.48015873015873E-05, mathEvaluator.evaluate("-1/2/3 /4 /5 /6 /7/ 8"));

            MathEvaluator mathEvaluator1 = new MathEvaluator();
            Assert.AreEqual(999999, mathEvaluator1.evaluate("999999/1"));
        }


        [TestMethod]
        public void sum_sub_mul_div_pareth()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(1.25, mathEvaluator.evaluate("1+3/(3*4)"));
        }


        [TestMethod]
        public void pow()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(125, mathEvaluator.evaluate("5^3"));
        }



        [TestMethod]
        public void variable_sum_mul_sub_assigm_div_pow()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
           Assert.AreEqual(3, mathEvaluator.evaluate("A=3"));
            Assert.AreEqual(2, mathEvaluator.evaluate("B=2")); 
            Assert.AreEqual(5, mathEvaluator.evaluate("C=A+B"));
            Assert.AreEqual(25, mathEvaluator.evaluate("C^B"));
            Assert.AreEqual(5, mathEvaluator.evaluate("A+C*B/B-A"));
            Assert.AreEqual(3, mathEvaluator.evaluate("ABC=3"));
            Assert.AreEqual(2, mathEvaluator.evaluate("BARD=2"));
            Assert.AreEqual(5, mathEvaluator.evaluate("CADR=ABC+BARD"));
            Assert.AreEqual(33, mathEvaluator.evaluate("X=33"));
            Assert.AreEqual(-1, mathEvaluator.evaluate("(10-X)/23"));
        }



        [TestMethod]
        public void error()
        {
            MathEvaluator mathEvaluator = new MathEvaluator();
            Assert.AreEqual(null, mathEvaluator.evaluate("-1/2/3///// /4 /5 /6 /7/ 8"));
            MathEvaluator mathEvaluator1 = new MathEvaluator();
            Assert.AreEqual(null, mathEvaluator1.evaluate("+-1/2/3/ /4 /5 /6 /7/ 8"));
            MathEvaluator mathEvaluator2 = new MathEvaluator();
            Assert.AreEqual(null, mathEvaluator2.evaluate("*1/2/3/ /4 /5 /6 /7/ 8"));
            MathEvaluator mathEvaluator3 = new MathEvaluator();
            Assert.AreEqual(null, mathEvaluator3.evaluate("1/2/3/ $/4 /5 /6 /7/ 8"));
        }

        [TestMethod]
        public void myFunction()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "fn ZeroValue input => input = 0", null);
            check(ref interpret, "fn sum x y => x + y", null);
            check(ref interpret, "A = 5", 5);
            check(ref interpret, "ZeroValue A + 5", 5);
            check(ref interpret, "sum 1 ZeroValue 0", 1);
        }

        [TestMethod]
        public void assigment()
        {
            MathEvaluator interpret = new MathEvaluator();

            check(ref interpret, "ZZ123    =   1", 1);
            check(ref interpret, "ZZ_123    =   1", 1);
            check(ref interpret, "123ZZ_    =   1", null);
            check(ref interpret, "ZZ    =   1", 1);
            check(ref interpret, "ZZ   ", 1);
            check(ref interpret, "x = 13 + (y = 3)", 16);
            check(ref interpret, "x = 13 + (y = 3)", 16);
            check(ref interpret, "A = B = 7", 7);
            check(ref interpret, "A", 7);
            check(ref interpret, "B", 7);
        }
        [TestMethod]
        public void OverWritingFunction()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "a = 0", 0);
            check(ref interpret, "fn inc x => x + 1", null);
            check(ref interpret, "a = inc a", 1);
            check(ref interpret, "fn inc x => x + 2", null);
            check(ref interpret, "a = inc a", 3);
        }

        [TestMethod]
        public void FloatValue()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "1.4 + 1.6", 3);
            check(ref interpret, "3 /2", 1.5);
            check(ref interpret, "1.125 - 0.125", 1);
        }

        //codewars tests
        private static void check(ref MathEvaluator interpret, string inp, double? res)
        {
            double? result = -9999.99;
            try { result = interpret.evaluate(inp); } catch (Exception) { result = null; }
            if (result != res) 
                Assert.Fail("input(\"" + inp + "\") == <" + res + "> and not <" + result + "> => wrong solution, aborted!"); 
            else 
                Console.WriteLine("input(\"" + inp + "\") == <" + res + "> was ok");
        }

        [TestMethod]
        public void BasicArithmeticTests()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "1 + 1", 2);
            check(ref interpret, "2 - 1", 1);
            check(ref interpret, "2 * 3", 6);
            check(ref interpret, "8 / 4", 2);
            check(ref interpret, "7 % 4", 3);
        }

        [TestMethod]
        public void VariablesTests()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "x = 1", 1);
            check(ref interpret, "x", 1);
            check(ref interpret, "x + 3", 4);
            check(ref interpret, "y", null);
        }

        [TestMethod]
        public void FunctionsTests()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "fn avg x y => (x + y) / 2", null);
            check(ref interpret, "avg 4 2", 3);
            check(ref interpret, "avg 7", null);
            check(ref interpret, "avg 7 2 4", null);
        }

        [TestMethod]
        public void ConflictsTests()
        {
            MathEvaluator interpret = new MathEvaluator();
            check(ref interpret, "fn x x => 0", null);
            check(ref interpret, "fn avg => 0", null);
            check(ref interpret, "avg = 5", null);
        }


    }
}
