using Microsoft.EntityFrameworkCore;
using Shared.EFC.Tables;
using System.Net;
using UT.Data;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Shared.EFC
{
    public class Context : ExtendedDbContext
    {
        #region Properties
        public DbSet<User> User { get; set; }
        public DbSet<Person> Person { get; set; }

        #endregion //Properties
        #region Constructors
        public Context() : this(IPAddress.Parse("127.0.0.1"), 3306, "root", "", "test", Types.Mysql)
        {

        }

        public Context(IPAddress ip, int port, string username, string password, string db, ExtendedDbContext.Types type) 
            : base(ip, port, username, password, db, type)
        {

        }
        #endregion //Constructors

        #region Overrides
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Firstname).IsRequired();
                entity.Property(e => e.Lastname).IsRequired();
                entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.Password).IsRequired();
                entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
                entity.Property(e => e.Person).IsRequired();
                //entity.HasOne(e => e.Person).WithMany(e => e.Users);
            });
        }
        #endregion //Overrides
    }
}
