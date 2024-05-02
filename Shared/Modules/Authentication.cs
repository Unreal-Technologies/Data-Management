using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Controls.Validated;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class Authentication : ExtendedForm, IMainFormModlet
    {
        #region Members
        private Label? lbl_username, lbl_password;
        private ValidatedTextBox? tb_username, tb_password;
        private Button? btn_login;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            Authenticate, GetRoleAccess
        }
        #endregion //Enums

        #region Constructors
        public Authentication() 
            : base()
        {
            InitializeComponent();
            Text = "Authentication";
            Load += Authentication_Load;
        }
        #endregion //Constructors

        #region Public Methods
        public void OnServerInstallation(DbContext? context)
        {
            if (context == null)
            {
                return;
            }
            if (context is not ExtendedDbContext ctx)
            {
                return;
            }

            //Person person = new()
            //{
            //    Firstname = "Admin",
            //    Lastname = "Admin"
            //};
            //ctx.Person.Add(person);

            //User user = new()
            //{
            //    Username = "admin",
            //    Password = "test",
            //    Person = person
            //};
            //ctx.User.Add(user);

            //Role role = new()
            //{
            //    Description = "Administrator",
            //    Access = [Role.AccessTiers.Administrator, Role.AccessTiers.User],
            //};
            //ctx.Role.Add(role);

            //UserRole userRole = new()
            //{
            //    Role = role,
            //    User = user
            //};
            //ctx.UserRole.Add(userRole);
            //ctx.SaveChanges();
        }

        public byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            //base.OnLocalServerAction(stream, ip);
            //if(this.Context == null)
            //{
            //    return null;
            //}
            //object? packet = Packet<Actions, object>.Decode(stream);
            //if(packet == null)
            //{
            //    return null;
            //}

            //switch(((Packet<Actions, object>)packet).Description)
            //{
            //    case Actions.GetRoleAccess:
            //        packet = Packet<Actions, Guid>.Decode(stream);
            //        if(packet == null)
            //        {
            //            return null;
            //        }
            //        Guid userId = ((Packet<Actions, Guid>)packet).Data;
            //        Role?[] roles = [.. this.Context.UserRole.Where(x => x.User != null && x.User.Id == userId).Select(x => x.Role)];

            //        List<Role.AccessTiers> tiers = [];
            //        foreach(Role? role in roles)
            //        {
            //            if(role == null || role.Access == null)
            //            {
            //                continue;
            //            }
            //            foreach(Role.AccessTiers tier in role.Access)
            //            {
            //                if(!tiers.Contains(tier))
            //                {
            //                    tiers.Add(tier);
            //                }
            //            }
            //        }

            //        return Packet<bool, Role.AccessTiers[]>.Encode(true, [.. tiers]);
            //    case Actions.Authenticate:
            //        packet = Packet<Actions, Tuple<string, string>>.Decode(stream);
            //        if(packet == null)
            //        {
            //            return null;
            //        }
            //        Tuple<string, string>? auth = ((Packet<Actions, Tuple<string, string>>)packet).Data;
            //        if(auth == null)
            //        {
            //            return null;
            //        }

            //        string username = auth.Item1;
            //        string password = auth.Item2;

            //        string? encPassword = Aes.Encrypt(password, User.Key)?.Md5();

            //        User? user = this.Context?.User.Where(x => x.Username == Aes.Encrypt(username, User.Key) && x.Password == encPassword && x.Start <= DateTime.Now && x.End >= DateTime.Now).FirstOrDefault();
            //        if(user == null)
            //        {
            //            return Packet<bool, string?>.Encode(false, Strings.String_WrongUsernameOrPassword);
            //        }
            //        this.WriteUserLog(user, Strings.GetKey(Strings.Word_Login));
            //        return Packet<bool, User>.Encode(true, user);
            //}
            return null;
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            se.Add(Authenticate, "Authenticate", -1);
        }

        public void OnClientConfiguration(Form? form)
        {
        }

        public void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
        }

        public void OnServerConfiguration(DbContext? context)
        {
        }
        #endregion //Public Methods

        #region Private Methods
        private void Authentication_Load(object? sender, EventArgs e)
        {
            BringToFront();
        }

        private bool Authenticate(SequentialExecution sequentialExecution, ManualResetEvent resetEvent)
        {
            if (ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            //if(this.DialogResult != DialogResult.Retry)
            //{
            //    ApplicationState.Reset = false;
            //    self.IsValid = false;
            //    self.Exit();
            //    Application.ExitThread();
            //    Application.Exit();
            //    return true;
            //}

            //self.IsValid = false;
            return false;
        }

        private void Btn_login_Click(object? sender, EventArgs e)
        {
            //Validator validator = new();
            //validator.Add(this.tb_username);
            //validator.Add(this.tb_password);
            //validator.Validate();

            //if(!validator.IsValid || this.tb_username == null || this.tb_password == null)
            //{
            //    return;
            //}

            //string username = this.tb_username.Control.Text;
            //string password = this.tb_password.Control.Text;

            //byte[] request = Packet<Actions, Tuple<string, string>>.Encode(Actions.Authenticate, new Tuple<string, string>(username, password));
            //byte[]? response = ApplicationState.Client?.Send(request, ModletCommands.Commands.Action, this);
            //if(response == null)
            //{
            //    return;
            //}

            //Packet<bool, object>? result = Packet<bool, object>.Decode(response);
            //if(result == null)
            //{
            //    return;
            //}

            //bool state = result.Description;
            //if (!state)
            //{
            //    string? message = Packet<bool, string?>.Decode(response)?.Data;
            //    MessageBox.Show(message, Strings.Word_Information, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    this.DialogResult = DialogResult.Retry;
            //    return;
            //}
            //Packet<bool, User>? decodedResult = Packet<bool, User>.Decode(response);
            //if(ApplicationState.Client == null)
            //{
            //    return;
            //}
            //User? user = decodedResult?.Data;
            //ApplicationState.Client.AuthenticatedUser = user;

            //if (user != null)
            //{
            //    request = Packet<Actions, Guid>.Encode(Actions.GetRoleAccess, user.Id);
            //    response = ApplicationState.Client?.Send(request, ModletCommands.Commands.Action, this);

            //    ApplicationState.Access = Packet<bool, Role.AccessTiers[]>.Decode(response)?.Data;
            //}

            //this.DialogResult = DialogResult.OK;
            //if (this.BaseForm != null) //Hide Splash

            //{
            //    Invoker<Form>.Invoke(this.BaseForm, delegate (Form form, object[]? data)
            //    {
            //        form.Visible = false;
            //    });
            //}
        }

        private void InitializeComponent()
        {
            lbl_username = new Label();
            lbl_password = new Label();
            tb_username = new ValidatedTextBox();
            tb_password = new ValidatedTextBox();
            btn_login = new Button();
            SuspendLayout();
            // 
            // lbl_username
            // 
            lbl_username.AutoSize = true;
            lbl_username.Location = new Point(56, 73);
            lbl_username.Name = "lbl_username";
            lbl_username.Size = new Size(108, 20);
            lbl_username.TabIndex = 0;
            lbl_username.Text = "Username:";
            // 
            // lbl_password
            // 
            lbl_password.AutoSize = true;
            lbl_password.Location = new Point(56, 106);
            lbl_password.Name = "lbl_password";
            lbl_password.Size = new Size(108, 20);
            lbl_password.TabIndex = 1;
            lbl_password.Text = "Password:";
            // 
            // tb_username
            // 
            tb_username.IsRequired = true;
            tb_username.Location = new Point(179, 66);
            tb_username.Name = "tb_username";
            tb_username.Size = new Size(200, 27);
            tb_username.TabIndex = 2;
            // 
            // tb_password
            // 
            tb_password.IsRequired = true;
            tb_password.Location = new Point(179, 99);
            tb_password.Name = "tb_password";
            tb_password.Size = new Size(200, 27);
            tb_password.TabIndex = 3;
            // 
            // btn_login
            // 
            btn_login.Location = new Point(179, 132);
            btn_login.Name = "btn_login";
            btn_login.Size = new Size(177, 34);
            btn_login.TabIndex = 4;
            btn_login.Text = "Login";
            btn_login.UseVisualStyleBackColor = true;
            btn_login.Click += Btn_login_Click;
            // 
            // Authentication
            // 
            ClientSize = new Size(400, 182);
            Controls.Add(btn_login);
            Controls.Add(tb_password);
            Controls.Add(tb_username);
            Controls.Add(lbl_password);
            Controls.Add(lbl_username);
            Name = "Authentication";
            Controls.SetChildIndex(lbl_username, 0);
            Controls.SetChildIndex(lbl_password, 0);
            Controls.SetChildIndex(tb_username, 0);
            Controls.SetChildIndex(tb_password, 0);
            Controls.SetChildIndex(btn_login, 0);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion //Private Methods
    }
}
