using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Chat.Channel
{
    class ChannelManager
    {

        private static ChannelManager _instance;
        private List<ServerChannel> _channels = new List<ServerChannel>();

        private ChannelManager() {}

        public static ChannelManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ChannelManager();
                _instance._channels.Add(new ServerChannel("+lobby", false, true));
            }
            return _instance;
        }
        
        public void CreateChannel(string name, bool locked)
        {
            if (!name.StartsWith("+"))
            {
                name = "+" + name;
            }

            ServerChannel channel = new ServerChannel(name.ToLower(), locked, false);
            if (_channels.Contains(channel))
            {
                return;
            }

            _channels.Add(channel);
        }

        public ServerChannel GetLobby()
        {
            foreach (ServerChannel channel in _channels)
            {
                if (channel._isLobbyChannel)
                {
                    return channel;
                }
            }

            return null;
        }

        public bool Exists(string channelName)
        {
            foreach (ServerChannel channel in _channels)
            {
                if (channel._name.Equals(channelName.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Locked(string channelName)
        {
            foreach (ServerChannel channel in _channels)
            {
                if (channel._name.Equals(channelName.ToLower()))
                {
                    return channel._locked;
                }
            }

            // Say it's locked by default if it doesn't exist.
            return true;
        }

        public void Join(ServerClient client, string channelName)
        {
            foreach (ServerChannel channel in _channels)
            {
                if (channel._name.Equals(channelName.ToLower()))
                {
                    ServerChannel oldChannel = client._channel;
                    client._channel = channel;
                    channel.BroadcastMessage("[+] >> " + client._nick + " joined the channel!");
                    oldChannel.BroadcastMessage("[+] >> " + client._nick + " left the channel!");
                    return;
                }
            }
        }
        
        public string ChannelList()
        {
            string list = "";

            int total = _channels.Count;
            for (int i = 0; i < total; i++)
            {
                if (i == total - 1)
                {
                    list += _channels[i]._name;
                } else
                {
                    list += _channels[i]._name + ", ";
                }
            }

            return list;
        }

    }
}
