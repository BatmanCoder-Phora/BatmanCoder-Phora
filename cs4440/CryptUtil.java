
import java.io.*;
import java.util.Random;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.security.MessageDigest;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Map;

/**
 * @author Professor Xu
 * @co-author Sephora Bateman
 * Description: 
 *  This class encrypt and decrypt an 8 byte block of information based on a randomized 8 bit key.
 *  The Encryption is a Feistel style encryption with four rounds, the function the 
 *  right side is being ran through is a simple byte addition(shifting). 
 *  The Decryption is styled just like the encryption. 
 *  The padding for the blocks is roughly styled after the PKCS #5/PKCS #7 padding strutcure. 
 *  
 *  References:
 *   Professor Xu
 *   TA Ella Moskun(helped me with the padding)
 *  
 * Testing(On test document):
 *    Correctness = Matching 
 *    Strength = 7.0 - 7.5(depending on key)
 */

public class CryptUtil {
    public static byte[] createSha1(File file) throws Exception {
        MessageDigest digest = MessageDigest.getInstance("SHA-1");
        InputStream fis = new FileInputStream(file);
        int n = 0;
        byte[] buffer = new byte[8192];
        while (n != -1) {
            n = fis.read(buffer);
            if (n > 0) {
                digest.update(buffer, 0, n);
            }
        }
        fis.close();
        return digest.digest();
    }

    public static boolean compareSha1(String filename1, String filename2) throws Exception {
        File file1 = new File(filename1);
        File file2 = new File(filename2);
        byte[] fsha1 = CryptUtil.createSha1(file1);
        byte[] fsha2 = CryptUtil.createSha1(file2);
        return Arrays.equals(fsha1, fsha2);
    }

    public static double getShannonEntropy(String s) {
        int n = 0;
        Map<Character, Integer> occ = new HashMap<>();

        for (int c_ = 0; c_ < s.length(); ++c_) {
            char cx = s.charAt(c_);
            if (occ.containsKey(cx)) {
                occ.put(cx, occ.get(cx) + 1);
            } else {
                occ.put(cx, 1);
            }
            ++n;
        }

        double e = 0.0;
        for (Map.Entry<Character, Integer> entry : occ.entrySet()) {
            char cx = entry.getKey();
            double p = (double) entry.getValue() / n;
            e += p * log2(p);
        }
        return -e;
    }

    public static double getShannonEntropy(byte[] data) {

        if (data == null || data.length == 0) {
            return 0.0;
        }

        int n = 0;
        Map<Byte, Integer> occ = new HashMap<>();

        for (int c_ = 0; c_ < data.length; ++c_) {
            byte cx = data[c_];
            if (occ.containsKey(cx)) {
                occ.put(cx, occ.get(cx) + 1);
            } else {
                occ.put(cx, 1);
            }
            ++n;
        }

        double e = 0.0;
        for (Map.Entry<Byte, Integer> entry : occ.entrySet()) {
            byte cx = entry.getKey();
            double p = (double) entry.getValue() / n;
            e += p * log2(p);
        }
        return -e;
    }

    public static double getFileShannonEntropy(String filePath) {
        try {
            byte[] content;
            content = Files.readAllBytes(Paths.get(filePath));
            return CryptUtil.getShannonEntropy(content);
        } catch (IOException e) {
            e.printStackTrace();
            return -1;
        }

    }

    private static double log2(double a) {
        return Math.log(a) / Math.log(2);
    }

    public static void doCopy(InputStream is, OutputStream os) throws IOException {
        byte[] bytes = new byte[64];
        int numBytes;
        while ((numBytes = is.read(bytes)) != -1) {
            os.write(bytes, 0, numBytes);
        }
        os.flush();
        os.close();
        is.close();
    }

    public static Byte randomKey() {
        int leftLimit = 48; // numeral '0'
        int rightLimit = 122; // letter 'z'
        int targetStringLength = 8;
        Random random = new Random();
        String generatedString = random.ints(leftLimit, rightLimit + 1)
                .filter(i -> (i <= 57 || i >= 65) && (i <= 90 || i >= 97))
                .limit(targetStringLength)
                .collect(StringBuilder::new, StringBuilder::appendCodePoint, StringBuilder::append)
                .toString();
        return generatedString.getBytes()[0];
    }

