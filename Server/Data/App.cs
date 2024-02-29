using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;
using UT.Data;
using UT.Data.Attributes;
using UT.Data.DBE;
using UT.Data.IO;
using UT.Data.IO.Assemblies;
using UT.Data.Modlet;

namespace Server.Data
{
    internal partial class App
    {
        private const int Padding = 96;

        #region Members
        private Configuration? configuration;
        #endregion //Members

        #region Constructors
        public App()
        {
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
            configuration.Add("Port", this.configuration.Port);
            configuration.Add("DBC", App.GetDbc(this.configuration));

            IModlet[] list = Modlet.Load(null);
            ExtendedConsole.BoxMode(true, App.Padding);
            foreach(IModlet mod in list)
            {
                server.Register(mod, ref configuration);
                ExtendedConsole.WriteLine("Loaded <Green>" + mod.ToString()+"</Green>");
            }
            ExtendedConsole.WriteLine("Loaded <red>" + list.Length + "</red> module(s).");
            ExtendedConsole.BoxMode(false);

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
            Type? t = Type.GetType(configuration.Dbc.Type);
            if (t == null)
            {
                return null;
            }
            IPAddress ip = IPAddress.Parse(configuration.Dbc.IP);

            ExtendedConsole.BoxMode(true, App.Padding);
            ExtendedConsole.WriteLine("Checking <yellow>" + t.Name + "</yellow> server connection on <yellow>" + ip.ToString() + ":" + configuration.Dbc.Port + "</yellow>");
            if(!Network.IsServerReachable(ip, configuration.Dbc.Port))
            {
                ExtendedConsole.WriteLine("<red>Cannot reach server</red>");
                ExtendedConsole.BoxMode(false);
                return null;
            }
            ExtendedConsole.WriteLine("<green>Server OK</green>");

            
            IDatabaseConnection? dbc = Activator.CreateInstance(t) as IDatabaseConnection;
            ExtendedConsole.BoxMode(false);

            return dbc;
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
                Dictionary<string, string> dbcConfig = App.GetDbcConnectionInfo(dbcType);

                Configuration cfg = new()
                {
                    Port = port.Value,
                    Dbc = new Configuration.Database()
                    {
                        Type = dbcType.AssemblyQualifiedName,
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

        private static Tuple<IPAddress, int> GetDbcConnectionInfo_Dbserver(Type dbc, DefaultAttribute[] defaults)
        {
            DefaultAttribute? serverDefaultA = defaults.Where(x => x.Name == "Server").FirstOrDefault();
            string serverDefault = serverDefaultA == null ? "???" : serverDefaultA.Value;

            DefaultAttribute? portDefaultA = defaults.Where(x => x.Name == "Port").FirstOrDefault();
            string portDefault = portDefaultA == null ? "???" : portDefaultA.Value;

            ExtendedConsole.WriteLine("Wich server is running the " + dbc.Name + " database (default '<green>" + serverDefault + "</green>'):");
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

            ExtendedConsole.WriteLine("Wich port is the " + dbc.Name + " server running on (default '<green>" + portDefault + "</green>'):");
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

        private static Tuple<string, string, string> GetDbcConnectionInfo_Db(Type dbc, DefaultAttribute[] defaults, Tuple<IPAddress, int> serverInfo)
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
                return App.GetDbcConnectionInfo_Db(dbc, defaults, serverInfo);
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

            if (Activator.CreateInstance(dbc) is not IDatabaseConnection dbcInstance)
            {
                throw new NotImplementedException();
            }

            if(!dbcInstance.Connect(serverInfo.Item1, serverInfo.Item2, dbString, userString, passString))
            {
                ExtendedConsole.WriteLine("<red>Can't connect to database '" + dbString + "'</red>");
                return App.GetDbcConnectionInfo_Db(dbc, defaults, serverInfo);
            }
            dbcInstance.Close();

            return new Tuple<string, string, string>(dbString, userString, passString);
        }

        private static Dictionary<string, string> GetDbcConnectionInfo(Type dbc)
        {
            DefaultAttribute[] defaults = dbc.GetCustomAttributes<DefaultAttribute>().ToArray();

            Dictionary<string, string> buffer = [];

            Tuple<IPAddress, int> serverInfo = App.GetDbcConnectionInfo_Dbserver(dbc, defaults);
            Tuple<string, string, string> dbInfo = App.GetDbcConnectionInfo_Db(dbc, defaults, serverInfo);

            buffer.Add("Server", serverInfo.Item1.ToString());
            buffer.Add("Port", serverInfo.Item2.ToString());
            buffer.Add("Database", dbInfo.Item1);
            buffer.Add("Username", dbInfo.Item2);
            buffer.Add("Password", dbInfo.Item3);

            return buffer;
        }

        private static Type? GetDbcType()
        {
            Dictionary<string, Type> supported = [];
            foreach(Type type in Loader.GetInstances<IDatabaseConnection>(false).Select(x => x.GetType()))
            {
                supported.Add(type.Name.ToLower(), type);
            }
            string[] keys = [.. supported.Keys];

            ExtendedConsole.WriteLine("Supported databases: '<yellow>" + string.Join("</yellow>', '<yellow>", keys) + "</yellow>'");
            ExtendedConsole.WriteLine("Wich database should be connected to (default '<green>" + keys[0] +"</green>'):");
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
