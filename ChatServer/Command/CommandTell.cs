using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;
using ChatServer.Repository;

namespace ChatServer.Command
{
    class CommandTell : ServerCommand
    {

        public CommandTell()
        {
            this._name = "tell";
            this._description = "Private message someone.";

            CommandManager.GetInstance().RegisterCommand("tell", this);
        }

        public override void Execute(ServerClient client, List<string> args)
        {
            if (args.Count < 2)
            {
                client.SendMessage("Usage: -tell <name> <message...>");
                return;
            }

            string person = args[0];
            string message = "";

            for (int i = 1; i < args.Count; i++)
            {
                if (i == args.Count - 1)
                {
                    message += args[i];
                } else
                {
                    message += args[i] + " ";
                }
            }

            ServerClient recipient = ClientRepository.GetInstance().GetClient(person);
            if (recipient == null)
            {
                client.SendMessage("That person is not connected.");
                return;
            }

            if (recipient.Equals(client))
            {
                client.SendMessage("You cannot send messages to yourself!");
                return;
            }
            
            client.SendMessage($"PRIVATE | [{client._nick} > {recipient._nick}] {message}");
            recipient.SendMessage($"PRIVATE | [{client._nick} > {recipient._nick}] {message}");
        }

    }
}