    /**
     * Encryption (Bytes)
     *
     * @param data
     * @param key
     * @return encrypted bytes
     * @throws IOException 
     */
    public static byte[] cs4440Encrypt(byte[] data, Byte key) throws IOException {
    byte[] cipherdata = new byte[8];

    // Do encryption.
    byte[] L1,R2,R1,L2,E;
    R1 = new byte[data.length/2];
    R2 = new byte[data.length/2];
    L1 = new byte[data.length/2];
    L2 = new byte[data.length/2];    
    E = new byte[data.length/2];
    
    L1 = Arrays.copyOfRange(data,0, data.length/2);
    R1 = Arrays.copyOfRange(data, data.length/2, data.length);
    
    //round one
    E = decryptEncryptHelper(R1,key);
    L2 = R1;
    for(int i = 0; i < R1.length; i++)
    	R2[i] = (byte) (L1[i]^E[i]);
    
    // round two 
    byte[] E2= new byte[data.length/2];
    byte[] L3 = new byte[data.length/2];
    byte[] R3 = new byte[data.length/2];
    
    E2 = decryptEncryptHelper(R2,(key));
    L3 = R2;
    for(int j = 0; j < R3.length; j++)
    	R3[j] = (byte) (L2[j]^E2[j]);
    
    // round three
    byte[] E3= new byte[data.length/2];
    byte[] L4 = new byte[data.length/2];
    byte[] R4 = new byte[data.length/2];
    E3 = decryptEncryptHelper(R3,(byte)(key));
    L4 = R3;
    for(int j = 0; j < R3.length; j++)
    	R4[j] = (byte) (L3[j]^E3[j]);
 // round four
    byte[] E4 = new byte[data.length/2];
    byte[] L5 = new byte[data.length/2];
    byte[] R5 = new byte[data.length/2];
    E4 = decryptEncryptHelper(R4,(byte)(key));
    L5 = R4;
    for(int j = 0; j < R3.length; j++)
    	R5[j] = (byte) (L4[j]^E4[j]);

    // merge the two halves back together and store them in cipherdata. 
    ByteArrayOutputStream outputStream = new ByteArrayOutputStream( );
    outputStream.write( R5 );
    outputStream.write( L5 );
    cipherdata = outputStream.toByteArray();
    return cipherdata;
    }
   /**
    * A helper method for my encryption and decryption methods. 
    * @param r1
    * @param key
    * @return
    */
    private static byte[] decryptEncryptHelper(byte[] r1, byte key) {
	   byte[] output = new byte[r1.length];
		for(int i = 0; i < r1.length; i++)
			output[i] = (byte) ((int)r1[i]+(int)key);
		return output;
	}


	/**
     * Encryption (file)
     *
     * @param plainfilepath
     * @param cipherfilepath
     * @param key
     */
    public static int encryptDoc(String plainfilepath, String cipherfilepath, Byte key) {
    	 try
         {
    		 // Grab the two file paths 
             InputStream input = new FileInputStream(plainfilepath);
             OutputStream output = new FileOutputStream(cipherfilepath);
             // create the block and the padding for the block 
             byte[] block = new byte[8];
             byte[] text = input.readAllBytes();
             int numBlocks = text.length/8 +1;
             byte[] padded = new byte[numBlocks * 8];
             System.arraycopy(text, 0, padded, 0, text.length);
             byte pad = (byte)(padded.length - text.length);
             // add the padding to the padded array.
             for (int i = text.length; i < padded.length; i++)
                 padded[i] = (byte) pad;
             // add the padding to the block and write block to ciphertext
             for (int i = 0; i < padded.length; i += 8)
             {
                 block = Arrays.copyOfRange(padded, i, i + 8);
                 output.write(cs4440Encrypt(block, key));
             }
             // close input and output streams 
             input.close();
             output.close();
             return 0;
         } 
         catch (Exception e) {
             return -1;
         }
    }

    /**
     * decryption
     *
     * @param data
     * @param key
     * @return decrypted content
     * @throws IOException 
     */

    public static byte[] cs4440Decrypt(byte[] data, Byte key) throws IOException {
        // TODO
	byte[] plaindata = new byte[8];
	 // do decryption.
    byte[] L1,R2,R1,L2,E;
    R1 = new byte[data.length/2];
    R2 = new byte[data.length/2];
    L1 = new byte[data.length/2];
    L2 = new byte[data.length/2];    
    E = new byte[data.length/2];
    
    L1 = Arrays.copyOfRange(data,0, data.length/2);
    R1 = Arrays.copyOfRange(data, data.length/2, data.length);
    
    //round one
    E = decryptEncryptHelper(R1,(byte)key);
    L2 = R1;
    for(int i = 0; i < R1.length; i++)
    	R2[i] = (byte) (L1[i]^E[i]);
    
    // round two 
    byte[] E2= new byte[data.length/2];
    byte[] L3 = new byte[data.length/2];
    byte[] R3 = new byte[data.length/2];
    E2 = decryptEncryptHelper(R2,(byte)(key));
    L3 = R2;
    for(int j = 0; j < R3.length; j++)
    	R3[j] = (byte) (L2[j]^E2[j]);
 // round three
    byte[] E3= new byte[data.length/2];
    byte[] L4 = new byte[data.length/2];
    byte[] R4 = new byte[data.length/2];
    E3 = decryptEncryptHelper(R3,(byte)(key));
    L4 = R3;
    for(int j = 0; j < R3.length; j++)
    	R4[j] = (byte) (L3[j]^E3[j]);
    // round four
    byte[] E4 = new byte[data.length/2];
    byte[] L5 = new byte[data.length/2];
    byte[] R5 = new byte[data.length/2];
    E4 = decryptEncryptHelper(R4,(byte)(key));
    L5 = R4;
    for(int j = 0; j < R3.length; j++)
    	R5[j] = (byte) (L4[j]^E4[j]);
    
    ByteArrayOutputStream outputStream = new ByteArrayOutputStream( );
    outputStream.write( R5 );
    outputStream.write( L5 );
    plaindata = outputStream.toByteArray();
	return plaindata;
    }

