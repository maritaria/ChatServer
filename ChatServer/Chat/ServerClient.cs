using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

using ChatServer.Chat.Channel;

namespace ChatServer.Chat
{
    class ServerClient
    {

        public string _nick { get; set; }
        public bool _muted { get; set; } = false;

        public TcpClient _client { get; }
        public string _ipAddress { get; }

        public ServerChannel _channel { get; set; }

        public ServerClient(string nick, TcpClient client)
        {
            this._nick = nick;
            this._client = client;
            this._ipAddress = client.Client.RemoteEndPoint.ToString();
            this._channel = ChannelManager.GetInstance().GetLobby();
        }
        
        public void SendMessage(string message)
        {
            NetworkStream stream = _client.GetStream();
            ASCIIEncoding encoder = new ASCIIEncoding();
            byte[] buffer = encoder.GetBytes($"\n\r{message}\n\r");
            stream.Write(buffer, 0, buffer.Length);
            stream.Flush();
        }

    }
}
