using Shared.Controls;
using UT.Data.Attributes;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Modules
{
    [Position(0)]
    public partial class Main : ExtendedModletForm
    {
        #region Constructors
        public Main()
        {
            InitializeComponent();
            Screen? ps = Screen.PrimaryScreen;
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
        }

        private void Main_KeyUp(object? sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F11 && InfoBar != null)
            {
                bool swappedVisibility = !InfoBar.Visible;
                InfoBar.Visible = swappedVisibility;
            }
        }
        #endregion //Private Methods
    }
}
