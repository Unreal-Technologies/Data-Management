namespace Client
{
    internal class App
    {
        #region Properties
        public static Configuration Configuration { get; set; } = new Configuration();
        public static bool AutoRestart { get; set; } = false;
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

                //throw new NotImplementedException("Goto Main");
            }
        }
        #endregion //Constructors
    }
}
