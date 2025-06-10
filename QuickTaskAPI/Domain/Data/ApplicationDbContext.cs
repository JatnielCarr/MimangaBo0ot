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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Manga>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Author).IsRequired();
            entity.Property(e => e.PublicationDate).IsRequired();
            entity.Property(e => e.Volumes).IsRequired();
            entity.Property(e => e.IsOngoing).IsRequired();
        });
    }
} 