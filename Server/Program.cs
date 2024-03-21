using Shared.EFCore;
using System.Net;
using System.Windows.Forms;
using UT.Data;
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
                this.Port = 0;
                this.Db = new Database();
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
                    this.Port = 0;
                    this.Ip = string.Empty;
                    this.Type = string.Empty;
                    this.Username = string.Empty;
                    this.Db = string.Empty;
                }
                #endregion //Constructors
            }
            #endregion //Classes
        }

        internal class App
        {
            #region Constructors
            public App()
            {
                Settings? settings = App.GetSettings();

                //Context context = new Context(Context.CreateMysqlConnection(IPAddress.Parse("127.0.0.1"), 3306, "root", "", "test"));
                //ModletServer modletServer = new ModletServer(new string[] { "127.0.0.1" }, 1404);
            }
            #endregion //Constructors

            #region Private Methods
            private static Settings? GetSettings()
            {
                LocalConfig localConfig = new("Settings");
                ExtendedConsole.BoxMode(true, Program.Padding);
                if (!localConfig.Exists)
                {
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
                    ExtendedConsole.WriteLine("Loaded <yellow>" + localConfig.Path+"</yellow>");
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
