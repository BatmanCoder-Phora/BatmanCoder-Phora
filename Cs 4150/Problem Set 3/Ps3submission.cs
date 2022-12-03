/*
* Author: Sephora Bateman 
* Class: CS 4150
* Problem Set #3
*/

/*
 * WHAT TO WORK ON FOR CODE:
 *  - work on the timing issue. 
 *     
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Problem_Set_3
{
    public class Ps3submission
    {
        // ending variables. 
        public static int minisl = int.MaxValue;
        public static long result;

        // variables for comparison and storing.
        public static long islandNumbers;
        public static long numOfDestinations;

        //A long array that stores the bitmaps for all the islands. 
        public static long[] inputislandsAndFerries = new long[37];

        /// <summary>
        /// main function to slove for min islands. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int minislands = 0;
            long islandCoverage = 0;
            long islandsWithShops = 0;
            string stringAnser = "";
            // is there a way to get rid of all these loops.

            string line = Console.ReadLine();
            string[] linespilt = line.Split(' ');
            numOfDestinations = Int32.Parse(linespilt[0]);
            long numOfFerryRoutes = Int32.Parse(linespilt[1]);

            // grab the number of destinations. 
            for (int i = 1; i <= numOfDestinations; i++)
            {
                islandNumbers = (islandNumbers | (1L << i));
            } 
            //   input all the ferry route data into a data structure. 
            for (int i = 0; i < numOfFerryRoutes; i++)
            {
                inputIslandAndFerryData(line);
            }
            int index = 1;
            // locate the min number of shops 
            findMinNumOfShops(islandsWithShops, islandCoverage, index, minislands);


            Console.WriteLine(minisl);
            for (int i = 1; i <= numOfDestinations; i++)
            {
                if ((result & (1L << i)) != 0)
                    stringAnser = stringAnser + (i.ToString() + " ");
            }
            Console.WriteLine(stringAnser.Trim());

        }

        /// <summary>
        ///  Stores the islands and the ferry routes in a dictionary also figure out the destination. 
        /// </summary>
        /// <param name="i"></param>
        private static void inputIslandAndFerryData(string line)
        {
            line = Console.ReadLine().Trim();
            string[] spilt = line.Split(' ');
            int island = Int32.Parse(spilt[0]);
            int ferry = Int32.Parse(spilt[1]);
            // make sure every isalnd has it's self in the bit map.
            if (inputislandsAndFerries[island] == 0)
                inputislandsAndFerries[island] = (1L << island);
            if (inputislandsAndFerries[ferry] == 0)
                inputislandsAndFerries[ferry] = (1L << ferry);
            // update the value to inculde the ferry routes. 
            long value = inputislandsAndFerries[island];
            long valueferry = inputislandsAndFerries[ferry];
            inputislandsAndFerries[island] = (value | (1L << ferry));
            inputislandsAndFerries[ferry] = (valueferry | (1L << island));
        }

        /// <summary>
        /// Finds the smallest number of shops that need to be built. 
        /// </summary>
        /// <param name="islands"> the list of islands.</param>
        /// <param name="islandCoverage">The islands we have passed</param>
        /// <param name="index">the island we are checking.</param>
        private static void findMinNumOfShops(long islandsWithShops, long islandCoverage, int index, int minislands)
        {

            long oldislandCoverage = islandCoverage;
            // find the new soultion
            if (minislands >= minisl)
                return;
            if (islandCoverage == islandNumbers)
            {
                minisl = minislands;
                result = islandsWithShops;
                return;
            }
            // base case for no soultion. 
            if (index > numOfDestinations)
                return;

            long value = inputislandsAndFerries[index];
            islandCoverage = islandCoverage | value;
            if (value == 0) // orphan cluase. 
            {
                minislands++;
                islandCoverage = islandCoverage | (1L << index);
                islandsWithShops = islandsWithShops | (1L << index);
                findMinNumOfShops(islandsWithShops, islandCoverage, index + 1, minislands);
            }
            else if (islandCoverage == oldislandCoverage)
            {
                findMinNumOfShops(islandsWithShops, oldislandCoverage, index + 1, minislands);
            }
            else
            {
                findMinNumOfShops(islandsWithShops, oldislandCoverage, index + 1, minislands);
                minislands++;
                islandsWithShops = islandsWithShops | (1L << index);
                findMinNumOfShops(islandsWithShops, islandCoverage, index + 1, minislands);
            }

        }
    }
}