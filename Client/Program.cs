using Shared;
using Shared.EFC.Tables;
using Shared.Modlet;

namespace Client
{
    internal static class Program
    {
        #region Properties
        public static User? User { get; set; }
        public static AuthenticatedModletClient? Client { get; set; }
        #endregion //Properties

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