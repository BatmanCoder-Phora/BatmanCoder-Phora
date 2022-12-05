using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System;
using System.Collections.Generic;

/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   none 
/// Date:      1/29/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// 
///  
/// </summary>
namespace FormulaTests
{
    [TestClass]
    public class FormulaUnitTests
    {
        /// <summary>
        /// test out a single number 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestSingleNumber()
        {
            Formula f = new Formula("5");
            Assert.AreEqual((double)5, f.Evaluate(s => 0));
        }
        /// <summary>
        ///  tests out complex nested parens 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensRight()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual((double)6, f.Evaluate(s => 1));
        }
        /// <summary>
        ///  tests out complex nested parens
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplexNestedParensLeft()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual((double)12, f.Evaluate(s => 2));
        }
        /// <summary>
        /// test repeated var
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestRepeatedVar()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual((double)0, f.Evaluate(s => 3));
        }
        /// <summary>
        ///  test negative 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNegative()
        {
            Formula f = new Formula("-9");
        }
        /// <summary>
        /// test negative again 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNegativeParens()
        {
            Formula f = new Formula("-(5+10)");
        }
        /// <summary>
        /// test compelx formula 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestComplex1()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestEmptyParens()
        {
            Formula f = new Formula("5+()*3");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestNotVaildExcpetion ()
        {
            Formula f = new Formula("(x7*3)", s=> s.ToLower(), s=> (s == "x7") ? false : true);
        }/// <summary>
         /// test for an excpetion
         /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestExcpetion()
        {
            Formula f = new Formula("(x7*3)", s => s.ToLower(), s => (s == "x7") ? false : true);
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMoreClosedParens()
        {
            Formula f = new Formula("(5+2))*7");
        }
        /// <summary>
         /// test for an excpetion
         /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestdoubleOperators ()
        {
            Formula f = new Formula("5++2");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestMoreOpenParens()
        {
            Formula f = new Formula("((((5+2)*7)");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestemptyFormula()
        {
            Formula f = new Formula("");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestOnlyOperator()
        {
            Formula f = new Formula("+");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestemptyFormula2()
        {
            Formula f = new Formula(" ");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestTwoOperatorNextTooEachOther()
        {
            Formula f = new Formula("3++2");
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestUnknownVariable()
        {
            Formula f = new Formula("4*X1");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZero()
        {
            Formula f = new Formula("4/0");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw  new DivideByZeroException("divideByZero"); }), typeof(FormulaError));
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZero1()
        {
            Formula f = new Formula("(8+8)/(8-8)");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw new DivideByZeroException("divideByZero"); }), typeof(FormulaError));
        }
        /// <summary>
        ///  test noramlizer
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNormalizerGetVars()
        {
            Formula f = new Formula("2+y1", s => s.ToUpper(), s => true);
            HashSet<string> vars = new HashSet<string>(f.GetVariables());

            Assert.IsTrue(vars.SetEquals(new HashSet<string> { "Y1" }));
        }
        /// <summary>
        ///  testing not equal 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNotEquals()
        {
            Formula f = new Formula("3+a1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("2+X1", s => s.ToUpper(), s => true);
            Assert.IsTrue(!f.Equals(f2));
        }
        /// <summary>
        /// test equal 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEquals()
        {
            Formula f = new Formula("3+a1", s => s.ToUpper(), s => true);
            Formula f2 = new Formula("3+A1", s => s.ToUpper(), s => true);
            Assert.IsTrue(f.Equals(f2));
        }
        /// <summary>
        /// test to equals 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEqualsStringAndNull()
        {
            Formula f = new Formula("3+a1", s => s.ToUpper(), s => true);
            Assert.IsFalse(f.Equals(null));
            Assert.IsFalse(f.Equals(""));
        }
        /// <summary>
        /// test equals 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEqualsMethod()
        {
            Formula f = new Formula("3");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f == f2);
        }
        /// <summary>
        /// test null equals 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestEqualsNullMethods()
        {
            Formula f1 = new Formula("3");
            Formula f2 = new Formula("3");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
            Assert.IsFalse(f1 != f2);
        }
        /// <summary>
        /// test null equals 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNotEqualsMethod()
        {
            Formula f = new Formula("3");
            Formula f2 = new Formula("5");
            Assert.IsTrue(f != f2);
            Assert.IsFalse(f == f2);
        }
        /// <summary>
        /// test not equal 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNotEqualsAndEqualsNullMethods()
        {
            Formula f = new Formula("3");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f != null);
            Assert.IsTrue(null != f2);
            Assert.IsFalse(null != null);
            Assert.IsTrue(null == null);
        }
        /// <summary>
        /// test hashcode 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestGetHashCodeMethod()
        {
            Formula f = new Formula("3 + 5");
            Formula f2 = new Formula("3 + 5");
            Assert.IsTrue(f.GetHashCode() == f2.GetHashCode());
        }
        /// <summary>
        /// test fasle hashcode 
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestGetHashCodeMethodFalse()
        {
            Formula f = new Formula("3 + 5");
            Formula f2 = new Formula("3 + (4-6)* 8");
            Assert.IsTrue(f.GetHashCode() != f2.GetHashCode());
        }
        /// <summary>
        /// test strings
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestString()
        {
            Formula f = new Formula("2-5+7");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }
        /// <summary>
        /// test get vars complex
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestVarsComplex()
        {
            Formula f = new Formula("A1+Y2-B3");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "A1", "Y2", "B3" };
            Assert.AreEqual(actual.Count, 3);
            Assert.IsTrue(expected.SetEquals(actual));
        }
    }


}
