using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection;
using UT.Data;

namespace Shared.EFCore
{
    public class Context : ExtendedDbContext
    {
        public Context(Configuration configuration) : base(configuration)
        {
            this.SavingChanges += ExtendedDbContext_SavingChanges;
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
