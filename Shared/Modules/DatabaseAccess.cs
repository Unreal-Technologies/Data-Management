using UT.Data;
using UT.Data.Attributes;
using UT.Data.DBE;
using UT.Data.IO;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(int.MinValue)]
    public class DatabaseAccess : IModlet
    {
        #region Constants
        public const string AuthenticationKey = "0x0405780232";
        #endregion //Constants

        #region Implementations

        void IModlet.OnClientConfiguration(ModletClient client)
        {
        }

        void IModlet.OnGlobalServerAction(byte[]? stream)
        {
        }

        byte[]? IModlet.OnLocalServerAction(byte[]? stream)
        {
            return null;
        }

        void IModlet.OnSequentialExecutionConfiguration(SequentialExecution se)
        {
        }

        void IModlet.OnServerConfiguration(ref Dictionary<string, object> configuration)
        {
            configuration.Add("DBC", new Mysql());
        }
        #endregion //Implementations
    }
}
