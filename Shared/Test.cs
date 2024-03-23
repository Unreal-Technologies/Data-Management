using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;

namespace Shared
{
    [Position(int.MinValue)]
    public class Test : IModlet
    {
        public void OnClientConfiguration(Form? form)
        {
            //throw new NotImplementedException();
        }

        public void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
            //throw new NotImplementedException();
        }

        public byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            //throw new NotImplementedException();
            return null;
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            //throw new NotImplementedException();
        }

        public void OnServerConfiguration(DbContext? context)
        {
            //throw new NotImplementedException();
        }

        public void OnServerInstallation(DbContext? context)
        {
            //throw new NotImplementedException();
        }
    }
}
