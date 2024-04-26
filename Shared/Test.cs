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
        }

        public void OnGlobalServerAction(byte[]? stream, IPAddress ip)
        {
        }

        public byte[]? OnLocalServerAction(byte[]? stream, IPAddress ip)
        {
            return null;
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
        }

        public void OnServerConfiguration(DbContext? context)
        {
        }

        public void OnServerInstallation(DbContext? context)
        {
        }
    }
}
