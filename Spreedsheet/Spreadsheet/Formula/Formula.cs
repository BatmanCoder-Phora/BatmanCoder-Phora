// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens
/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   none 
/// Date:      1/24/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
    /// and "x 23" consists of a variable "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {
        /**
         * creating needed variables
         */
        private IEnumerable<string> formula;
        Func<string, string> normalize;
        Func<string, bool> isValid;
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
            this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            this.normalize = normalize;
            this.isValid = isValid;
            this.formula = verifyThatFormulaIsVaild(formula); // add method that parses it.
        }

        /// <summary>
        /// This parses that formula
        /// </summary>
        /// <param name="formula">string that contains the formula</param>  
        /// <returns>the correct formula or an error </returns> 
        private IEnumerable<string> verifyThatFormulaIsVaild(string formula)
        {
            List<string> finalFormula = new List<string>();
            IEnumerable<string> formulaRunThrough = GetTokens(formula);
            var formulaRun = formulaRunThrough.ToList();
            int numberOfNumbers = 0;
            int numberOfOPerators = 0;
            int opeParCounter = 0;
            int closPsrCounter = 0;
            if (ReferenceEquals(formula, " ") || ReferenceEquals(formula, "") || formulaRun.Count() < 1)
            {
                throw new FormulaFormatException("spmething went wrong");
            }
            for (int i = 0; i < formulaRun.Count(); i++)
            {
                if (isAVaraiable(formulaRun[i]))
                {
                    formulaRun[i] = normalize(formulaRun[i]);
                }
                // Starting Token Rule
                if (i == 0)
                {
                    isNotanyOfThese(formulaRun[i], ")");
                }
                //  Ending Token Rule
                if (i == formulaRun.Count() - 1)
                {

                    isNotanyOfThese(formulaRun[i], "(");
                }
                // Extra Token Rule
                if (Double.TryParse(formulaRun[i], out double result) || isValid(formulaRun[i]) || formulaRun[i] == ")")
                {
                    if (formula.Count() > 1)
                    {
                        if (i != formulaRun.Count() - 1)
                        {
                            if (formulaRun[i + 1] != "(")
                            {

                                if ((IsAnOPerator(formulaRun[i + 1]) == false) && (IsAnOPerator(formulaRun[i]) == false))
                                {
                                    if (formulaRun[i] == "(" || formulaRun[i + 1] == ")")
                                    {
                                        if (!(Double.TryParse(formulaRun[i + 1], out double result2)) && !isValid(formulaRun[i + 1]) && formulaRun[i + 1] != ")")
                                        {
                                            throw new FormulaFormatException("something went wrong");
                                        }
                                    }
                                    else
                                    {
                                        throw new FormulaFormatException("something went wrong");
                                    }
                                }
                                else if ((IsAnOPerator(formulaRun[i + 1]) == true) && (IsAnOPerator(formulaRun[i]) == true))
                                {
                                    throw new FormulaFormatException("something went wrong");
                                }
                            }
                        }
                    }
                }
                if (formulaRun[i] == "(" || IsAnOPerator(formulaRun[i]))
                {
                    if (!(Double.TryParse(formulaRun[i + 1], out double result1)) && !isValid(formulaRun[i + 1]) || formulaRun[i + 1] == ")")
                    {
                        throw new FormulaFormatException("Something went wrong");
                    }
                }
                if (IsAnOPerator(formulaRun[i]))
                {
                    numberOfOPerators++;
                }
                if (Double.TryParse(formulaRun[i], out double result4) || isAVaraiable(formulaRun[i]))
                {
                    numberOfNumbers++;
                }
                if (formulaRun[i] == "(")
                {
                    opeParCounter++;
                }
                if (formulaRun[i] == ")")
                {
                    closPsrCounter++;
                }
                // Right Parentheses Rule
                if (closPsrCounter > opeParCounter)
                {
                    throw new FormulaFormatException("Something went wrong");
                }
            }
            if (opeParCounter != closPsrCounter || numberOfOPerators > numberOfNumbers || closPsrCounter > opeParCounter)
            {
                throw new FormulaFormatException("Something went wrong");
            }
            foreach (string token2 in formulaRun)
            {
                finalFormula.Add(token2);
            }
            return finalFormula;
        }
        /// <summary>
        /// a method to make sure that token doesn't equal any of these 
        /// </summary>
        /// <param name="tokenInFormula">The token we are at</param>
        /// <param name="eitherOpenOrClosedPar">the par that is being compared</param>
        private void isNotanyOfThese(string tokenInFormula, string eitherOpenOrClosedPar)
        {
            if (!(Double.TryParse(tokenInFormula, out double result1)) && !(isValid(tokenInFormula)) && tokenInFormula != eitherOpenOrClosedPar || IsAnOPerator(tokenInFormula))
            {
                throw new FormulaFormatException("Something went wrong");
            }
        }
        /// <summary>
        /// Sees if it is an operator
        /// </summary>
        /// <param name="tokenInFormula">The token we are at</param>
        private bool IsAnOPerator(string tokenInFormula)
        {
            if (tokenInFormula == "*" || tokenInFormula == "/" || tokenInFormula == "+" || tokenInFormula == "-")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            /**
               * variables to keeps track of value and operator stacks and spilt the string. 
               * */
            Stack<double> valueStack = new Stack<double>(); // stack storing the values(4,5,6)
            Stack<char> operatorStack = new Stack<char>(); //stack storing the operators(+,/,-
            int thingsInPar = 0;
            foreach (string currentToken in formula)
            {
                bool isNotAvAriable = false;
                // if t is an int 
                if (Int32.TryParse(currentToken, out int outputnumber))
                {
                    isNotAvAriable = true;
                    if (valueStack.Count >= 2)
                    {
                        if (operatorStack.Peek() == '*' || operatorStack.Peek() == '/')
                        {
                            if (operatorStack.Contains('*') && operatorStack.Peek() == '/')
                            {
                                char operatorThis = operatorStack.Pop();
                                valueStack.Push(computeNewNumber(valueStack, operatorStack));
                                operatorStack.Push(operatorThis);
                            }
                            valueStack.Push(outputnumber);
                            double newAnswer = computeNewNumber(valueStack, operatorStack);
                            valueStack.Push(newAnswer);
                        }
                        else
                        {
                            valueStack.Push(outputnumber);
                        }
                    }
                    else
                    {
                        valueStack.Push(outputnumber);
                    }
                }
                // if t is * or /
                else if (currentToken == "*" || currentToken == "/")
                {
                    isNotAvAriable = true;
                    operatorStack.Push(char.Parse(currentToken));
                }
                // if t is (
                else if (currentToken == "(")
                {
                    isNotAvAriable = true;
                    operatorStack.Push(char.Parse(currentToken));
                }
                // if t is + or -
                else if (currentToken == "+" || currentToken == "-")
                {
                    if (operatorStack.Contains('('))
                    {
                        thingsInPar++;
                    }
                    isNotAvAriable = true;
                    if (operatorStack.Contains('('))
                    {
                        if (operatorStack.Contains('('))
                        {
                            thingsInPar++;
                        }
                        operatorStack.Push(char.Parse(currentToken));
                        continue;
                    }
                    else
                    {
                        calculateTheResult(valueStack, operatorStack);
                    }
                    operatorStack.Push(char.Parse(currentToken));
                }
                // if t is )
                else if (currentToken == ")")
                {
                    isNotAvAriable = true;
                    calculateTheResult(valueStack, operatorStack);
                    operatorStack.Pop();
                }
                else if (isNotAvAriable == false)
                {
                    // if it is a variable
                    if (isAVaraiable(currentToken) == true && isValid(currentToken))
                    {
                        try
                        {
                            valueStack.Push(lookup(normalize(currentToken)));
                        }
                        catch (Exception)
                        {
                            return new FormulaError("Not vaild variable");
                        }
                    }
                }
            }
            if (checkeToSeeDivByZero(valueStack, operatorStack) == true)
            {
                return new FormulaError("divideByZero");
            }
            calculateTheResult(valueStack, operatorStack);
            calculateTheLastOfThem(operatorStack, valueStack);
            calculateTheLastOfThem(operatorStack, valueStack);
            return valueStack.Pop(); // answer 
        }
        /// <summary>
        /// method that helps me with the formulaError or division. 
        /// </summary>
        /// <param name="valueStack">A stack containg the formula numbers</param>
        /// <param name="operatorStack">A stack containg the formula operators</param> 
        private bool checkeToSeeDivByZero(Stack<double> valueStack, Stack<char> operatorStack)
        {
            if (operatorStack.Count() > 0)
            {
                if (operatorStack.Peek() == '/')
                    if (valueStack.Peek() == 0)
                        return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// if there is anything left on the stack it calculates it. 
        /// </summary>
        /// <param name="operatorStack">A stack containg the formula operators</param> 
        /// <param name="valueStack">A stack containg the formula numbers</param> 
        private static void calculateTheLastOfThem(Stack<char> operatorStack, Stack<double> valueStack)
        {
            if (operatorStack.Count > 0 && valueStack.Count >= 2)
            {
                calculateTheResult(valueStack, operatorStack);
            }
        }

        /// <summary>
        /// calculates the result based on the operator stack. 
        /// </summary>
        /// <param name="valueStack">A stack containg the formula numbers</param> a stack of values
        /// <param name="operatorStack">A stack containg the formula operators</param> a stack of operators
        private static void calculateTheResult(Stack<double> valueStack, Stack<char> operatorStack)
        {
            if (valueStack.Count >= 2)
            {
                if (operatorStack.Peek() == '+' || operatorStack.Peek() == '-' || operatorStack.Peek() == '*' || operatorStack.Peek() == '/')
                {
                    valueStack.Push(computeNewNumber(valueStack, operatorStack));
                }
            }
        }

        /// <summary>
        /// Check to see if something is a varaiable returns true or false.
        /// </summary>
        /// <param name="currentToken"> a current token</param> string varaiable 
        /// <returns></returns>
        private static bool isAVaraiable(string currentToken)
        {
            return Regex.IsMatch(currentToken, "^[a-zA-Z][0-9]+");
        }
        /// <summary>
        /// selects which operation by the operatorAns then computes it the correct way 
        /// </summary>
        /// <param name="valueStack">A stack containg the formula numbers</param> 
        /// <param name="operatorStack">A stack containg the formula operators</param>
        /// /// <returns></returns>
        private static double computeNewNumber(Stack<double> valueStack, Stack<char> operatorStack)
        {
            // varaiables pulled from the stacks
            double firstNumber = valueStack.Pop();
            double secondNumber = valueStack.Pop();
            char operatorPop = operatorStack.Pop();
            // decides which operation to do
            if (operatorPop == '+')
            {
                return firstNumber + secondNumber;
            }
            if (operatorPop == '-')
            {
                return secondNumber - firstNumber;
            }
            if (operatorPop == '/')
            {
                return secondNumber / firstNumber;
            }
            else
            {
                return firstNumber * secondNumber;
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<String> variableList = new HashSet<string>();
            foreach (String tokenInFormula in formula)
            {
                String tokenInFormulaTrimmed = tokenInFormula.Trim();
                if (isAVaraiable(tokenInFormulaTrimmed) == true)
                {
                    variableList.Add(normalize(tokenInFormulaTrimmed)); // add that variable to the variable list
                }
            }
            return variableList;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            string formulaToString = "";
            foreach (string tokenInFormula in formula)
            {
                string tokenInFormulaTrimmed = tokenInFormula.Trim();
                if (isAVaraiable(tokenInFormulaTrimmed) == true && isValid(tokenInFormulaTrimmed) == true)
                {
                    normalize(tokenInFormulaTrimmed);
                    formulaToString = formulaToString + tokenInFormula; // if it's a variable normalize it then add it.
                }
                else
                {
                    formulaToString = formulaToString + tokenInFormula;
                }
            }
            return formulaToString;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            if (obj == null || obj == "")
            {
                return false;
            }
            Formula objFormula = (Formula)obj;
            return objFormula.GetHashCode() == this.GetHashCode();
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))
            {
                return false;
            }
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
            {
                return true;
            }

            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            if (ReferenceEquals(f1, null) && ReferenceEquals(f2, null))
            {
                return false;
            }
            if (ReferenceEquals(f1, null) || ReferenceEquals(f2, null))
            {
                return true;
            }
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }
    }

    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}


