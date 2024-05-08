using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Modlet;

namespace Shared.Controls
{
    public class ExtendedMdiModletForm : ExtendedForm, IMdiFormModlet
    {
        #region Members
        private ModletClient? client;
        private Session? session;
        private Form? root;
        private DbContext? context;
        #endregion //Members

        #region Properties
        public ModletClient? Client { get { return client; } }
        public Session? Session { get { return session; } }
        public Form? Root { get { return root; } }
        public DbContext? Context { get { return context; } }
        #endregion //Properties

        #region Public Methods
        public void ShowMdi<T>()
            where T : ExtendedMdiModletForm
        {
            T? me = (T?)Activator.CreateInstance(typeof(T));
            if (me != null)
            {
                me.MdiParent = MdiParent;
                me.Title = Title;
                me.Text = Text;
                me.WindowState = FormWindowState.Normal;
                if(Root is IMainMenuContainer mmc && Root is ExtendedForm eForm && Root is IMdiParentModlet mpm)
                {
                    me.Size = new Size(eForm.Width - mmc.MenuStrip.Width - 4, eForm.Height - 4 - eForm.InfoBar?.Height ?? 0);
                    me.Location = new Point(0, 0);
                    mpm.OnFullscreenChanged += me.Mpm_OnFullscreenChanged;
                }

                me.Show();
            }
        }

        private void Mpm_OnFullscreenChanged(object? sender, EventArgs e)
        {
            if (sender is IMdiParentModlet mpm)
            {
                WindowState = mpm.IsFullScreen ? FormWindowState.Maximized : FormWindowState.Normal;
            }
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

        public virtual void OnServerInstallation(DbContext? context)
        {
        }

        public virtual byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            return null;
        }

        public virtual void OnServerConfiguration(DbContext? context)
        {
            this.context = context;
        }

        public virtual void OnMenuCreation()
        {
        }
        #endregion //Public Methods
    }
}
