using Microsoft.EntityFrameworkCore;
using Shared.Efc.Tables;
using UT.Data.Efc;

namespace Shared.Efc
{
    public class SharedModContext(ExtendedDbContext.Configuration configuration) : ExtendedDbContext(configuration)
    {
        #region Properties
        public DbSet<Person> People { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        #endregion //Properties
    }
}
