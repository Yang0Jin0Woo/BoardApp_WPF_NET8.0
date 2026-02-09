using BoardApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BoardApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.Author).HasMaxLength(50).IsRequired();
                e.Property(x => x.Content).IsRequired();
                e.Property(x => x.CreatedAtUtc).IsRequired();
                e.Property(x => x.UpdatedAtUtc).IsRequired();
            });
        }

        public override int SaveChanges()
        {
            ApplyTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ApplyTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ApplyTimestamps()
        {
            var now = DateTimeOffset.UtcNow;

            foreach (var entry in ChangeTracker.Entries<Post>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAtUtc = now;
                    entry.Entity.UpdatedAtUtc = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Property(p => p.CreatedAtUtc).IsModified = false;
                    entry.Entity.UpdatedAtUtc = now;
                }
            }
        }

        internal static string GetDbPath()
        {
            var dir = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "BoardApp");

            Directory.CreateDirectory(dir);

            return Path.Combine(dir, "board.db");
        }
    }

    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlite($"Data Source={AppDbContext.GetDbPath()}");
            return new AppDbContext(optionsBuilder.Options);
        }
    }
}

