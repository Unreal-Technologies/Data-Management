using Shared.Controls;
using Shared.Efc;
using Shared.Efc.Tables;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Controls;
using UT.Data.Encryption;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(-1)]
    public partial class Authentication : ExtendedModletForm
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
        }
        #endregion //Constructors

        #region Public Methods
        public override byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            Actions? action = GetInputType<Actions>(stream);
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
            Tuple<string, string>? auth = GetContent<Actions, Tuple<string, string>>(stream);
            if (auth == null)
            {
                return null;
            }

            string username = auth.Item1;
            string password = auth.Item2;

            string? encPassword = Aes.Encrypt(password, User.Key)?.Md5();

            User? user = smc.Users.Where(x => x.Username == Aes.Encrypt(username, User.Key) && x.Password == encPassword && x.Start <= DateTime.Now && x.End >= DateTime.Now).FirstOrDefault();
            if (user == null)
            {
                return CreatePacket(false, "Wrong Username or Password");
            }
            return CreatePacket(true, user);
        }

        private static byte[]? OnLocalServerAction_GetRoleAccess(SharedModContext smc, byte[]? stream)
        {
            Guid? userId = GetContent<Actions, Guid>(stream);
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
            return CreatePacket<bool, Role.AccessTiers[]>(true, [.. tiers]);
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
                CreatePacket(
                    Actions.Authenticate, 
                    new Tuple<string, string>(username, password)
                ),
                ModletCommands.Commands.Action,
                this
            );
            bool? state = GetInputType<bool>(response);
            if(state == null)
            {
                return;
            }

            if (!state.Value)
            {
                string? message = GetContent<bool, string>(response);
                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            User? user = ReadPacket<bool, User>(response)?.value;

            Session.Add("User-Authentication", user);
            if (user != null)
            {
                Role.AccessTiers[]? result = GetContent<bool, Role.AccessTiers[]>(
                    Client.Send(
                        CreatePacket(
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
