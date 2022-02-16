using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Context
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<GameLink> Links { get; set; }
        public DbSet<Download> Downloads { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            //modelBuilder.HasDefaultSchema("retro");

            // Games Table
            modelBuilder.Entity<Game>(entity =>
            {                
                entity.ToTable("games");
                entity.HasKey(e => e.Id);
                
                entity.HasMany(e => e.GenreLinks)
                    .WithOne(p => p.Game);
               
                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Games)
                    .HasForeignKey("platform");                
            });

            // GenreLink Table
            modelBuilder.Entity<GenreLink>(entity =>
            {
                entity.ToTable("genrelinks");
                entity.HasKey(e => e.Id);

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.GenreLinks);
            });
        
            // Genre Table
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("genres");
                entity.HasKey(e => e.Id);                
            });

            // Platform Table
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("platforms");
                entity.HasKey(e => e.Id).HasName("id");

                entity.HasMany(d => d.Games)
                    .WithOne(p => p.Platform);
             
            });

            // GameLink Table
           modelBuilder.Entity<GameLink>(entity =>
            {
                entity.ToTable("links");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Game)
                    .WithMany(p => p.GameLinks);
            });

            //Downloads table
            modelBuilder.Entity<Download>(entity =>
            {
                entity.ToTable("downloads");
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Game)
                    .WithMany(p => p.Downloads);

            });

            // Downloads table
        /*    modelBuilder.Entity<Download>(entity =>
            {
                entity.ToTable("lg_downloads");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("download_id");

                entity.Property(e => e.Dt)
                    .HasColumnName("dt")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Downloads)
                    .HasConstraintName("FK_lg_downloads_gb_games")
                    .HasForeignKey("game_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Downloads)
                    .HasConstraintName("FK_lg_downloads_rg_users")
                    .HasForeignKey("user_id");
            });
        */
            // Rating
        /*    modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("lg_ratings");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("rating_id");

                entity.Property(e => e.Dt)
                    .HasColumnName("dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.RatingValue).HasColumnName("rating");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Ratings)                                        
                    .HasConstraintName("FK_lg_ratings_gb_games")
                    .HasForeignKey("game_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasConstraintName("FK_lg_ratings_rg_users")
                    .HasForeignKey("user_id");
            });
        */
            base.OnModelCreating(modelBuilder);
        }
       
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
