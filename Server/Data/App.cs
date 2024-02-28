using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;
using UT.Data;
using UT.Data.IO;
using UT.Data.Modlet;

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
            Dictionary<string, object> configuration = [];
            configuration.Add("Port", this.configuration.Port);

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
        private void LoadConfig()
        {
            LocalConfig lc = new("Configuration");
            if (!lc.Exists)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Configuration Missing.");
                Console.ForegroundColor = ConsoleColor.Gray;

                int? port = App.GetPort();
                if (port == null)
                {
                    return;
                }
                Configuration cfg = new()
                {
                    Port = port.Value
                };

                lc.Save(cfg);
            }
            this.configuration = lc.Load<Configuration>();
        }

        private static int? GetPort()
        {
            Console.WriteLine("Wich port to listen to:");
            string? selected = Console.ReadLine();
            if (selected == null || selected == String.Empty || !Regex.Match(selected, @"[0-9]{1,5}").Success)
            {
                return null;
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
