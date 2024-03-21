using Shared.Controls;
using Shared.EFCore;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Controls;

namespace Server
{
    public partial class Configuration : ExtendedForm
    {
        #region Constants
        private const int MaxPortNumber = 65536;
        #endregion //Constants

        #region Constructors
        public Configuration()
        {
            InitializeComponent();
            this.Load += Configuration_Load;
        }
        #endregion //Constructors

        #region Public Methods
        internal Program.Settings GetSettings()
        {
            Program.Settings settings = new()
            {
                Port = (int)this.gb_server_vnud_port_dynamic.Control.Value
            };
            settings.Db.Port = (int)this.gb_database_vnud_port_dynamic.Control.Value;
            settings.Db.Ip = this.gb_database_vipa_ip_dynamic.Control.Text;
            settings.Db.Type = this.gb_database_vcb_type_dynamic.Control.SelectedItem?.ToString() ?? string.Empty;
            settings.Db.Username = this.gb_database_vtb_username_dynamic.Control.Text;
            settings.Db.Password = this.gb_database_vtb_password_dynamic.Control.Text;
            settings.Db.Db = this.gb_database_vtb_database_dynamic.Control.Text;

            return settings;
        }
        #endregion //Public Methods

        #region Private Methods
        private void Configuration_Load(object? sender, EventArgs e)
        {
            this.SetDefaultConfiguration();
        }

        private void SetDefaultConfiguration()
        {
            this.gb_database_vnud_port_dynamic.Control.Maximum = Configuration.MaxPortNumber;
            this.gb_server_vnud_port_dynamic.Control.Maximum = Configuration.MaxPortNumber;

            this.gb_database_vnud_port_dynamic.Control.Value = 3306;
            this.gb_server_vnud_port_dynamic.Control.Value = 1404;

            foreach (ExtendedDbContext.Types type in Enum.GetValues(typeof(ExtendedDbContext.Types)))
            {
                this.gb_database_vcb_type_dynamic.Control.Items.Add(type);
            }
            this.gb_database_vcb_type_dynamic.Control.SelectedItem = this.gb_database_vcb_type_dynamic.Control.Items[0];
            this.gb_database_vcb_type_dynamic.Control.DropDownStyle = ComboBoxStyle.DropDownList;

            this.gb_database_vtb_username_dynamic.Control.Text = "root";

            IPAddress ip = Network.LocalIPv4(NetworkInterfaceType.Ethernet).First();
            List<string> networkParts = new(ip.ToString().Split('.'));
            networkParts.RemoveAt(networkParts.Count - 1);

            this.gb_server_lbl_device_dynamic.Text = Dns.GetHostName();
            this.gb_database_vipa_ip_dynamic.Control.Text = string.Join('.', networkParts) + ".xxx";
        }
        #endregion //Private Methods

        private void Btn_save_Click(object sender, EventArgs e)
        {
            Validator validator = new();
            validator.Add(this.gb_server_vnud_port_dynamic);
            validator.Add(this.gb_database_vcb_type_dynamic);
            validator.Add(this.gb_database_vnud_port_dynamic);
            validator.Add(this.gb_database_vipa_ip_dynamic);
            validator.Add(this.gb_database_vtb_username_dynamic);
            validator.Add(this.gb_database_vtb_password_dynamic);
            validator.Add(this.gb_database_vtb_database_dynamic);
            validator.Validate();

            if(validator.IsValid)
            {
                IPAddress dbIp = IPAddress.Parse(gb_database_vipa_ip_dynamic.Control.Text);
                int dbPort = (int)this.gb_database_vnud_port_dynamic.Control.Value;
                Context.Types? type = (Context.Types?)this.gb_database_vcb_type_dynamic.Control?.SelectedItem;
                string dbUsername = this.gb_database_vtb_username_dynamic.Control.Text;
                string dbPassword = this.gb_database_vtb_password_dynamic.Control.Text;
                string dbDatabase = this.gb_database_vtb_database_dynamic.Control.Text;

                if (type == null)
                {
                    validator.SetError(this.gb_database_vcb_type_dynamic, "Select Error, undefined type?");
                }
                else if (!Network.IsServerReachable(dbIp, dbPort))
                {
                    string message = "Cannot reach server.";
                    validator.SetError(this.gb_database_vipa_ip_dynamic, message);
                    validator.SetError(this.gb_database_vnud_port_dynamic, message);
                }
                else
                {
                    Context.Configuration? configuration = Context.CreateConnection(type.Value, dbIp, dbPort, dbUsername, dbPassword, dbDatabase);
                    if (configuration == null)
                    {
                        string message = "Configuration Error";
                        validator.SetError(this.gb_database_vipa_ip_dynamic, message);
                        validator.SetError(this.gb_database_vnud_port_dynamic, message);
                        validator.SetError(this.gb_database_vtb_username_dynamic, message);
                        validator.SetError(this.gb_database_vtb_password_dynamic, message);
                        validator.SetError(this.gb_database_vtb_database_dynamic, message);
                    }
                    else
                    {
                        Context context = new(configuration);
                        if (!context.Database.CanConnect())
                        {
                            string message = "Cannot connect to database.";
                            validator.SetError(this.gb_database_vipa_ip_dynamic, message);
                            validator.SetError(this.gb_database_vnud_port_dynamic, message);
                            validator.SetError(this.gb_database_vtb_username_dynamic, message);
                            validator.SetError(this.gb_database_vtb_password_dynamic, message);
                            validator.SetError(this.gb_database_vtb_database_dynamic, message);
                        }
                        else
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
        }
    }
}
