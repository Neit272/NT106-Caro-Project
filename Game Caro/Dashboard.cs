using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Caro
{
    public partial class Dashboard : Form
    {
        private Server Server;
        private List<Client> Clients = new List<Client>();

        public Dashboard()
        {
            InitializeComponent();

            Server = new Server();
        }

        private void serverBtn_Click(object sender, EventArgs e)
        {
            Server.Show();
            serverBtn.Enabled = false;
            clientBtn.Enabled = true;
        }

        private void clientBtn_Click(object sender, EventArgs e)
        {
            Client Client = new Client();
            Client.Show();
            Clients.Add(Client);
        }
    }
}
