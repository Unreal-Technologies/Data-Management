using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Efc;
using UT.Data.Modlet;

namespace Shared.Controls
{
    public class ExtendedMainModletForm : ExtendedForm, IMainFormModlet
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
        public virtual void OnClientConfiguration(Form? form, ModletClient client, Session session)
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
            this.context = context?.Select(this);
        }
        #endregion //Public Methods
    }
}
