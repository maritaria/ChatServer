using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;
using ChatServer.Chat.Channel;

namespace ChatServer.Command
{
    class CommandChannels : ServerCommand
    {

        public CommandChannels()
        {
            this._name = "channels";
            this._description = "List all available channels.";
            CommandManager.GetInstance().RegisterCommand("channels", this);
        }

        public override void Execute(ServerClient client, List<string> args)
        {
            client.SendMessage("--------------------------");
            client.SendMessage(ChannelManager.GetInstance().ChannelList());
            client.SendMessage("--------------------------");
        }

    }
}
