/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   Corbin Gurnee 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// We, Sephora Bateman and Corbin Gurnee , certify that We wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in our README file. 
/// </summary>
using Microsoft.Extensions.Logging;
using System;

namespace CS3500
{
    class ShowTimeAndThread
    {
        static void Main(string[] args)
        {
        }
        /// <summary>
        /// Converts Infrmation about the Show,Time and Thread into a string. 
        /// </summary>
        /// <returns>A string version of Show,Time and Thread.</returns>
        public string TurnInfomationIntoAString<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception)
        {
            string DateTimeThreadInfo = $"{DateTime.Now} ({System.Threading.Thread.CurrentThread.ManagedThreadId}) - {logLevel.ToString()[0]}" +
                $"{logLevel.ToString()[1]}{logLevel.ToString()[2]}{logLevel.ToString()[3]}{logLevel.ToString()[4]} - {state} ";
            return DateTimeThreadInfo;
        }
    }
}
