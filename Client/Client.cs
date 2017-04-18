using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Client
{
    class Client
    {
        const int PORT = 7777;
        const string SERVER_IP = "127.0.0.1";

        public Client()
        {
            TcpClient server = new TcpClient(SERVER_IP, PORT);
            Console.WriteLine("Client has connected to server");
            NetworkStream networkStream = server.GetStream();

            StreamWriter streamWriter = new StreamWriter(networkStream);
            StreamReader streamReader = new StreamReader(networkStream);
            streamWriter.AutoFlush = true;

            while(true)
            {
                string header = "CAN READ";
                streamWriter.WriteLine(header);

                if (networkStream.DataAvailable)
                {
                    string type = Convert.ToString(streamReader.ReadLine());
                    int x = Convert.ToInt32(streamReader.ReadLine());
                    int y = Convert.ToInt32(streamReader.ReadLine());

                    Console.WriteLine($"{type} : <{x}, {y}>");
                }

                Thread.Sleep(100);
            }
             

        }

    }
}
