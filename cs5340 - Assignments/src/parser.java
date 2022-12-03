import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Scanner;
import java.util.Stack;

/**
 * This is a parser class that spilt up a sentance and finds it's dependencies. 
 * @author Sephora Bateman
 * 
 * Assignment: Programming Homework Two 
 * Class: Natural Languages
 * 
 */
public class parser
{
	/**
	 * The Goal Variable for keeping track of dependancies.  
	 */
    static HashMap<String,Integer> Dependency = new HashMap<String,Integer>();
    
    /**
     * The entry point into my class. 
     * @param args
     * @throws IOException
     */
	public static void main(String[] args) throws IOException {
		if (args[0].substring(1).equals("genops")&& args.length > 2)
		{
			File operatorsFile = new File(args[1]);
			File goldFile = new File(args[2]);
			GenopsParsingAlgorithm(goldFile, operatorsFile);
		}
		else if (args[0].substring(1).equals("simulate"))
		{
			File sentopsFile = new File(args[1]);
			TransitionParsingAlgorithm(sentopsFile);
		}

	}

	/**
	 * The algorithm to sort through the golden file. 
	 * @param goldFile: The file of golde parsed words.  
	 * @param operatorsFile: The files containg operators. 
	 * @throws IOException: Will throw an expecption. 
	 */
	@SuppressWarnings("resource")
	private static void GenopsParsingAlgorithm(File goldFile, File operatorsFile) throws IOException {
		
		ArrayList<String> listOfSentances = new ArrayList<String>();
		Scanner scan = new Scanner(goldFile);

		String sentance = "ROOT ";
		System.out.print("SENTENCE: ");
		scan.nextLine();
		
		while (scan.hasNextLine()) {
			String splitLine = scan.nextLine();
			listOfSentances.add(splitLine);
			String[] printableString = splitLine.split("\\t");
			sentance += printableString[1].trim() + " ";
		}
		scan.close();

		String[] spiltSentance = sentance.split(" ");
		String fullSentance = PrintSentance(spiltSentance);
		spiltSentance = AddNumbersToSentance(spiltSentance);
		
		System.out.println(fullSentance.substring(0, fullSentance.length() - 1));
		
		// Prints Golden Dependencies 
		System.out.print("GOLD DEPENDENCIES");
		System.out.println();
		HashMap<String,String> goldList = FindGoldDependencies(listOfSentances, spiltSentance);
		System.out.println();
		
		// Prints operations 
		System.out.print("GENERATING PARSING OPERATORS");
		System.out.println();
		ArrayList<String> finalCommands = ParseAndFinzalizeOperators(goldList, spiltSentance);
		System.out.println();
		
		// Print final operators. 
		System.out.print("FINAL OPERATOR SEQUENCE");
		System.out.println();
		PrintFinal(finalCommands);
	}

	/**
	 * Helper function to parse the operators and return the final cut of operators. 
	 * @param goldList
	 * @param spiltSentance
	 * @return
	 */
	private static ArrayList<String> ParseAndFinzalizeOperators(HashMap<String, String> goldList,
			String[] spiltSentance)
	{
		
		Stack<String> segment = new Stack<String>();
		ArrayList<String> commands = new ArrayList<String>();
		HashMap<String,Integer> tempDep = new HashMap<String,Integer>();
		
		segment.push("ROOT(0)");
		int pushInt = 1;
		
		// keep looking until the stack is empty. 
		while(!segment.isEmpty())
		{
			if(segment.size() < 2)
			{
			    System.out.println("Shift");
			    commands.add("Shift");
			    segment.push(spiltSentance[pushInt]);
			    pushInt++;
			}
			else
			{
				String top = segment.pop();
				String topMinusOne = segment.pop();
				String LeftArc = top + " --> " + topMinusOne;
				String RightArc = topMinusOne + " --> " + top;
				
				if(tempDep.containsKey(top)!= true)
					tempDep.put(top, 0);
				if(tempDep.containsKey(topMinusOne)!= true)
					tempDep.put(topMinusOne,0);
				
				// If it's a vaild left Arc 
				if(goldList.containsKey(LeftArc))
				{
					tempDep = AddDependancy(tempDep,top,topMinusOne);
					String ouput = goldList.get(LeftArc);
					String[] split = goldList.get(LeftArc).split(" ");
					commands.add("LeftArc_"+split[2]);
					System.out.println("LeftArc_"+split[2]+" to produce: " + ouput);
					segment.push(top);
				}
				// if it's a vaild right arc with all of the dependencies generated
				else if(goldList.containsKey(RightArc)&& GenALLDependencies(tempDep,top)== true)
				{
					tempDep = AddDependancy(tempDep,top,topMinusOne);
					String ouput = goldList.get(RightArc);
					String[] split = goldList.get(RightArc).split(" ");
					System.out.println("RightArc_"+split[2]+" to produce: " + ouput);
					commands.add("RightArc_"+split[2]);
					if(!topMinusOne.equals("ROOT(0)"))
						segment.push(topMinusOne);
				}
				// if neither arcs are vaild. 
				else
				{
					 tempDep = AddDependancy(tempDep,top,topMinusOne);
					 System.out.println("Shift");
					 commands.add("Shift");
					 segment.push(topMinusOne);
					 segment.push(top);
					 segment.push(spiltSentance[pushInt]);
					 pushInt++;
				}	
			}
		}
		pushInt = 0;
		return commands;
	}

