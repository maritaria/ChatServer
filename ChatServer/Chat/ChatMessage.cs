using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Chat
{
    class ChatMessage
    {

        public ServerClient _client { get; }
        public string _message { get; }

        public bool _isCommanded { get; } = false;
        public string _command { get; } = "";
        public List<string> _commandArgs { get; } = new List<string>();

        public ChatMessage(ServerClient client, string message)
        {
            this._client = client;

            // Replace backslashes with nothing, 'cuz I ain't having people adding new lines and tabs and shit.
            this._message = message.Replace("\\", "");

            if (_message.StartsWith("-"))
            {
                this._isCommanded = true;
                this._command = message.Split(' ')[0].Substring(1);
                
                if (message.Split(' ').Length > 1)
                {
                    for (int i = 1; i < message.Split(' ').Length; i++)
                    {
                        _commandArgs.Add(message.Split(' ')[i]);
                    }
                }
            }
        }

        public override string ToString()
        {
            return _message;
        }

    }
}
