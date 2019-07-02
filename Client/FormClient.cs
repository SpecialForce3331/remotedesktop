using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
        }

        private void ButtonConnect_Click(object sender, EventArgs e)
        {
            String ipAddr = TextBoxIpAddress.Text;
            int port = Int32.Parse(textBoxPort.Text);
            RDTcpClient client = new RDTcpClient(ipAddr, port);
            client.Connect();
        }
    }
}
