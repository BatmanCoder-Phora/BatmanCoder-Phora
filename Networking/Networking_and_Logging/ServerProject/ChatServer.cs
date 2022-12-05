/// <summary> 
/// Author:   Pro. Jim St. Germain 
/// Partner:   Corbin Gurnee and Sephora Bateman 
/// Date:      2/8/20
/// Course:    CS 3500, University of Utah, School of Computing 
/// Copyright: CS 3500 and Sephora Bateman  - This work may not be copied for use in Academic Coursework. 
/// 
/// We, Sephora Bateman and Corbin Gurnee , certify that We wrote this code from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in our README file. 
/// </summary>

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CS3500
{
    /// <summary>
    /// A simple server for sending simple text messages to multiple clients
    /// </summary>
    public class ChatServer
    {
        /// <summary>
        /// keep track of how big a message to send... keep getting bigger!
        /// </summary>
        private long larger = 5000;
        ILogger Logger;
        private int NumberMessagesSent = 0;

        public ChatServer(ILogger logger)
        {
            Logger = logger;
        }
        public static void Main(string[] args)
        {
            ServiceCollection services = new ServiceCollection();
            CustomFileLogProvider provider = new CustomFileLogProvider();
            services.AddLogging(configure =>
            {
                configure.AddConsole();
                configure.AddProvider(provider);
                configure.SetMinimumLevel(LogLevel.Information);
            });
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                ILogger<ChatServer> logger = serviceProvider.GetRequiredService<ILogger<ChatServer>>();
                ChatServer server = new ChatServer(logger);
                logger.LogDebug("Server code has started. \n");
                try
                {
                    server.StartServer();
                    
                }
                catch(Exception e)
                {
                    logger.LogCritical($"A {e} error occurred that caused startServer to fail. \n");
                }
                //Thread.Sleep(1000);
                //provider.Dispose();
                lock (server)
                {
                    logger.LogInformation("The Server is closing! \n");
                    provider.Dispose();
                }
            }

        }
        /// <summary>
        /// A list of all clients currently connected
        /// </summary>
        public List<Socket> clients = new List<Socket>();

        private TcpListener listener;

        /// <summary>
        ///   Nothing to "construct" at this time
        /// </summary>
        public ChatServer(bool input)
        {
        }
        /// <summary>
        /// Start accepting Tcp socket connections from clients
        /// </summary>
        public void StartServer()
        {
            Console.WriteLine("Enter port to listen on (default 11000):");
            string portStr = Console.ReadLine();

            int port;
            Logger.LogInformation("Server has started. \n");
            if (!Int32.TryParse(portStr, out port))
            {
                Logger.LogWarning("User did not put in a number for the port.");
                port = 11000;
            }
            listener = new TcpListener(IPAddress.Any, port);
            Console.WriteLine($"Server waiting for clients here: 127.0.0.1 on port {port}");

            listener.Start();

            // This begins an "event loop".
            // ConnectionRequested will be invoked when the first connection arrives.
            // TODO: we should be passing the TcpListener as the last argument, instead 
            //       of having it as a member of the class.
            try
            {
                listener.BeginAcceptSocket(ConnectionRequested, null);
            }
            catch(Exception e)
            {
                Logger.LogError($"An error {e} occurred when failing to connect to a client. \n");
            }

            // waits for the user to type messages, then sends them
            SendMessage();
        }

        public void StartServerNoUser()
        {
            Console.WriteLine("Enter port to listen on (default 11000):");

            int port;
            Logger.LogInformation("Server has started. \n");
            if (!Int32.TryParse("11000", out port))
            {
                Logger.LogWarning("User did not put in a number for the port.");
                port = 11000;
            }
            listener = new TcpListener(IPAddress.Any, port);
            Console.WriteLine($"Server waiting for clients here: 127.0.0.1 on port {port}");

            listener.Start();

            // This begins an "event loop".
            // ConnectionRequested will be invoked when the first connection arrives.
            // TODO: we should be passing the TcpListener as the last argument, instead 
            //       of having it as a member of the class.
            try
            {
                listener.BeginAcceptSocket(ConnectionRequested, null);
            }
            catch (Exception e)
            {
                Logger.LogError($"An error {e} occurred when failing to connect to a client. \n");
            }

            // waits for the user to type messages, then sends them
            //SendMessage();
        }

        /// <summary>
        /// A callback for invoking when a socket connection is accepted
        /// </summary>
        /// <param name="ar"></param>
        private void ConnectionRequested(IAsyncResult ar)
        {
            Console.WriteLine("Contact from client");

            // Get the socket
            clients.Add(listener.EndAcceptSocket(ar));
            Logger.LogInformation("Client has Connected. \n");
            // continue an event-loop that will allow more clients to connect
            try
            {
                listener.BeginAcceptSocket(ConnectionRequested, null);
            }
            catch(Exception e)
            {
                Logger.LogError($"An error occurred {e} that caused a client connect to fail.");
            }
        }


        /// <summary>
        /// Continuously ask the user for a message to send to the client
        /// </summary>
        private void SendMessage()
        {
            while (true)
            {
                NumberMessagesSent = 0;
                Console.WriteLine("enter a message to send");
                string message = Console.ReadLine();
                if (message == "largemessage")
                {
                    message = GenerateLargeMessage();
                }

                //
                // Begin sending the message
                //
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                List<Socket> toRemove = new List<Socket>();

                Console.WriteLine($"   Sending a message of size: {message.Length}");
                
                foreach (Socket s in clients)
                {
                    try
                    {
                        s.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, s);
                    }
                    catch (Exception) // Begin Send fails if client is closed
                    {
                        toRemove.Add(s);
                    }
                }
                
                // update list of "current" clients by removing closed clients
                foreach (Socket s in toRemove)
                {
                    Logger.LogInformation($"Client {s} disconnected. \n");
                    Console.WriteLine($"Client {s} disconnected");
                    clients.Remove(s);
                }
                Logger.LogInformation($"A message of size: {message.Length} has been sent, and sent to " +
                    $"{NumberMessagesSent} clients. \n");
            }
        }
        /// <summary>
        /// Continuously ask the user for a message to send to the client
        /// </summary>
        public void SendMessage(string message)
        {
            
                if (message == "largemessage")
                {
                    message = GenerateLargeMessage();
                }

                //
                // Begin sending the message
                //
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                List<Socket> toRemove = new List<Socket>();

                Console.WriteLine($"   Sending a message of size: {message.Length}");

                foreach (Socket s in clients)
                {
                    try
                    {
                        s.BeginSend(messageBytes, 0, messageBytes.Length, SocketFlags.None, SendCallback, s);
                    }
                    catch (Exception) // Begin Send fails if client is closed
                    {
                        toRemove.Add(s);
                    }
                }

                // update list of "current" clients by removing closed clients
                foreach (Socket s in toRemove)
                {
                    Console.WriteLine($"Client {s} disconnected");
                    clients.Remove(s);
                }
            
        }
        /// <summary>
        /// Generate a big string of the letter a repeated...
        /// </summary>
        /// <returns></returns>
        private string GenerateLargeMessage()
        {
            StringBuilder retval = new StringBuilder();

            for (int i = 0; i < larger; i++)
                retval.Append("a");
            retval.Append(".");

            larger += larger;

            return retval.ToString();
        }


        /// <summary>
        /// A callback invoked when a send operation completes
        /// </summary>
        /// <param name="ar"></param>
        private void SendCallback(IAsyncResult ar)
        {
            // Nothing much to do here, just conclude the send operation so the socket is happy.
            // 
            //   This code could be useful to update the state of a program once you know a client
            //   has received some information, for example, if you had a counter of successfully sent 
            //   messages you would increment it here.
            //
            NumberMessagesSent += 1;
            Socket client = (Socket)ar.AsyncState;
            long send_length = 0;
            try
            {
                send_length = client.EndSend(ar);
            }
            catch (Exception e)
            {
                Logger.LogError($"an {e} error occurred when sending. ");
            }

            //Console.WriteLine($"   Sent a message of size: {send_length}");

        }

    }
}
