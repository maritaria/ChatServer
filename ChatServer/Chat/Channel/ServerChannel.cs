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

        public List<ChannelMessage> _messages { get; private set; } = new List<ChannelMessage>();
        
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

        public void LogMessage(ServerClient client, string message)
        {
            if (!_isLobbyChannel)
            {
                return;
            }

            if (_messages.Count == 10)
            {
                _messages.RemoveAt(0);
            }

            _messages.Add(new ChannelMessage(client._nick, message));
        }

    }
}
