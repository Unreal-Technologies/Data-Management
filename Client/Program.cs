using Shared.EFC.Tables;
using Shared.Modlet;
using UT.Data.Modlet;

namespace Client
{
    internal static class Program
    {
        #region Properties
        public static User? User { get; set; }
        public static bool Reset { get; set; }
        public static AuthenticatedModletClient? Client { get; set; }
        #endregion //Properties

        #region Main
        [STAThread]
        static void Main()
        {
            Program.Reset = true;
            ApplicationConfiguration.Initialize();

            while (Program.Reset)
            {
                Program.Reset = false;
                Application.Run(new Splash());
            }
        }
        #endregion //Main
    }
}