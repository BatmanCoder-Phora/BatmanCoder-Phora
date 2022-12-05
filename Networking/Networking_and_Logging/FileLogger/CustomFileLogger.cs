
/// <summary> 
/// Author:    Corbin Gurnee
/// Partner:   Sephora Bateman 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// We, Sephora Bateman and Corbin Gurnee , certify that We wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in our README file. 
/// </summary>
/// 
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace CS3500
{
    class CustomFileLogger : ILogger
    {
        // variables for this class
        public string categoryName;
        public StreamWriter writer;
        private bool Append = true;
        /// <summary>
        /// Constructor for CustomFileLogger opening the file. 
        /// </summary>
        /// <param name="categoryName"></param>
        public CustomFileLogger(string categoryName)
        {
            this.categoryName = categoryName;
            if (File.Exists("Log_" + categoryName + ".txt"))
                Append = true;
            else
                Append = false;
            writer = new StreamWriter("Log_" + categoryName + ".txt", true);
            try
            {
                if (!File.Exists("Log_"+categoryName+".txt"))
                {
                    File.Create("Log_" + categoryName + ".txt").Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR: Log path not found on local disk: {0}", e.Message);
                Console.ReadKey();
            }
        }
        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for</typeparam>
        /// <param name="state">The identifier for the scope.</param>
        /// <returns>An IDisposable</returns>
        public IDisposable BeginScope<TState>(TState state)
        {
            /**
             *  One of the basic reasons of scoping is to keep variables in different parts of program distinct from one another.
             *  Scope is an important component of name resolution, which is in turn fundamental to language semantics. Name resolution
             * (including scope) varies between programming languages, and within a programming language, varies by type of entity; the rules
             *  for scope are called scope rules or scoping rules.
             */
            throw new NotImplementedException();
        }
        /// <summary>
        /// Checks if the given logLevel is enabled.
        /// </summary>
        /// <param name="logLevel">level to be checked.</param>
        /// <returns>A bool</returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            //Do not need to Implement or comment. 
            throw new NotImplementedException();
        }
        /// <summary>
        /// Writes a log entry.
        /// </summary>
        /// <typeparam name="TState">The type of the state to begin scope for</typeparam>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a String message of the state and exception.</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            ShowTimeAndThread showTimeAndThread = new ShowTimeAndThread();
            string logErrorMessage = showTimeAndThread.TurnInfomationIntoAString(logLevel, eventId, state, exception);
            writer.Write(logErrorMessage);
            writer.Flush();
        }
        /// <summary>
        /// Saves and Closes the writer
        /// </summary>
        public void Close()
        {
            writer.Dispose();
            writer.Close();
        }
    }
}
