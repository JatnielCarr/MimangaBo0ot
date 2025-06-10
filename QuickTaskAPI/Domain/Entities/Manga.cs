namespace QuickTaskAPI.Domain.Entities;

public class Manga
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Author { get; set; }
    public string? Genre { get; set; }
    public DateTime PublicationDate { get; set; }
    public int Volumes { get; set; }
    public bool IsOngoing { get; set; }

    public Manga()
    {
        Title = string.Empty;
        Author = string.Empty;
        PublicationDate = DateTime.MinValue;
    }
} 