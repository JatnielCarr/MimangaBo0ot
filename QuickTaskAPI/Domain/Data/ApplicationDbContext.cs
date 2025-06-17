using Microsoft.EntityFrameworkCore;
using QuickTaskAPI.Domain.Entities;

namespace QuickTaskAPI.Domain.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Manga> Mangas { get; set; }
    public DbSet<Genre> Genres { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar entidad Genre
        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        // Configurar entidad Manga
        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PublicationDate).IsRequired();
            entity.Property(e => e.Volumes).IsRequired();
            entity.Property(e => e.IsOngoing).IsRequired();
            
            // Configurar relación con Genre - ahora es opcional
            entity.HasOne(e => e.Genre)
                  .WithMany(g => g.Mangas)
                  .HasForeignKey(e => e.GenreId)
                  .IsRequired(false) // Hace que la relación sea opcional
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
} 