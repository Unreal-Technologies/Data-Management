using Shared;

namespace Client
{
    internal static class Program
    {
        #region Main
        [STAThread]
        static void Main()
        {
            ApplicationState.Reset = true;
            ApplicationConfiguration.Initialize();

            while (ApplicationState.Reset)
            {
                ApplicationState.Reset = false;
                Application.Run(new Splash());
            }
        }
        #endregion //Main
    }
}