using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class Authentication : CustomForm, IModlet
    {
        #region Constants
        public const string AuthenticationKey = "0x44ff731a";
        #endregion //Constants

        #region Members
        private Label? lbl_username;
        private Label? lbl_password;
        private TextBox? tb_username;
        private TextBox? tb_password;
        private Button? btn_login;
        private ModletClient? client;
        private Context? context;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            Authenticate
        }
        #endregion //Enums

        #region Implementations
        void IModlet.OnServerInstallation(DbContext? context)
        {
            if(context == null)
            {
                return;
            }
            if (context is not Context ctx)
            {
                return;
            }

            Person person = new()
            {
                Firstname = "Admin",
                Lastname = "Admin"
            };
            ctx.Person.Add(person);

            User user = new()
            {
                Username = "admin",
                Password = "test",
                Person = person
            };
            ctx.User.Add(user);
            ctx.SaveChanges();
        }

        void IModlet.OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
            if(context == null || context is not Context ctx)
            {
                throw new Exception("No Database Access");
            }
            this.context = ctx;
        }

        void IModlet.OnClientConfiguration(ModletClient client)
        {
            this.client = client;
        }

        void IModlet.OnGlobalServerAction(byte[]? stream) { }

        byte[]? IModlet.OnLocalServerAction(byte[]? stream)
        {
            object? packet = Packet<Actions, object>.Decode(stream);
            if(packet == null)
            {
                return null;
            }

            switch(((Packet<Actions, object>)packet).Description)
            {
                case Actions.Authenticate:
                    packet = Packet<Actions, Tuple<string, string>>.Decode(stream);
                    if(packet == null)
                    {
                        return null;
                    }
                    Tuple<string, string>? auth = ((Packet<Actions, Tuple<string, string>>)packet).Data;
                    if(auth == null)
                    {
                        return null;
                    }

                    string username = auth.Item1;
                    string password = auth.Item2;

                    throw new NotImplementedException();
                    //User? user = User.Authenticate(this.dbc, username, password);
                    //if (user == null)
                    //{
                    //    return Packet<bool, string?>.Encode(false, "No DB Yet, Cannot validate user '" + username + "'");
                    //}
                    //return Packet<bool, User>.Encode(true, user);
            }

            return null;
        }

        void IModlet.OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            se.Add(this.Authenticate, "Authenticate", -1);
        }
        #endregion //Implementations

        #region Constructors
        public Authentication() 
            : base()
        {
            this.InitializeComponent();
        }
        #endregion //Constructors

        #region Private Methods
        private bool Authenticate(SequentialExecution self)
        {
            if(this.ShowDialog() == DialogResult.OK)
            {
                return true;
            }

            self.IsValid = false;
            return false;
        }

        private void Btn_login_Click(object? sender, EventArgs e)
        {
            if (this.tb_password == null || this.tb_username == null || this.client == null)
            {
                return;
            }

            string username = this.tb_username.Text;
            string password = this.tb_password.Text;
            if(username == String.Empty || password == String.Empty)
            {
                return;
            }

            byte[] request = Packet<Actions, Tuple<string, string>>.Encode(Actions.Authenticate, new Tuple<string, string>(username, password));
            byte[]? response = this.client.Send(request, ModletCommands.Commands.Action, this);
            if(response == null)
            {
                return;
            }

            Packet<bool, object>? result = Packet<bool, object>.Decode(response);
            if(result == null)
            {
                return;
            }

            bool state = result.Description;
            if (!state)
            {
                string? message = Packet<bool, string?>.Decode(response)?.Data;
                MessageBox.Show(message, "Info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void InitializeComponent()
        {
            this.lbl_username = new System.Windows.Forms.Label();
            this.lbl_password = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_username
            // 
            this.lbl_username.AutoSize = true;
            this.lbl_username.Location = new System.Drawing.Point(30, 31);
            this.lbl_username.Name = "lbl_username";
            this.lbl_username.Size = new System.Drawing.Size(108, 20);
            this.lbl_username.TabIndex = 0;
            this.lbl_username.Text = "Username:";
            // 
            // lbl_password
            // 
            this.lbl_password.AutoSize = true;
            this.lbl_password.Location = new System.Drawing.Point(30, 65);
            this.lbl_password.Name = "lbl_password";
            this.lbl_password.Size = new System.Drawing.Size(108, 20);
            this.lbl_password.TabIndex = 1;
            this.lbl_password.Text = "Password:";
            // 
            // tb_username
            // 
            this.tb_username.Location = new System.Drawing.Point(144, 28);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(150, 28);
            this.tb_username.TabIndex = 2;
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(144, 62);
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.Size = new System.Drawing.Size(150, 28);
            this.tb_password.TabIndex = 3;
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(144, 96);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(150, 34);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = "Login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.Btn_login_Click);
            // 
            // Authentication
            // 
            this.ClientSize = new System.Drawing.Size(348, 172);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.lbl_password);
            this.Controls.Add(this.lbl_username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Authentication";
            this.Text = "Authentication";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion //Private Methods
    }
}