    /**
     * Decryption (file)
     * @param plainfilepath
     * @param cipherfilepath
     * @param key
     */
    public static int decryptDoc(String cipherfilepath, String plainfilepath, Byte key) {
    	  try
          {         
              InputStream input = new FileInputStream(cipherfilepath);
              OutputStream output = new FileOutputStream(plainfilepath);
              byte[] ciphertext = input.readAllBytes();
              byte[] block = new byte[8];
              // for every block before the last block write it to the plain text
            for (int i = 0; i < ciphertext.length - 8; i += 8)
            {
                block = Arrays.copyOfRange(ciphertext, i, i + 8);
                output.write(cs4440Decrypt(block, key));
            }
 // grab the last block decrypt it, find the padding and then write the bytes in the block without the pad. 
            block = Arrays.copyOfRange(ciphertext, ciphertext.length - 8, ciphertext.length);
            block = cs4440Decrypt(block, key);
            byte pad = block[block.length - 1];
            for (int i = 0; i < 8 - pad; i++)
                output.write(block[i]);  
              
            output.close();
            input.close();
              return 0;
          } 
          catch (Exception e) 
          {
              return -1;
          }
    }

    public static void main(String[] args) {

        String targetFilepath = "";
        String encFilepath = "";
        String decFilepath = "";
        if (args.length == 3) {
            try {
                File file1 = new File(args[0].toString());
                if (file1.exists() && !file1.isDirectory()) {
                    targetFilepath = args[0].toString();
                } else {
                    System.out.println("File does not exist!");
                    System.exit(1);
                }

                encFilepath = args[1].toString();
                decFilepath = args[2].toString();
            } catch (Exception e) {
                e.printStackTrace();
                System.exit(1);
            }
        } else {
            //targetFilepath = "cs4440-a1-testcase1.html";
            System.out.println("Usage: java CryptoUtil file_to_be_encrypted encrypted_file decrypted_file");
            System.exit(1); 
        } 

        Byte key = randomKey();
        String src = "ABCDEFGH";
        System.out.println("[*] Now testing plain sample： " + src);
        try {
            byte[] encrypted = CryptUtil.cs4440Encrypt(src.getBytes(), key);
            StringBuilder encsb = new StringBuilder();
            for (byte b : encrypted) {
                encsb.append(String.format("%02X ", b));
            }
            System.out.println("[*] The  encrypted sample  [Byte Format]： " + encsb);
            double entropyStr = CryptUtil.getShannonEntropy(encrypted.toString());
            System.out.printf("[*] Shannon entropy of the text sample (to String): %.12f%n", entropyStr);
            double entropyBytes = CryptUtil.getShannonEntropy(encrypted);
            System.out.printf("[*] Shannon entropy of encrypted message (Bytes): %.12f%n", entropyBytes);

            byte[] decrypted = CryptUtil.cs4440Decrypt(encrypted, key);
	    if (Arrays.equals(decrypted, src.getBytes())){
                System.out.println("[+] It works!  decrypted ： " + decrypted);
            } else {
                System.out.println("Decrypted message does not match!");
            }

            // File Encryption
            System.out.printf("[*] Encrypting target file: %s \n", targetFilepath);
            System.out.printf("[*] The encrypted file will be: %s \n", encFilepath);
            System.out.printf("[*] The decrypted file will be: %s \n", decFilepath);

            CryptUtil.encryptDoc(targetFilepath, encFilepath, key);
            CryptUtil.decryptDoc(encFilepath, decFilepath, key);

            System.out.printf("[+] [File] Entropy of the original file: %s \n",
                    CryptUtil.getFileShannonEntropy(targetFilepath));
            System.out.printf("[+] [File] Entropy of encrypted file: %s \n",
                    CryptUtil.getFileShannonEntropy(encFilepath));

            if (CryptUtil.compareSha1(targetFilepath, decFilepath)) {
                System.out.println("[+] The decrypted file is the same as the source file");
            } else {
                System.out.println("[+] The decrypted file is different from the source file.");
                System.out.println("[+] $ cat '<decrypted file>' to to check the differences");
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
