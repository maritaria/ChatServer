using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ChatServer.Chat;

namespace ChatServer.Command
{
    abstract class ServerCommand
    {

        public string _name { get; protected set; }
        public string _description { get; protected set; }

        public abstract void Execute(ServerClient client, List<string> args);

    }
}
