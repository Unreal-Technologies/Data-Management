using System.Net;
using UT.Data;
using UT.Data.Controls;

namespace Client
{
    public partial class AppConfiguration : ExtendedForm
    {
        public AppConfiguration()
        {
            InitializeComponent();
            Text = "Configuration";

            vtbTitle.Control.Text = App.Configuration.Title;
            vipaIp.Control.Text = App.Configuration.Server;

            vnudPort.Control.Minimum = 1;
            vnudPort.Control.Maximum = 65536;
            vnudPort.Control.Value = App.Configuration.Port;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Validator validator = new();
            validator.Add(vtbTitle);
            validator.Add(vipaIp);
            validator.Add(vnudPort);
            validator.Validate();

            if(validator.IsValid && IPAddress.TryParse(vipaIp.Control.Text, out IPAddress? ip))
            {
                int port = (int)vnudPort.Control.Value;
                bool reachable = Network.IsServerReachable(ip, port);
                bool reachable1 = reachable;
                if (reachable || !(reachable1 || MessageBox.Show("Server is not reachable, continue?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
                {
                    App.Configuration.Title = vtbTitle.Control.Text;
                    App.Configuration.Server = ip.ToString();
                    App.Configuration.Port = port;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
