namespace QuickTaskAPI.Domain.Models;
using System.Text.Json.Serialization;

public class MangaResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime PublicationDate { get; set; }
    public int Volumes { get; set; }
    public bool IsOngoing { get; set; }
    public int? GenreId { get; set; }
    [JsonPropertyName("genero")]
    public GenreResponseDto? Genre { get; set; }
}

public class GenreResponseDto
{
    public int Id { get; set; }
    [JsonPropertyName("genero")]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
} 