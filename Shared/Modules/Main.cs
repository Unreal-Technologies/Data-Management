using Shared.Controls;
using Shared.Data;
using Shared.Interfaces;
using System.Drawing;
using System.Windows.Forms;
using UT.Data.Attributes;

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

        #region Constructors
        public Main()
        {
            InitializeComponent();
            Screen? ps = Screen.PrimaryScreen;
            menuStack = new MenuStack();
            if(ps == null)
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

        #region Private Methods
        private void Main_Load(object? sender, EventArgs e)
        {
            KeyUp += Main_KeyUp;

            BringToFront();

            if (InfoBar != null)
            {
                menuStrip.BackColor = InfoBar.BackColor;
                menuStrip.Location = new Point(0, InfoBar.Height);
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
