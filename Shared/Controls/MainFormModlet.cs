using Microsoft.EntityFrameworkCore;
using Shared.EFC.Tables;
using Shared.Modlet;
using System.Net;
using System.Windows.Forms;
using UT.Data;

namespace Shared.Controls
{
    public abstract class MainFormModlet<TContext> : CustomForm, IMainFormModlet
        where TContext : DbContext
    {
        #region Members
        private TContext? context;
        private IPAddress? lastAction;
        #endregion //Members

        #region Properties
        public TContext? Context { get { return this.context; } }
        #endregion //Properties

        #region Implementations

        public abstract void OnSequentialExecutionConfiguration(SequentialExecution se);
        public abstract void OnServerInstallation(DbContext? context);
        public virtual void OnClientConfiguration(Form? form) { }
        public virtual void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
            this.lastAction = ip;
        }
        public virtual void OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
            if (context == null || context is not TContext ctx)
            {
                throw new Exception("No Database Access");
            }
            this.context = ctx;
        }
        public virtual byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            this.lastAction = ip;
            return null;
        }
        #endregion //Implementations

        #region Protected Methods
        protected void WriteUserLog(User user, string text)
        {
            Log log = new()
            {
                Text = text,
                User = user,
                IP = this.lastAction,
                Language = Strings.Current
            };
            this.context?.Add(log);
            this.context?.SaveChanges();
        }
        #endregion //Protected Methods
    }
}
