using System;

namespace Test_The_Evaluator_Console_App
{
    /// <summary> 
    /// Author:    Sephora Bateman  
    /// Partner:   none 
    /// Date:      1/13/20
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    ///
    ///  
    /// </summary>
    class EvaluatorTestProgram
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("                                             Testing out a varaiable                                                 ");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to A2 + 5 should be 8");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("A2 + 5", s => 3));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to A7+ (5+5) should be 50");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("A7 + (5 + 5)", s => 40));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and muiltiplication
                Console.WriteLine("The answer to (A3 + 5) * 5 should be 40");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(A3 + 5) * 5", s => 3));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to (A3 + B3) should be 4");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(A3 + B3)", s => 2));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to (c3 + b3) should be 18");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(c3 + b3)", s => 9));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to (c12 + b13) should be 140");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(c12 + b13)", s => 70));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                        Testing out different expressions                                            ");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing one
                Console.WriteLine("The answer to '2' should be 2");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to '5 + 5' should be 10");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("5 + 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing muiltiplcation
                Console.WriteLine("The answer to '5 * 5' should be 25");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("5 * 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing subtraction
                Console.WriteLine("The answer to '25 - 5' should be 20");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("25 - 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing /
                Console.WriteLine("The answer 'to '25 / 5' should be 5");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("25 / 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to '25 + 5 + 3' should be 33");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("25 + 5 + 3", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding
                Console.WriteLine("The answer to '25+(5+5)' should be 35");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("25 + (5 + 5)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to '(23 + 5) * 5' should be 140");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(23 + 5) * 5", null));
                // testing adding and *
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("The answer to  '3 + (23 + 5) * 5' should be 143");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("3 + (23 + 5) * 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to  '3 + (23 + 25) * 5' should be 243");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("3 + (23 + 25) * 5", null));

                // testing adding and * 
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("The answer to  '3 + ((23 + 5) + 8) * 5' should be 183");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("3 + ((23 + 5) + 8) * 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *

                Console.WriteLine("The answer to  '(23 + 5) + (8 * 5)' should be 68");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(23 + 5) + (8 * 5)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *

                Console.WriteLine("The answer to  '(23 + 5) * (8 * 5)' should be 1,120");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(23 + 5) * (8 * 5)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing /

                Console.WriteLine("The answer to  '0 / 3'  should be an 0");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("0/3", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to  '2*6+3'  should be an 15");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2*6+3", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to  '4+6*3'  should be an 22");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("4+6*3", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to  '2*(3+17)'  should be an 40");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2*(3+17)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding

                Console.WriteLine("The answer to  '2+(3+15)'  should be an 20");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2+(3+15)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing adding and *
                Console.WriteLine("The answer to  '2+(3+9*9)'  should be an 86");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2+(3+9*9)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing one
                Console.WriteLine("The answer to '2' should be 2");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing longer one
                Console.WriteLine("The answer to  '4 * 4 + (2 * 5 + 8) - 9'   should be an 25");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("4 * 4 + (2 * 5 + 8) - 9", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("                                                  EXCPETION TESTING                                                  ");
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to  'ACDF + 2'   should be an 4");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("ACDF + 2", s => 2));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to  'xx + 2'   should be an 4");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("xx + 2", s => 2));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to  '2^2'   should be an 4");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("2 ^ 2", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to (A3 + 5) * 5 should be an Error");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(A3 + 5) * 5", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to  '(23 + 5) / (8 - 8)' should be an Error");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(23 + 5) / (8 - 8)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                Console.WriteLine("The answer to  '(23 + 5) / (8)' should be an Error");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("(23 + 5) / (8)", null));
                Console.WriteLine("---------------------------------------------------------------------------------------------------------------------");
                // testing excpetion
                Console.WriteLine("The answer to  '3 /0'  should be an Error");
                Console.WriteLine("The result given back was " + FormulaEvaluator.Evaluator.Evaluate("3/0", null));
            }
            catch (Exception excpetion)
            {
                Console.WriteLine(excpetion.Message);
            }
        }
    }
}
