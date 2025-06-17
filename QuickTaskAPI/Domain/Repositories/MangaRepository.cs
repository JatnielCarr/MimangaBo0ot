using Microsoft.EntityFrameworkCore;
using QuickTaskAPI.Domain.Data;
using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Domain.Repositories;

public class MangaRepository : IMangaRepository
{
    private readonly ApplicationDbContext _context;

    public MangaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Manga>> GetAllAsync()
    {
        return await _context.Mangas.Include(m => m.Genre).ToListAsync();
    }

    public async Task<PaginatedResult<Manga>> GetPaginatedAsync(int pageNumber, int pageSize)
    {
        // Validar parámetros
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Max(1, Math.Min(pageSize, 100)); // Máximo 100 items por página

        try
        {
            // Obtener el total de items
            var totalItems = await _context.Mangas.CountAsync();

            // Obtener los items de la página actual con Genre incluido
            var items = await _context.Mangas
                .Include(m => m.Genre)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Manga>(items, totalItems, pageNumber, pageSize);
        }
        catch (Exception)
        {
            // Fallback: datos de prueba cuando no hay conexión a BD
            var mockMangas = GenerateMockMangas();
            var totalItems = mockMangas.Count();
            var items = mockMangas
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Manga>(items, totalItems, pageNumber, pageSize);
        }
    }

    private IEnumerable<Manga> GenerateMockMangas()
    {
        return new List<Manga>
        {
            new Manga { Id = 1, Title = "One Piece", Author = "Eiichiro Oda", GenreId = 2, PublicationDate = new DateTime(1997, 7, 22), Volumes = 100, IsOngoing = true },
            new Manga { Id = 2, Title = "Naruto", Author = "Masashi Kishimoto", GenreId = 1, PublicationDate = new DateTime(1999, 9, 21), Volumes = 72, IsOngoing = false },
            new Manga { Id = 3, Title = "Dragon Ball", Author = "Akira Toriyama", GenreId = 1, PublicationDate = new DateTime(1984, 11, 20), Volumes = 42, IsOngoing = false },
            new Manga { Id = 4, Title = "Bleach", Author = "Tite Kubo", GenreId = 1, PublicationDate = new DateTime(2001, 8, 7), Volumes = 74, IsOngoing = false },
            new Manga { Id = 5, Title = "Attack on Titan", Author = "Hajime Isayama", GenreId = 4, PublicationDate = new DateTime(2009, 9, 9), Volumes = 34, IsOngoing = false },
            new Manga { Id = 6, Title = "My Hero Academia", Author = "Kōhei Horikoshi", GenreId = 1, PublicationDate = new DateTime(2014, 7, 7), Volumes = 38, IsOngoing = true },
            new Manga { Id = 7, Title = "Demon Slayer", Author = "Koyoharu Gotouge", GenreId = 1, PublicationDate = new DateTime(2016, 2, 15), Volumes = 23, IsOngoing = false },
            new Manga { Id = 8, Title = "Jujutsu Kaisen", Author = "Gege Akutami", GenreId = 1, PublicationDate = new DateTime(2018, 3, 5), Volumes = 25, IsOngoing = true },
            new Manga { Id = 9, Title = "Chainsaw Man", Author = "Tatsuki Fujimoto", GenreId = 1, PublicationDate = new DateTime(2018, 12, 3), Volumes = 15, IsOngoing = true },
            new Manga { Id = 10, Title = "Spy x Family", Author = "Tatsuya Endo", GenreId = 3, PublicationDate = new DateTime(2019, 3, 25), Volumes = 12, IsOngoing = true }
        };
    }

    public async Task<Manga?> GetByIdAsync(int id)
    {
        return await _context.Mangas.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Manga> AddAsync(Manga manga)
    {
        _context.Mangas.Add(manga);
        await _context.SaveChangesAsync();
        return manga;
    }

    public async Task<bool> UpdateAsync(Manga manga)
    {
        var existingManga = await _context.Mangas.FindAsync(manga.Id);
        if (existingManga == null)
            return false;

        _context.Entry(existingManga).CurrentValues.SetValues(manga);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
            return false;

        _context.Mangas.Remove(manga);
        await _context.SaveChangesAsync();
        return true;
    }
} 