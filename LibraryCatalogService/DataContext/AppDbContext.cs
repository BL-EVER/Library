using Common.Utilities;
using LibraryCatalogService.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryCatalogService.DataContext
{
    public class AppDbContext : DbContext
    {
        public DbSet<Book> books { get; set; }
        public DbSet<Category> categories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category)
                .WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId);

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
