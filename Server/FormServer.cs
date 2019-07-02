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
        private RDTcpServer server;
        private Thread serverThread;
        private CancellationTokenSource cancelTokenSource;
        private CancellationToken token;
        private const string WAIT_FOR_ACTION = "Waiting for action!";

        public FormServer()
        {
            InitializeComponent();
            server = new RDTcpServer();
            labelServerStatus.Text = WAIT_FOR_ACTION;
        }

        private void ServerStart()
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
            labelServerStatus.Text = WAIT_FOR_ACTION;
            Console.WriteLine("Server stopped");
        }

        private void ButtonServerStart_Click(object sender, EventArgs e)
        {
            if (serverThread != null && !serverThread.ThreadState.Equals(ThreadState.Unstarted))
            {
                stopServer();
            }
            cancelTokenSource = new CancellationTokenSource();
            token = cancelTokenSource.Token;

            serverThread = new Thread(ServerStart);
            serverThread.Start();

            while (!serverThread.IsAlive)
            {
                Thread.Sleep(500);
            }
            labelServerStatus.Text = "Waiting for incoming connection...";
            Console.WriteLine("Server started");

        }

        private void ButtonStopServer_Click(object sender, EventArgs e)
        {
            stopServer();
        }
    }
}
