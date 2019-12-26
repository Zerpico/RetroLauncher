using RetroLauncher.Data.Model;
using RetroLauncher.Data.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RetroLauncher.Model;

namespace RetroLauncher.Service
{
    public class FileService //: IFileService
    {

        public FileService()
        {

        }

        public async void UpdateGame(Game game)
        {
            using (var db = new LocalGameContext())
            {
                if(db.Set<Game>().Any( a => a.GameId == game.GameId))
                {
                    db.Games.Update(game);
                }
                else
                {
                    db.Games.Add(game);
                }
                await db.SaveChangesAsync();
            }
        }

        public async Task<Game> GetGame(Game game)
        {
            using (var db = new LocalGameContext())
            {
                if( await db.Set<GamePath>().AnyAsync( a => a.GameId == game.GameId))
                {
                    var find = db.GamePaths.Where(d => d.GameId == game.GameId).First();

                    game.LocalPath = find;
                    return game;
                }

            }
            return game;
        }
    }

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
            lock(lockSave)
            {
                return base.SaveChanges();
            }
        }

    }
}