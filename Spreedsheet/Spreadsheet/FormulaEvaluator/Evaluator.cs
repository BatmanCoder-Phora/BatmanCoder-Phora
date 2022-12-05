using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaEvaluator
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
    /// This file has two helper methods, that are used in the method Evaluate. Evalaute is the method computes the answer to an inflix expression
    ///  
    /// </summary>
    public class Evaluator
    {
        /// <summary>
        /// This is the delagte for our lookUp which takes in a string and returns an int 
        /// </summary>
        /// <param name="variable_name"></param>
        /// <returns></returns>
        public delegate int Lookup(String variable_name);
        /// <summary>
        /// Calculates the result of an infilx expression.
        /// </summary>
        /// <param name="expression"></param> - infilx expression
        /// <param name="variableEvaluator"></param> - lookup varaiable 
        /// <returns></returns>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            /**
             * variables to keeps track of value and operator stacks and spilt the string. 
             * */
            Stack<int> valueStack = new Stack<int>(); // stack storing the values(4,5,6)
            Stack<char> operatorStack = new Stack<char>(); //stack storing the operators(+,/,-)
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
            int thingsInPar = 0;

            for (int i = 0; i < substrings.Length; i++) {
                substrings[i] = substrings[i].Trim();
                bool isNotAvAriable = false;
                bool containedOperator = false;
                if (substrings[i] == " " || substrings[i] == "")
                {
                    continue;
                }
                // if t is an int 
                else if (Int32.TryParse(substrings[i], out int outputnumber))
                {
                    if (operatorStack.Contains('('))
                    {
                        thingsInPar++;
                    }
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
                            int newAnswer = computeNewNumber(valueStack, operatorStack);
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
             else if (substrings[i] == "*" || substrings[i] == "/")
                {
                    if (operatorStack.Contains('('))
                    {
                        thingsInPar++;
                    }
                    containedOperator = true;
                    isNotAvAriable = true;
                    operatorStack.Push(char.Parse(substrings[i]));
                }
                // if t is (
               else if (substrings[i] == "(")
                {
                    isNotAvAriable = true;
                    operatorStack.Push(char.Parse(substrings[i]));
                }
                // if t is + or -
              else  if (substrings[i] == "+" || substrings[i] == "-")
                {
                    if (operatorStack.Contains('('))
                    {
                        thingsInPar++;
                    }
                    containedOperator = true;
                    isNotAvAriable = true;
                    if (operatorStack.Contains('('))
                    {
                        if (operatorStack.Contains('('))
                        {
                            thingsInPar++;
                        }
                        operatorStack.Push(char.Parse(substrings[i]));
                        continue;
                    }
                    else
                    {
                        calculateTheResult(valueStack, operatorStack);
                    }
                    operatorStack.Push(char.Parse(substrings[i]));
                }
                // if t is )
              else  if (substrings[i] == ")")
                {
                    if(thingsInPar <= 1)
                    {
                        throw new ArgumentException();
                    }
                    isNotAvAriable = true;
                    calculateTheResult(valueStack, operatorStack);
                    if (operatorStack.Contains('(') == false || containedOperator == true)
                    {
                        throw new ArgumentException();
                    }
                    operatorStack.Pop();
                }
               else if (isNotAvAriable == false)
                {
                    // if it is a variable
                    if (isAVaraiable(substrings[i]) == true)
                    {
                        try
                        {
                            valueStack.Push(variableEvaluator(substrings[i]));
                        }
                        catch (Exception)
                        {
                            throw new ArgumentException();
                        }
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }
            }
            if(substrings.Length <= 1)
            {
                if (substrings[0] == " " || substrings[0] == "")
                {
                    throw new ArgumentException();
                }
            }
            calculateTheResult(valueStack, operatorStack);
            calculateTheLastOfThem(operatorStack, valueStack);
            calculateTheLastOfThem(operatorStack, valueStack);
            if (operatorStack.Count > 0)
            {
                throw new ArgumentException();
            }
            return valueStack.Pop(); // answer 
        }
        /// <summary>
        /// helps calculate the rest
        /// </summary>
        /// <param name="operatorStack"></param> a stack of operators
        /// <param name="valueStack"></param> a stack of values
        private static void calculateTheLastOfThem(Stack<char> operatorStack, Stack<int> valueStack)
        {
            if (operatorStack.Count > 0 && valueStack.Count >= 2)
            {
                calculateTheResult(valueStack, operatorStack);
            }
        }

        /// <summary>
        /// calculates the result based on the operator stack. 
        /// </summary>
        /// <param name="valueStack"></param> the stack containing the values
        /// <param name="operatorStack"></param> the stack containg the operators
        private static void calculateTheResult(Stack<int> valueStack, Stack<char> operatorStack)
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
        /// <param name="currentToken"></param> the varaiable in question 
        /// <returns></returns>
        private static bool isAVaraiable(string currentToken)
        {
            return Regex.IsMatch(currentToken, "^[a-zA-Z][0-9]+");
        }
        /// <summary>
        /// selects which operation by the operatorAns then computes it the correct way 
        /// </summary>
        /// <param name="valueStack"></param> a stack of values
        /// <param name="operatorStack"></param> a stack of operators
        /// <returns></returns>
        private static int computeNewNumber(Stack<int> valueStack, Stack<char> operatorStack)
        {
            // varaiables pulled from the stacks
            int firstNumber = valueStack.Pop();
            int secondNumber = valueStack.Pop();
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
                try
                {
                    return secondNumber / firstNumber;
                }
                catch (Exception)
                {
                    throw new  ArgumentException();
                }
            }
            else
            {
                return firstNumber * secondNumber;
            }
        }
    }
}
