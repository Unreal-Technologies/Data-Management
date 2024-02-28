using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UT.Data;
using UT.Data.DBE;
using UT.Data.IO;
using UT.Data.IO.Assemblies;
using UT.Data.Modlet;
using UT.Data.Extensions;

namespace Server.Data
{
    internal class App
    {
        #region Members
        private Configuration? configuration;
        #endregion //Members

        #region Constructors
        public App()
        {
            Version? version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null)
            {
                throw new NotImplementedException("Cannot get version information.");
            }

            ExtendedConsole.WriteLine("Version <yellow>"+ version.ToString() + "</yellow>");
            ExtendedConsole.WriteLine("© Unreal Technologies <yellow>" + DateTime.Now.Year.ToString() + "</yellow>");

            List<IPAddress> addresses = new(Network.LocalIPv4(NetworkInterfaceType.Ethernet))
            {
                IPAddress.Parse("127.0.0.1")
            };

            this.LoadConfig();
            if(this.configuration == null)
            {
                return;
            }

            ModletServer server = new(addresses.ToArray(), this.configuration.Port);
            Dictionary<string, object?> configuration = [];
            configuration.Add("Port", this.configuration.Port);
            configuration.Add("DBC", App.GetDbc(this.configuration));

            IModlet[] list = Modlet.Load(null);
            foreach(IModlet mod in list)
            {
                server.Register(mod, ref configuration);
                ExtendedConsole.WriteLine("Loaded <Green>" + mod.ToString()+"</Green>");
            }
            ExtendedConsole.WriteLine("Loaded <red>" + list.Length + "</red> module(s).");

            server.Start();
        }
        #endregion //Constructors

        #region Public Methods

        public static void Initialize()
        {
            _ = new App();
        }
        #endregion //Public Methods

        #region Private Methods
        private static IDatabaseConnection? GetDbc(Configuration configuration)
        {
            Type? t = Type.GetType(configuration.DbcType);
            if(t == null)
            {
                return null;
            }
            return Activator.CreateInstance(t) as IDatabaseConnection;
        }

        private void LoadConfig()
        {
            LocalConfig lc = new("Configuration");
            if (!lc.Exists)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Configuration Missing.");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("Leave values blank for default.");

                int? port = App.GetPort();
                if (port == null)
                {
                    return;
                }
                Type? dbcType = App.GetDbcType();
                if(dbcType == null || dbcType.AssemblyQualifiedName == null)
                {
                    return;
                }

                Configuration cfg = new()
                {
                    Port = port.Value,
                    DbcType = dbcType.AssemblyQualifiedName
                };

                lc.Save(cfg);
            }
            this.configuration = lc.Load<Configuration>();
        }

        private static Type? GetDbcType()
        {
            Dictionary<string, Type> supported = [];
            foreach(Type type in Loader.GetInstances<IDatabaseConnection>(false).Select(x => x.GetType()))
            {
                supported.Add(type.Name.ToLower(), type);
            }
            string[] keys = [.. supported.Keys];

            ExtendedConsole.WriteLine("Supported databases: <yellow>" + string.Join("</yellow>, <yellow>", keys) + "</yellow>");
            ExtendedConsole.WriteLine("Wich database should be connected to (default <green>" + keys[0] +"</green>):");
            string? selected = Console.ReadLine();
            if(selected == null || selected == String.Empty)
            {
                return supported[keys[0]];
            }
            if(!supported.ContainsKey(selected))
            {
                ExtendedConsole.WriteLine("<red>" + selected + "</red> is not supported, try again.");
                return App.GetDbcType();
            }

            return supported[selected];
        }

        private static int? GetPort()
        {
            ExtendedConsole.WriteLine("Wich port to listen to (default <green>1404</green>):");
            string? selected = Console.ReadLine();
            if (selected == null || selected == String.Empty || !Regex.Match(selected, @"[0-9]{1,5}").Success)
            {
                return 1404;
            }
            if (!int.TryParse(selected, out int iOut))
            {
                return null;
            }
            return iOut;
        }
        #endregion //Private Methods
    }
}
