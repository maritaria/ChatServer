using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using ChatServer.Chat;

namespace ChatServer.Repository
{
    class ClientRepository
    {

        private static ClientRepository _instance;
        private List<ServerClient> _clients = new List<ServerClient>();

        private ClientRepository() {}

        public static ClientRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ClientRepository();
            }
            return _instance;
        }

        public void Store(ServerClient client)
        {
            _clients.Add(client);
        }

        public void Remove(TcpClient client)
        {
            ServerClient toRemove = null;
            foreach (ServerClient serverClient in _clients)
            {
                if (serverClient._client.Equals(client))
                {
                    toRemove = serverClient;
                }
            }

            _clients.Remove(toRemove);
        }

        public ServerClient GetClient(TcpClient tcpClient)
        {
            foreach (ServerClient client in _clients)
            {
                if (client._client.Equals(tcpClient))
                {
                    return client;
                }
            }

            return null;
        }

        public ServerClient GetClient(string nickname)
        {
            foreach (ServerClient client in _clients)
            {
                if (client._nick.Equals(nickname))
                {
                    return client;
                }
            }

            return null;
        }

        public bool Has(TcpClient tcpClient)
        {
            return (GetClient(tcpClient) != null);
        }

        public bool Has(string nick)
        {
            return (GetClient(nick) != null);
        }

        public List<ServerClient> GetAllClients()
        {
            return _clients;
        }

    }
}
