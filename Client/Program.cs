using UT.Data.Modlet;

namespace Client
{
    internal static class Program
    {
        #region Main
        [STAThread]
        public static void Main()
        {
            _ = new App();
        }
        #endregion //Main

        #region Classes
        public class Settings
        {
            #region Properties
            public string Ip { get; set; }
            public int Port { get; set; }
            #endregion //Properties

            #region Constructors
            public Settings()
            {
                this.Ip = string.Empty;
            }
            #endregion //Constructors
        }

        public class App
        {
            public App()
            {
                Splash splash = new();
                Application.Run(splash);
            }
        }
        #endregion //Classes
    }
}
