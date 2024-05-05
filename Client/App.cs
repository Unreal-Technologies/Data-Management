using UT.Data;
using UT.Data.Modlet;

namespace Client
{
    internal class App
    {
        #region Properties
        public static Configuration Configuration { get; set; } = new Configuration();
        public static bool AutoRestart { get; set; } = false;
        public static IModlet[]? MainModlets { get; set; } = null;
        public static IModlet[]? SubModlets { get; set; } = null;
        public static ModletClient? Client { get; set; } = null;
        public static Session Session { get; set; } = [];
        #endregion //Properties

        #region Constructors
        public App()
        {
            bool isFirstRun = true;

            while (AutoRestart || isFirstRun)
            {
                isFirstRun = false;
                Splash splash = new();

                Application.Run(splash);
                if(splash.DialogResult == DialogResult.Abort) //Stop Entire Process when splash is closed
                {
                    return;
                }
            }
        }
        #endregion //Constructors
    }
}
