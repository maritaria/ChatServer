using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Repository;

namespace ChatServer.Chat.Channel
{
    class ServerChannel
    {

        public string _name { get; private set; }
        public bool _locked { get; set; } = false;
        public bool _isLobbyChannel { get; private set; } = false;
        
        public ServerChannel(string name, bool locked, bool isLobbyChannel)
        {
            this._name = name;
            this._locked = locked;
            this._isLobbyChannel = isLobbyChannel;
        }

        public void BroadcastMessage(string message)
        {
            foreach (ServerClient client in ClientRepository.GetInstance().GetAllClients())
            {
                if (client._channel.Equals(this))
                {
                    client.SendMessage(message);
                }
            }
        }

    }
}
