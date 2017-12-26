using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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

            Socket clientSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);

            //send message
            string sendMsg = "abcdefg";
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendMsg);
            clientSocket.Send(sendBytes);

            //receive message
            string recMsg = "";
            byte[] recBytes = new byte[4096];
            int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
            recMsg += Encoding.ASCII.GetString(recBytes, 0, bytes);
            Console.WriteLine(recMsg);
            //clientSocket.Close();
            Console.ReadKey();
        }
    }
}
