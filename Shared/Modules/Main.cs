using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.Modlet;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;
using System.Drawing;
using System.Security.Principal;

namespace Shared.Modules
{
    [Position(int.MinValue + 1)]
    public class Main : CustomForm, IMainFormModlet
    {
        #region Members
        private AuthenticatedModletClient? client;
        private MenuStrip? menuStrip;
        #endregion //Members

        #region Constructors
        public Main()
            : base()
        {
            this.InitializeComponent();
            Screen? screen = Screen.PrimaryScreen;
            this.MinimumSize = new Size(1280, 1024);
            if (screen != null)
            {
                this.MaximumSize = screen.Bounds.Size;
            }
        }
        #endregion //Constructors

        #region Implementations
        public void OnClientConfiguration(ModletClient client)
        {
            this.client = client as AuthenticatedModletClient;
            this.Shown += Main_Shown;
        }

        public void OnGlobalServerAction(byte[]? stream)
        {
        }

        public byte[]? OnLocalServerAction(byte[]? stream)
        {
            return null;
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            se.Add(this.SeInitialize, "Launching");
        }

        public void OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
        }

        public void OnServerInstallation(DbContext? context)
        {
        }
        #endregion //Implementations

        #region Private Methods
        private void Main_Shown(object? sender, EventArgs e)
        {
            MenuItem menu = MenuItem.Root();
            FillBaseMenuTree(menu);

            if (this.client != null)
            {
                foreach (IMdiFormModlet item in UT.Data.Modlet.Modlet.Load<IMdiFormModlet>(null).Cast<IMdiFormModlet>())
                {
                    item.OnClientConfiguration(this.client);
                    item.OnMenuCreation(menu);
                }
            }

            RenderMenu(menu);
        }

        private static void RenderMenu(MenuItem menu)
        {

        }

        private static void FillBaseMenuTree(MenuItem menu)
        {
            MenuItem account = MenuItem.Submenu();
            menu.Add("Account", account);

            account.Add("l1", MenuItem.Line());
            account.Add("Logout", MenuItem.Button(delegate () { MessageBox.Show("Logout"); }));
        }

        private bool SeInitialize(SequentialExecution self)
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return true;
            }

            self.IsValid = false;
            return false;
        }

        private void InitializeComponent()
        {
            menuStrip = new MenuStrip();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(278, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // Main
            // 
            ClientSize = new Size(278, 244);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "Main";
            Text = "Data Manager";
            TopMost = false;
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion //Private Methods
    }
}
