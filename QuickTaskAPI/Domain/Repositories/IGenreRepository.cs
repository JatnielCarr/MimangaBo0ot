using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Domain.Repositories;

public interface IGenreRepository
{
    Task<IEnumerable<Genre>> GetAllAsync();
    Task<PaginatedResult<Genre>> GetPaginatedAsync(int pageNumber, int pageSize);
    Task<Genre?> GetByIdAsync(int id);
    Task<Genre> AddAsync(Genre genre);
    Task<bool> UpdateAsync(Genre genre);
    Task<bool> DeleteAsync(int id);
} 