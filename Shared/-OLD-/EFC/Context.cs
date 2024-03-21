using Microsoft.EntityFrameworkCore;
using Shared.EFC.Tables;
using System.Net;
using UT.Data;

namespace Shared.EFC
{
    public class Context(IPAddress ip, int port, string username, string password, string db, ExtendedDbContext.Types type) : ExtendedDbContext(ip, port, username, password, db, type)
    {
        #region Properties
        public DbSet<User> User { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<History> History { get; set; }

        #endregion //Properties
        #region Constructors
        public Context() : this(IPAddress.Parse(Server), Port, Username, Password, "test", Types.Mysql) //Temp need 2 fix
        {
            ExtendedConsole.WriteLine("<red>Remove public constructor without parameters</red>");
        }
        #endregion //Constructors

        #region Overrides
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            //optionsBuilder.LogTo(Console.WriteLine);
        }
        #endregion //Overrides
    }
}
