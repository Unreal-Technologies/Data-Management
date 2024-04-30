using Shared;

namespace Client
{
    internal class Configuration
    {
        #region Properties
        public string Title { get; set; }
        public int Port { get; set; }
        public string Server { get; set; }
        #endregion //Properties

        #region Constructors
        public Configuration()
        {
            Title = SharedResources.Title;
            Server = "127.0.0.1";
            Port = 1404;
        }
        #endregion //Constructors
    }
}
