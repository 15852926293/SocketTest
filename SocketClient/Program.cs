using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 6000;
            string host = "127.0.0.1";

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);
            //create a thread to receive message from server.
            Thread recth = new Thread(new ParameterizedThreadStart(recMsg));
            recth.Start(clientSocket);
            //main thread to receive msg from client
            while (true)
            {
                //send message
                string sendMsg = Console.ReadLine();
                byte[] sendBytes = Encoding.ASCII.GetBytes(sendMsg);
                clientSocket.Send(sendBytes);
                if (sendMsg == "bye")
                {
                    break;
                }

                /*
                //receive message
                string recMsg = "";
                byte[] recBytes = new byte[4096];
                int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
                recMsg += Encoding.ASCII.GetString(recBytes, 0, bytes);
                Console.WriteLine(recMsg);
                //clientSocket.Close();
                Console.ReadKey();
                */
            }
            clientSocket.Close();
            // kill the rec message thread
            recth.Abort();
            Console.WriteLine("End of client program !");
            Console.ReadKey();
        }

        public static void recMsg(object clientSocket)
        {
            Socket cSocket = (Socket)clientSocket;
            while (true)
            {
                //receive message
                string recMsg = "";
                byte[] recBytes = new byte[4096];
                int bytes = cSocket.Receive(recBytes, recBytes.Length, 0);
                recMsg += Encoding.ASCII.GetString(recBytes, 0, bytes);
                Console.WriteLine(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss") +"\n" + recMsg);
            }
        }
    }
}
