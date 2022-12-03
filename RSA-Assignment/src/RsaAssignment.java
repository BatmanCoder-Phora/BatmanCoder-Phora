import java.math.BigInteger;
import java.util.Random;

/**
 * This is the RSA based extra cerdit assignment diven at the end of the Spring 2022- algorithm symester.
 * 
 * @author Sephora Bateman
 *
 *	NOTE: This implmenation(as of right now) has up to STEP two working. 
 *	 - UPDATE: Steps one-three should be working.
 */
public class RsaAssignment {
	
	public static BigInteger p;
	public static BigInteger q;
	public static BigInteger n;
	public static BigInteger phi;
	public static BigInteger e;
	public static BigInteger d;


	public static int[] size;
	
	/**
	 * The entry point for my RSA extra cerdit assignment.
	 * @param args
	 */
    public static void main(String[] args)
    { 
    	/**************/
    	/** STEP ONE **/
    	/*************/
    	System.out.println("ENTERING STEP ONE...");
    	String message = "My RSA extra credit messages! ";
    	int[] sizes = new int[message.length()];
    	
    	// Encrypt The message
    	System.out.println("Starting Message: " + message);
    	BigInteger number = EncodeMessageSingleNumber(message,sizes);
    	System.out.println("Encoded Number: " + number);
    	
    	// Decrypt the message
    	String returnedMessage = DecodeMessageSingleNumber(number,sizes);
    	System.out.println("Decoded Message: "+ returnedMessage);
    	
    	// see if the messages match.
    	CheckIfSTEPPassed(returnedMessage,message,"STEP ONE");
    	System.out.println();
    	
    	
    	/**************/
    	/** STEP TWO **/
    	/**************/
    	System.out.println("ENTERING STEP TWO...");
    	AssignRSAVariables(message.length()*4); 
    	System.out.println();
    	
    	/**************/
    	/**STEP THREE**/
    	/**************/
    	System.out.println("ENTERING STEP THREE...");
    	// E-Key first 
    	String rasMessage = "Rsa STEP THREE message TEST";
        BigInteger cipherE = RASEncryption(rasMessage,e,"E-key");
    	String decryptedStringE = RSADecryption(cipherE,d,"E-key");	
     	CheckIfSTEPPassed(decryptedStringE,rasMessage,"STEP THREE E-KEY FIRST");
     	// D-Key first
     	BigInteger cipherD = RASEncryption(rasMessage,d,"D-key");
    	String decryptedStringD = RSADecryption(cipherD,e,"D-key");	
     	CheckIfSTEPPassed(decryptedStringD,rasMessage,"STEP THREE D-KEY FIRST");
    	System.out.println();
    }
    /**
     * This sees if the decxrypted message matches the message passed in. If the don't 
     * the STEP fails. If they do the STEP passes. 
     * @param returnedMessage: The decrypted message
     * @param message: The orginal message
     * @param string: The STEP you are at. 
     */
    private static void CheckIfSTEPPassed(String returnedMessage, String message, String string)
    {
    	if(returnedMessage.equals(message) == true)
    		System.out.println(string + " PASSED!");
    	else
    		System.out.println(string + " FAILED!");
		
	}
    /**
     * This is the RSADecrpytion(STEP THREE) 
     * @param cipher: The cipher is the thing being passed in. 
     * @param d2: The dKey 
     * @param string: The Key we are on.  
     * @return: A decrypted message.
     */
	private static String RSADecryption(BigInteger cipher, BigInteger key, String string) {
		// Decryption: meassge = (cyphier)^d mod n;
		String message = "";
		BigInteger decrypt = cipher.modPow(key, n);
		System.out.println("Decoded Message For "+ string + " first : " + decrypt);
		
		String decryptString = decrypt.toString();
		int first = 0;
		int last = 0;
		for(int i = 0 ; last  < decryptString.length(); i++)
		{
			last += size[i];
			if(last > decryptString.length())
			{
				break;
			}
			char tempchar = (char) Integer.parseInt(decryptString.substring(first, last));
			if(Character.isLowerCase(tempchar)||Character.isUpperCase(tempchar)||tempchar == ' ')
			{
				message += tempchar;
				first = last;
			}
			else
			{
				first = last;
			}
		}
		System.out.println("Decoded Text For "+ string + " first : " + message);
		return message;
	}
    /**
     * This is the RSAEncrpytion(STEP THREE) 
     * @param message: The orginal message
     * @param e2:The key being passedIn
     * @param string: The key we are using
     * @return: The decrypted output. 
     */
	private static BigInteger RASEncryption(String message, BigInteger key, String string)
	{
		System.out.println("Orignal Text For "+ string + " first : " + message);		
		size = new int[message.length()];
        // Encryption: cyphier = (message)^e mod n;

		BigInteger encoded = EncodeMessageSingleNumber(message,size);
		System.out.println("Orignal Message For "+ string + " first : " + encoded);
		// encrpyt the whole message number. 
		BigInteger encrypt = encoded.modPow(key, n);
		
		System.out.println("Encoded Cypher For "+ string + " first : " + encrypt);
		
		return encrypt;
	}

