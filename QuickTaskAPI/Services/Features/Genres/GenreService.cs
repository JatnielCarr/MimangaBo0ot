using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Repositories;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Services.Features.Genres;

public class GenreService
{
    private readonly IGenreRepository _genreRepository;

    public GenreService(IGenreRepository genreRepository)
    {
        _genreRepository = genreRepository;
    }

    public async Task<IEnumerable<GenreResponseDto>> GetAll()
    {
        var genres = await _genreRepository.GetAllAsync();
        return genres.Select(MapToResponseDto);
    }

    public async Task<PaginatedResult<GenreResponseDto>> GetPaginated(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _genreRepository.GetPaginatedAsync(pageNumber, pageSize);
        return new PaginatedResult<GenreResponseDto>(
            paginatedResult.Items.Select(MapToResponseDto),
            paginatedResult.TotalItems,
            paginatedResult.PageNumber,
            paginatedResult.PageSize
        );
    }

    public async Task<GenreResponseDto?> GetById(int id)
    {
        var genre = await _genreRepository.GetByIdAsync(id);
        return genre != null ? MapToResponseDto(genre) : null;
    }

    public async Task<GenreResponseDto> Add(CreateGenreDto createDto)
    {
        var genre = new Genre
        {
            Name = createDto.Name,
            Description = createDto.Description
        };

        var newGenre = await _genreRepository.AddAsync(genre);
        return MapToResponseDto(newGenre);
    }

    public async Task<bool> Update(int id, CreateGenreDto updateDto)
    {
        var existingGenre = await _genreRepository.GetByIdAsync(id);
        if (existingGenre == null)
            return false;

        existingGenre.Name = updateDto.Name;
        existingGenre.Description = updateDto.Description;

        return await _genreRepository.UpdateAsync(existingGenre);
    }

    public async Task<bool> Delete(int id)
    {
        return await _genreRepository.DeleteAsync(id);
    }

    private static GenreResponseDto MapToResponseDto(Genre genre)
    {
        return new GenreResponseDto
        {
            Id = genre.Id,
            Name = genre.Name,
            Description = genre.Description
        };
    }
} 