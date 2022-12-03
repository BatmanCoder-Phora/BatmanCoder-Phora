/*
 * Author: Sephora Bateman 
 * Class: CS 4150
 * Problem Set #1
 */
using System;
using System.Collections.Generic;

namespace submission
{

    public class submission
    {
        // public variables to store groups of matched words. 
        public static Dictionary<string, HashSet<string>> matchedwords = new Dictionary<string, HashSet<string>>();
        public static HashSet<string> sortedwords = new HashSet<string>();
        static void Main(string[] args)
        {
            // n and k variables and create other needed any other variables
            int numberOfWords;
            int result;
            string line;
            // get the numbers of words in the list and their length. 
            line = Console.ReadLine();
            string[] linespilt = line.Split(' ');
            Int32.TryParse(linespilt[0], out numberOfWords);

            // grab the rest of the words and pair them. 
            for (int i = 0; i < numberOfWords; i++)
            {
                line = Console.ReadLine();
                FindAnagramAl1(line);
            }
            // see how many groups of anagrams are in the dictionary 
             result = matchedwords.Count;
            //print the number of groups that are anagrams 
            Console.WriteLine(result);
        }
 
        /// A method that checks if a word is an anagram for an of the words in a list. 
        /// </summary>
        /// <param name="line">The word we are check is an anagram</param>
        /// <exception cref="NotImplementedException"></exception>
        public static void FindAnagramAl1(string line)
        {
            // Variable to track when a word matches
            bool wasadded = false;
            // spilt the word we pass into the method
            char[] word = line.ToCharArray();
            Array.Sort(word);
            string RestringWord = new string(word);
            /* run through the hashset if any match another stored word add them
               to the dictionary(if it isn't already a key add one). */
            foreach (string test in sortedwords)
            {
                if (test.Equals(RestringWord))
                {
                    wasadded = true;
                    if(matchedwords.ContainsKey(test) == false)
                        matchedwords.Add(RestringWord, new HashSet<string>() { });
                    matchedwords[test].Add(RestringWord);
                }
            }
            // if there are no matches add it to the hashset. 
            if (!wasadded)
            {
                string input = new string(word);
                sortedwords.Add(input.Trim());
            }
                
        }
    }
}
