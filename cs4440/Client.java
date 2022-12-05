import javax.crypto.Cipher;
import javax.crypto.spec.IvParameterSpec;
import javax.crypto.spec.SecretKeySpec;
import java.security.*;
import java.security.spec.PKCS8EncodedKeySpec;
import java.security.spec.X509EncodedKeySpec;
import java.util.Base64;
import java.net.Socket;
import java.io.ByteArrayOutputStream;
import java.io.BufferedReader;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;

/*
 * CS 4440 
 * Assignment Two: Secure communication between server and client 
 * 
 * Author: Professor Xu 
 * Co-Author: Sephora Bateman
 * 
 *  This class is the client end of the server/client secure commincation assignment. This class receives and sends a series
 *  of encrypted and decrypted messages to and from the server. 
 *  
 *  The port for this commincation is set perviously to the connection. 
 *  
 *  Testing: working in eclispe, havn't tested on the command line yet.
 *  
 *  Submission 3/20
 */

public class Client {
    private static final int PORT = 1337;
    private static final String SRVPUBKEY = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCDAZzhQdUIHkWLtDIe0rXONPYAwGY8WxiOqc7DAfJL/xoXQkYG0zep766kqkCHFOzuu5EKU2g03QbbWINgxGt6t2LM6ZyqoRJhM7g3mLxZ4TsH5hwElc6eq/KHGRuPE/f/eOmBWAVOVLgKdpHDZGzdA7MZjuvjYEgRhISr3/YKnQIDAQAB";

    //open commincation channel between the server and the client 
    private static BufferedReader socketIn;
	private static DataOutputStream socketOut;	
	private static Socket clientsocket;	
	
    public static void main(String[] args) throws IOException {

    try
      {
    	/*2.generate a pair of RSA public+private keys. RSAUtils.java contains an API (getKeyPair) for that.*/
        KeyPair keyPair = RSAUtils.getKeyPair();
        String privateKey = new String(Base64.getEncoder().encode(keyPair.getPrivate().getEncoded()), "UTF-8");
        String publicKey = new String(Base64.getEncoder().encode(keyPair.getPublic().getEncoded()), "UTF-8");
     
        
        /*3.The client will open a socket trying to connect to the server.*/
        clientsocket = new Socket("localhost",PORT);
		socketIn = new BufferedReader(new InputStreamReader(clientsocket.getInputStream()));
		socketOut = new DataOutputStream(clientsocket.getOutputStream());
		
			//not sure if this is needed. Is system.out.println the correct way to do it?
		System.out.println("Got Client's connetion: " + clientsocket.getRemoteSocketAddress().toString());
		System.out.println( "====RSA====");
		System.out.println("Private key's length: " + privateKey.length());
	    System.out.println("Public key's length: " + publicKey.length());
		
        /*4.Once connected to the server, the client will send the first message to the server*/
        String sign = RSAUtils.sign(publicKey, RSAUtils.getPrivateKey(privateKey));
		System.out.println("Signature: " + sign);
		System.out.println("[+] Client's sending public_key + signature");
		socketOut.writeBytes(sign + "\n" +publicKey + "\n");
        
        /*5.The server will send a message back to the client, which the client should receive*/
        String encryptedAES = socketIn.readLine().trim();
        System.out.println("Encrypted AES key: " + encryptedAES);
        
        String line = socketIn.readLine().trim(); // spilt the second line to get the randomstring and clientIP 
        String[] spilt = line.split(" ");
        String clientRandString = spilt[0];
        String clientIP = spilt[1];
        
        String signAESRandString = socketIn.readLine().trim();
        

        //6.
         /*(i) verify the signature of the encrypted AES key */
        
        // THIS IS THE PART THAT IS CURRENTLY FAILING
        boolean result = RSAUtils.verify(encryptedAES+clientRandString, RSAUtils.getPublicKey(SRVPUBKEY), signAESRandString);
        if(result == false)
         {
        	System.out.println("Server signature for the encrypted AES is not valid");
        	endconnection();
        	return;
         } 
        
        System.out.println("Server signature is valid");

        /*(ii) decrypt the encrypted AES key to get the original AES key */
        String decryptAES = RSAUtils.decryptByPrivateKey(encryptedAES, RSAUtils.getPrivateKey(privateKey));
        System.out.println("[+] Client received Server's AES key: " + decryptAES); 
        System.out.println("[+] Client received Server's Random String: " + clientRandString);
           
        /*(iii) encrypt the random string with the original AES key */
        String enString = RSAUtils.aesEncrypt(clientRandString, decryptAES);
        System.out.println("[+] enString: " + enString); 
        System.out.println("[+] Client IP from server: " + clientIP); 
        String psign = RSAUtils.sign(enString, RSAUtils.getPrivateKey(privateKey)); 
        
        /*(iv) send a message which contains the encrypted random string as well as its signature back to the server */
        socketOut.writeBytes(psign + "\n" + enString + "\n");
        
        /*7. Once receiving the message from the client, the server will send an OK message back*/
        String Ok = socketIn.readLine().trim();
        String signOfOk = socketIn.readLine().trim();
        System.out.println("[+] Received server's sig: " + signOfOk);
        System.out.println("[+] Received server's message: " + Ok);
        
        
        /*8. Once receiving the message from the server, the client will verify the signature; if the signature is OK, close the connection. */
        boolean resultsecond = RSAUtils.verify(Ok, RSAUtils.getPublicKey(SRVPUBKEY), signOfOk); 
        
        if(resultsecond != true) //verified is ok close connection
    	   System.out.println("[+] fail. OK message was not received from server");    
        else  
        	System.out.println("[+] Pass.");
       
         endconnection();
        }
        catch(Exception e)
        {
        	e.printStackTrace();
        }
        
    }
    /*
     * End the connection between the server and the client
     */
    public static void endconnection() throws IOException
    {
    	clientsocket.close();
    	socketIn.close();
    	socketOut.close();
    }
}

