using Shared.Controls;
using Shared.Data;
using Shared.Efc;
using Shared.Efc.Tables;
using Shared.Interfaces;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data.Attributes;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(0)]
    public partial class Main : ExtendedMainModletForm, IMdiParentModlet, IMainMenuContainer
    {
        #region Members
        private readonly MenuStack menuStack;
        private bool isFullScreen = false;
        #endregion //Members

        #region Events
        public event EventHandler? OnFullscreenChanged;
        #endregion //Events

        #region Properties
        public bool IsFullScreen { get { return isFullScreen; } }
        public MenuStack MenuStack { get { return menuStack; } }
        public MenuStrip MenuStrip { get { return menuStrip; } }
        #endregion //Properties

        #region Enums
        public enum Actions
        {
            GetPerson
        }
        #endregion //Enums

        #region Constructors
        public Main()
        {
            InitializeComponent();
            Screen? ps = Screen.PrimaryScreen;
            menuStack = new MenuStack();
            if (ps == null)
            {
                return;
            }

            Location = ps.Bounds.Location;
            Size = ps.Bounds.Size;
            KeyPreview = true;
            Text = "Main";

            Load += Main_Load;
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
                case Actions.GetPerson:
                    return OnLocalServerAction_GetPerson(smc, stream);
                default:
                    break;
            }
            return null;
        }
        #endregion //Public Methods

        #region Private Methods
        private static byte[]? OnLocalServerAction_GetPerson(SharedModContext smc, byte[]? stream)
        {
            Guid? userId = ModletStream.GetContent<Actions, Guid>(stream);
            if (userId == Guid.Empty)
            {
                return null;
            }

            Person? person = smc.Users.Where(x => x.Id == userId).Select(x => x.Person).FirstOrDefault();

            return ModletStream.CreatePacket<bool, Person?>(true, person);
        }

        private void Main_Load(object? sender, EventArgs e)
        {
            KeyUp += Main_KeyUp;

            BringToFront();

            if (InfoBar != null)
            {
                menuStrip.BackColor = InfoBar.BackColor;
                menuStrip.Location = new Point(0, InfoBar.Height);
                if (
                    Session != null && 
                    Session.TryGetValue("User-Authentication", out object? value) && 
                    value is User user
                )
                {
                    Person? person = ModletStream.GetContent<bool, Person>(
                        Client?.Send(
                            ModletStream.CreatePacket(
                                Actions.GetPerson,
                                user.Id
                            ),
                            ModletCommands.Commands.Action,
                            this
                        )
                    );
                    if (person != null)
                    {
                        InfoBar.Subtitle = person.Name() ?? user.Id.ToString();
                    }
                }
            }
        }

        private void Main_KeyUp(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F11 && InfoBar != null)
            {
                bool swappedVisibility = !InfoBar.Visible;
                InfoBar.Visible = swappedVisibility;
                menuStrip.Visible = swappedVisibility;
                isFullScreen = !swappedVisibility;
                OnFullscreenChanged?.Invoke(this, new EventArgs());
            }
        }
        #endregion //Private Methods
    }
}
