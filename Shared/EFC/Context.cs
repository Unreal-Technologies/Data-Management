using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Shared.EFC.Tables;
using System.Net;
using System.Reflection;
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

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    //modelBuilder.Entity<Person>(entity =>
        //    //{
        //    //    entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
        //    //});

        //    //modelBuilder.Entity<User>(entity =>
        //    //{
        //    //    entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
        //    //});

        //    //modelBuilder.Entity<Role>(entity =>
        //    //{
        //    //    entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
        //    //});

        //    //modelBuilder.Entity<UserRole>(entity =>
        //    //{
        //    //    entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
        //    //});

        //    //modelBuilder.Entity<Log>(entity =>
        //    //{
        //    //    entity.Property(e => e.TransStartDate).ValueGeneratedOnAddOrUpdate();
        //    //});
        //}
        #endregion //Overrides
    }
}
