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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {            
            modelBuilder.HasDefaultSchema("retro");

            // Games Table
            modelBuilder.Entity<Game>(entity =>
            {                
                entity.ToTable("gb_games");
                entity.HasKey(e => e.Id).HasName("gb_games_pkey");               
                entity.Property(e => e.Id).HasColumnName("game_id");

                entity.Property(e => e.Annotation)
                    .HasColumnName("annotation")
                    .HasMaxLength(2000);

                entity.Property(e => e.Developer)
                    .HasColumnName("developer")
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("game_name")
                    .HasMaxLength(100);

                entity.Property(e => e.Year).HasColumnName("year");

                entity.Property(e => e.NameOther)
                    .HasColumnName("name_other")
                    .HasMaxLength(100);

                entity.Property(e => e.NameSecond)
                    .HasColumnName("name_second")
                    .HasMaxLength(100);
             
                entity.HasOne(e => e.Genre)
                    .WithMany(p => p.Games)                    
                    .HasConstraintName("FK_gb_games_gb_genres")
                    .HasForeignKey("genre_id");
              
                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Games)
                    .HasForeignKey("platform_id")                    
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            // Genre Table
            modelBuilder.Entity<Genre>(entity =>
            {
                entity.ToTable("gb_genres");
                entity.HasKey(e => e.Id).HasName("gb_genres_pkey");
                entity.Property(e => e.Id).HasColumnName("genre_id");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasColumnName("genre_name")
                    .HasMaxLength(50);
            });

            // Platform Table
            modelBuilder.Entity<Platform>(entity =>
            {
                entity.ToTable("gb_platforms");
                entity.HasKey(e => e.Id).HasName("gb_platforms_pkey");             
                entity.Property(e => e.Id).HasColumnName("platform_id");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasColumnName("alias")
                    .HasMaxLength(10);

                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasColumnName("platform_name")
                    .HasMaxLength(50);
            });

            // GameLink Table
            modelBuilder.Entity<GameLink>(entity =>
            {
                entity.ToTable("gb_links");
                entity.HasKey(e => e.Id);    
                entity.Property(e => e.Id).HasColumnName("link_id");
                entity.Property(e => e.TypeUrl).HasColumnName("type_url");

                entity.HasOne(e => e.Game)
                    .WithMany(p => p.GameLinks)
                    .HasConstraintName("FK_gb_links_gb_games")
                    .HasForeignKey("game_id");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(1000);
            });


            // Downloads table
            modelBuilder.Entity<Download>(entity =>
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

            // Rating
            modelBuilder.Entity<Rating>(entity =>
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

            base.OnModelCreating(modelBuilder);
        }
       
        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
