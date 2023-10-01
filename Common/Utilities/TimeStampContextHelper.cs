using Common.Model;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace Common.Utilities
{
    public static class TimeStampContextHelper
    {
        public static void UpdateTimeStamps(ChangeTracker changeTracker)
        {
            var entries = changeTracker
                .Entries()
                .Where(e => e.Entity is TimeStampBase && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added && ((TimeStampBase)entityEntry.Entity).CreatedAt == DateTime.MinValue)
                {
                    ((TimeStampBase)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }

                ((TimeStampBase)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}
