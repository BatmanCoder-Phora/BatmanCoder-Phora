/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   none 
/// Date:      2/10/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source. All references used in the completion of the assignment are cited in my README file. 
/// </summary>
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using SS;
using System;
using System.Collections.Generic;
using System.Xml;

namespace SpreadsheetTests
{
    [TestClass]
    public class SpreadsheetTests
    {
        // Start of excpetion Testing
        /// <summary>
        /// Testing to see if name is null and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNullGetCellContent()
        {
            AbstractSpreadsheet sheet = new Spreadsheet( s=> true, s => s.ToUpper(), "version");
            sheet.GetCellContents(null);
        }
        /// <summary>
        /// Testing to see if name is null and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNull()
        {

            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "2");
        }
        /// <summary>
        /// Testing to see if text is null and throws ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestTextNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");

            sheet.SetContentsOfCell("A3", (string)null);
        }
        /// <summary>
        /// Testing to see if Formula is null and throws ArgumentNullException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestFormulaNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A3", (string)null);
        }
        /// <summary>
        /// Testing to see if name is null and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNullTwo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell(null, "=3+2");
        }
        /// <summary>
        /// Testing to see if name is not vaild and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TesNameNotVaildGetCellContent()
        {

            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.GetCellContents(null);
        }
        /// <summary>
        /// Testing to see if name is not vaild and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNotVaild()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("93a", "2.3");
        }
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNotVaildTwo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => (s == "A3") ? false : true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A4", "2.3");
            sheet.SetContentsOfCell("A3", "2.3");
        }
        /// <summary>
        /// Testing to see if name is not vaild and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNotVaildThree()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("93a", " ");
        }
        /// <summary>
        /// Testing to see if name is not vaild and throws InvalidNameException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameNotVaildSFour()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("93a", "=3+2");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetFormulaCircularException()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A1", "=C1+A2");
            Sheet.SetContentsOfCell("A2", "=V4+V5");
            Sheet.SetContentsOfCell("A1", "=A1+A1");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetFormulaCircularExceptionDoulbe()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A1", "2");
            Sheet.SetContentsOfCell("A2", "=V4+V5");
            Sheet.SetContentsOfCell("A1", "=A1+A1");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetFormulaCircularExceptonText()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet();
            Sheet.SetContentsOfCell("A1", "text");
            Sheet.SetContentsOfCell("A2", "=V4+V5");
            Sheet.SetContentsOfCell("A1", "=A1+A1");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetFormulaIncorrectFormula()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet();
            Sheet.SetContentsOfCell("A1", "text");
            Sheet.SetContentsOfCell("A2", "=2V");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetFormulaIncorrectFormulaTwo()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A2", "=2V + 2 - ");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetFormulaIncorrectFormulaThree()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A2", "=(2V + 2))");
        }
        /// <summary>
        /// Testing to see if  throws CircularException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestSetFormulaIncorrectFormulaFour()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A1", "text");
            Sheet.SetContentsOfCell("A2", "=((2V + 2)");
        }
        /// <summary>
        /// Test to see if we can fins the same version. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestNotVersion()
        {
            AbstractSpreadsheet sheet = new Spreadsheet("fileName3", s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A3", "=3+2");
            sheet.Save("fileName3");
            AbstractSpreadsheet sheet2 = new Spreadsheet("fileName", s => true, s => s.ToUpper(), "deafault");

        }
        /// <summary>
        /// Tests to see if we can open a file that is not therer 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestCanNOtOpenFindFile()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "default");
            sheet.SetContentsOfCell("A3", "=3+2");
            sheet.Save("fileName3");
            AbstractSpreadsheet sheet2 = new Spreadsheet("fileName", s => true, s => s.ToUpper(), "");

        }
        /// <summary>
        ///  Test to see if it is not a vaild formula 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestReadXmlNotVaildFormula()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "=((2-3)");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "");
        }
        /// <summary>
        /// Test that it is not a vaild name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestReadXmlNotVaildName()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteElementString("name", "9B");
                writer.WriteStartElement("cell");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "");
        }
        /// <summary>
        /// Tests to see if any change we have to the spreadsheet
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestReadXmlNotspreadsheetIsNotName()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet23");
                writer.WriteAttributeString("version", "");

                writer.WriteElementString("name", "A3");
                writer.WriteStartElement("cell");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "");
        }
        /// <summary>
        /// sees if anything goes wrong with save. 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void TestReadXmlNotspreadsheetSaveNull()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save((string)null);
        }
        // End of excpetion Testing
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZeroFormulaError()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A5", "=4/0");
            Assert.IsInstanceOfType(Sheet.GetCellValue("A5"), typeof(FormulaError));
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestDivideByZeroFormulaErrorTwo()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("A2", "=4");
            Sheet.SetContentsOfCell("A3", "= 3-3");
            Sheet.SetContentsOfCell("A5", "=A2/A3");
            Assert.IsInstanceOfType(Sheet.GetCellValue("A5") , typeof(FormulaError));
        }
        /// <summary>
        /// test for an excpetion
        /// </summary>
        [TestMethod(), Timeout(5000)]
        public void TestNotVaildVariableFormulaError()
        {
            AbstractSpreadsheet Sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Sheet.SetContentsOfCell("X7", "Hello");
            Sheet.SetContentsOfCell("A5", "= X7 - 0");
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestsetNumber()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A3", "3");
            Assert.AreEqual((double)3, sheet.GetCellContents("A3"));
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestsetText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            Assert.AreEqual("Hello", sheet.GetCellContents("A5"));
        }
        /// <summary>
        /// Test Value for text content
        /// </summary>
        [TestMethod]
        public void TestGetValueText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            Assert.AreEqual("Hello", sheet.GetCellValue("A5"));
        }
        /// <summary>
        /// Test Value for an cell that doesn't exsists
        /// </summary>
        [TestMethod]
        public void TestGetValueEmptyCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Assert.AreEqual("", sheet.GetCellValue("A5"));
        }
        /// <summary>
        /// Test Value for Formula content
        /// </summary>
        [TestMethod]
        public void TestGetValueFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "=2+1");
            Assert.AreEqual((double) 3, sheet.GetCellValue("A5"));
        }
        /// <summary>
        /// Test Value for Formula using lookup
        /// </summary>
        [TestMethod]
        public void TestGetValueFormulaUsingLookUP()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "2");
            sheet.SetContentsOfCell("A4", "=A5*2");
            Assert.AreEqual((double)4, sheet.GetCellValue("A4"));
        }
        /// <summary>
        /// Test Value for Formula using lookup
        /// </summary>
        [TestMethod]
        public void TestGetValueFormulaUsingLookUPTwo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "=2+4");
            sheet.SetContentsOfCell("A4", "=A5*2");
            Assert.AreEqual((double)12, sheet.GetCellValue("A4"));
        }
        /// <summary>
        /// Test Value for double content
        /// </summary>
        [TestMethod]
        public void TestGetValueDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "3");
            Assert.AreEqual((double)3,sheet.GetCellValue("A5"));
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestsetFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2"); 
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestSetFormulaToDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2");
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
            sheet.SetContentsOfCell("A5", "2.0");
            Assert.AreEqual((double)2.0, sheet.GetCellContents("A5"));
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestSetFormulaToText()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2");
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
            sheet.SetContentsOfCell("A5", "change");
            Assert.AreEqual("change", sheet.GetCellContents("A5"));
        }
        /// <summary>
        /// Testing getting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestGetCells()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2");
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
            sheet.SetContentsOfCell("A4", "=B3-A5");
            sheet.SetContentsOfCell("A5", "=A3-A6");
        }
        /// <summary>
        /// Testing setting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestSetFormulaTonewFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2");
            Formula f2 = new Formula("3-22");
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
            sheet.SetContentsOfCell("A5", "=3-22");
            Assert.AreEqual(f2, sheet.GetCellContents("A5"));
        }
        /// <summary>
        /// Testing getting the contents of a cell 
        /// </summary>
        [TestMethod]
        public void TestGetContentOfCellElse()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Assert.AreEqual("" , sheet.GetCellContents("A2"));
        }
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        [TestMethod]
        public void TestGetContent()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f2 = new Formula("A1*2");
            Formula f3 = new Formula("B1+A1");
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("A1", "3");
            IList<string> testList = new List<string>() { "A1","B1", "C1" };
            IList<string> testList2 = sheet.SetContentsOfCell("A1", "3");
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        [TestMethod]
        public void TestGetContentTwo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f2 = new Formula("A1*2");
            Formula f3 = new Formula("B1+A1");
            sheet.SetContentsOfCell("B1", "=A1*2");
            IList<string> testList = new List<string>() { "A1", "B1"};
            IList<string> testList2 = sheet.SetContentsOfCell("A1", "3");
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// <summary>
        ///  This test makes sure that when we replace the dpendees, they are no longer there. 
        /// </summary>
        [TestMethod]
        public void TestSetContentDoulbeEmptyList()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("C1", "2");
            IList<string> testList = new List<string>() { "A1", "B1",};
            IList<string> testList2 = sheet.SetContentsOfCell("A1", "3");
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// <summary>
        /// This test makes sure that when we replace the dpendees, they are no longer there. 
        /// </summary>
        [TestMethod]
        public void TestSetContentDoulbeandTextEmptyListTwo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("C1", "2");
            sheet.SetContentsOfCell("B1", "text");
            IList<string> testList = new List<string>() { "A1" };
            IList<string> testList2 = sheet.SetContentsOfCell("A1", "3");
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// <summary>
        /// Testing Get Depdenees
        /// </summary>
        [TestMethod]
        public void TestGetAllNoNEMptyCells()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("D1", "=B1 - C1");
            IList<string> testList = new List<string>() { "A1", "B1", "C1", "D1" };
            IList<string> testList2 = (List<string>)sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// <summary>
        /// Testing Get All NoN-EmptyCells
        /// </summary>
        [TestMethod]
        public void TestGetAllNoNEmptyCellsWithAnEmptyCell()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("D1", "=B1 - C1");
            IList<string> testList = new List<string>() { "A1", "B1", "D1", "E1" };
            IList<string> testList2 = (List<string>)sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(testList.Count, testList2.Count);
        }
        /// <summary>
        /// See if we can recalculate 
        /// </summary>
        [TestMethod]
        public void TestRecalculate()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            Formula f = new Formula("3+2");
            sheet.SetContentsOfCell("A5", "=3+2");
            Assert.AreEqual(f, sheet.GetCellContents("A5"));
            Assert.AreEqual((double)5, sheet.GetCellValue("A5"));
            sheet.SetContentsOfCell("B5", "=A5+2");
            Assert.AreEqual((double)7, sheet.GetCellValue("B5"));
            sheet.SetContentsOfCell("A5", "=1+2");
            Assert.AreEqual((double)5, sheet.GetCellValue("B5"));
        }
        /// <summary>
        /// See if we can recalculate 
        /// </summary>
        [TestMethod]
        public void TestRecalculateTwoTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "4");
            sheet.SetContentsOfCell("B5", "=A5+2");
            sheet.SetContentsOfCell("C5", "=A5*2");
            sheet.SetContentsOfCell("D5", "=A5-2");
            Assert.AreEqual((double)6, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)8, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)2, sheet.GetCellValue("D5"));
            sheet.SetContentsOfCell("A5", "5");
            Assert.AreEqual((double)7, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)10, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)3, sheet.GetCellValue("D5"));
        }
        /// <summary>
        /// See if we can recalculate 
        /// </summary>
        [TestMethod]
        public void TestRecalculateThree()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "4");
            sheet.SetContentsOfCell("B5", "=A5+2");
            sheet.SetContentsOfCell("C5", "=A5*2");
            sheet.SetContentsOfCell("D5", "=A5-2");
            sheet.SetContentsOfCell("E5", "=A5-D5");
            Assert.AreEqual((double)6, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)8, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)2, sheet.GetCellValue("D5"));
            Assert.AreEqual((double)2, sheet.GetCellValue("E5"));
            sheet.SetContentsOfCell("A5", "5");
            Assert.AreEqual((double)7, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)10, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)3, sheet.GetCellValue("D5"));
            sheet.SetContentsOfCell("D5", "5");
            Assert.AreEqual((double)0, sheet.GetCellValue("E5"));
        }
        /// <summary>
        /// See if we can recalculate 
        /// </summary>
        [TestMethod]
        public void TestRecalculateFour()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "4");
            sheet.SetContentsOfCell("B5", "=A5+2");
            sheet.SetContentsOfCell("C5", "=B5*2");
            sheet.SetContentsOfCell("D5", "=A5-C5");
            sheet.SetContentsOfCell("E5", "=10-D5");
            Assert.AreEqual((double)6, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)12, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)-8, sheet.GetCellValue("D5"));
            Assert.AreEqual((double)18, sheet.GetCellValue("E5"));
            sheet.SetContentsOfCell("C5", "1");
            Assert.AreEqual((double)6, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)1, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)3, sheet.GetCellValue("D5"));
            Assert.AreEqual((double)7, sheet.GetCellValue("E5"));
            sheet.SetContentsOfCell("A5", "10");
            Assert.AreEqual((double)12, sheet.GetCellValue("B5"));
            Assert.AreEqual((double)1, sheet.GetCellValue("C5"));
            Assert.AreEqual((double)9, sheet.GetCellValue("D5"));
            Assert.AreEqual((double)1, sheet.GetCellValue("E5"));
        }
        /// <summary>
        /// huge method for setting 
        /// </summary>
        [TestMethod]
        public void stressTestSetContents()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version"); ;
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.SetContentsOfCell("F1", " Hello ");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.SetContentsOfCell("F1", "text");
            Assert.AreEqual("text", sheet.GetCellContents("F1"));
            sheet.SetContentsOfCell("G1", "33");
            sheet.SetContentsOfCell("J2", "text");
            sheet.SetContentsOfCell("_1", "4");
            IList<string> testerLsit = (List<string>)sheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(8, testerLsit.Count);
        }
        /// <summary>
        ///  test writing the information to an xml writer.
        /// </summary>
        [TestMethod]
        public void TestSave()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save("filenameTest1");
        }
        /// <summary>
        ///  STRESS TEST
        /// </summary>
        [TestMethod]
        public void TestStressTest()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.SetContentsOfCell("F1", " Hello ");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.SetContentsOfCell("F1", "text");
            Assert.AreEqual("text", sheet.GetCellContents("F1"));
            sheet.SetContentsOfCell("G1", "33");
            sheet.SetContentsOfCell("J2", "text");
            sheet.SetContentsOfCell("_1", "4");
            sheet.Save("filenameTester");
            AbstractSpreadsheet sheet2 = new Spreadsheet("filenameTester", s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("E1", "5");
            sheet.SetContentsOfCell("H1", "3");
            sheet.SetContentsOfCell("E1", "5");
            sheet.SetContentsOfCell("P1", "6");
            sheet.Save("filenameTester2");
        }
        /// <summary>
        /// try get saved version then add on to it 
        /// </summary>
        [TestMethod]
        public void TestgetsavedversionTheninputingMoreInfo()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save("filenameTest2");
            AbstractSpreadsheet sheet2 = new Spreadsheet("filenameTest2", s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("E1", "5");
            sheet.SetContentsOfCell("H1", "3");
            sheet.Save("filenameTest3");
        }
        /// <summary>
        /// Test to see if a file has been changed since it was opened.
        /// </summary>
        [TestMethod]
        public void TestChangedFalse()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save("filenameTest4");
            AbstractSpreadsheet sheet2 = new Spreadsheet("filenameTest4", s => true, s => s.ToUpper(), "version");
            Assert.AreEqual(false, sheet2.Changed);
        }
        /// <summary>
        /// Test to see if a file has been changed since it was opened.
        /// </summary>
        [TestMethod]
        public void TestChangedTrue()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save("filenameTest5");
            AbstractSpreadsheet sheet2 = new Spreadsheet("filenameTest5", s => true, s => s.ToUpper(), "version");
            sheet.SetContentsOfCell("H1", "=6");
            Assert.AreEqual(false, sheet2.Changed);
        }
        /// <summary>
        ///  Checks to make sure we can find it if it was made form an empty constuctor
        /// </summary>
        [TestMethod]
        public void TestEmptyContructorSaveGetSavedVersion()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A5", "Hello");
            sheet.SetContentsOfCell("A1", "3");
            sheet.SetContentsOfCell("B1", "=A1*A1");
            sheet.SetContentsOfCell("C1", "8");
            sheet.SetContentsOfCell("D1", "=B1+A1");
            sheet.SetContentsOfCell("F1", "=B1 - C1");
            sheet.Save("filenameTest6");
            AbstractSpreadsheet sheet2 = new Spreadsheet("filenameTest6", s => true, s => s.ToUpper(), "default");
            sheet.SetContentsOfCell("H1", "=6");
            sheet.Save("filenameTest6");
        }
        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void TestGivenToTestWritingInTestClass()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "");
        }
        /// <summary>
        /// Tests the writer in  the test class
        /// </summary>
        [TestMethod]
        public void TestWriterInTestClasAndReader()
        {
            using (XmlWriter writer = XmlWriter.Create("save.txt")) // NOTICE the file with no path
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");
                writer.WriteAttributeString("version", "");

                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "A1");
                writer.WriteElementString("contents", "hello");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
            AbstractSpreadsheet ss = new Spreadsheet("save.txt", s => true, s => s, "");
        }

    }
     
}
