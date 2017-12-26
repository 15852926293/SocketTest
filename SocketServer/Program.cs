using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        public static string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);
            foreach (IPAddress ipa in localhost.AddressList)
            {
                if (ipa.AddressFamily == AddressFamily.InterNetwork)
                return ipa.ToString();
            }
            return "";
        }
        static void Main(string[] args)
        {
            int port = 6000;
            //string host = GetIpAddress();
            string host = "127.0.0.1";
            if (host == "")
            {
                throw new Exception("Get host IPV4 Address Failed");
            }
            //string host = "127.0.0.1";

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sSocket.Bind(ipe);
            sSocket.Listen(10);
            Console.WriteLine("Waitting for client connect...");

            //receive message
            Socket serverSocket = sSocket.Accept();
            Console.WriteLine(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
            Console.WriteLine("Connection established!\n" + "Client Info:" + serverSocket.RemoteEndPoint.ToString() + "\n");
            //create a thread for send message to client 
            Thread sendth = new Thread(new ParameterizedThreadStart(sendMsg));
            sendth.Start(serverSocket);
            //main thread to receive msg from client
            while(true)
            {
                string recMsg = "";
                byte[] recbyte = new byte[4096];
                int bytes = serverSocket.Receive(recbyte, recbyte.Length, 0);
                recMsg += Encoding.ASCII.GetString(recbyte, 0, bytes);
                Console.WriteLine(DateTime.Now.ToString("yyy-MM-dd HH:mm:ss"));
                Console.WriteLine(recMsg + "\n");
                if (recMsg == "bye")
                {
                    break;
                }
                /*
                //send message
                string sendMsg = "From server:Hello!";
                byte[] sendByte = Encoding.ASCII.GetBytes(sendMsg);
                serverSocket.Send(sendByte, sendByte.Length, 0);
                //serverSocket.Close();
                Console.ReadKey();
                */
            }
            serverSocket.Close();
            //kill the send message thread
            sendth.Abort();
            Console.WriteLine("End of server program !");
            Console.ReadKey();
        }

        public static void sendMsg(object serverSocket)
        {
            Socket sSocket = (Socket)serverSocket;
            while (true)
            {
                //send message
                string sendMsg = Console.ReadLine();
                byte[] sendByte = Encoding.ASCII.GetBytes(sendMsg);
                sSocket.Send(sendByte, sendByte.Length, 0);
            }
        }
    }
}
