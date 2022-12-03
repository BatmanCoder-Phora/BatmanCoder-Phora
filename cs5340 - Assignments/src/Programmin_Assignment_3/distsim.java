package Programmin_Assignment_3;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.io.IOException;
import java.text.DecimalFormat;
import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Hashtable;
import java.util.Map;
import java.util.Scanner;
import java.util.TreeMap;
import java.util.regex.Matcher;
import java.util.regex.Pattern;


public class distsim
{
	//variables to keep track of counts.
	public static Integer trainSen = 0;
	public static Integer testSen = 0;
	public static Integer GoldSen = 0;
    public static String contextOcurr;
    
    //List to keep track of vocab lists. 
    public static ArrayList<String> distinctGoldSense;
    public static ArrayList<String> vocabList;
    public static Map<String,Integer>vocabListVector;
    //Variables to help round the printing
	public static FileWriter distsimWriter;
	private static final DecimalFormat df = new DecimalFormat("0.00");
	
	
    /**
     * The entry point into my class. 
     * @param args
     * @throws IOException
     */
	public static void main(String[] args) throws IOException
	{
		// grab the information from the commandLine
		File trainFile = new File(args[0]);
		File testFile = new File(args[1]);
		File stopwordsFile = new File(args[2]);
		int kValue = Integer.parseInt(args[3]);

		//Create output file.
		File outputFile = new File(testFile+".distsim");
		distsimWriter = new FileWriter(outputFile);
		
		vocabList = new ArrayList<String>();
		distinctGoldSense = new ArrayList<String>();
		
		ArrayList<String> stopWords = StoreStopWords(stopwordsFile);
		Map<String,ArrayList<String>> goldFile = ReadThroughGoldFile(trainFile,kValue,stopWords);		
		ArrayList<String> test = new ArrayList<String> (TrimVobList(stopWords));
		
		Collections.sort(test);

		
		vocabListVector = CreatVectorArray(test);
		ArrayList<String> output = distsimAlgorithm(stopWords,goldFile,testFile,kValue);
		
		//Starting Files.
		distsimWriter.append("Number of Training Sentences = " +trainSen +"\n");
		distsimWriter.append("Number of Test Sentences  = " + testSen +"\n");
		distsimWriter.append("Number of Gold Senses = " + testSen + "\n");
		distsimWriter.append("Vocabulary Size = " + test.size() + "\n");	
			
	   for(String sentance : output)
		{
			distsimWriter.append(sentance);
			distsimWriter.append("\n");
		} 
		
		distsimWriter.flush();
		distsimWriter.close();
	}
	/**
	 * This turns my vocabList into a vocab Map So frequency can be kept track of. 
	 * @param newVobList: the list that is being converted. 
	 * @return
	 */
	private static Map<String, Integer> CreatVectorArray(ArrayList<String> test) 
	{
		Map<String, Integer> temp = new HashMap<String, Integer>();
		for(String word :test )
		{
			temp.put(word, 0);
		}
		return temp;
	}
	/**
	 * Takes the vocab list we have be and trims it down even further. 
	 * @param stopWords 
	 * @return
	 */
	private static HashSet<String> TrimVobList(ArrayList<String> stopWords) {
		Map<String,Integer> vocb = FrequencyCount();
		HashSet<String> finalVocabSet = new HashSet<String>();
		for(String key : vocb.keySet())
		{
			if(vocb.get(key)> 1)
				if(WordMeetsQulaifications(key,stopWords))
					finalVocabSet.add(key);
		}
		return finalVocabSet;
	}
	/**
	 * This counts the Frequency of each word in my vocab. 
	 * @return
	 */
	private static Map<String, Integer> FrequencyCount() 
	{
	    Map<String, Integer> freqMap = new HashMap<>();
	    for(String key :vocabList)
	        freqMap.compute(key, (s1, count) -> count == null ? 1 : count + 1);
	    
		return freqMap;
	}
	
