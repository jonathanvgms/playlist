using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Novit.Academia.ChallengePlaylist.Models
{
    public partial class PlaylistContexto : DbContext
    {
        public PlaylistContexto()
        {
        }

        public PlaylistContexto(DbContextOptions<PlaylistContexto> options)
            : base(options)
        {
        }

        public virtual DbSet<Playlist> Playlists { get; set; } = null!;
        public virtual DbSet<Song> Songs { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Playlist>(entity =>
            {
                entity.HasKey(e => e.IdPlaylist)
                    .HasName("PK_Playlist_1");

                entity.ToTable("Playlist");

                entity.Property(e => e.Description)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Song>(entity =>
            {
                entity.HasKey(e => e.IdSong);

                entity.ToTable("Song");

                entity.Property(e => e.Album)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Artist)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasMany(d => d.IdPlaylists)
                    .WithMany(p => p.IdSongs)
                    .UsingEntity<Dictionary<string, object>>(
                        "SongPlaylist",
                        l => l.HasOne<Playlist>().WithMany().HasForeignKey("IdPlaylist").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SongPlaylist_Playlist"),
                        r => r.HasOne<Song>().WithMany().HasForeignKey("IdSong").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK_SongPlaylist_Song"),
                        j =>
                        {
                            j.HasKey("IdSong", "IdPlaylist");

                            j.ToTable("SongPlaylist");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
