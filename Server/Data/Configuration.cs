namespace Server.Data
{
    internal class Configuration
    {
        #region Properties
        public int Port { get; set; }
        public Database Dbc { get; set; }
        #endregion //Properties

        #region Constructors
        public Configuration()
        {
            this.Port = 0;
            this.Dbc = new Database();
        }
        #endregion //Constructors

        #region Classes
        internal class Database
        {
            #region Properties
            public string Type { get; set; }
            public string IP { get; set; }
            public int Port { get; set; }
            public string Db { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            #endregion //Properties

            #region Constructors
            public Database()
            {
                this.Type = "";
                this.IP = "";
                this.Port = 0;
                this.Db = "";
                this.Username = "";
                this.Password = "";
            }
            #endregion //Constructors
        }
        #endregion //Classes
    }
}
