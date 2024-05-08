using Microsoft.EntityFrameworkCore;
using Shared;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using UT.Data;
using UT.Data.Efc;
using UT.Data.Extensions;
using UT.Data.Modlet;

namespace Server
{
    internal static class Program
    {
        #region Constants
        private const int Padding = 96;
        #endregion //Constants

        #region Main
        [STAThread]
        public static void Main()
        {
            _ = new App();
        }
        #endregion //Main

        #region Classes
        internal class App
        {
            #region Members
            private ModletServer? server;
            private readonly Dictionary<string, ExtendedDbContext> contexts;
            private readonly bool installationMode;
            #endregion //Members

            #region Constructors
            public App()
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                contexts = [];
                installationMode = false;
                if(Debugger.IsAttached)
                {
                    installationMode = true;
                }

                ServerInfo();
                StartModletServer();
            }
            #endregion //Constructors

            #region Private Methods
            private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
            {
                server?.Stop();
                foreach (ExtendedDbContext context in contexts.Values)
                {
                    context.Dispose();
                }
            }

            private static void ServerInfo()
            {
                Version? version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");

                ExtendedConsole.BoxMode(true, Padding);
                ExtendedConsole.WriteLine("<yellow>Data Management</yellow> - <cyan>Server</cyan>", ExtendedConsole.Alignment.Center);
                ExtendedConsole.WriteLine("-".Repeat(Padding));
                ExtendedConsole.WriteLine(string.Format(SharedResources.Version, "<yellow>" + version.ToString() + "</yellow>"));
                ExtendedConsole.WriteLine(string.Format(SharedResources.Copyright, "<yellow>" + DateTime.Now.Year.ToString() + "</yellow>"));
                ExtendedConsole.BoxMode(false);
            }

            private void StartModletServer()
            {
                ExtendedConsole.BoxMode(true, Padding);
                ExtendedConsole.WriteLine("Server Info", ExtendedConsole.Alignment.Center);
                ExtendedConsole.WriteLine("-".Repeat(Padding));
                List<IPAddress> addresses = new(Network.LocalIPv4(NetworkInterfaceType.Ethernet))
                {
                    IPAddress.Parse("127.0.0.1")
                };

                ModletServer modletServer = new(addresses.ToArray(), int.Parse(Resources.Port));
                ExtendedConsole.WriteLine("Starting server on <yellow>" + string.Join("</yellow>, <yellow>", addresses) + "</yellow> on <cyan>" + Resources.Port + "</cyan>");

                ServerContext context = new();
                if (installationMode)
                {
                    context.EnsureCreated();
                }

                IModlet[] list = Modlet.Load<IModlet>();
                foreach (IModlet mod in list)
                {
                    if (installationMode)
                    {
                        mod.OnServerInstallation(context);
                    }
                    modletServer.Register(mod, context);
                    ExtendedConsole.WriteLine("Loaded <Green>" + mod.ToString() + "</Green>");
                }
                ExtendedConsole.WriteLine("Initialized <red>" + list.Length + "</red> module(s)");

                server = modletServer;
                modletServer.Start();
                ExtendedConsole.BoxMode(false);
            }
            #endregion //Private Methods
        }
        #endregion //Classes
    }
}
