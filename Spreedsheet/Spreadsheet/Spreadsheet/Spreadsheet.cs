/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   none 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// </summary>
using System;
using System.Collections.Generic;
using SpreadsheetUtilities;
using System.Text.RegularExpressions;
using System.Xml;

namespace SS
{
    // testing to see if this will go to the other branch 
    public class Spreadsheet : AbstractSpreadsheet
    {

        /**
      * variables for this assigment
      */
        Dictionary<string, Cell> spreadsheetCells;
        DependencyGraph cellsDependencyGraph;
        public string fileName;
        public override bool Changed { get ; protected set; }

        /// <summary>
        ///  Empty contructor 
        /// </summary>
        public Spreadsheet() : base(s => true, s => s, "default")
        {
            spreadsheetCells = new Dictionary<string, Cell>();
            cellsDependencyGraph = new DependencyGraph();
        }
        /// <summary>
        ///  This is contructor number two.
        /// </summary>
        /// <param name="isValid"> A function that says if the name if vaild of not</param>
        /// <param name="normalize">A function that chnages the name to something else</param>
        /// <param name="version">The version that we are saving</param>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            spreadsheetCells = new Dictionary<string, Cell>();
            cellsDependencyGraph = new DependencyGraph();
        }
        /// <summary>
        /// This is the thrid contructor , new fact string fileName.
        /// </summary>
        /// <param name="fileName">The name of the file we are saving it to</param>
        /// <param name="isValid">A function that says if the name if vaild of not</param>
        /// <param name="normalize">A function that chnages the name to something else</param>
        /// <param name="version">The version that we are saving</param>
        public Spreadsheet(string fileName, Func<string, bool> isValid, Func<string, string> normalize, string version) : base(isValid, normalize, version)
        {
            spreadsheetCells = new Dictionary<string, Cell>();
            cellsDependencyGraph = new DependencyGraph();
            this.fileName = fileName;
            bool spreadsheetWasHit = false;
            string message = "";
            string cell_Name = "";
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;

                if (GetSavedVersion(fileName) != Version)
                {
                    message = "Incorrect version";
                    throw new SpreadsheetReadWriteException(message);
                }
                using (XmlReader reader = XmlReader.Create(fileName, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    spreadsheetWasHit = true;
                                    break;
                                case "cell":
                                    break;
                                case "name":
                                        reader.Read();
                                        cell_Name = reader.Value;
                                    break;
                                case "contents":
                                    reader.Read();
                                    string cell_Content = reader.Value;
                                    SetContentsOfCell(cell_Name, cell_Content);
                                    break;
                            }
                        }
                    }
                }
                if(spreadsheetWasHit == false)
                {
                    message = "Spreadsheet is not the name";
                    throw new SpreadsheetReadWriteException(message);
                }
                Changed = false;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException(message);
            }
        }
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        /// <param name="name">name of the cell</param>
        /// <returns></returns>
        public override object GetCellContents(string name)
        {
            if (ReferenceEquals(name, null) || !IsVaild(Normalize(name)))
                throw new InvalidNameException();
            if (spreadsheetCells.TryGetValue(Normalize(name), out Cell cell))
                return cell.GetCellContent();
            else
                return "";
        }
        /// <summary>
        ///  If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </summary>
        /// <param name="name">The name of the cell in the spreadSheet</param>
        /// <returns></returns>
        public override object GetCellValue(string name)
        {
            if (spreadsheetCells.TryGetValue(Normalize(name), out Cell cell)) // if name is in dictionary get the cell 
            {
                return cell.GetCellValue();
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Lookup used for the cells 
        /// </summary>
        /// <param name="name">The name of the cell in the spreadSheet</param>
        /// <returns></returns>
        private double FormulaLookup(string name)
        {
            if (spreadsheetCells.TryGetValue(Normalize(name), out Cell cell)) // if name is in dictionary get the cell 
            {
                if (Double.TryParse(cell.GetCellValue().ToString(), out double cellDouble))
                {
                    return cellDouble;
                }
            }
            throw new ArgumentNullException();
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            IList<string> Cells = new List<string>();
            foreach (KeyValuePair<string, Cell> spreadSheetCellToken in spreadsheetCells)
            {
                Cells.Add(spreadSheetCellToken.Key);
            }
            return Cells;
        }
        /// <summary>
        ///Returns the version information of the spreadsheet saved in the named file.
        /// If there are any problems opening, reading, or closing the file, the method
        /// should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename">The name of the file we are svaing to the computer</param>
        /// <returns></returns>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreWhitespace = true;
                using (XmlReader reader = XmlReader.Create(filename, settings))
                    while (reader.Read())
                        if (reader.IsStartElement())
                            if (reader.Name == "spreadsheet")
                                return reader["version"];
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Something went wrong opening or Reading the file");
            }
            return "";
        }
        /// <summary>
        ///  Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// <param name="filename"> The name of the file </param>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);
                    foreach (KeyValuePair<string, Cell> spreadSheetCellToken in spreadsheetCells)
                    {
                        writer.WriteStartElement("cell");
                        if (spreadSheetCellToken.Value.GetCellContent() is Formula)
                        {
                            writer.WriteElementString("name", spreadSheetCellToken.Key.ToString());
                            writer.WriteElementString("contents", "=" + spreadSheetCellToken.Value.GetCellContent().ToString());
                        }
                        else
                        {
                            writer.WriteElementString("name", spreadSheetCellToken.Key.ToString());
                            writer.WriteElementString("contents", spreadSheetCellToken.Value.GetCellContent().ToString());
                        }
                        writer.WriteEndElement();
                    }
                    writer.WriteEndElement(); // closes spreadsheet
                    writer.WriteEndDocument(); // ends the whole document
                }
            }
            catch (Exception)
            {
                throw  new SpreadsheetReadWriteException("Something went wrong writing the file");
            }
        }
        /// <summary>
        ///         ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="ArgumentNullException"> 
        ///   If the content parameter is null, throw an ArgumentNullException.
        /// </exception>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        ///
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="content">The content we want to put unti the cell</param>
        /// <returns></returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            
            if (ReferenceEquals(name, null) || ReferenceEquals(content, null))
            {
             
                if (ReferenceEquals(content, null))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    throw new InvalidNameException();
                }
            }
            name = Normalize(name);
            if (!IsVaild(name) || !IsValid(name))
            {
                throw new InvalidNameException();
            }
            else
            {
                content = content.Trim();
                if (Double.TryParse(content, out double returnDouble))
                {
                    SetContentsOfCell(name, returnDouble);
                }
                else if (ReferenceEquals(content, ""))
                { }
                else if (content[0].ToString() != "=")
                {
                    SetCellContents(name, content);
                }
                else
                {
                    string new_Content = RemoveEqualFromInputString(content);
                    try
                    {
                        Formula cellsFormula = new Formula(new_Content);
                        SetCellContents(name, cellsFormula);
                    }
                    catch (CircularException)
                    {
                        throw new CircularException();
                    }
                    catch (FormulaFormatException)
                    {
                        throw new FormulaFormatException("Something was wrong with your formula");
                    }
                }
            }
            Changed = true;
            return GetCellRecalculateList(name);
        }
        /// <summary>
        /// Takes the = sign out of the content
        /// </summary>
        /// <param name="content"> The content we are given to put in the cell </param>
        /// <returns></returns>
        private string RemoveEqualFromInputString(string content)
        {
            string finalStringContent = "";
            for (int pointInString = 0; pointInString < content.Length; pointInString++)
            {
                if (content[pointInString] != '=')
                {
                    finalStringContent += content[pointInString];
                }
            }
            return finalStringContent;
        }

        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell. 
        /// </summary>
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// <param name="name"> The name of the cell </param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            return cellsDependencyGraph.GetDependents(name);
        }


        /// <summary>
        /// The contents of the named cell becomes text.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="text"> text content</param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, string text)
        {
            OldContentWasAFormulaNoAddToDic(name, text, new List<string>());
            return GetCellRecalculateList(name);
        }
        /// <summary>
        /// The contents of the named cell becomes number.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="number"> the number to put in the cell </param>
        /// <returns></returns>
        protected override IList<string> SetContentsOfCell(string name, double number)
        {
            OldContentWasAFormulaNoAddToDic(name, number, new List<string>());
            return GetCellRecalculateList(name);
        }
        /// <summary>
        /// If the formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException, and no change is made to the spreadsheet.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// list consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// list {A1, B1, C1} is returned.
        /// </summary>
        /// <param name="name">name of cell </param>
        /// <param name="formula"> putting in a formula </param>
        /// <returns></returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            if (spreadsheetCells.TryGetValue(name, out Cell cell))
            {
                object oldCellContent = cell.GetCellContent();
                SetContentAndValueForAFormula(formula, name);
                try
                {
                    GetCellsToRecalculate(name);
                }
                catch (CircularException) // if an circular Exception was found, we need to change the cells content back to what it was, then throw the excpetion.
                {
                    if (Double.TryParse(oldCellContent.ToString(), out double returnDouble))
                    {
                        SetContentsOfCell(name, returnDouble);
                    }
                    else if (oldCellContent is Formula)
                    {
                        SetCellContents(name, (Formula)oldCellContent);
                    }
                    else if (oldCellContent is String)
                    {
                        SetCellContents(name, oldCellContent.ToString());
                    }
                    throw new CircularException();
                }

            }
            else
            {
                spreadsheetCells.Add(name, new Cell(formula));
                SetContentAndValueForAFormula(formula, name);
            }
            return GetCellRecalculateList(name);
        }
        /// <summary>
        /// CHANGE NAME
        /// </summary>
        /// <param name="formula"> The formula we are putting into the content</param>
        /// <param name="name">The name of the cell</param>
        private void SetContentAndValueForAFormula(Formula formula, string name)
        {

            cellsDependencyGraph.ReplaceDependees(name, formula.GetVariables());
            if (spreadsheetCells.TryGetValue(name, out Cell cell2))
            {
                cell2.ChangeContentOfCell(formula);
                cell2.ChangeCellValue(formula.Evaluate(FormulaLookup));
            }
        }

        /// <summary>
        /// Returns the list gotten from get cells to recalculate.
        /// </summary>
        /// <param name="name"> The name of the cell </param>
        /// <returns></returns>
        private IList<string> GetCellRecalculateList(string name)
        {
            IList<string> cellThatNeedToRecalculate = new List<string>();
            foreach (string cellName in GetCellsToRecalculate(name))
            {
                cellThatNeedToRecalculate.Add(cellName);
            }
            foreach (string cellName in cellThatNeedToRecalculate)
            {
                if (spreadsheetCells.TryGetValue(cellName, out Cell cell))
                {
                    if (cell.GetCellContent() is Formula)
                    {
                        Formula cellformula = new Formula(cell.GetCellContent().ToString());
                        cell.ChangeCellValue(cellformula.Evaluate(FormulaLookup));
                    }
                    else
                    {
                        cell.ChangeCellValue(cell.GetCellContent());
                    }
                }
            }
            return cellThatNeedToRecalculate;
        }

        /// <summary>
        /// Checks to see if the cell is in the dictionary,
        /// then checks to see if the old Content was a formula,
        /// if it is it gets rid of the dependency tied to that 
        /// formula and puts the new content in. 
        /// If dictionary did not have name it adds the name and the 
        /// cell content into the dictionary.
        /// </summary>
        /// <param name="cellName">name of the cell</param>
        /// <param name="cellContent">content of the cell</param>
        /// <param name="replaceWith">what we replace the Dependency with</param>
        private void OldContentWasAFormulaNoAddToDic(string cellName, object cellContent, List<string> replaceWith)
        {
            if (spreadsheetCells.TryGetValue(cellName, out Cell cell)) // if name is in dictionary get the cell 
            {
                if (this.GetCellContents(cellName) is Formula)
                {
                    cellsDependencyGraph.ReplaceDependees(cellName, replaceWith);
                }
                cell.ChangeContentOfCell(cellContent);
                cell.ChangeCellValue(cellContent);
            }
            else
            {
                spreadsheetCells.Add(cellName, new Cell(cellContent)); // add to dictionary
                if (spreadsheetCells.TryGetValue(cellName, out Cell cell2))
                {
                    cell2.ChangeCellValue(cellContent);
                }
            }
        }
        /// <summary>
        /// The method check to see if the given name is a vaild name 
        /// </summary>
        /// <param name="name">The name of the cell </param>
        /// <returns></returns>
        private bool IsVaild(string name)
        {
            return Regex.IsMatch(name, "^[_a-zA-Z][a-zA-Z0-9]+");
        }
        /// <summary>
        ///  This class keeps track of the cells that our in the spreadsheet
        /// </summary>
        private class Cell
        {
            /// <summary>
            ///  object that stores what the cells content is. 
            /// </summary>
            object cellContent;
            object cellValue;
            /// <summary>
            /// Consturctor for a cell. 
            /// </summary>
            /// <param name="Content"> The content the want in the cell</param>
            public Cell(object Content)
            {
                cellContent = Content;
            }
            /// <summary>
            /// Changes the content of the cell 
            /// </summary>
            /// <param name="newcontent">the new content wanted in the cell</param>
            public void ChangeContentOfCell(object newcontent)
            {
                cellContent = newcontent;
            }
            /// <summary>
            /// Gets the contents of the cell 
            /// </summary>
            /// <returns></returns>
            public object GetCellContent()
            {
                return cellContent;
            }
            /// <summary>
            /// Gets the Value of the cell 
            /// </summary>
            /// <returns></returns>
            public object GetCellValue()
            {
                return cellValue;
            }
            /// <summary>
            /// sets the Value of the cell 
            /// </summary>
            /// <returns></returns>
            public void ChangeCellValue(object newValue)
            {
                cellValue = newValue;
            }
        }


    }
}
