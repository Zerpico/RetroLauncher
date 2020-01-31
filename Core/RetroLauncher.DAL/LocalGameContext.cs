using Microsoft.EntityFrameworkCore;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.DAL
{
    public class LocalGameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePath> GamePaths { get; set; }
        public LocalGameContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite($"DataSource={Storage.Source.PathLocalDb}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().ToTable("Games");
            modelBuilder.Entity<GamePath>().ToTable("GamePaths");
        }

        object lockSave = new object();
        public override int SaveChanges()
        {
            lock (lockSave)
            {
                return base.SaveChanges();
            }
        }

    }
}
