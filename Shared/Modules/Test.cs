using Microsoft.EntityFrameworkCore;
using Shared.Modlet;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.Modlet;

namespace Shared.Modules
{
    [Position(10)]
    public class Test : IMdiFormModlet
    {
        public void OnClientConfiguration(ModletClient client, Form? main)
        {
        }

        public void OnGlobalServerAction(byte[]? stream)
        {
            throw new NotImplementedException();
        }

        public byte[]? OnLocalServerAction(byte[]? stream)
        {
            throw new NotImplementedException();
        }

        public void OnMenuCreation(MenuItem menu)
        {
        }

        public void OnSequentialExecutionConfiguration(SequentialExecution se)
        {
            throw new NotImplementedException();
        }

        public void OnServerConfiguration(DbContext? context, ref Dictionary<string, object?> configuration)
        {
        }

        public void OnServerInstallation(DbContext? context)
        {
        }
    }
}
