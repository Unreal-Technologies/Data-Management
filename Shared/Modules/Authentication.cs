using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using Shared.Modlet;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Encryption;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class Authentication : CustomForm, IMainFormModlet
    {
        #region Members
        private Label? lbl_username;
        private Label? lbl_password;
        private TextBox? tb_username;
        private TextBox? tb_password;
        private Button? btn_login;
        private Context? context;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            Authenticate, GetRoleAccess
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

            Role role = new()
            {
                Description = "Administrator",
                Access = [Role.AccessTiers.Administrator, Role.AccessTiers.User],
            };
            ctx.Role.Add(role);

            UserRole userRole = new()
            {
                Role = role,
                User = user
            };
            ctx.UserRole.Add(userRole);
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

        void IModlet.OnClientConfiguration(Form? splash) { }

        void IModlet.OnGlobalServerAction(byte[]? stream) { }

        byte[]? IModlet.OnLocalServerAction(byte[]? stream)
        {
            if(this.context == null)
            {
                return null;
            }
            object? packet = Packet<Actions, object>.Decode(stream);
            if(packet == null)
            {
                return null;
            }

            switch(((Packet<Actions, object>)packet).Description)
            {
                case Actions.GetRoleAccess:
                    packet = Packet<Actions, Guid>.Decode(stream);
                    if(packet == null)
                    {
                        return null;
                    }
                    Guid userId = ((Packet<Actions, Guid>)packet).Data;
                    Role?[] roles = [.. this.context.UserRole.Where(x => x.User != null && x.User.Id == userId).Select(x => x.Role)];

                    List<Role.AccessTiers> tiers = [];
                    foreach(Role? role in roles)
                    {
                        if(role == null || role.Access == null)
                        {
                            continue;
                        }
                        foreach(Role.AccessTiers tier in role.Access)
                        {
                            if(!tiers.Contains(tier))
                            {
                                tiers.Add(tier);
                            }
                        }
                    }

                    return Packet<bool, Role.AccessTiers[]>.Encode(true, [.. tiers]);
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

                    string? encPassword = Aes.Encrypt(password, User.Key)?.Md5();

                    User? user = this.context?.User.Where(x => x.Username == Aes.Encrypt(username, User.Key) && x.Password == encPassword && x.Start <= DateTime.Now && x.End >= DateTime.Now).FirstOrDefault();
                    if(user == null)
                    {
                        return Packet<bool, string?>.Encode(false, "Wrong username or password.");
                    }
                    return Packet<bool, User>.Encode(true, user);
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
            if(this.DialogResult != DialogResult.Retry)
            {
                ApplicationState.Reset = false;
                self.IsValid = false;
                self.Exit();
                Application.ExitThread();
                Application.Exit();
                return true;
            }

            self.IsValid = false;
            return false;
        }

        private void Btn_login_Click(object? sender, EventArgs e)
        {
            if (this.tb_password == null || this.tb_username == null)
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
            byte[]? response = ApplicationState.Client?.Send(request, ModletCommands.Commands.Action, this);
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
                this.DialogResult = DialogResult.Retry;
                return;
            }
            Packet<bool, User>? decodedResult = Packet<bool, User>.Decode(response);
            if(ApplicationState.Client == null)
            {
                return;
            }
            User? user = decodedResult?.Data;
            ApplicationState.Client.AuthenticatedUser = user;

            if (user != null)
            {
                request = Packet<Actions, Guid>.Encode(Actions.GetRoleAccess, user.Id);
                response = ApplicationState.Client?.Send(request, ModletCommands.Commands.Action, this);

                ApplicationState.Access = Packet<bool, Role.AccessTiers[]>.Decode(response)?.Data;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void InitializeComponent()
        {
            lbl_username = new Label();
            lbl_password = new Label();
            tb_username = new TextBox();
            tb_password = new TextBox();
            btn_login = new Button();
            SuspendLayout();
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new System.Drawing.Point(30, 31);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new System.Drawing.Size(108, 20);
            lbl_username.TabIndex = 0;
            lbl_username.Text = "Username:";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new System.Drawing.Point(30, 65);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new System.Drawing.Size(108, 20);
            lbl_password.TabIndex = 1;
            lbl_password.Text = "Password:";
            // 
            // tb_username
            // 
            tb_username.Location = new System.Drawing.Point(144, 28);
            tb_username.Name = "tb_username";
            tb_username.Size = new System.Drawing.Size(150, 28);
            tb_username.TabIndex = 2;
            // 
            // tb_password
            // 
            tb_password.Location = new System.Drawing.Point(144, 62);
            tb_password.Name = "tb_password";
            tb_password.PasswordChar = '*';
            tb_password.Size = new System.Drawing.Size(150, 28);
            tb_password.TabIndex = 3;
            // 
            // btn_login
            // 
            btn_login.Location = new System.Drawing.Point(144, 96);
            btn_login.Name = "btn_login";
            btn_login.Size = new System.Drawing.Size(150, 34);
            btn_login.TabIndex = 4;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += Btn_login_Click;
            // 
            // Authentication
            // 
            ClientSize = new System.Drawing.Size(348, 172);
            Controls.Add(btn_login);
            Controls.Add(tb_password);
            Controls.Add(tb_username);
            Controls.Add(lbl_password);
            Controls.Add(lbl_username);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "Authentication";
            Text = "Authentication";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion //Private Methods
    }
}
