using Shared.Controls;
using System.Net;
using System.Net.NetworkInformation;
using UT.Data;
using System.Text.RegularExpressions;
using Client.ServerConfiguration;


namespace Client.ServerConfiguration
{
    public partial class Form : CustomForm
    {
        #region Constructors
        public Form()
        {
            InitializeComponent();
        }
        #endregion //Constructors

        #region Members
        private ServerConfigurationSet? set;
        #endregion //Members

        #region Properties
        internal ServerConfigurationSet? Set
        {
            get { return this.set; }
        }
        #endregion //Properties

        #region Events
        private void Form_Load(object sender, EventArgs e)
        {
            IPAddress ip = Network.LocalIPv4(NetworkInterfaceType.Ethernet).First();
            List<string> networkParts = new(ip.ToString().Split('.'));
            networkParts.RemoveAt(networkParts.Count - 1);
            
            this.lbl_computerRight.Text = Dns.GetHostName();
            this.tb_serverIP.Text = string.Join('.', networkParts) + ".xxx";
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void Btn_save_Click(object sender, EventArgs e)
        {
            if (!Regex.Match(this.tb_serverIP.Text, @"([0-9]{1,3}(\.)){3}[0-9]{1,3}").Success)
            {
                MessageBox.Show("Invalid IP format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            IPAddress? outIp;
            if(!IPAddress.TryParse(this.tb_serverIP.Text, out outIp))
            {
                MessageBox.Show("Invalid IP format", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.set = new ServerConfigurationSet()
            {
                Port = (int)this.nud_serverPort.Value,
                Server = outIp.ToString()
            };
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        #endregion //Events
    }
}
