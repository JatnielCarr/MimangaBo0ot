namespace QuickTaskAPI.Domain.Models;

public class CreateMangaDto
{
    public required string Title { get; set; }
    public required string Author { get; set; }
    public DateTime PublicationDate { get; set; }
    public int Volumes { get; set; }
    public bool IsOngoing { get; set; }
    public int? GenreId { get; set; }
} 