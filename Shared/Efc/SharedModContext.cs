using Microsoft.EntityFrameworkCore;
using Shared.Efc.Tables;
using UT.Data.Efc;

namespace Shared.Efc
{
    public class SharedModContext(ExtendedDbContext.Configuration configuration) : ExtendedDbContext(configuration)
    {
        #region Constructors
        public SharedModContext() : this(
            new Configuration(
                "server=127.0.0.1;port=3306;database=dnd-manager;user=root;password=;", 
                Types.Mysql
            )
        )
        {

        }
        #endregion //Constructors

        #region Public Methods
        public override bool Migrate() 
        {
            if (Database.GetPendingMigrations().Any())
            {
                try
                {
                    Database.Migrate();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
        #endregion //Public Methods

        #region Properties
        public DbSet<Person> People { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Content> Contents { get; set; }
        #endregion //Properties
    }
}
