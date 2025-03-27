using ArtExhibition.Domain;
using ArtExhibition.Domain.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ArtExhibition.Infrastructure.Context
{
    public class ArtExhibitionDbContext : IdentityDbContext<User>

    {
        public ArtExhibitionDbContext(DbContextOptions<ArtExhibitionDbContext> options) : base(options) { }

        public DbSet<Artist> Artists { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<FavoriteArtWork> FavoriteArtWorks { get; set; }
        public DbSet<ArtworkGallery> ArtworkGalleries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Artist to Artwork (One-to-Many)
            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Artworks)
                .WithOne(aw => aw.Artist)
                .HasForeignKey(aw => aw.ArtistID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascading cycles

            // Artist to Galleries (One-to-Many)
            modelBuilder.Entity<Artist>()
                .HasMany(a => a.Galleries)
                .WithOne(g => g.Artist)
                .HasForeignKey(g => g.ArtistID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents cascading cycles

            // ArtworkGallery (Many-to-Many)
            modelBuilder.Entity<ArtworkGallery>()
                .HasOne(ag => ag.Artwork)
                .WithMany(a => a.ArtworkGalleries)
                .HasForeignKey(ag => ag.ArtworkID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents multiple cascade paths

            modelBuilder.Entity<ArtworkGallery>()
                .HasOne(ag => ag.Gallery)
                .WithMany(g => g.ArtworkGalleries)
                .HasForeignKey(ag => ag.GalleryID)
                .OnDelete(DeleteBehavior.Restrict); // Prevents multiple cascade paths

            // Unique constraint for FavoriteArtWork
            modelBuilder.Entity<FavoriteArtWork>()
                .HasIndex(fa => new { fa.UserID, fa.ArtworkID })
                .IsUnique();

            // Unique constraint for ArtworkGallery
            modelBuilder.Entity<ArtworkGallery>()
                .HasIndex(ag => new { ag.ArtworkID, ag.GalleryID })
                .IsUnique();
           //User can be artist
            modelBuilder.Entity<Artist>()
              .HasOne(a => a.User)
              .WithOne()
              .HasForeignKey<Artist>(a => a.Id)
              .IsRequired()
              .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
