using MultiplayerServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMultiplayerFramework
{
    class ServerProgram
    {
        public static void Main(string[] args)
        {
            String portNumber = "9000";

            Console.WriteLine("Enter Port Number: ");
            //portNumber = Console.ReadLine();

            Server server = new Server(int.Parse(portNumber));
            server.Setup();
            server.StartServer();

            Console.WriteLine("Server Shut Down");
        }
    }
}
