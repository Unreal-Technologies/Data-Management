using Shared.EFC.Tables;
using System.Net;
using UT.Data.Modlet;

namespace Shared.Modlet
{
    public class AuthenticatedModletClient : ModletClient
    {
        #region Properties
        public User? AuthenticatedUser { get; set; }
        #endregion //Properties

        #region Constructors
        public AuthenticatedModletClient(string ip, int port) : base(ip, port)
        {
        }

        public AuthenticatedModletClient(IPAddress ip, int port) : base(ip, port)
        {

        }

        public AuthenticatedModletClient(int port) : base(port)
        {

        }
        #endregion //Constructors
    }
}