	/**
	 * Helper method that checks to see if all Dependencices of the top string have been generated.
	 * @param tempDep: The local dependency graph 
	 * @param top: The string we are checking the depdencies for.
	 * @return A modified TempDep
	 */
	private static boolean GenALLDependencies(HashMap<String, Integer> tempDep, String top)
	{
		if(tempDep.containsKey(top))
		{
			if(tempDep.get(top) == Dependency.get(top))
				return true;
			else
				return false;
		}
		return false;
	}

	/**
	 * A helper method to find all of the Gold Dependencies in a file.  
	 * @param listOfSentances: The gold rules.
	 * @param spiltSentance: The words in a sentance. 
	 * @return A updated version of the golden Dependencies. 
	 */
	private static HashMap<String, String> FindGoldDependencies(ArrayList<String> listOfSentances, String[] spiltSentance) {
	
		HashMap<String,String> returnGoldList = new HashMap<String,String>();
		
		for (String message : listOfSentances)
		{
			String[] splitLine = message.split("\\t");
			String input = splitLine[1].trim() + "(" + splitLine[0] + ")";
			int lookup = Integer.parseInt(splitLine[2].trim());

			String connection = spiltSentance[lookup];

			returnGoldList = PrintConnection(splitLine, input, connection,returnGoldList);

		}
		return returnGoldList;
	}


	/**
	 * Helper method to print connection between two strings.
	 * @param splitLine: The given sentance split up
	 * @param input: The Start of the connection. 
	 * @param connection: The type of connection
	 * @param returnGoldList: The list the connecntions are saved in. 
	 * @return: return a modified list of connections. 
	 */
	private static HashMap<String, String> PrintConnection(String[] splitLine, String input, String connection, HashMap<String, String> returnGoldList) {
		
		if (splitLine[3].equals("root"))
		{
			String connection1 = "ROOT(0) -- " + splitLine[3] + " --> " + input;
			String relation = "ROOT(0) --> " + input;
			if(Dependency.containsKey(input)==false)
				Dependency.put(input, 0);
			System.out.println(connection1);			
			returnGoldList.put(relation,connection1);
			return returnGoldList;
		}
		else
		{
			String connection1 = connection + " -- " + splitLine[3] + " --> " + input;
			String relation = connection + " --> " + input;
			AddDependancy(Dependency,connection,input);
			System.out.println(connection1);			
			returnGoldList.put(relation,connection1);
			return returnGoldList;
		}
	}
	/**
	 * Add the dependancy between a root node and it's next state.
	 * @param dependency2: The dependancy graph we are working with. 
	 * @param splitLine: The given sentance split
	 * @param connection: The node that the input is connected to. 
	 * @param input: The start node of the connection. 
	 * @return: An updated Dependancy graph.  
	 */
	private static HashMap<String, Integer> AddDependancy(HashMap<String, Integer> dependency2, String connection, String input)
	{
		
		if(dependency2.containsKey(input)!= true)
			dependency2.put(input, 0);
		if(dependency2.containsKey(connection)!= true)
			dependency2.put(connection,0);
		
		dependency2.put(connection, dependency2.get(connection) + 1);
		
		return dependency2;
	}


