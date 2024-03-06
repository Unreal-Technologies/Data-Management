using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.IO;
using UT.Data.Modlet;

namespace Server.Data
{
    internal partial class App
    {
        #region Constants
        private const int Padding = 96;
        #endregion //Constants

        #region Members
        private Configuration? configuration;
        private readonly ModletServer? server;
        private bool installationMode = false;
        private readonly List<DbContext> contexts;
        #endregion //Members

        #region Constructors
        public App()
        {
            this.contexts = [];
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
            Version? version = Assembly.GetExecutingAssembly().GetName().Version ?? throw new NotImplementedException("Cannot get version information.");
            
            ExtendedConsole.BoxMode(true, App.Padding);
            ExtendedConsole.WriteLine("Version <yellow>"+ version.ToString() + "</yellow>");
            ExtendedConsole.WriteLine("© Unreal Technologies <yellow>" + DateTime.Now.Year.ToString() + "</yellow>");
            ExtendedConsole.BoxMode(false);

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
            
            App.CheckDbcServer(this.configuration);
            configuration.Add("Port", this.configuration.Port);

            IModlet[] list = Modlet.Load<IModlet>(null);
            ExtendedConsole.BoxMode(true, App.Padding);
            foreach(IModlet mod in list)
            {
                DbContext? context = GetDbContext(mod, this.configuration);
                if(context != null)
                {
                    contexts.Add(context);
                }

                if(this.installationMode)
                {
                    context?.Database.EnsureCreated();
                    context?.SaveChanges();

                    mod.OnServerInstallation(context);
                }
                server.Register(mod, context, ref configuration);
                ExtendedConsole.WriteLine("Loaded <Green>" + mod.ToString()+"</Green>");
            }
            ExtendedConsole.WriteLine("Loaded <red>" + list.Length + "</red> module(s).");
            ExtendedConsole.BoxMode(false);

            this.server = server;
            server.Start();
        }
        #endregion //Constructors

        #region Events
        private void CurrentDomain_ProcessExit(object? sender, EventArgs e)
        {
            this.server?.Stop();
            foreach(DbContext context in this.contexts)
            {
                context.Dispose();
            }
        }
        #endregion //Events

        #region Public Methods

        public static void Initialize()
        {
            _ = new App();
        }
        #endregion //Public Methods

        #region Private Methods
        private static DbContext? GetDbContext(IModlet mod, Configuration configuration)
        {
            Assembly assembly = mod.GetType().Assembly;
            Type? type = assembly.ExportedTypes.Where(x => x.BaseType == typeof(ExtendedDbContext)).FirstOrDefault();
            if (type == null)
            {
                return null;
            }

            object[] param = [IPAddress.Parse(configuration.Dbc.IP), configuration.Dbc.Port, configuration.Dbc.Username, configuration.Dbc.Password, configuration.Dbc.Db, ExtendedDbContext.Types.Mysql];

            try
            {
                return Activator.CreateInstance(type, param) as DbContext;
            }
            catch (Exception ex)
            {
                ExtendedConsole.WriteLine(ex.Message);
                return null;
            }
        }

        private static void CheckDbcServer(Configuration configuration)
        {
            Type? t = Type.GetType(configuration.Dbc.Type);
            if (t == null)
            {
                return;
            }
            IPAddress ip = IPAddress.Parse(configuration.Dbc.IP);

            ExtendedConsole.BoxMode(true, App.Padding);
            ExtendedConsole.WriteLine("Checking <yellow>" + t.Name + "</yellow> server connection on <yellow>" + ip.ToString() + ":" + configuration.Dbc.Port + "</yellow>");
            if(!Network.IsServerReachable(ip, configuration.Dbc.Port))
            {
                ExtendedConsole.WriteLine("<red>Cannot reach server</red>");
                ExtendedConsole.BoxMode(false);
                return;
            }
            ExtendedConsole.WriteLine("<green>Server OK</green>");

            ExtendedConsole.BoxMode(false);
        }

        private void LoadConfig()
        {
            LocalConfig lc = new("Configuration");
            if (!lc.Exists)
            {
                this.installationMode = true;
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
                ExtendedDbContext.Types? dbcType = App.GetDbcType();
                if(dbcType == null)
                {
                    return;
                }
                Dictionary<string, string> dbcConfig = App.GetDbcConnectionInfo(dbcType.Value);

                Configuration cfg = new()
                {
                    Port = port.Value,
                    Dbc = new Configuration.Database()
                    {
                        Type = dbcType.ToString() ?? "",
                        IP = dbcConfig["Server"],
                        Port = int.Parse(dbcConfig["Port"]),
                        Db = dbcConfig["Database"],
                        Username = dbcConfig["Username"],
                        Password = dbcConfig["Password"]
                    }
                };

                lc.Save(cfg);
            }
            this.configuration = lc.Load<Configuration>();
        }

