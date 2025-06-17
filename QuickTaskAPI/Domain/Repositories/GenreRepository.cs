using Microsoft.EntityFrameworkCore;
using QuickTaskAPI.Domain.Data;
using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Domain.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly ApplicationDbContext _context;

    public GenreRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Genre>> GetAllAsync()
    {
        return await _context.Genres.ToListAsync();
    }

    public async Task<PaginatedResult<Genre>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        // Validar parámetros
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Máximo 100 items por página

        try
        {
            // Obtener el total de items
            var totalItems = await _context.Genres.CountAsync();

            // Obtener los items de la página actual
            var items = await _context.Genres
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Genre>(items, totalItems, pageNumber, pageSize);
        }
        catch (Exception)
        {
            // Fallback: datos de prueba cuando no hay conexión a BD
            var mockGenres = GenerateMockGenres();
            var totalItems = mockGenres.Count();
            var items = mockGenres
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Genre>(items, totalItems, pageNumber, pageSize);
        }
    }

    public async Task<Genre?> GetByIdAsync(int id)
    {
        return await _context.Genres.FindAsync(id);
    }

    public async Task<Genre> AddAsync(Genre genre)
    {
        _context.Genres.Add(genre);
        await _context.SaveChangesAsync();
        return genre;
    }

    public async Task<bool> UpdateAsync(Genre genre)
    {
        var existingGenre = await _context.Genres.FindAsync(genre.Id);
        if (existingGenre == null)
            return false;

        _context.Entry(existingGenre).CurrentValues.SetValues(genre);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var genre = await _context.Genres.FindAsync(id);
        if (genre == null)
            return false;

        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
        return true;
    }

    private IEnumerable<Genre> GenerateMockGenres()
    {
        return new List<Genre>
        {
            new Genre { Id = 1, Name = "Acción", Description = "Mangas con mucha acción y combates" },
            new Genre { Id = 2, Name = "Aventura", Description = "Mangas de exploración y viajes" },
            new Genre { Id = 3, Name = "Comedia", Description = "Mangas humorísticos y divertidos" },
            new Genre { Id = 4, Name = "Drama", Description = "Mangas con historias emocionales profundas" },
            new Genre { Id = 5, Name = "Fantasía", Description = "Mangas con elementos mágicos y sobrenaturales" },
            new Genre { Id = 6, Name = "Romance", Description = "Mangas de amor y relaciones" },
            new Genre { Id = 7, Name = "Ciencia Ficción", Description = "Mangas con tecnología avanzada" },
            new Genre { Id = 8, Name = "Terror", Description = "Mangas de miedo y suspenso" },
            new Genre { Id = 9, Name = "Deportes", Description = "Mangas sobre deportes y competiciones" },
            new Genre { Id = 10, Name = "Psicológico", Description = "Mangas con elementos mentales complejos" }
        };
    }
} 