namespace QuickTaskAPI.Domain.Models;

public class CreateGenreDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
} 