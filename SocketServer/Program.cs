using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SocketServer
{
    class Program
    {
        public static string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostEntry(hostName);    //方法已过期，可以获取IPv4的地址
                                                                    //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[1];

            return localaddr.ToString();
        }
        static void Main(string[] args)
        {
            int port = 6000;
            string host = GetIpAddress();
            //string host = "127.0.0.1";

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sSocket.Bind(ipe);
            sSocket.Listen(0);
            Console.WriteLine("Start listen,please wait...");

            //receive message
            Socket serverSocket = sSocket.Accept();
            Console.WriteLine("Connection established!");
            string recMsg = "";
            byte[] recbyte = new byte[4096];
            int bytes = serverSocket.Receive(recbyte, recbyte.Length, 0);
            recMsg += Encoding.ASCII.GetString(recbyte, 0, bytes);
            Console.WriteLine(recMsg);

            //send message
            string sendMsg = "From server:Hello!";
            byte[] sendByte = Encoding.ASCII.GetBytes(sendMsg);
            serverSocket.Send(sendByte,sendByte.Length,0);
            //serverSocket.Close();
            Console.ReadKey();
        }
    }
}
