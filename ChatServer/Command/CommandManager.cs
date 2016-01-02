using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;

namespace ChatServer.Command
{
    class CommandManager
    {

        private static CommandManager _instance;
        public Dictionary<string, ServerCommand> _commands { get; private set; } = new Dictionary<string, ServerCommand>();
        
        private CommandManager() {}

        public static CommandManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CommandManager();
            }
            return _instance;
        }

        public void RegisterCommand(string commandName, ServerCommand command)
        {
            if (_commands.ContainsKey(commandName))
            {
                return;
            }

            _commands.Add(commandName.ToLower(), command);
        }
        
        public void UnregisterCommand(string commandName)
        {
            _commands.Remove(commandName.ToLower());
        }

        public void HandleCommandExecution(ServerClient client, string commandName, List<string> arguments)
        {
            if (commandName.ToLower().Equals("help"))
            {
                Help(client);
                return;
            }

            ServerCommand cmd;
            _commands.TryGetValue(commandName, out cmd);

            if (cmd == null)
            {
                client.SendMessage("Command not found. Use -help for a list of available commands.");
                return;
            }

            cmd.Execute(client, arguments);
        }

        public void Help(ServerClient client)
        {
            if (_commands.Count == 0)
            {
                client.SendMessage("No commands available.");
                return;
            }

            client.SendMessage("--------------------------");
            foreach (string key in _commands.Keys)
            {
                ServerCommand command;
                _commands.TryGetValue(key, out command);

                if (command != null)
                {
                    client.SendMessage("[i] >> " + key + " : " + command._description);
                }
            }
            client.SendMessage("--------------------------");
        }

    }
}
