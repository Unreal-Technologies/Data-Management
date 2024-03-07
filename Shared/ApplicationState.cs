using Shared.Modlet;

namespace Shared
{
    public class ApplicationState
    {
        #region Properties
        public static bool Reset { get; set; }
        public static AuthenticatedModletClient? Client { get; set; }
        #endregion //Properties
    }
}
