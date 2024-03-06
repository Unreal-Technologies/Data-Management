using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.Modlet;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;

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
            if (screen != null)
            {
                this.Size = screen.Bounds.Size;
            }
        }
        #endregion //Constructors

        #region Implementations
        public void OnClientConfiguration(ModletClient client)
        {
            this.client = client as AuthenticatedModletClient;
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
            menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            menuStrip.Location = new System.Drawing.Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new System.Drawing.Size(278, 24);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip1";
            // 
            // Main
            // 
            ClientSize = new System.Drawing.Size(278, 244);
            Controls.Add(menuStrip);
            IsMdiContainer = true;
            MainMenuStrip = menuStrip;
            Name = "Main";
            Text = "Main";
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion //Private Methods
    }
}
