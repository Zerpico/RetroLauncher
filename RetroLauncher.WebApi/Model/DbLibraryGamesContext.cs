using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RetroLauncher.WebApi.Model
{
    public partial class DbLibraryGamesContext : DbContext
    {
        public DbLibraryGamesContext()
        {
        }

        public DbLibraryGamesContext(DbContextOptions<DbLibraryGamesContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<GameLink> GameLinks { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<Download> Downloads { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {                       
                //optionsBuilder.UseSqlServer("Data Source=localhost; Database=retro_library; Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.3-servicing-35854");

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GameId)
                    .HasName("PK__gb_games__FFE11FCF2AA390B2");

                entity.ToTable("gb_games");

                entity.HasIndex(e => e.GameId)
                    .HasName("I_gb_games_platform_id");

                entity.HasIndex(e => e.GenreId)
                    .HasName("I_gb_games_genre_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.Annotation)
                    .HasColumnName("annotation")
                    .HasMaxLength(2000);

                entity.Property(e => e.Developer)
                    .HasColumnName("developer")
                    .HasMaxLength(100);

                entity.Property(e => e.GameName)
                    .IsRequired()
                    .HasColumnName("game_name")
                    .HasMaxLength(100);

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.NameOther)
                    .HasColumnName("name_other")
                    .HasMaxLength(100);

                entity.Property(e => e.NameSecond)
                    .HasColumnName("name_second")
                    .HasMaxLength(100);

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.Property(e => e.Year).HasColumnName("year");

                entity.HasOne(d => d.Genre)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GenreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__gb_games__genre___4E88ABD4");

                entity.HasOne(d => d.Platform)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.PlatformId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__gb_games__platfo__4F7CD00D");
            });

            modelBuilder.Entity<Genre>(entity =>
            {
                entity.HasKey(e => e.GenreId)
                    .HasName("PK__gb_genre__18428D420BD04564");

                entity.ToTable("gb_genres");

                entity.Property(e => e.GenreId).HasColumnName("genre_id");

                entity.Property(e => e.GenreName)
                    .IsRequired()
                    .HasColumnName("genre_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GameLink>(entity =>
            {
                entity.HasKey(e => e.LinkId)
                    .HasName("PK__gb_links__93B0078C16D53534");

                entity.ToTable("gb_links");

                entity.HasIndex(e => e.GameId)
                    .HasName("I_gb_links_gameid");

                entity.Property(e => e.LinkId).HasColumnName("link_id");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.TypeUrl).HasColumnName("type_url");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasColumnName("url")
                    .HasMaxLength(1000);

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.GameLinks)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__gb_links__game_i__52593CB8");
            });

            modelBuilder.Entity<Platform>(entity =>
            {
                entity.HasKey(e => e.PlatformId)
                    .HasName("PK__gb_platf__5F8F663C63B07D51");

                entity.ToTable("gb_platforms");

                entity.Property(e => e.PlatformId).HasColumnName("platform_id");

                entity.Property(e => e.Alias)
                    .IsRequired()
                    .HasColumnName("alias")
                    .HasMaxLength(10);

                entity.Property(e => e.PlatformName)
                    .IsRequired()
                    .HasColumnName("platform_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Download>(entity =>
            {
                entity.HasKey(e => e.DownloadId)
                    .HasName("PK__lg_downl__2EDDE1CD1DF1AC31");

                entity.ToTable("lg_downloads");

                entity.HasIndex(e => e.GameId)
                    .HasName("I_lg_downloads_gameid");

                entity.Property(e => e.DownloadId).HasColumnName("download_id");

                entity.Property(e => e.Dt)
                    .HasColumnName("dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Downloads)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lg_downlo__game___5629CD9C");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Downloads)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lg_downlo__user___5535A963");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.HasKey(e => e.RatingId)
                    .HasName("PK__lg_ratin__D35B278B4B333EE6");

                entity.ToTable("lg_ratings");

                entity.HasIndex(e => e.GameId)
                    .HasName("IX_lg_ratings_gameid");

                entity.Property(e => e.RatingId).HasColumnName("rating_id");

                entity.Property(e => e.Dt)
                    .HasColumnName("dt")
                    .HasColumnType("datetime");

                entity.Property(e => e.GameId).HasColumnName("game_id");

                entity.Property(e => e.RatingValue).HasColumnName("rating");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lg_rating__game___59FA5E80");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Ratings)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__lg_rating__user___59063A47");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__rg_users__B9BE370F504E26AE");

                entity.ToTable("rg_users");

                entity.HasIndex(e => e.MachineSid)
                    .HasName("I_gb_users_sid");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(100);

                entity.Property(e => e.MachineSid).HasColumnName("machine_sid");

                entity.Property(e => e.UserName)
                    .HasColumnName("user_name")
                    .HasMaxLength(50);
            });
        }
    }
}
