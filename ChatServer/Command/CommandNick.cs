using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;
using ChatServer.Repository;

namespace ChatServer.Command
{
    class CommandNick : ServerCommand
    {

        public CommandNick()
        {
            this._name = "nick";
            this._description = "Set your nickname.";
            CommandManager.GetInstance().RegisterCommand("nick", this);
        }

        public override void Execute(ServerClient client, List<string> args)
        {
            if (args.Count < 1)
            {
                client.SendMessage("Usage: /nick <nickname>");
                return;
            }

            string nickname = args[0];

            // Check for a client with the nickname
            foreach (ServerClient serverClient in ClientRepository.GetInstance().GetAllClients())
            {
                if (serverClient._nick.ToLower().Equals(nickname.ToLower()))
                {
                    client.SendMessage("That nickname is already in use!");
                    return;
                }
            }

            // Nicknames must be at least 3 characters long, and cannot contain - or %.
            if (nickname.Length < 3)
            {
                client.SendMessage("Nicknames must be 3+ characters in length.");
                return;
            }

            if (nickname.Contains("%") || nickname.Contains("-") || nickname.Contains("+"))
            {
                client.SendMessage("Nicknames cannot contain %, + or -.");
                return;
            }

            if (!client._nick.Equals("%"))
            {
                client._channel.BroadcastMessage("[i] >> " + client._nick + " is now known as " + args[0]);
            } else
            {
                client._channel.BroadcastMessage("[+] >> " + nickname + " joined the channel.");
            }
            client._nick = nickname;
        }

    }
}
