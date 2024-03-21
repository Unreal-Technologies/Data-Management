using Shared;

namespace Client
{
    internal static class Program
    {
        #region Main
        [STAThread]
        static void Main()
        {
            Strings.Language = Strings.Languages.Nl;
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