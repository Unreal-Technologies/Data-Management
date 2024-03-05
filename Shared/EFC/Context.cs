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

        #endregion //Properties
        #region Constructors
        public Context() : this(IPAddress.Parse("127.0.0.1"), 3306, "root", "", "test", Types.Mysql)
        {

        }
        #endregion //Constructors

        #region Overrides
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
                entity.HasOne(e => e.Person).WithMany(e => e.Users);
            });
        }
        #endregion //Overrides
    }
}
