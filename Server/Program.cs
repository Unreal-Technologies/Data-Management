using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using UT.Data;
using UT.Data.Extensions;
using UT.Data.IO;
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
        internal class Settings
        {
            #region Properties
            public int Port { get; set; }
            public Database Db { get; set; }
            #endregion //Properties

            #region Constructors
            public Settings()
            {
                Port = 0;
                Db = new Database();
            }
            #endregion //Constructors

            #region Classes
            internal class Database
            {
                #region Properties
                public int Port { get; set; }
                public string Ip { get; set; }
                public string Type { get; set; }
                public string Username { get; set; }
                public string? Password { get; set; }
                public string Db { get; set; }
                #endregion //Properties

                #region Constructors
                public Database()
                {
                    Port = 0;
                    Ip = string.Empty;
                    Type = string.Empty;
                    Username = string.Empty;
                    Db = string.Empty;
                }
                #endregion //Constructors
            }
            #endregion //Classes
        }

        internal class App
        {
            #region Members
            private ModletServer? server;
            private readonly Dictionary<string, ExtendedDbContext> contexts;
            private bool installationMode;
            #endregion //Members

            #region Constructors
            public App()
            {
                AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
                contexts = [];
                installationMode = false;

                App.ServerInfo();
                Settings settings = GetSettings() ?? throw new NotImplementedException("Coding error?");
                StartModletServer(settings);
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

                ExtendedConsole.BoxMode(true, Program.Padding);
                ExtendedConsole.WriteLine("<yellow>Data Management</yellow> - <cyan>Server</cyan>", ExtendedConsole.Alignment.Center);
                ExtendedConsole.WriteLine("-".Repeat(Program.Padding));
                ExtendedConsole.WriteLine(string.Format("Version {0}", "<yellow>" + version.ToString() + "</yellow>"));
                ExtendedConsole.WriteLine(string.Format("© Unreal Technologies {0}", "<yellow>" + DateTime.Now.Year.ToString() + "</yellow>"));
                ExtendedConsole.BoxMode(false);
            }

            private void StartModletServer(Settings settings)
            {
                ExtendedConsole.BoxMode(true, Program.Padding);
                ExtendedConsole.WriteLine("Server Info", ExtendedConsole.Alignment.Center);
                ExtendedConsole.WriteLine("-".Repeat(Program.Padding));
                List<IPAddress> addresses = new(Network.LocalIPv4(NetworkInterfaceType.Ethernet))
                {
                    IPAddress.Parse("127.0.0.1")
                };
                ModletServer server = new(addresses.ToArray(), settings.Port);
                ExtendedConsole.WriteLine("Starting server on <yellow>" + string.Join("</yellow>, <yellow>", addresses) + "</yellow> on <cyan>" + settings.Port + "</cyan>");

                IModlet[] list = Modlet.Load<IModlet>(null);
                foreach (IModlet mod in list)
                {
                    ExtendedDbContext? context = GetDbContext(mod, settings);

                    if (installationMode)
                    {
                        context?.Database.EnsureCreated();
                        context?.SaveChanges();

                        mod.OnServerInstallation(context);
                    }
                    server.Register(mod, context);
                    ExtendedConsole.WriteLine("Loaded <Green>" + mod.ToString() + "</Green>");
                }
                ExtendedConsole.WriteLine("Initialized <red>" + list.Length + "</red> module(s)");

                server = server;
                server.Start();
                ExtendedConsole.BoxMode(false);
            }

            private ExtendedDbContext? GetDbContext(IModlet mod, Settings settings)
            {
                Assembly assembly = mod.GetType().Assembly;
                Type? type = assembly.ExportedTypes.Where(x => x.BaseType == typeof(ExtendedDbContext)).FirstOrDefault();
                if (type == null)
                {
                    return null;
                }

                Settings.Database db = settings.Db;
                ExtendedDbContext.Types? contextType = Enum.Parse(typeof(ExtendedDbContext.Types), db.Type) as ExtendedDbContext.Types?;
                if(contextType == null)
                {
                    return null;
                }

                ExtendedDbContext.Configuration? configuration = ExtendedDbContext.CreateConnection(contextType.Value, IPAddress.Parse(db.Ip), db.Port, db.Username, db.Password, db.Db);
                if(configuration == null)
                {
                    return null;
                }

                string key = configuration.Type.ToString() + ":" + configuration.ConnectionString;
                if(contexts.TryGetValue(key, out ExtendedDbContext? value))
                {
                    return value;
                }

                object[] param = [configuration];

                try
                {
                    if (Activator.CreateInstance(type, param) is not ExtendedDbContext context)
                    {
                        return null;
                    }
                    contexts.Add(key, context);
                    return context;
                }
                catch (Exception ex)
                {
                    ExtendedConsole.WriteLine(ex.Message);
                    return null;
                }
            }

            private Settings? GetSettings()
            {
                LocalConfig localConfig = new("Settings");
                ExtendedConsole.BoxMode(true, Program.Padding);
                ExtendedConsole.WriteLine("Settings", ExtendedConsole.Alignment.Center);
                ExtendedConsole.WriteLine("-".Repeat(Program.Padding));
                if (!localConfig.Exists)
                {
                    installationMode = true;
                    ExtendedConsole.WriteLine("<red>Configuration is Missing</red>");
                    Configuration configuration = new();
                    DialogResult result = configuration.ShowDialog();

                    while (result != DialogResult.OK && result != DialogResult.Cancel)
                    {
                        Console.Write("Configuration Error");
                        result = configuration.ShowDialog();
                    }

                    if (result == DialogResult.Cancel)
                    {
                        Application.ExitThread();
                        Application.Exit();
                        return null;
                    }

                    Settings settings = configuration.GetSettings();
                    localConfig.Save(settings);
                    ExtendedConsole.WriteLine("<green>Saving Configuration</green>");
                }
                Settings? output;
                try
                {
                    output = localConfig.Load<Settings>();
                    ExtendedConsole.WriteLine("Loaded <yellow>" + localConfig.Path + "</yellow>");
                }
                catch (Exception)
                {
                    output = null;
                }
                ExtendedConsole.BoxMode(false);
                return output;
            }
            #endregion //Private Methods
        }
        #endregion //Classes
    }
}
