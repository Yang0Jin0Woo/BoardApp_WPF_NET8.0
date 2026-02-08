using BoardApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;

namespace BoardApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Post> Posts => Set<Post>();

        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured) return;

            var dbPath = GetDbPath();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>(e =>
            {
                e.Property(x => x.Title).HasMaxLength(200).IsRequired();
                e.Property(x => x.Author).HasMaxLength(50).IsRequired();
                e.Property(x => x.Content).IsRequired();
            });
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
            var dbPath = AppDbContext.GetDbPath();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new AppDbContext(options);
        }
    }
}
