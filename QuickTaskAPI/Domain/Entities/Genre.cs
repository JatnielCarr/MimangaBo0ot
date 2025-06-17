namespace QuickTaskAPI.Domain.Entities;

public class Genre
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    
    // Navigation property - relación uno a muchos con Manga
    public ICollection<Manga> Mangas { get; set; } = new List<Manga>();

    public Genre()
    {
        Name = string.Empty;
    }
} 