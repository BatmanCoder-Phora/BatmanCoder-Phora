Author:     Sephora Bateman 
Partner:    None
Date:       1/13/20
Course:     CS 3500, University of Utah, School of Computing
Assignment: FormulaEvaluator,DependencyGraph,Formula class
Copyright:  CS 3500 and Sephora Bateman - This work may not be copied for use in Academic Coursework.

1a. Comments to Evaluators:
ASSIGNMENT ONE:
  - One thing I would like to state is that in my testing class. The section Testing Exception, should bring up exceptions like the stack is empty, value stack is empty, Argument
   Exception and divide by zero exception. To test them out you need to test one at a time to see the message that is displayed.
 - Another thing I would like to state is that I left the code for a foreach loop I used commented out there, I changed to a normal for-loop because of the String.trim() 
   method does not work on a variable created in a for-each loop. 
ASSIGNMENT TWO: 
- in my FindAndRemove helper method, I have code that would remove s or t from the dictionary if its count was equal to zero. However I could not find a way to test it so, I 
 comment it out. 
 - In my ReplaceDependents and ReplaceDependees, I looked up the best way to loop through an IEmmurable. In the resources I looked at a for each loop was the best, now there are 
two ways to replace something, just overwrite the value that was originally in there or deletes what was fist there then add the new value. Since there is a remove dependency and 
an addDependency. I looped through it twice once removing and then adding.
ASSIGNMENT THREE:
  - I coudn't get 100% on my code coverage beacuse a lot of my if statments have multiple parts to them, so it was hard to hit all at once. Which resulted in a lot of purple. which brought
  down my average just a little bit. Another thing I can't seem to get my == and != methods for both formulas null to return the false or true. Just something to note. 
  -  Some of my tests are from the unit tests  that were given from assigment one, changed to work for this assigment. Also I got how to test for the formulaError from one of my old tests.
ASSIGNMENT FOUR:
  - For setContent formula, when a circular Exception is caught, make no change to the spreadsheet. So we need to set the content of the cell back to what it was before. 
  We can do that by calling the setContent methods. 
 ASSIGMENT FIVE:
 -I based my xml writer and xml reader from the code I found in the nations and state.cs (xml demo) given in class.



 1b. Time Mangement (Acutal Hours / Expected hours):
 ASSIGNMENT ONE:
     10-12/15
ASSIGNMENT TWO:
     9-10/12
ASSIGNMENT THREE:
	 16-17/14
ASSIGNMENT FOUR:
       19/20
ASSIGNMENT FIVE:
        16/20




2. Assignment Specific Topics
   The FORMULA EVALUATOR is the math behind the spreadsheet. When someone types in a formula for example (2+3) or A2+C4 the answer should appear in that cell that they were typed in. 
   The  Formula Evaluator does those calculations then answers back to the spreadsheet. The DEPENDENCY GRAPH keeps track of all the dependencies in the spreadsheet. For an example if the 
   result of B1 is dependent on the result from A2 and D3.  The FORMULA CLASS is an improvment of the formual Evaluator adding more detail and more cilent choices. The SPREADSHEET CLASS 
   starts putting cells that are needed for a spreadsheet. Assignment Five adds on to assignment four , the value is added to the class, the value is the same as
   the content for double and string, but if it is a formula the content is the string version of the formula while the value is the evaluated number/FormulaError. We also added xml 
   writer and reader. 




3. Consulted Peers:
ASSIGNMENT ONE:
- TA:
	Jolie Uk

- Classmates and Random People:
	John Smith 

ASSIGNMENT TWO:
 -Teacher(Lecture) 

ASSIGNMENT THREE:
 Aspen Evens 
 Tony

ASSIGNMENT FOUR: 
 TA's
 Professor St. Germain

ASSIGNMENT FIVE:
 TA's
 Kyle Skinner


4. References:
ASSIGNMENT ONE:
  1. https://stackoverflow.com/questions/10744305/how-to-create-a-gitignore-file 
  2. https://stackoverflow.com/questions/1181419/verifying-that-a-string-contains-only-letters-in-c-sharp 
  3. http://www.csharp411.com/remove-whitespace-from-c-strings/ 
  4. https://stackoverflow.com/questions/14894503/foreach-to-trim-string-values-in-string-array
ASSIGNMENT TWO:
  1. https://stackoverflow.com/questions/1273139/c-sharp-java-hashmap-equivalent
  2. https://stackoverflow.com/questions/1532814/how-to-loop-through-a-collection-that-supports-ienumerable
ASSIGNMENT THREE:
  1. https://stackoverflow.com/questions/1654209/how-to-access-index-in-ienumerable-object-in-c
  2. https://docs.microsoft.com/en-us/dotnet/api/system.object.referenceequals?view=netframework-4.8
  3. Some of my tests are from the unit tests  that were given from assigment one, changed to work for this assigment. Also I got how to test for the formulaError from one of my old tests
ASSIGNMENT FOUR:
  1. https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.keyvaluepair-2?view=netframework-4.8 
ASSIGNMENT FIVE
  1. https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/inheritance
  2. https://docs.microsoft.com/en-us/dotnet/api/system.xml.xmlreadersettings.ignorewhitespace?view=netframework-4.8
  3. https://github.com/uofu-cs3500-spring20/lecture_notes/blob/master/XMLDemo/Nations/Nation.cs
  4. https://stackoverflow.com/questions/686571/why-does-assert-isinstanceoftype0-gettype-typeofint-fail


 
5. Code Coverage:
ASSIGMENT TWO:
Dependency Graph - 100.00 %
Dependency Graph Testing - 96.37%
ASSIGMENT THREE:
Formula- 96.86 %
FormulaUnitTests - 94.65% 
ASSIGMENT FOUR:
Spreadsheet - 100.00 %
SpreadsheetTest - 90.97 %
ASSIGMENT FIVE
Spreadsheet -  100.00 %
SpreadsheetTest - 93.44 %

6. My Software Practices:
ASSIGNMENT FOUR:
- I have helper methods to less code re-use 
- I starting coding a little bit, then I test what I have written, then again write a little bit more and write tests. 
ASSIGNMENT FIVE:
- I have helper methods to reduce code re-use 
- I have grading test from the last assigment, making sure those still work(Regression Testing). They are renamed , becuase the old name did not make any sense. 
- I starting coding a little bit, then I test what I had written, then again write a little bit more and write more tests. 
