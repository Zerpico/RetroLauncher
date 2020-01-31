using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RetroLauncher.Common;
using RetroLauncher.DAL.Model;

namespace RetroLauncher.DAL
{
    public sealed class LocalGameContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<GamePath> GamePaths { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlite($"DataSource={Storage.Source.PathLocalDb}");
            if (!File.Exists($"{Storage.Source.PathLocalDb}"))
            {
                CreateFile(Storage.Source.PathLocalDb);
            }
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

        /// <summary>
        /// Creates a database file.  This just creates a zero-byte file which SQLite
        /// will turn into a database when the file is opened properly.
        /// </summary>
        /// <param name="databaseFileName">The file to create</param>
        private void CreateFile(string databaseFileName)
        {
            var fs = File.Create(databaseFileName);
            fs.Close();
        }
    }
}
