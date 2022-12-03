package Programmin_Assignment_3;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Hashtable;
import java.util.List;
import java.util.Map;
import java.util.Scanner;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class lesk
{
	public static FileWriter leakWriter;
    /**
     * The entry point into my class. 
     * @param args
     * @throws IOException
     */
	public static void main(String[] args) throws IOException
	{
		// Grab the files
		File testFile = new File(args[0]);
		File definitionsFile = new File(args[1]);
		File stopwordsFile = new File(args[2]);
		
		File outputFile = new File(testFile+".lesk");
		leakWriter = new FileWriter(outputFile);
		
		ArrayList<String> stopWords = StoreStopWords(stopwordsFile);
		Map<String,List<String>> definitions = GetInfoFromDefFile(definitionsFile);
		 
	    LeakAlgorithmTwo(testFile,definitions,stopWords);	
		leakWriter.flush();
		leakWriter.close();

	}
	/**
	 * This is thhe base of my algorithm it runs through every occurances and print the conext for 
	 * each word definition. 
	 * @param testFile: The file of ocurrances
	 * @param definitions: The different definitions. 
	 * @param stopWords: The words we do not counte in matching.
	 * @throws IOException
	 */
	private static void LeakAlgorithmTwo(File testFile, Map<String, List<String>> definitions, ArrayList<String> stopWords) throws IOException {
		Scanner scan = new Scanner(testFile);
		Map<Integer,List<String>> values = new Hashtable<Integer,List<String>>() ;
		while(scan.hasNextLine())
		{
		    List<String> currentSentance = Arrays.asList(scan.nextLine().split(" "));
			int test = 0;
			for(String defWord : definitions.keySet())
			{
				List<String> def = definitions.get(defWord);
				for(String word : def)
				{
					if(currentSentance.contains(word) && !stopWords.contains(word)&& NotAlphabetical(word))
						test++;					
				}
				if(values.get(test) == null)
				{
					List<String> temp = new ArrayList<String>();
					temp.add(defWord);
					values.put(test, temp);
				}
				else
				{
					List<String> temp = values.get(test);
					temp.add(defWord);
					values.put(test, temp);
				}
				test = 0;
			}
			PrintContextNumbers(values);
			values = new Hashtable<Integer,List<String>>();
			leakWriter.append("\n");
		}
		scan.close();
	}
	/**
	 * This tells me when a word does not have any letters in it.
	 * @param word: The word that is being tested. 
	 * @return
	 */
	private static boolean NotAlphabetical(String word) {
		Pattern r = Pattern.compile("^[a-zA-Z]+");
	      Matcher m = r.matcher(word);
         if(m.find()== true)
			return true;
		return false;
	}
	/**
	 * This prints the map of the context values in order. 
	 * @param values: The values gotten from see what macthes. 
	 * @throws IOException
	 */
	private static void PrintContextNumbers(Map<Integer, List<String>> values) throws IOException 
	{
		String output = "";
		for(Integer count : values.keySet())
		{
			List<String> tempList = values.get(count);
			Collections.sort(tempList);
			for(String word : tempList)
			{
				output += word +"(" + count + ") ";
			}
			
		}
		leakWriter.append(output.substring(0, output.length()-1));
	}

	/**
	 * The method stores each defenition of the context to see if there is a connection.
	 * @param definitionsFile: The file containing the definitions. 
	 * @return: A map of the words in the definitions to the word. 
	 * @throws FileNotFoundException
	 */
	private static Map<String, List<String>> GetInfoFromDefFile(File definitionsFile) throws FileNotFoundException {
		Scanner scan = new Scanner(definitionsFile);
		Map<String, List<String>> def = new Hashtable<String, List<String>>();
		while(scan.hasNextLine())
		{
			List<String> merge = new ArrayList<String>();
			
		    String sentance = scan.nextLine();
		    String[]sentancespilt  = sentance.split("\t");
			String word = sentancespilt[0];
			List<String> temp = Arrays.asList(sentancespilt[1].split(" "));		 
		    List<String> secTemp = Arrays.asList(sentancespilt[2].split(" "));	

		   
		    merge.addAll(temp);
		    merge.addAll(secTemp);
		    
		    def.put(word, merge);
		}
		scan.close();
		return def;

	}
	
/**
    * Looks through the stopWordFile and stores all the stop words in a list
    * @param stopwordsFile: The file with the stored words. 
    * @return: A list of the stored words
    * @throws FileNotFoundException
    */
	private static ArrayList<String> StoreStopWords(File stopwordsFile) throws FileNotFoundException {
		Scanner scan = new Scanner(stopwordsFile);
		ArrayList<String> words = new ArrayList<String>();
		while(scan.hasNextLine())
		{
			String word = scan.nextLine();
			if(words.contains(word)!= true)
			   words.add(word);
		}
		scan.close();
		return words;
	}
}
