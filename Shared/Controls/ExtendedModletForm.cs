using Microsoft.EntityFrameworkCore;
using Shared.Interfaces;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Controls;
using UT.Data.Modlet;

namespace Shared.Controls
{
    public class ExtendedModletForm : ExtendedForm, IMainFormModlet
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

        #region Protected Methods
        protected static (TKey? key, TValue? value)? ReadPacket<TKey, TValue>(byte[]? stream)
        {
            Packet<TKey, TValue>? packet = Packet<TKey, TValue>.Decode(stream);
            if (packet == null)
            {
                return null;
            }
            return (packet.Description, packet.Data);
        }

        protected static byte[] CreatePacket<TKey, TValue>(TKey key, TValue value)
        {
            return Packet<TKey, TValue>.Encode(key, value);
        }

        protected static T? GetInputType<T>(byte[]? stream)
            where T : struct
        {
            var packet = ReadPacket<T, object>(stream);
            if (packet == null)
            {
                return null;
            }

            return packet.Value.key;
        }

        protected static TData? GetContent<TKey, TData>(byte[]? stream)
            where TKey : struct
        {
            if (stream == null)
            {
                return default;
            }
            var packet = ReadPacket<TKey, TData>(stream);
            if (packet == null)
            {
                return default;
            }

            return packet.Value.value;
        }
        #endregion //Protected Methods

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
        #endregion //Public Methods
    }
}
