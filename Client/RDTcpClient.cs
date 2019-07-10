using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class RDTcpClient
    {
        private readonly IPAddress serverAddr;
        private readonly int serverPort;

        public void Connect()
        {
            TcpClient client = new TcpClient();
            byte[] buffer = new byte[1024];

            try
            {
                client.Connect(serverAddr, serverPort);
                NetworkStream stream = client.GetStream();

                while (stream.DataAvailable)
                {
                    stream.Read(buffer, 0, buffer.Length);
                    Console.WriteLine(Encoding.UTF8.GetChars(buffer));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (client != null)
                {
                    client.Close();
                }
            }
        }

        public RDTcpClient(String ipAddr, int port)
        {
            serverAddr = IPAddress.Parse(ipAddr);
            serverPort = port;
        }
    }
}
