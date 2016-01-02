using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Chat;

namespace ChatServer
{
    class Program
    {

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                int port = -1;
                Int32.TryParse(args[0], out port);

                if (port != -1)
                {
                    new Server(port);
                    return;
                }
            }
            new Server();
        }

    }
}
