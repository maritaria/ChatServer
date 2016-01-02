using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Threading;
using System.Net;

using ChatServer.Repository;
using ChatServer.Command;
using ChatServer.Chat.Channel;

namespace ChatServer.Chat
{
    class Server
    {

        public static Server _instance { get; private set; }

        private TcpListener _tcpListener;
        private Thread _listeningThread;
        private ClientRepository _clientRepository;

        private int _port { get; } = 4242;

        public Server()
        {
            initialise();
        }

        public Server(int port)
        {
            this._port = port;
            initialise();
        }

        public void initialise()
        {
            this._clientRepository = ClientRepository.GetInstance();
            this._tcpListener = new TcpListener(IPAddress.Any, _port);
            this._listeningThread = new Thread(new ThreadStart(listen));
            this._listeningThread.Start();

            Server._instance = this;

            // Register all commands
            new CommandNick();
            new CommandJoin();
            new CommandChannels();

            // Create additional channels
            ChannelManager.GetInstance().CreateChannel("twitch", false);
            ChannelManager.GetInstance().CreateChannel("locked-test", true);
        }

        public void listen()
        {
            this._tcpListener.Start();
            Console.WriteLine("###################################");
            Console.WriteLine("#            Chat Server          #");
            Console.WriteLine("#            by SysVoid           #");
            Console.WriteLine("###################################");
            Console.WriteLine($">> Listening on port: {_port}");

            while (true)
            {
                TcpClient client = this._tcpListener.AcceptTcpClient();

                // % = temporary username
                ClientRepository.GetInstance().Store(new ServerClient("%", client));
                Console.WriteLine($"Client {client.Client.RemoteEndPoint} connected!");

                Thread clientThread = new Thread(new ParameterizedThreadStart(handleClientCommunication));
                clientThread.Start(client);
            }
        }

        public void handleClientCommunication(object client)
        {
            TcpClient tcpClient = (TcpClient) client;

            if (tcpClient == null)
            {
                return;
            }

            NetworkStream clientStream = tcpClient.GetStream();

            if (!_clientRepository.Has(tcpClient))
            {
                _clientRepository.Store(new ServerClient("%", tcpClient));
            }

            ServerClient serverClient = _clientRepository.GetClient(tcpClient);
            if (serverClient == null)
            {
                // Disconnect the client if, for whatever reason, they are not in the repository.
                DisconnectFromServer(tcpClient, "Repository Error");
                return;
            }

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    // Read the incomming message, but only 4096 bytes of it.
                    // Hopefully that's enough for most messages.
                    bytesRead = clientStream.Read(message, 0, 4096);
                } catch (Exception e)
                {
                    // Some socket error occurred!
                    Console.WriteLine(e.StackTrace);
                    break;
                }

                if (bytesRead == 0)
                {
                    // Client disconnected... we don't need them anyway!
                    // *cries in corner*
                    _clientRepository.Remove(tcpClient);
                    Console.WriteLine($"Client {tcpClient.Client.RemoteEndPoint} disconnected!");

                    if (!serverClient._nick.Equals("%"))
                    {
                        serverClient._channel.BroadcastMessage("[-] >> " + serverClient._nick + " left the channel.");
                    }
                    return;
                }

                // Yay! Messages!
                string messageString = Encoding.UTF8.GetString(message, 0, bytesRead);
                ChatMessage msg = new ChatMessage(serverClient, messageString);

                if (serverClient._nick.Equals("%"))
                {
                    if (!msg._isCommanded || !msg._command.Equals("nick"))
                    {
                        serverClient.SendMessage("You cannot chat on this server until you set a nickname!");
                        serverClient.SendMessage("To set your nickname, use: -nick <nickname>");
                    } else
                    {
                        CommandManager.GetInstance().HandleCommandExecution(serverClient, msg._command, msg._commandArgs);
                    }
                } else
                {
                    if (msg._isCommanded)
                    {
                        CommandManager.GetInstance().HandleCommandExecution(serverClient, msg._command, msg._commandArgs);
                    } else
                    {
                        Console.WriteLine($"[msg] ({serverClient._channel._name}) <{serverClient._nick}> {msg}");
                        serverClient._channel.BroadcastMessage("<" + serverClient._nick + "> " + msg._message);
                    }
                }
            }
        }

        public void DisconnectFromServer(TcpClient client, string reason)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = Encoding.UTF8.GetBytes($"\n\rYou were disconnected from the chat server: {reason}\n\r");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
            client.Close();
            _clientRepository.Remove(client);
        }

        public void BroadcastMessage(string message)
        {
            foreach (ServerClient client in ClientRepository.GetInstance().GetAllClients())
            {
                client.SendMessage(message);
            }
        }

    }
}