	/**
	 * A method to help print the sentance. 
	 * @param spiltSentance: the sentance we are printing. 
	 */
	private static String PrintSentance(String[] spiltSentance) {
		String sentance = "";
		for (int i = 1; i < spiltSentance.length; i++)
			sentance += spiltSentance[i] + " ";
		return sentance;
	}

////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	/**
	 * The algorithm for parsing a sentops file
	 * @param sentopsFile: The given file
	 * @throws IOException
	 */
	private static void TransitionParsingAlgorithm(File sentopsFile) throws IOException {
		// Open file and begin to read
		Scanner scan = new Scanner(sentopsFile);
		String sentance = scan.nextLine();

		System.out.print(sentance + " ");
		System.out.println();

		// Save the sentance in an array
		String[] sentanceSaved = AddNumbersToSentance(sentance.split(" "));
		ArrayList<String> operators = new ArrayList<String>();

		// get operators.
		String operatorSub = scan.nextLine();
		System.out.print(operatorSub);
		System.out.println();

		String operatorPrint = "";
		String newSen = "";

		while (scan.hasNextLine()) {
			String[] operator = scan.nextLine().split(" ");
			if (operator.length == 2) {
				newSen = operator[0].trim() + "_" + operator[1].trim();
				operators.add(newSen);
				operatorPrint += newSen + " ";
			} else {

				operators.add(operator[0].trim());
				newSen = operator[0].trim();
				operatorPrint += newSen + " ";
			}
		}

		System.out.println(operatorPrint.substring(0, operatorPrint.length()));

		// Print Shifitng and Connections
		ArrayList<String> Final = FindShiftingAndConnections(operators, sentanceSaved);
		System.out.println();
		System.out.println("FINAL DEPENDENCY PARSE");
		if(Final.size() == 0)
			System.out.print("No dependencies");
		else
		   PrintFinal(Final);
		scan.close();
	}
	/**
	 * Prints the list passed in on a new line for each element.  
	 * @param final1
	 */
	private static void PrintFinal(ArrayList<String> final1) {

		for (String sentance : final1)
			System.out.println(sentance);
	}

	/**
	 * Added the corsponding number to the word in the sentance. 
	 * @param split: The Sentance. 
	 * @return: A new string array containing the numbers. 
	 */
	private static String[] AddNumbersToSentance(String[] split) {
		String[] returnAnswer = new String[split.length];
		for (int i = 0; i < split.length; i++)
		{
				returnAnswer[i] = split[i] + "(" + i + ")";
		}

		return returnAnswer;
	}

	/**
	 * Prints the operations and arcs of the file. 
	 * @param operators: The operatos we are looping through 
	 * @param sentanceSaved: The senatcne passed in. 
	 * @return: A new array of outputs. 
	 * @throws IOException
	 */
	private static ArrayList<String> FindShiftingAndConnections(ArrayList<String> operators, String[] sentanceSaved)
			throws IOException {
		ArrayList<String> returnList = new ArrayList<String>();
		Stack<String> senBuild = new Stack<String>();
		System.out.println();
		System.out.println("PARSING NOW");
		senBuild.push("ROOT(0)");

		int senInt = 1;
		for (int i = 0; i < operators.size(); i++)
		{

			if (operators.get(i).equals("Shift")) {
				if (i < sentanceSaved.length - 1) {
					System.out.print("Shifting " + sentanceSaved[senInt]);
					senBuild.push(sentanceSaved[senInt]);
					System.out.println();
				} else {
					System.out.print("Shifting " + sentanceSaved[senInt]);
					senBuild.push(sentanceSaved[senInt]);
					System.out.println();
				}
				senInt++;
			} 
			else
			{
				String[] currentSent = operators.get(i).split("_");
				String top = senBuild.pop();
				String topMinusOne = senBuild.pop();
				String command = currentSent[1];
				String connection = "";

				if (currentSent[0].equals("LeftArc")) {
					connection = "LeftArc_" + command + " to produce: " + top + " -- " + command + " --> "
							+ topMinusOne;
					returnList.add(top + " -- " + command + " --> " + topMinusOne);
					System.out.println(connection);
					senBuild.push(top);
				}
				if (currentSent[0].equals("RightArc")) {
					connection = "RightArc_" + command + " to produce: " + topMinusOne + " -- " + command + " --> "
							+ top;
					returnList.add(topMinusOne + " -- " + command + " --> " + top);
					System.out.println(connection);
					senBuild.push(topMinusOne);
				}
			}
		}
		senInt = 0;
		return returnList;
	}
}
