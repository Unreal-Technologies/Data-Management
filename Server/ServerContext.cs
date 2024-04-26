using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using UT.Data;
using UT.Data.Extensions;

namespace Server
{
    public class ServerContext : ExtendedDbContext
    {
        public ServerContext() 
            : base(new Configuration(Resources.DbConnectionString, Resources.DbType.AsEnum<Types>() ?? Types.Mysql))
        {
            SavingChanges += ExtendedDbContext_SavingChanges;
        }

        #region Private Methods
        private void ExtendedDbContext_SavingChanges(object? sender, SavingChangesEventArgs e)
        {
            if (sender is not DbContext context)
            {
                return;
            }

            foreach (EntityEntry entry in context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Added && entry.State != EntityState.Modified)
                {
                    continue;
                }

                object entity = entry.Entity;
                Type type = entity.GetType();
                PropertyInfo? transstartdate = type.GetProperty("TransStartDate");
                transstartdate?.SetValue(entity, DateTime.Now);
            }
        }
        #endregion //Private Methods
    }
}
