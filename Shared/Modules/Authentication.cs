using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Encryption;
using UT.Data.Extensions;
using UT.Data.Modlet;
using Application = System.Windows.Forms.Application;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class Authentication : MainFormModlet<Context>
    {
        #region Members
        private Label? lbl_username, lbl_password;
        private Validated<TextBox>? tb_username, tb_password;
        private Button? btn_login;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            Authenticate, GetRoleAccess
        }
        #endregion //Enums

        #region Implementations
        public override void OnServerInstallation(DbContext? context)
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

        public override byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            base.OnLocalServerAction(stream, ip);
            if(this.Context == null)
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
                    Role?[] roles = [.. this.Context.UserRole.Where(x => x.User != null && x.User.Id == userId).Select(x => x.Role)];

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

                    User? user = this.Context?.User.Where(x => x.Username == Aes.Encrypt(username, User.Key) && x.Password == encPassword && x.Start <= DateTime.Now && x.End >= DateTime.Now).FirstOrDefault();
                    if(user == null)
                    {
                        return Packet<bool, string?>.Encode(false, Strings.String_WrongUsernameOrPassword);
                    }
                    this.WriteUserLog(user, Strings.GetKey(Strings.Word_Login));
                    return Packet<bool, User>.Encode(true, user);
            }
            return null;
        }

        public override void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            se.Add(this.Authenticate, Strings.Word_Authenticate, -1);
        }
        #endregion //Implementations

        #region Constructors
        public Authentication() 
            : base()
        {
            this.InitializeComponent();
            this.Load += Authentication_Load;
        }
        #endregion //Constructors

        #region Private Methods
        private void Authentication_Load(object? sender, EventArgs e)
        {
            this.Reposition();
        }

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
            Validator validator = new();
            validator.Add(this.tb_username);
            validator.Add(this.tb_password);
            validator.Validate();

            if(!validator.IsValid || this.tb_username == null || this.tb_password == null)
            {
                return;
            }

            string username = this.tb_username.Control.Text;
            string password = this.tb_password.Control.Text;

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
                MessageBox.Show(message, Strings.Word_Information, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            tb_username = new Validated<TextBox>(delegate (TextBox control) { return control.Text; });
            tb_password = new Validated<TextBox>(delegate (TextBox control) { return control.Text; });
            btn_login = new Button();
            SuspendLayout();
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new Point(30, 31);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(108, 20);
            lbl_username.TabIndex = 0;
            lbl_username.Text = Strings.Word_Username + ":";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new Point(30, 65);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new Size(108, 20);
            lbl_password.TabIndex = 1;
            lbl_password.Text = Strings.Word_Password + ":";
            // 
            // tb_username
            // 
            tb_username.Location = new Point(144, 28);
            tb_username.Name = "tb_username";
            tb_username.Control.Size = new Size(150, 28);
            tb_username.TabIndex = 2;
            tb_username.IsRequired = true;
            // 
            // tb_password
            // 
            tb_password.Location = new Point(144, 62);
            tb_password.Name = "tb_password";
            tb_password.Control.PasswordChar = '*';
            tb_password.Control.Size = new Size(150, 28);
            tb_password.TabIndex = 3;
            tb_password.IsRequired = true;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(144, 96);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(150, 34);
            btn_login.TabIndex = 4;
            btn_login.Text = Strings.Word_Login;
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += Btn_login_Click;
            // 
            // Authentication
            // 
            ClientSize = new Size(400, 150);
            Controls.Add(btn_login);
            Controls.Add(tb_password);
            Controls.Add(tb_username);
            Controls.Add(lbl_password);
            Controls.Add(lbl_username);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Text = Strings.Word_Authenticate;

            ResumeLayout(false);
            PerformLayout();
        }

        private void Reposition()
        {
            if(this.lbl_password == null || this.lbl_username == null || this.tb_password == null || this.tb_username == null || this.btn_login == null)
            {
                return;
            }

            this.SuspendLayout();

            this.lbl_username.Size = this.lbl_username.PreferredSize;
            this.lbl_password.Size = this.lbl_password.PreferredSize;

            int padding = 3;
            int lblWidth = int.Max(this.lbl_username.Width, this.lbl_password.Width);
            int tbWidth = int.Max(this.tb_password.Width, this.tb_username.Width);

            int sum = lblWidth + tbWidth + padding;
            int offset = (this.Width - sum) / 4;

            int lblXs = offset;
            int lblXe = lblXs + lblWidth;

            int tbXs = lblXe + padding;

            this.lbl_username.Location = new Point(lblXe - this.lbl_username.Width, this.tb_username.Location.Y + ((this.tb_username.Height - this.lbl_username.Height) / 2));
            this.lbl_password.Location = new Point(lblXe - this.lbl_password.Width, this.tb_password.Location.Y + ((this.tb_password.Height - this.lbl_password.Height) / 2));
            this.tb_username.Location = new Point(tbXs, this.tb_username.Location.Y);
            this.tb_password.Location = new Point(tbXs, this.tb_password.Location.Y);
            this.btn_login.Location = new Point(tbXs, this.btn_login.Location.Y);

            this.ResumeLayout(false);
        }
        #endregion //Private Methods
    }
}
