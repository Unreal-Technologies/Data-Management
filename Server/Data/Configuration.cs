namespace Server.Data
{
    internal class Configuration
    {
        public int Port { get; set; }
        public Database Dbc { get; set; }

        internal class Database
        {
            public string Type { get; set; }
            public string IP { get; set; }
            public int Port { get; set; }
            public string Db { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
