/*
 * Author: Sephora Bateman 
 * Class: CS 4150
 * Problem Set #2
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem_Set_2
{
    public class Ps2submission
    {
        
        static public int last;
        static public int beforeLast;
        static public int lastslope;
        static public int firstslope;
        static public int minvalue = int.MaxValue;
        static public int first;
        static public int afterFirst;
        static public bool firstHasNotBeenChanged = true;
        public static Dictionary<int, int> values = new Dictionary<int, int>();
        /// <summary>
        /// The main function for a program that finds the minium vales in a circle array. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int min = 0;
            // read the array size 
            string line = Console.ReadLine();
            int arrayLength = Int32.Parse(line);
            arrayLength -= 1;

            // get the first point and last point only once and add them to the values dictionary depending on the length 
            if (arrayLength == 0)
            { min = 0; }
            else if (arrayLength == 1)
            {
                first = queryAraay(0);
                afterFirst = queryAraay(0 + 1);
                if (first > afterFirst)
                    min = 1;
                else
                    min = 0;
            }
            else if (arrayLength == 2)
            {
                first = queryAraay(0);
                afterFirst = queryAraay(0 + 1);
                last = queryAraay(arrayLength);
                if (first < afterFirst && first < last)
                    min = 0;
                else if (afterFirst < first && afterFirst < last)
                    min = 1;
                else
                    min = arrayLength;
            }
            else
            {
                first = queryAraay(0);
                afterFirst = queryAraay(1);
                beforeLast = queryAraay(arrayLength - 1);
                last = queryAraay(arrayLength);
                values.Add(0, first);
                values.Add(arrayLength, last);
                values.Add(1, afterFirst);
                values.Add(arrayLength - 1, beforeLast);
                firstslope = afterFirst - first;
                lastslope = last - beforeLast;

                // find the min 
                algorithmToFindMin(0, arrayLength);
            }


            // print the poistion in the array of where the minimum is. 

            foreach (KeyValuePair<int, int> kvp in values)
                if (kvp.Value == minvalue)
                    min = kvp.Key;

            Console.WriteLine("minimum" + " " + min);
        }
        /// <summary>
        /// An algorithm to find the position of the minimum value.
        /// </summary>
        /// <param name="arrayLength"></param>
        /// <exception cref="NotImplementedException"></exception>
        private static void algorithmToFindMin(int start, int arrayLength)
        {
            //local 
            int mid = 0, midsec = 0;
            int midcalculation = (start + arrayLength) / 2;
            int addtionalp;
            bool wasAdded = false;

            //base case
            if (midcalculation == start || midcalculation == arrayLength)
                return;

            // something to help find rether we grab the second middle point before or after the middle. 
            if ((firstslope > 0 && lastslope > 0))
            { addtionalp = midcalculation + 1; wasAdded = true; }
            else
            { addtionalp = midcalculation - 1; }

            // sees if the dictionary already found the value for the requested numbers if not makes a query.
            if (values.ContainsKey(midcalculation))
                values.TryGetValue((int)midcalculation, out mid);
            else
            {
                mid = queryAraay(midcalculation);
                values.Add(midcalculation, mid);
            }
            if (values.ContainsKey(addtionalp))
                values.TryGetValue(addtionalp, out midsec);
            else
            {
                midsec = queryAraay(addtionalp);
                values.Add(addtionalp, midsec);
            }
            // figures out how to find the slope. 
            int midslope;
            if (wasAdded)
                midslope = midsec - mid;
            else
                midslope = mid - midsec;

            // update the min. 
            if (first < last && firstHasNotBeenChanged)
                minvalue = first;
            if(last < first)
                minvalue = last;
            if (midsec < minvalue)
                minvalue = midsec;
            if (mid < midsec && mid < minvalue)
                minvalue = mid;

            // if the midslope is positive than check if the start is positive depending on the response shift the graph. 
            if (midslope > 0)
            {
                if (firstslope > 0)
                {
                    firstHasNotBeenChanged = false;
                    if (mid > first)
                    {
                        firstslope = midslope;
                        first = mid;
                        algorithmToFindMin(midcalculation, arrayLength);
                    }
                    else if (mid < first)
                    {
                        lastslope = midslope;
                        first = mid;
                        algorithmToFindMin(start, midcalculation);
                    }
                }
                else
                {
                    lastslope = midslope;
                    last = mid;
                    algorithmToFindMin(start, midcalculation);
                }
            }
            // if the midslope is negative than check if the start is negative depending on the response shift the graph. 
            if (midslope < 0)
            {
                if (firstslope < 0)
                {
                    firstHasNotBeenChanged = false;
                    if (mid < first)
                    {
                        firstslope = midslope;
                        first = mid;
                        algorithmToFindMin(midcalculation, arrayLength);
                    }
                    else if (mid > first)
                    {
                        lastslope = midslope;
                        last = mid;
                        algorithmToFindMin(start, midcalculation);
                    }
                }
                else
                {
                    firstslope = midslope;
                    first = mid;
                    algorithmToFindMin(midcalculation, arrayLength);
                }
            }
        }


        /// <summary>
        /// Helpful method to make queries. 
        /// </summary>
        /// <param name="queryNumber">The number we ant the information for</param>
        private static int queryAraay(int queryNumber)
        {
            Console.WriteLine("query" + " " + queryNumber);
            string line = Console.ReadLine();
            int qn = Int32.Parse(line);
            return qn;
        }
    }
}
