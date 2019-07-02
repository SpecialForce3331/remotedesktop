using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteDesktop
{
    public partial class FormServer : Form
    {
        RDTcpServer server;
        Thread serverThread;
        CancellationTokenSource cancelTokenSource;
        CancellationToken token;

        public FormServer()
        {

            server = new RDTcpServer();

            InitializeComponent();
            labelServerStatus.Text = "Waiting for action!";
        }

        private void serverStart()
        {
            server.Start(token);
        }

        private void stopServer()
        {
            cancelTokenSource.Cancel();
            while (serverThread.IsAlive)
            {
                Thread.Sleep(500);
            }
            labelServerStatus.Text = "Waiting for action!";
            Console.WriteLine("Server stopped");
        }

        private void buttonServerStart_Click(object sender, EventArgs e)
        {
            if (serverThread != null && !serverThread.ThreadState.Equals(ThreadState.Unstarted))
            {
                stopServer();
            }
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            serverThread = new Thread(serverStart);
            serverThread.Start();

            while (!serverThread.IsAlive)
            {
                Thread.Sleep(500);
            }
            labelServerStatus.Text = "Waiting for incoming connection...";
            Console.WriteLine("Server started");

        }

        private void buttonStopServer_Click(object sender, EventArgs e)
        {
            stopServer();
        }
    }
}
