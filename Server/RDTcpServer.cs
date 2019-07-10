using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace RemoteDesktop
{
    class RDTcpServer
    {
        private readonly IPAddress listenAddr = IPAddress.Parse("127.0.0.1");
        private const int port = 9999;
        private TcpListener server;

        private void HandleConnection(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            const String message = "Server message";
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
        }

        private Bitmap TakeScreenshot()
        {
            Rectangle totalSize = Rectangle.Empty;

            foreach (Screen s in Screen.AllScreens)
                totalSize = Rectangle.Union(totalSize, s.Bounds);

            Bitmap screenShotBMP = new Bitmap(totalSize.Width, totalSize.Height, PixelFormat.Format32bppArgb);
            Graphics screenShotGraphics = Graphics.FromImage(screenShotBMP);
            screenShotGraphics.CopyFromScreen(totalSize.X, totalSize.Y, 0, 0, totalSize.Size, CopyPixelOperation.SourceCopy);
            screenShotGraphics.Dispose();

            return screenShotBMP;
        }

        private void CompressScreenshoot()
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 40;
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
