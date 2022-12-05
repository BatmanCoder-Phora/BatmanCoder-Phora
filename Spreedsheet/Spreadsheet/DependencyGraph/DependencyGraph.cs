// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
// (Clarified meaning of dependent and dependee.)
// (Clarified names in solution/project structure.)
/// Changes ///
// Author:    Sephora Bateman  
// Partner:   none 
// Date:      1/18/20
// Course:    CS 3500, University of Utah, School of Computing 
// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
// 
// I, Sephora Bateman , certify that I wrote this code from scratch and did not copy it in part or whole from  
// another source.  All references used in the completion of the assignment are cited in my README file.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// 
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /**
         * Private variables
         **/
        private Dictionary<string, HashSet<String>> DependeesDictionary;
        private Dictionary<string, HashSet<String>> DependentsDictionary;
        private int dependencySize;
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependencySize = 0;
            DependeesDictionary = new Dictionary<string, HashSet<String>>();
            DependentsDictionary = new Dictionary<string, HashSet<String>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return dependencySize; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (DependentsDictionary.ContainsKey(s))
                {
                    return DependentsDictionary[s].Count;
                }
                 return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            if (DependeesDictionary.ContainsKey(s))
                if (DependeesDictionary[s].Count > 0)
                {
                    return true;
                }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            if (DependentsDictionary.ContainsKey(s))
                if (DependentsDictionary[s].Count > 0)
                {
                    return true;
                }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (DependeesDictionary.ContainsKey(s))
                return new HashSet<string>(DependeesDictionary[s]);
            else
                return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (DependentsDictionary.ContainsKey(s))
                return new HashSet<string>(DependentsDictionary[s]);
            else
                return new HashSet<string>();
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            if (!(DependeesDictionary.ContainsKey(s) && DependentsDictionary.ContainsKey(t)) && t != null)
            {
                dependencySize++;
            }
            // if dependees already contains s then just add t to that list.
            if (DependeesDictionary.ContainsKey(s))
            {
                DependeesDictionary[s].Add(t);
            }
            // if not then add s and add a new list that contains t.
            else
            {
                DependeesDictionary.Add(s, new HashSet<string>() { t });
            }
            // if dependents already contains t then just add s to that list.
            if (DependentsDictionary.ContainsKey(t))
            {
                DependentsDictionary[t].Add(s);
            }
            // if not then add t and add a new list that contains s.
            else
            {
                DependentsDictionary.Add(t, new HashSet<string>() { s });
            }
        }
        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (DependeesDictionary.ContainsKey(s))
            {
                FindAndRemoveADependency(DependeesDictionary, s, t);
            }
            if (DependentsDictionary.ContainsKey(t))
            {
                FindAndRemoveADependency(DependentsDictionary, t, s);
            }
            dependencySize--;
        }
        /// <summary>
        ///  The Helper method is used during the RemoveDependency method and removes a certin Dependency or removes it the key from the dictionary.
        /// </summary>
        /// <param name="dependees"></param>
        /// <param name="s"></param> Either the string we want to find, or the string we want to remove.
        /// <param name="t"></param> Either the string we want to find, or the string we want to remove.
        private void FindAndRemoveADependency(Dictionary<string, HashSet<string>> dependees, string s, string t)
        {
            /*if(dependees[s].Count == 0)
            {
                dependees.Remove(s);
            }*/
            dependees[s].Remove(t);            
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            IEnumerable<string> oldDependents = GetDependents(s);
            foreach (string oldDependentToken in oldDependents)
            {
                // There is already a method that can remove a dpendency, used remove Dependency to empty out the old-Dependees(s).
                RemoveDependency(s, oldDependentToken); 
            }
            foreach (string newDependentsToken in newDependents)
            {
                // then the use of the addDependency method to add back the new dpendees in Dependees(s).
                AddDependency(s, newDependentsToken); 
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            IEnumerable<string> oldDependees = GetDependees(s);   
            foreach (string oldDependeesToken in oldDependees)
            {
                // There is already a method that can remove a dpendency, used remove Dependency to empty out the old-Dependees(s).
                RemoveDependency(oldDependeesToken, s);  
            }
            foreach (string newDependeesToken in newDependees)
            {
                // then the use of the addDependency method to add back the new dpendees in Dependees(s).
                AddDependency(newDependeesToken, s);  
            }
        }

    }

}
