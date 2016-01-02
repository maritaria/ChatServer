using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;
using ChatServer.Chat.Channel;

namespace ChatServer.Command
{
    class CommandJoin : ServerCommand
    {

        public CommandJoin()
        {
            this._name = "join";
            this._description = "Join a channel.";
            CommandManager.GetInstance().RegisterCommand("join", this);
        }

        public override void Execute(ServerClient client, List<string> args)
        {
            if (args.Count < 1)
            {
                client.SendMessage("Usage: -join +<channel>");
                return;
            }

            string channel = args[0].ToLower();

            if (!channel.StartsWith("+"))
            {
                channel = "+" + channel;
            }

            if (client._channel._name.Equals(channel))
            {
                client.SendMessage("You are already in that channel.");
                return;
            }

            if (!ChannelManager.GetInstance().Exists(channel))
            {
                client.SendMessage("That channel does not exist.");
                return;
            }

            if (ChannelManager.GetInstance().Locked(channel))
            {
                client.SendMessage("That channel is locked!");
                return;
            }

            ChannelManager.GetInstance().Join(client, channel);
        }

    }
}
