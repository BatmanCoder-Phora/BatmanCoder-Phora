
/// <summary> 
/// Author:    Corbin Gurnee
/// Partner:   Sephora Bateman 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// We, Sephora Bateman and Corbin Gurnee , certify that We wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in our README file. 
/// </summary>
using Microsoft.Extensions.Logging;
namespace CS3500
{
    public class CustomFileLogProvider : ILoggerProvider
    {
        CustomFileLogger Logger;
        /// <summary>
        /// Creates a new ILogger instance.
        /// </summary>
        /// <param name="categoryName">The category name for messages produced by the logger.</param>
        /// <returns>An ILogger</returns>
        public ILogger CreateLogger(string categoryName)
        {
            Logger = new CustomFileLogger(categoryName);
            return Logger;
        }
        /// <summary>
        /// Disposes the Logger 
        /// </summary>
        public void Dispose()
        {          
            Logger?.Close();
            Logger = null;
        }
    }
}
