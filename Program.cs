using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication
{
    public class Program
    {
        public void Main(string[] args)
        {
            
            int port = GetFreePort();       
            Console.WriteLine("Port Created at: " + port.ToString());
            Console.WriteLine("Now trying to bind...");
            
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    var t1 = DateTime.UtcNow;
                    var dthTimeout = TimeSpan.FromSeconds(30);
                    while (!socket.Connected && DateTime.UtcNow - t1 < dthTimeout)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            //Changed from IPAddress.Loopback to IPAddress.Parse("127.0.0.1")
                            socket.Connect(new IPEndPoint(IPAddress.Loopback, port));
                        }
                        catch (SocketException)
                        {
                            // this happens when the DTH isn't listening yet
                        }
                    }
                    
                    if (!socket.Connected)
                    {
                        // reached timeout
                        Console.WriteLine("Socket not Connected!");
                        return;
                    }
                }
            
            Console.ReadLine();
        }
        
        private static int GetFreePort()
        {
            var l = new TcpListener(IPAddress.Loopback, 0);
            l.Start();
            int port = ((IPEndPoint)l.LocalEndpoint).Port;
            l.Stop();
            return port;
        }
    }
}
