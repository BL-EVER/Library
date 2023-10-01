using Common.Utilities;
using LibraryOrderService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryOrderService.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Order> orders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            TimeStampContextHelper.UpdateTimeStamps(ChangeTracker);
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            TimeStampContextHelper.UpdateTimeStamps(ChangeTracker);
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
