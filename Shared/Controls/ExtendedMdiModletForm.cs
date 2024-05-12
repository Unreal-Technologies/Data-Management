using Shared.Efc.Tables;
using Shared.Interfaces;
using Shared.Modules;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Efc;
using UT.Data.Modlet;

namespace Shared.Controls
{
    public class ExtendedMdiModletForm : ExtendedForm, IMdiFormModlet
    {
        #region Members
        private ModletClient? client;
        private Session? session;
        private Form? root;
        private ServerContext? context;
        #endregion //Members

        #region Properties
        public ModletClient? Client { get { return client; } }
        public Session? Session { get { return session; } }
        public Form? Root { get { return root; } }
        public ServerContext? Context { get { return context; } }
        #endregion //Properties

        #region Protected Methods
        protected byte[]? Request<Tkey, Tinput>(Tkey key, Tinput input)
            where Tkey : struct
        {
            return Client?.Send(
                ModletStream.CreatePacket(
                    key,
                    input
                ),
                ModletCommands.Commands.Action,
                this
            );
        }

        protected Tdata? Request<Tdata, Tkey, Tinput>(Tkey key, Tinput input)
            where Tkey : struct
        {
            return ModletStream.GetContent<bool, Tdata>(Request(key, input));
        }
        #endregion //Protected Methods

        #region Public Methods
        public T? ShowMdi<T>()
            where T : ExtendedMdiModletForm
        {
            T? me = (T?)Activator.CreateInstance(typeof(T));
            if (me != null)
            {
                me.MdiParent = MdiParent;
                me.Title = Title;
                me.Text = Text;
                me.session = Session;
                me.client = Client;
                me.root = Root;
                me.WindowState = FormWindowState.Normal;
                if(Root is IMainMenuContainer mmc && Root is ExtendedForm eForm && Root is IMdiParentModlet mpm)
                {
                    me.Size = new Size(eForm.Width - mmc.MenuStrip.Width - 4, eForm.Height - 4 - eForm.InfoBar?.Height ?? 0);
                    me.Location = new Point(0, 0);
                    mpm.OnFullscreenChanged += me.Mpm_OnFullscreenChanged;
                }

                me.Show();
                return me;
            }
            return default;
        }

        public User? AuthenticatedUser()
        {
            if(
                Session != null &&
                Session.TryGetValue("User-Authentication", out object? value) &&
                value is User user
            )
            {
                return user;
            }
            return null;
        }

        public void OnClientConfiguration(Form? form, ModletClient client, Session session)
        {
            this.client = client;
            this.session = session;
            root = form;
        }

        public virtual void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
        }

        public virtual void OnServerInstallation(ServerContext? context)
        {
        }

        public virtual byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            return null;
        }

        public virtual void OnServerConfiguration(ServerContext? context)
        {
            this.context = context;
        }

        public virtual void OnMenuCreation()
        {
        }
        #endregion //Public Methods

        #region Private Methods
        private void Mpm_OnFullscreenChanged(object? sender, EventArgs e)
        {
            if (sender is IMdiParentModlet mpm)
            {
                WindowState = mpm.IsFullScreen ? FormWindowState.Maximized : FormWindowState.Normal;
            }
        }
        #endregion //Private Methods
    }
}
