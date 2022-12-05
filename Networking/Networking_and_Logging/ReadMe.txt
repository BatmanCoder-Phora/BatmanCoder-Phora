Author:     Sephora Bateman 
Partner:    Corbin Gurnee
Date:       3/25/20
Course:     CS 3500, University of Utah, School of Computing
GitHub ID:  u1261969
Repo:       assignment-seven-logging-and-networking-the-batman
Commit #:   
Assignment: Assignment #7
Copyright:  CS 3500 and [Sephora Bateman and Corbin Gurnee ] - This work may not be copied for use in Academic Coursework.

1. Comments to Evaluators:
    Note: 
           Github URL: https://github.com/uofu-cs3500-spring20/assignment-seven-logging-and-networking-the-batman
           Github Commit # e11f467d22d434a3412dc66466db1f1d4994104a

    NOTE: In the Assignment Specific Topics the numbered solution to Issues exposed by the stress Test, matches with the numbered Issue.
          E.x: 1 goes to 1.

    NOTE: In our customFileLogger, we have a property called append that states if a file is new or append to. 
    
    NOTE: For are stress tests you can change the command line arguments for debugging with: Stress tests -> properties -> Debug -> 
          Application Arguments. Then put in with the one you want to test. E.x: server or local 3 or remote 2.

    IMPORTANT NOTE: Please read the comments above our stress tests there are some instructions with them.   

    IMPORTANT NOTE: Stress tests: Local Tests: id: 1,2,3,4 Remote Test: id: 5,6 .   


TO TEST: stress_Test_One: local 1 , stress_test_two_server_Local: local 2 , stress_test_2_client_local: local 3, 
stress_test_3_client: local 4, Stress_test_remote_client: remote 5. Stress_test_5_client: remote 6.

2. Assignment Specific Topics

    2a.FIXES

     A. Issues exposed by the Stress Tests 
             1. Protection level.
             2. Send Message did not allow for a string input. 
             3. In the stress test 2 client, all clients failed, the port was always -1. 
             5. StartServer can not work without server input.

     B. Solutions to Issues exposed by the Stress Tests 
             1. Change the protection level of each class to public. Also added another send message method into server 
             so an argument could be taken.
             Also, change the protection level of connect to server in client to be able to be accessed by my stress tests. 
             2. Added a second sendMessage to the class and had it take in a string. 
             3. added a second constructor to the client class, to set port to 11000. 
             5. Created another StartServer to work without server input. (startServerNoUser)

     C. Where we placed the Log messages and at what level to record the Information.
     In Server:

            LOG INFORMATION: The server has started, The client has either connected or disconnected. How long a message is and which
            client it was sent to. The server is closing. 

            LOG ERROR: When a client fails to connect after it began, Tried to connect fails before it even started, An error occurred 
            while sending information. 

     In Client:

            LOG INFORMATION:  We have a client who has been created a message, another two saying that a client has connected or 
            disconnected for the server. One more saying that a message was received. 

            LOG ERROR: When a client fails to connect to the server When a client tries to finalize the connection with EndConnect 
            When the client fails to receive the information. 
     Both: 
            LOG WARNING: Warns that no number was put in for port.

            LOG CRITICAL: if server.startSever failed.

3. Consulted Peers:

TA's
Pro. Jim
Tyler Tomlinson


4. References:

    1. https://www.geeksforgeeks.org/static-and-dynamic-scoping/
    2. https://en.wikipedia.org/wiki/Scope_(computer_science)#Definition
    3. Lecture Notes Repo( for client and server).
    4. Lecture 19 video ( for stress test class).


5. Time Tracking
Wensday - 9:00 - 10:30. 6:34-8:30    3.5 hours
Thursday - 10:00-1:30. 3:40-6:43     6.5 hours
Firday - 6:15- 10:15.                 4  hours
Total: 14 hours in total. 


6. Good Software Practices.
 1. S in solid each class is responsible for one thing in the project. 
 2. O in solid, we have an extension class. Which can also be the S in solid, because this class does only one thing.


7. Branching / Partnership 
   - The set up of the project's code(classes, getting chat client and server) and all the commenting were done by Sephora. 
   - Most of File Logger was done by Corbin Gurnee.
   - The rest was done by Pair Programming. 


8. Stress Tests:
     We have a total of six stress tests, four tests local while two tests remotely. Our comments have a little more info on them. 
