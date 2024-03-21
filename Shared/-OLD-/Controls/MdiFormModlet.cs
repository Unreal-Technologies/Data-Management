using Microsoft.EntityFrameworkCore;
using Shared.Modlet;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.IO;
using UT.Data.Modlet;

namespace Shared.Controls
{
    public abstract class MdiFormModlet<Tform, TmdiParent, Tcontext, Tenum> : CustomForm<Tform>, IMdiFormModlet
        where Tform : CustomForm
        where TmdiParent : CustomForm
        where Tcontext : DbContext
        where Tenum : Enum
    {
        #region Members
        private TmdiParent? main;
        private Tcontext? context;
        private IPAddress? lastAction;
        #endregion //Members

        #region Properties
        protected TmdiParent? Main { get { return this.main; } }
        protected Tcontext? Context { get { return this.context; } }
        protected IPAddress? LastAction { get { return this.lastAction; } }
        #endregion //Properties

        #region Implementations
        public virtual void OnClientConfiguration(Form? form)
        {
            this.main = form as TmdiParent;
        }
        public virtual void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
        }
        public virtual void OnServerInstallation(DbContext? context)
        {
        }
        public virtual void OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
            this.context = context as Tcontext;
        }
        public virtual void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
            this.lastAction = ip;
        }

        public virtual byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            this.lastAction = ip;
            if(stream == null || this.Context == null)
            {
                return null;
            }
            Packet<Tenum, byte[]>? packet = Packet<Tenum, byte[]>.Decode(stream);
            if (packet == null)
            {
                return null;
            }
            Tenum? action = packet.Description;
            if(action == null || packet.Data == null)
            {
                return null;
            }

            return this.OnDataReceived?.Invoke(action, packet.Data);
        }
        #endregion //Implementations

        #region Events
        public OnDataReceivedHandler? OnDataReceived { get; set; }
        #endregion //Events

        #region Delegates
        public delegate byte[]? OnDataReceivedHandler(Tenum action, byte[] stream);
        #endregion //Delegates

        #region Abstracts
        public abstract void OnMenuCreation(MenuItem menu);
        #endregion //Abstracts
        
        #region Public Methodss
        public byte[] Response<T>(T data)
            where T : class
        {
            return Packet<bool, byte[]>.Encode(true, Serializer<T>.Serialize(data));
        }

        public Treceive? Request<Tsend, Treceive>(Tsend data, Tenum action)
            where Treceive : class
            where Tsend : class
        {
            byte[]? result = Packet<bool, byte[]>.Decode(ApplicationState.Client?.Send(Packet<Tenum, byte[]>.Encode(action, Serializer<Tsend>.Serialize(data)), ModletCommands.Commands.Action, this))?.Data;
            if(result == null)
            {
                return default;
            }

            return Serializer<Treceive>.Deserialize(result);
        }

        public void Show<T>(Form? mdiParent, ShowHandler? handler = null)
            where T: CustomForm<T>, IMdiFormModlet
        {
            if(mdiParent == null)
            {
                return;
            }
            IMdiFormModlet? modlet = ApplicationState.Modules?.Where(x => x is T).FirstOrDefault();
            if(modlet == null)
            {
                return;
            }
            if (modlet is not CustomForm<T> form)
            {
                return;
            }
            handler?.Invoke(form as Tform);
            form.Show(mdiParent);
        }
        #endregion //Public Methods
    }
}
