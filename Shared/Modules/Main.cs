using Microsoft.EntityFrameworkCore;
using Shared.Controls;
using Shared.EFC;
using Shared.EFC.Tables;
using Shared.Modlet;
using System.Drawing;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Forms;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MinValue + 1)]
    public class Main : CustomForm, IMainFormModlet
    {
        #region Members
        private MenuStrip? menuStrip;
        private Form? splash;
        private Context? context;
        #endregion //Members

        #region Enums
        public enum Actions
        {
            Getperson
        }
        #endregion //Enumss

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
        public void OnClientConfiguration(Form? splash)
        {
            this.splash = splash;
            this.Shown += Main_Shown;
            this.FormClosing += Main_FormClosing;
        }

        private void Main_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if(this.DialogResult != DialogResult.Retry)
            {
                Application.ExitThread();
                Application.Exit();
            }
        }

        public void OnGlobalServerAction(byte[]? stream) { }

        public byte[]? OnLocalServerAction(byte[]? stream)
        {
            object? packet = Packet<Actions, object>.Decode(stream);
            if (packet == null)
            {
                return null;
            }

            switch (((Packet<Actions, object>)packet).Description)
            {
                case Actions.Getperson:
                    packet = Packet<Actions, Guid>.Decode(stream);
                    if (packet == null)
                    {
                        return null;
                    }
                    Guid userId = ((Packet<Actions, Guid>)packet).Data;
                    Person? person = this.context?.User.Where(x => x.Id == userId).Select(x => x.Person).FirstOrDefault();
                    return Packet<bool, Person?>.Encode(true, person);
            }

            return null;
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            se.Add(this.SeInitialize, "Launching");
        }

        public void OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
            this.context = context as Context;
        }

        public void OnServerInstallation(DbContext? context) { }
        #endregion //Implementations

        #region Private Methods
        private void Main_Shown(object? sender, EventArgs e)
        {
            User? user = ApplicationState.Client?.AuthenticatedUser;
            if(user == null)
            {
                this.Logout();
                return;
            }

            this.UpdateTitle(user);

            MenuItem menu = MenuItem.Root();
            this.FillBaseMenuTree(menu);

            if (ApplicationState.Client != null)
            {
                foreach (IMdiFormModlet item in UT.Data.Modlet.Modlet.Load<IMdiFormModlet>(null).Cast<IMdiFormModlet>())
                {
                    item.OnClientConfiguration(this);
                    item.OnMenuCreation(menu);
                }
            }

            if (this.menuStrip != null)
            {
                RenderMenu(menu, this.menuStrip);
            }
        }

        private void UpdateTitle(User? user)
        {
            if(user == null)
            {
                return;
            }
            Guid userId = user.Id;
            byte[] request = Packet<Actions, Guid>.Encode(Actions.Getperson, userId);
            byte[]? response = ApplicationState.Client?.Send(request, ModletCommands.Commands.Action, this);
            if(response != null)
            {
                Packet<bool, Person?>? packet = Packet<bool, Person?>.Decode(response);
                Person? person = packet?.Data;
                if(person != null)
                {
                    this.Text += " - " + person.Name();
                }
            }
        }

        private void Logout()
        {
            this.DialogResult = DialogResult.Retry;
            if (this.splash != null)
            {
                Invoker<Form>.Invoke(this.splash, delegate (Form form, object[]? data)
                {
                    form.TopMost = true;
                });
            }
            this.Close();
        }

        #region RenderMenu
        private static void RenderMenu(MenuItem menu, MenuStrip strip)
        {
            foreach(KeyValuePair<string, MenuItem> kvp in menu.Children)
            {
                MenuItem i = kvp.Value;
                string t = kvp.Key;

                switch(i.Type)
                {
                    case MenuItem.Types.Submenu:
                        RenderMenu(i, strip.Items.Add(t) as ToolStripMenuItem);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private static void RenderMenu(MenuItem menu, ToolStripMenuItem? strip)
        {
            if(strip == null)
            {
                return;
            }

            foreach (KeyValuePair<string, MenuItem> kvp in menu.Children)
            {
                MenuItem i = kvp.Value;
                string t = kvp.Key;

                switch (i.Type)
                {
                    case MenuItem.Types.Line:
                        strip.DropDownItems.Add(new ToolStripSeparator());
                        break;
                    case MenuItem.Types.Button:
                        ToolStripMenuItem item = new()
                        {
                            Text = t,
                        };
                        item.Click += i.OnClick;

                        strip.DropDownItems.Add(item);
                        break;
                    case MenuItem.Types.Submenu:
                        RenderMenu(i, strip.DropDownItems.Add(t) as ToolStripMenuItem);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
        #endregion //RenderMenu

        private void FillBaseMenuTree(MenuItem menu)
        {
            MenuItem account = MenuItem.Submenu();
            menu.Add("Account", account);

            account.Add("l1", MenuItem.Line());
            account.Add("Logout", MenuItem.Button(delegate (object? sender, EventArgs e) { this.Logout(); }));

            if(ApplicationState.Access != null && ApplicationState.Access.Contains(Role.AccessTiers.Administrator))
            {
                MenuItem adminitrator = MenuItem.Submenu();
                menu.Add("Administrator", adminitrator, 0);
            }
        }

        private bool SeInitialize(SequentialExecution self)
        {
            if (this.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            if (this.DialogResult != DialogResult.Retry)
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
            menuStrip.Size = new Size(278, 32);
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