	/**
     * This is the decrpytion part of step one. 
     * @param sizes: An array that keeps track of my ascii character sizes.
     * @param number: the number receieved from the encrpytion part of step one
     * @return A string contained a decoded message.
     * 
     * @implNote: Descibed decrpytion.(STEP ONE)
     * -  I take the number I received from the encryption function and I spilt it. 
     *    Then beacuse I know that every letter character has two numbers in it(beacuse
     *    of however I did my encryption method) I grab two numbers and convert it into
     *    a letter character and add it to my answer string.
     * - I use an array to keep track of what size the ascii vlues were.
     */
	private static String DecodeMessageSingleNumber(BigInteger number, int[] sizes)
	{	
		String answer = "";
		 String temp = number.toString();
		 
		int first = 0;
		int last = 0;
		for(int i = 0 ; i < sizes.length; i++)
	    {
			last += sizes[i];
			String one = temp.substring(first,last);
			int a = Integer.parseInt(one);
			answer += (char)a;
			first = last;
	    }  
		return answer;
	}
    /**
     * This is the Encrpytion part of step one. 
     * @param sizes: An array that keeps track of my ascii character sizes.
     * @param message: The message that is encrypted.
     * @return: a BigInteger number 
     * 
     * @implNote: Descibed encrpytion(STEP ONE)
     * 	- 	I choose to base my encrption based on the ascii values of each character letter. 
     *   	I loop through the input message and grab each Letter and convert it to ascii. 
     *   	As I loop I concate the converted ascii number together and when I reach the end
     *   	of the message I convert that new number into a BigInteger number and return it.  
     *  -   I use an array to keep track of what size the ascii vlues are.
     */
	private static BigInteger EncodeMessageSingleNumber(String message, int[] sizes)
	{
		//SetsBigIntegerArray();
	    BigInteger answer;
	    char[] messageArray = message.toCharArray();
	    String part = "";
	    	
	    for(int i = 0 ; i < messageArray.length;i++)
	    {
	    	int ascii = (int)(messageArray[i]);
	    	String temp = Integer.toString(ascii);
	    	sizes[i] = temp.length();
    	    part += temp;
	    }
	    answer = new BigInteger(part);
		return answer;
	}


	/**
	 * This function generates all the need varaiables to carry out a RSA encrpytion and decrption.
	 * 
	 * @param Limit: The sizes of the buytes allowed for the prime.
	 */
	public static void AssignRSAVariables(int limit)
    {
		try {
    		Random r = new Random();
        //1. Choose primes p and q n = pq
    	     p = BigInteger.probablePrime(limit, r);
    	     q = BigInteger.probablePrime(limit, r);
    	     n = q.multiply(p);
    	     System.out.println("p: " + p);
    	     System.out.println("q: " + q);
    	     System.out.println("n: " + n);
    	// 2. Compute phi = (p-1)(q-1);
    	     BigInteger qM = q.subtract(BigInteger.valueOf(1));
    	     BigInteger pM = p.subtract(BigInteger.valueOf(1));
    	     phi = pM.multiply(qM);
    	     System.out.println("phi: " + phi);
        // 3. Choose 0 < e < phi co prime with phi GCD(e,phi) = 1;    	         	  
    	     boolean keepLooping = true;
    	     while (keepLooping)
    	     {
    	    	 e = BigInteger.probablePrime(limit, r);
    	    	 if(e.gcd(phi).compareTo(BigInteger.valueOf(1))== 0 && e.compareTo(phi)!= 1)
    	    		 keepLooping = false;
    	     }
    	     System.out.println("e: " + e);
        // 4. Calculate d = e^-1 mod phi || de 1mod Phi.    	  
    	     d = e.modInverse(phi);
    	     System.out.println("d: " + d);
    	     
    	     System.out.println("STEP TWO COMPLETE!");
		}
		catch(Exception e)
		{
			e.printStackTrace();
		}
    }
}
