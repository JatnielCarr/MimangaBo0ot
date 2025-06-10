using Microsoft.EntityFrameworkCore;
using QuickTaskAPI.Domain.Data;
using QuickTaskAPI.Domain.Entities;

namespace QuickTaskAPI.Services.Features.Mangas;

public class MangaService
{
    private readonly ApplicationDbContext _context;

    public MangaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Manga>> GetAll()
    {
        return await _context.Mangas.ToListAsync();
    }

    public async Task<Manga?> GetById(int id)
    {
        return await _context.Mangas.FindAsync(id);
    }

    public async Task<Manga> Add(Manga manga)
    {
        _context.Mangas.Add(manga);
        await _context.SaveChangesAsync();
        return manga;
    }

    public async Task<bool> Update(Manga mangaToUpdate)
    {
        var existingManga = await _context.Mangas.FindAsync(mangaToUpdate.Id);
        if (existingManga == null)
            return false;

        _context.Entry(existingManga).CurrentValues.SetValues(mangaToUpdate);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var manga = await _context.Mangas.FindAsync(id);
        if (manga == null)
            return false;

        _context.Mangas.Remove(manga);
        await _context.SaveChangesAsync();
        return true;
    }
} 