        private static Tuple<IPAddress, int> GetDbcConnectionInfo_Dbserver(ExtendedDbContext.Types dbc, DefaultAttribute[] defaults)
        {
            DefaultAttribute? serverDefaultA = defaults.Where(x => x.Name == "Server").FirstOrDefault();
            string serverDefault = serverDefaultA == null ? "???" : serverDefaultA.Value;

            DefaultAttribute? portDefaultA = defaults.Where(x => x.Name == "Port").FirstOrDefault();
            string portDefault = portDefaultA == null ? "???" : portDefaultA.Value;

            ExtendedConsole.WriteLine("Wich server is running the " + dbc.ToString() + " database (default '<green>" + serverDefault + "</green>'):");
            string? server = Console.ReadLine();
            if (server == null || server == String.Empty)
            {
                server = serverDefault;
            }
            if (!IPAddress.TryParse(server, out IPAddress? serverAddress))
            {
                ExtendedConsole.WriteLine("<red>Invalid IP Address</red>");
                return App.GetDbcConnectionInfo_Dbserver(dbc, defaults);
            }

            ExtendedConsole.WriteLine("Wich port is the " + dbc.ToString() + " server running on (default '<green>" + portDefault + "</green>'):");
            string? portString = Console.ReadLine();

            if (portString == null || portString == String.Empty)
            {
                portString = portDefault;
            }
            if (!PortRegex().Match(portString).Success)
            {
                ExtendedConsole.WriteLine("'<red>" + portString + "</red>' is not a number, try again.");
                return App.GetDbcConnectionInfo_Dbserver(dbc, defaults);
            }

            if (!int.TryParse(portString, out int port))
            {
                throw new NotImplementedException();
            }

            ExtendedConsole.WriteLine("Checking network connection to <yellow>" + serverAddress + ":" + port + "</yellow>");
            if (!Network.IsServerReachable(serverAddress, port))
            {
                ExtendedConsole.WriteLine("Connection <red>Failed</red>");
                return App.GetDbcConnectionInfo_Dbserver(dbc, defaults);
            }
            ExtendedConsole.WriteLine("Connection <green>Ok</green>");

            return new Tuple<IPAddress, int>(serverAddress, port);
        }

        private static Tuple<string, string, string> GetDbcConnectionInfo_Db(DefaultAttribute[] defaults, Tuple<IPAddress, int> serverInfo)
        {
            DefaultAttribute? userDefaultA = defaults.Where(x => x.Name == "Username").FirstOrDefault();
            string userDefault = userDefaultA == null ? "???" : userDefaultA.Value;

            DefaultAttribute? passDefaultA = defaults.Where(x => x.Name == "Password").FirstOrDefault();
            string passDefault = passDefaultA == null ? "???" : passDefaultA.Value;

            ExtendedConsole.WriteLine("Database name:");
            string? dbString = Console.ReadLine();
            if (dbString == null || dbString == String.Empty)
            {
                ExtendedConsole.WriteLine("<red>Database name is required</red>");
                return App.GetDbcConnectionInfo_Db(defaults, serverInfo);
            }

            ExtendedConsole.WriteLine("Username (default '<green>" + userDefault + "</green>'):");
            string? userString = Console.ReadLine();
            if (userString == null || userString == String.Empty)
            {
                userString = userDefault;
            }

            ExtendedConsole.WriteLine("Password (default '<green>" + passDefault + "</green>'):");
            string? passString = Console.ReadLine();
            if (passString == null || passString == String.Empty)
            {
                passString = passDefault;
            }

            return new Tuple<string, string, string>(dbString, userString, passString);
        }

        private static Dictionary<string, string> GetDbcConnectionInfo(ExtendedDbContext.Types dbc)
        {
            DefaultAttribute[] defaults = typeof(ExtendedDbContext).GetCustomAttributes<DefaultAttribute>().ToArray();

            Dictionary<string, string> buffer = [];

            Tuple<IPAddress, int> serverInfo = App.GetDbcConnectionInfo_Dbserver(dbc, defaults);
            Tuple<string, string, string> dbInfo = App.GetDbcConnectionInfo_Db(defaults, serverInfo);

            buffer.Add("Server", serverInfo.Item1.ToString());
            buffer.Add("Port", serverInfo.Item2.ToString());
            buffer.Add("Database", dbInfo.Item1);
            buffer.Add("Username", dbInfo.Item2);
            buffer.Add("Password", dbInfo.Item3);

            return buffer;
        }

        private static ExtendedDbContext.Types? GetDbcType()
        {
            Dictionary<string, ExtendedDbContext.Types> supported = [];
            foreach(ExtendedDbContext.Types type in Enum.GetValues<ExtendedDbContext.Types>())
            {
                supported.Add(type.ToString().ToLower(), type);
            }
            string[] keys = [.. supported.Keys];

            ExtendedConsole.WriteLine("Supported databases: '<yellow>" + string.Join("</yellow>', '<yellow>", keys) + "</yellow>'");
            ExtendedConsole.WriteLine("Wich database should be connected to (default '<green>" + keys[0] +"</green>'):");
            string? selected = Console.ReadLine();
            if(selected == null || selected == String.Empty)
            {
                return supported[keys[0]];
            }
            if(!supported.TryGetValue(selected, out ExtendedDbContext.Types value))
            {
                ExtendedConsole.WriteLine("<red>" + selected + "</red> is not supported, try again.");
                return App.GetDbcType();
            }

            return value;
        }

        private static int? GetPort()
        {
            ExtendedConsole.WriteLine("Wich port to listen to (default '<green>1404</green>'):");
            string? selected = Console.ReadLine();
            if (selected == null || selected == String.Empty)
            {
                return 1404;
            }
            if(!PortRegex().Match(selected).Success)
            {
                ExtendedConsole.WriteLine("'<red>" + selected + "</red>' is not a number, try again.");
                return App.GetPort();
            }
            if (!int.TryParse(selected, out int iOut))
            {
                return null;
            }
            return iOut;
        }

        [GeneratedRegex(@"[0-9]{1,5}")]
        private static partial Regex PortRegex();
        #endregion //Private Methods
    }
}
