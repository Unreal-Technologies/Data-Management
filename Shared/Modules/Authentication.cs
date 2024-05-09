using Shared.Controls;
using Shared.Efc;
using Shared.Efc.Tables;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Controls.Validated;
using UT.Data.Encryption;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(-1)]
    public partial class Authentication : ExtendedMainModletForm
    {
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
            tb_password.Control.PasswordChar = '*';

            Text = "Authentication";
            Load += Authentication_Load;

            this.RadialTransform(
                25,
                x => x.GetType() != typeof(ValidatedTextBox) && x.GetType() != typeof(Label)
            ).BorderTransform(
                BorderStyle.FixedSingle,
                Color.Gray
            );

            btn_login.Width = tb_password.Control.Width;
        }
        #endregion //Constructors

        #region Public Methods
        public override byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            Actions? action = ModletStream.GetInputType<Actions>(stream);
            if (Context is not SharedModContext smc || action == null)
            {
                return null;
            }

            switch (action)
            {
                case Actions.GetRoleAccess:
                    return OnLocalServerAction_GetRoleAccess(smc, stream);
                case Actions.Authenticate:
                    return OnLocalServerAction_Authenticate(smc, stream);
                default:
                    break;
            }
            return null;
        }
        #endregion //Public Methods

        #region Private Methods
        private static byte[]? OnLocalServerAction_Authenticate(SharedModContext smc, byte[]? stream)
        {
            Tuple<string, string>? auth = ModletStream.GetContent<Actions, Tuple<string, string>>(stream);
            if (auth == null)
            {
                return null;
            }

            string username = auth.Item1;
            string password = auth.Item2;

            string? encPassword = Aes.Encrypt(password, User.Key)?.Md5();

            User? user = smc.Users.FirstOrDefault(x => 
                x.Username == Aes.Encrypt(username, User.Key) &&
                x.Password == encPassword &&
                x.Start <= DateTime.Now &&
                x.End >= DateTime.Now
            );
            if (user == null)
            {
                return ModletStream.CreatePacket(false, "Wrong Username or Password");
            }
            return ModletStream.CreatePacket(true, user);
        }

        private static byte[]? OnLocalServerAction_GetRoleAccess(SharedModContext smc, byte[]? stream)
        {
            Guid? userId = ModletStream.GetContent<Actions, Guid>(stream);
            if (userId == Guid.Empty)
            {
                return null;
            }

            Role?[] roles = [.. smc.UserRoles.Where(x => x.User != null && x.User.Id == userId).Select(x => x.Role)];

            List<Role.AccessTiers> tiers = [];
            foreach (Role? role in roles)
            {
                if (role == null || role.Access == null)
                {
                    continue;
                }
                foreach (Role.AccessTiers tier in role.Access)
                {
                    if (!tiers.Contains(tier))
                    {
                        tiers.Add(tier);
                    }
                }
            }
            return ModletStream.CreatePacket<bool, Role.AccessTiers[]>(true, [.. tiers]);
        }

        private void Authentication_Load(object? sender, EventArgs e)
        {
            BringToFront();
        }

        private void Btn_login_Click(object? sender, EventArgs e)
        {
            if(Client == null || Session == null)
            {
                return;
            }

            Validator validator = new();
            validator.Add(tb_username);
            validator.Add(tb_password);
            validator.Validate();

            if (!validator.IsValid || tb_username == null || tb_password == null)
            {
                return;
            }

            string username = tb_username.Control.Text;
            string password = tb_password.Control.Text;

            byte[]? response = Client.Send(
                ModletStream.CreatePacket(
                    Actions.Authenticate, 
                    new Tuple<string, string>(username, password)
                ),
                ModletCommands.Commands.Action,
                this
            );
            bool? state = ModletStream.GetInputType<bool>(response);
            if(state == null)
            {
                return;
            }

            if (!state.Value)
            {
                string? message = ModletStream.GetContent<bool, string>(response);
                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            User? user = ModletStream.ReadPacket<bool, User>(response)?.value;

            if (user != null)
            {
                Session.Add("User-Authentication", user);

                Role.AccessTiers[]? result = ModletStream.GetContent<bool, Role.AccessTiers[]>(
                    Client.Send(
                        ModletStream.CreatePacket(
                            Actions.GetRoleAccess, 
                            user.Id
                        ), 
                        ModletCommands.Commands.Action, 
                        this
                    )
                );
                if(result == null)
                {
                    return;
                }

                Session.Add("User-Access", result);
            }

            DialogResult = DialogResult.OK;
        }
        #endregion //Private Methods
    }
}
