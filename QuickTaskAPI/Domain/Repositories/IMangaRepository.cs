using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Domain.Repositories;

public interface IMangaRepository
{
    Task<IEnumerable<Manga>> GetAllAsync();
    Task<PaginatedResult<Manga>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<Manga?> GetByIdAsync(int id);
    Task<Manga> AddAsync(Manga manga);
    Task<bool> UpdateAsync(Manga manga);
    Task<bool> DeleteAsync(int id);
} 