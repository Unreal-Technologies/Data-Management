using Shared.EFC.Tables;
using Shared.Modlet;

namespace Shared
{
    public class ApplicationState
    {
        #region Properties
        public static bool Reset { get; set; }
        public static AuthenticatedModletClient? Client { get; set; }
        public static Role.AccessTiers[]? Access { get; set; }
        public static IMdiFormModlet[]? Modules { get; set; }
        public static Strings.Languages? Language { get; set; }
        #endregion //Properties
    }
}
