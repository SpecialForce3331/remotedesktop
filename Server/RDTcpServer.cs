using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RemoteDesktop
{
    class RDTcpServer
    {
        private IPAddress listenAddr = IPAddress.Parse("127.0.0.1");
        private const int port = 9999;
        TcpListener server;

        private void HandleConnection(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            String message = "Server message";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
        }

        public void Start(CancellationToken token)
        {
            try
            {
                server = new TcpListener(listenAddr, port);
                server.Start();
                while (!token.IsCancellationRequested)
                {
                    if (!server.Pending())
                    {
                        Console.WriteLine("Server has not pending connections");
                        Thread.Sleep(500);
                        continue;
                    }
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Client connected!");
                    HandleConnection(client);
                }
            }
            catch (InvalidOperationException e )
            {
                Console.WriteLine(e.Message);
            }
            catch (SocketException e )
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }

    }
}
