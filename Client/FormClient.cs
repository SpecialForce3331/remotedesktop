using System;
using System.Threading;
using System.Windows.Forms;

namespace Client
{
    public partial class FormClient : Form
    {
        private Thread thread;

        public FormClient()
        {
            InitializeComponent();
        }

        private void ClientConnect()
        {
            String ipAddr = TextBoxIpAddress.Text;
            int port = Int32.Parse(textBoxPort.Text);
            RDTcpClient client = new RDTcpClient(ipAddr, port);
            client.Connect();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            thread = new Thread(ClientConnect);
            thread.Start();
            ButtonConnect.Enabled = false;
            buttonDisconnect.Enabled = true;

            /*
            while (thread.IsAlive)
            {
                Thread.Sleep(500);
            }
            ButtonConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
            */
        }

    }
}