	/**
	 * This method finds the Signaturevector for the GoldSense.
	 * @param stopWords 
	 * @param key2: The GoldSense apassed into the function. 
	 * @param goldFile: The list of words and vocab. 
	 */
	private static Map<String,Integer> FindSignatureVector(String key, Map<String, ArrayList<String>> goldFile, ArrayList<String> stopWords) 
	{
        Map<String,Integer> localSigVector = new HashMap<String,Integer> (vocabListVector) ;
        ArrayList<String> lookUpValues = goldFile.get(key);
    	for(String values : lookUpValues)
    	{ 
    		if(vocabListVector.containsKey(values))
    		 {	
    			int intValue = localSigVector.get(values)+1;
    			localSigVector.put(values, intValue);
    		 }
    	}

		return localSigVector;
	}
	/**
	 * This is the main part of the algorithm that compute the cosine of the sigVector and the contextVector.
	 * @param kValue 
	 * @param stopWords: The Words we do not want to count.
	 * @param goldFile: The file we find the vectors goldSense.
	 * @param testFile: The file we get our test sentances from.
	 * @throws FileNotFoundException 
	 */
	private static ArrayList<String> distsimAlgorithm(ArrayList<String> stopWords, Map<String, ArrayList<String>> goldFile, File testFile, int kValue) throws FileNotFoundException {
		Scanner scan = new Scanner(testFile);
		TreeMap<Double,ArrayList<String>> results = new TreeMap<Double,ArrayList<String>>();
		ArrayList<String>  stringResult = new ArrayList<String>();
		
		while(scan.hasNextLine())
		{
			String currentSentance = scan.nextLine();
			testSen++;
			
			Map<String,Integer> contextVect = FindContextVector(currentSentance,kValue,stopWords);
			for(String key : distinctGoldSense)
			{
				Map<String,Integer> sigVector = FindSignatureVector(key,goldFile,stopWords);		
				double result = ComputeCosineSimilarity(key,contextVect,sigVector);
		
				ArrayList<String> temp = new ArrayList<String>();
				if(results.get(result)== null)
				{
					if(!temp.contains(key))
					   temp.add(key);
					results.put(result,temp);
				}
				else
				{
					temp = results.get(result);
					if(!temp.contains(key))
						temp.add(key);
					results.put(result,temp);
				}	
			}
			String output = SaveMapToString(results.descendingMap());
			stringResult.add(output);
			results = new TreeMap<Double,ArrayList<String>>();
		}	
		scan.close();
		return stringResult;
	}
	/**
	 * This adds the resulting map to a string so it can later be printed.
	 * @param results
	 * @return
	 */
	private static String SaveMapToString(Map<Double, ArrayList<String>> results) 
	{
	   String outputString = "";
	   for(Double key : results.keySet())		   
	   {
		   ArrayList<String> values = results.get(key);
		   Collections.sort(values);
		   for(String value :values)
		   {
			   String actualValue = value.split("_")[0];
			   outputString += actualValue + "("+df.format(key)+ ") "; 
		   }
	   }
	   return outputString;
	}

	/**
	 * This Computes the Cosine Similarity between the current sig Vector and the context vector. 
	 * @param key: The current gold Sense
	 * @param contextVect: The current contextvector
	 * @param sigVector: The current Signature vector
	 * @return: 
	 */
	private static double ComputeCosineSimilarity(String key, Map<String, Integer> contextVect,
			Map<String, Integer> sigVector) 
	{
		double dotProduct = 0;
		double powX = 0;
		double powY = 0;
		
		ArrayList<Integer> sigVectorList = new ArrayList<Integer>(sigVector.values());
		ArrayList<Integer> contextVectList = new ArrayList<Integer>(contextVect.values());
	    // ComputeCosineSimilarity between this context vector and this signature vector.
			for(int i = 0; i < sigVectorList.size(); i++)
			{
			int sigValue = sigVectorList.get(i);
			int contextValue = contextVectList.get(i);
			dotProduct += (sigValue*contextValue);
			powX += Math.pow(sigValue, 2);
			powY += Math.pow(contextValue, 2);
		}
		
		double sqrResult = (Math.sqrt(powX)) * (Math.sqrt(powY));
		if(sqrResult == 0)
			return 0.0;
		else
			return dotProduct /sqrResult;
	}
	/**
	 * This finds the contectVector of the currentSentance from the testfile.
	 * @param kValue 
	 * @param stopWords 
	 * @param currentSentance: The sentance that is being changed into a contectfile. 
	 */
	private static Map<String,Integer> FindContextVector(String currentSentance, int kValue, ArrayList<String> stopWords)
	{
		 Map<String,Integer> localcontextVector = new HashMap<String,Integer> (vocabListVector); // fix VocabVector
		 String[] spiltCurSen = currentSentance.split(" ");
		 int occurrenceLoc = 0;
		 String occurrence = "";
		 ArrayList<String> currentSenWords = new ArrayList<String>();
		 ArrayList<String> context = new ArrayList<String>();
			for(int i = 0; i < spiltCurSen.length;i++)
			{
				if(spiltCurSen[i].contains("<occurrence>"))
				{
					occurrenceLoc = i;
					String[] spiltoccurrenceLoc = spiltCurSen[i].split("[<>]");
					occurrence =spiltoccurrenceLoc[2];
					contextOcurr = occurrence;
				}
			   currentSenWords.add(spiltCurSen[i]);
			   if(kValue == 0&&!spiltCurSen[i].contains("<occurrence>"))
			   {
				   String currentWordLowered = toLowerCase(spiltCurSen[i]);
					if(WordMeetsQulaifications(currentWordLowered,stopWords))
					{
						vocabList.add(currentWordLowered);
						context.add(currentWordLowered);
					}
			   }
			}
			if(kValue != 0)
				context = CalculateKWindow(kValue,spiltCurSen,occurrenceLoc,currentSenWords,context,stopWords);
						
			for(String word : context)
			{
				if(word.substring(word.length()-1, word.length()).equals("."))
					word = word.substring(0,word.length()-1);
				if(localcontextVector.containsKey(word))
				{
					int value = localcontextVector.get(word) + 1;
					localcontextVector.put(word, value);
				}
			}
		 currentSenWords = new ArrayList<String>();
		 return localcontextVector;
	
	}

