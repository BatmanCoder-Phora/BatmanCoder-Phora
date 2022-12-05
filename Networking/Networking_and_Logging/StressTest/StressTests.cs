/// <summary> 
/// Author:    Sephora Bateman  
/// Partner:   Corbin Gurnee and Pro. Jim St. Germain 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// We, Sephora Bateman and Corbin Gurnee , certify that We wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in our README file. 
/// </summary>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;

namespace CS3500
{
    class StressTests
    {
        /// <summary>
        /// In this class, create a main function that takes in command line arguments.  Here are the possible arguments you must support:
        // server - create and run a server, allowing the user of your program to send messages
        // client - create and run a client, and wait until enter is pressed.
        // local # - create a server on the local machine and run stress test number '#'.  If the number is out of range, state so, and
        //  exit.  If it is a valid stress test, create one or more clients and start sending data.  The nature of the stress test is up 
        //  to you, but should be well documented
        //  remote # - assume a server is running at a remote location.  Ask for the server's ip address and port, and then fire up your 
        //  stress test of the given number, but using the remote server instead of a local one
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int test_id = 0;
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: [server] |[client] |[local #] | [remote #]");             
                return;
            }
            if (args[0].Equals("server"))
            {
                ChatServer.Main(args);
                return;
            }
            else if (args[0].Equals("client"))
            {
                ChatClient.Main(args);
                return;
            }
            else if (!(args[0].Equals("local") || args[0].Equals("remote")) || !(Int32.TryParse(args[1], out test_id)))
            {
                Console.WriteLine($"Didn't understand what you meant by:{args[0]}{args[1]}");
                Console.WriteLine("Exiting");
                return;
            }
            ServiceCollection services = new ServiceCollection();
            using (CustomFileLogProvider provider = new CustomFileLogProvider())
            {
                services.AddLogging(configure =>
                {
                    configure.AddConsole();
                    configure.AddProvider(provider);
                    configure.SetMinimumLevel(LogLevel.Debug);
                });
                // selects which stress test we test. 
                using (ServiceProvider serviceProvider = services.BuildServiceProvider())
                {
                    ILogger logger = serviceProvider.GetRequiredService<ILogger<StressTests>>();
                    if (test_id == 1)
                    {
                        stress_test_1(args[0].Equals("local"), logger);
                    }
                    if (test_id == 2)
                    {
                        stress_test_2_Server_local(args[0].Equals("local"), logger);
                    }
                    if (test_id == 3)
                    {
                        stress_test_2_Client_local(args[0].Equals("local"), logger);
                    }
                    if (test_id == 4)
                    {
                        stress_test_3_Client(args[0].Equals("local"), logger);
                    }
                    if (test_id == 5)
                    {
                        stress_test_4_Client(args[0].Equals("local"), logger);
                    }
                    if (test_id == 6)
                    {
                        stress_test_5_Client(args[0].Equals("local"), logger);
                    }

                }

            }

        }
        /// <summary>
        /// Test creating a client and a server, Do not need to start a server before hand. 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        static void stress_test_1(bool local, ILogger logger)
        {
            if (!local)
            {
                logger.LogError("this test is only for local tests");
                return;
            }
            // start server
            ChatServer server = new ChatServer(logger);
            server.StartServerNoUser(); 
            Thread.Sleep(1000);
            // start client
            ChatClient client = new ChatClient(logger, 11000); 
            client.ConnectToServer("127.0.0.1");
            Thread.Sleep(1000);
            server.SendMessage("Hello World");
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
        /// <summary>
        ///  Run server and test adding 10 clients on to it. You do not need to start a server before hand. 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        private static void stress_test_2_Server_local(bool local, ILogger logger)
        {
            if (!local)
            {
                logger.LogError("this test is only for local tests");
                return;
            }
            ChatServer server = new ChatServer(logger);
            server.StartServerNoUser(); // should contain a false;
            Thread.Sleep(1000);
            while (server.clients.Count <= 10)
            {
                // makes clients and connects them to server.
                Thread.Sleep(500);
                logger.LogInformation($"Still Waiting {server.clients.Count}/10 \n");
                ChatClient client = new ChatClient(logger, 11000);
                client.ConnectToServer("127.0.0.1");
            }
            Thread.Sleep(1000);
            Console.WriteLine("Press Enter to Exit");
            Console.ReadLine();
        }
        /// <summary>
        ///  Run server and test adding 100 clients on to it. You do not need to start a server before hand. 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        private static void stress_test_2_Client_local(bool local, ILogger logger)
        {
            if (!local)
            {
                logger.LogError("this test is only for local tests");
                return;
            }
            ChatServer server = new ChatServer(logger);
            server.StartServerNoUser(); // should contain a false;
            Thread.Sleep(1000);
            for (int i = 0; i < 100; i++)
            {
                ChatClient client = new ChatClient(logger, 11000);
                client.ConnectToServer("155.98.68.196"); // makes clients and connects them to server.
            }
            Thread.Sleep(10000);
            Console.Read();
        }
        /// <summary>
        ///  Run server and test adding 50 clients on to it. You do not need to start a server before hand. 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        private static void stress_test_3_Client(bool local, ILogger logger)
        {
            if (!local)
            {
                logger.LogError("this test is only for local tests");
                return;
            }
            ChatServer server = new ChatServer(logger);
            server.StartServerNoUser(); // starts a server
            Thread.Sleep(1000);
            for (int i = 0; i < 50; i++)
            {
                ChatClient client = new ChatClient(logger, 11000);
                client.ConnectToServer("127.0.0.1");// makes clients and connects them to server.
            }
            Console.ReadLine();
        }
        /// <summary>
        /// Creates a server access for some information,then continues adding 50 clients. Server must be going before hand 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        private static void stress_test_4_Client(bool local, ILogger logger)
        {
            if (local)
            {
                logger.LogError("this test is only for remote tests");
                return;
            }
            Console.WriteLine("Please input a server IP address:");
            string IP = Console.ReadLine();
            Console.WriteLine("Please input a port number:");
            string Porto = Console.ReadLine();
            Thread.Sleep(1000);
            for (int i = 0; i < 50; i++)
            {
                ChatClient client = new ChatClient(logger, Int32.Parse(Porto));
                client.ConnectToServer(IP);// makes clients and connects them to server.
            }
            Console.ReadLine();
        }
        /// <summary>
        ///  Creates a server access for some information,then continues adding 50 clients. Waits four seconds then adds another 50 clients. 
        ///  Server must be going before hand 
        /// </summary>
        /// <param name="local">Says if it is local or not</param>
        /// <param name="logger">The logger passed in from main</param>
        private static void stress_test_5_Client(bool local, ILogger logger)
        {
            if (local)
            {
                logger.LogError("This test is only for remote tests");
                return;
            }
            Console.WriteLine("Please input a server IP address:");
            string IP = Console.ReadLine();
            Console.WriteLine("Please input a port number:");
            string Porto = Console.ReadLine();
            Thread.Sleep(1000);
            for (int i = 0; i < 50; i++)
            {
                ChatClient client = new ChatClient(logger, Int32.Parse(Porto));
                client.ConnectToServer(IP); // makes clients and connects them to server.
                
            }
            Thread.Sleep(4000);
            for (int i = 0; i < 50; i++)
            {
                ChatClient client = new ChatClient(logger, Int32.Parse(Porto));
                client.ConnectToServer(IP);// makes clients and connects them to server.

            }
            Console.ReadLine();
        }

    }
}
