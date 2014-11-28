using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImplementationClient
{
    class ClientProgram
    {
        static void Main(string[] args)
        {
            
            String IpAddress = "127.0.0.1";
            String portNumber = "9000";
            
            Console.WriteLine("Client");

            Thread.Sleep(1500);

            MultiplayerClient.Client client = new MultiplayerClient.Client();
            
            /*
            Console.WriteLine("Enter IP Address: ");
            IpAddress = Console.ReadLine();

            Console.WriteLine("Enter Port Number: ");
            portNumber = Console.ReadLine();
            */

            client.Connect(IpAddress, int.Parse(portNumber));

            // Send Connection Packet
            Console.WriteLine("Press Enter To Send Empty Packet");
            Console.ReadLine();
            //client.SendMessage(""); 
            
            while (true) { 
                // Dont force close
            }

        }
    }
}