	/**
	 * This reads through the training file and gets all relvalent information from it. 
	 * @param kValue: The size of the sig and context window.  
	 * @param stopWords: A list of words we do not want to add to our list.
	 * @param trainFile: The trainfile we are pulling the information from. 
	 * @return
	 * @throws FileNotFoundException
	 */
	private static Map<String, ArrayList<String>> ReadThroughGoldFile(File trainFile, int kValue, ArrayList<String> stopWords) throws FileNotFoundException 
	{
	    //Infinate loop after adding things. 
		Scanner scan = new Scanner(trainFile);
		Map<String, ArrayList<String>> goldOccurances = new Hashtable<String, ArrayList<String>>();
	
		ArrayList<String> currentSenWords = new ArrayList<String>();
		ArrayList<String> tempVocab = new ArrayList<String>();

		int occurrenceLoc = 0;

		while(scan.hasNextLine())
		{
			trainSen++;
			
			String currentSentance = scan.nextLine();
			String[] spiltCurSen = currentSentance.split("\t");
			String def = spiltCurSen[0].substring(10,spiltCurSen[0].length());
			spiltCurSen = spiltCurSen[1].split(" ");

			
			for(int i = 0; i < spiltCurSen.length;i++)
			{
				if(spiltCurSen[i].contains("<occurrence>"))
					occurrenceLoc = i;
				
			   currentSenWords.add(spiltCurSen[i]);
			   if(kValue == 0&&!spiltCurSen[i].contains("<occurrence>"))
			   {
				   String currentWordLowered = toLowerCase(spiltCurSen[i]);
					if(WordMeetsQulaifications(currentWordLowered,stopWords))
					{
						vocabList.add(currentWordLowered);		   
						tempVocab.add(currentWordLowered);
					}
			   }
			}
			if(kValue != 0)
			{
				// Grab values of K on both sides
				tempVocab = CalculateKWindow(kValue,spiltCurSen,occurrenceLoc,currentSenWords,tempVocab,stopWords);
			}
			if(!distinctGoldSense.contains(def))
				distinctGoldSense.add(def);

			if( goldOccurances.get(def) == null)
				goldOccurances.put(def, tempVocab);
			else
			{
				ArrayList<String> merge = goldOccurances.get(def);
				merge.addAll(tempVocab);
				goldOccurances.put(def, merge);
			}
			tempVocab = new ArrayList<String>();
			currentSenWords = new ArrayList<String>(); 
		} 
		scan.close();
		return goldOccurances;
		
	}
	/**
	 * 
	 * @param kValue
	 * @param spiltCurSen
	 * @param occurrenceLoc
	 * @param currentSenWords
	 * @param tempVocab
	 * @param stopWords
	 * @return
	 */
	private static ArrayList<String> CalculateKWindow(int kValue, String[] spiltCurSen, int occurrenceLoc, ArrayList<String> currentSenWords, ArrayList<String> tempVocab, ArrayList<String> stopWords)
	{
		for(int i = 1 ;i<= kValue; i++)
		{
			if(occurrenceLoc + i < spiltCurSen.length)
			{
				String currentWord = currentSenWords.get(occurrenceLoc + i);
				String currentWordLowered = toLowerCase(currentWord);
				vocabList.add(currentWordLowered);
				tempVocab.add(currentWordLowered);
			}
			if(occurrenceLoc - i >= 0)	
			{
				String currentWord = currentSenWords.get(occurrenceLoc - i);	
				String currentWordLowered = toLowerCase(currentWord);				
				vocabList.add(currentWordLowered);
				tempVocab.add(currentWordLowered);
			}
		  }
		
		return tempVocab;
	}
	/**
	 * This method checks to see if the word is vaild to add to the list. 
	 * @param word: The word we are looking checking it's validity.
	 * @param temp: The tempary list we are checking from.
	 * @param stopWords: A list of words we do not want to add to our own.
	 * @return
	 */
	private static boolean WordMeetsQulaifications(String word, ArrayList<String> stopWords) 
	{
		  Pattern r = Pattern.compile(".*[a-zA-Z].*");
	      Matcher m = r.matcher(word);
		if(stopWords.contains(word))
			return false;
		else if(m.find()== false)
			return false;
		else
			return true;
	}
	/**
	 * This method converts to see if the capital of a word is inculded. 
	 * @param word: The word that is changed into a captial. 
	 * @return
	 */
	private static String toLowerCase(String word)
	{
		return word.toLowerCase();
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
