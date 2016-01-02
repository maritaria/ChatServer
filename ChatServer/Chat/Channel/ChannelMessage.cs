using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer.Chat.Channel
{
    class ChannelMessage
    {

        public string _nick { get; private set; }
        public string _message { get; private set; }

        public ChannelMessage(string nick, string message)
        {
            this._nick = nick;
            this._message = message;
        }

    }
}
