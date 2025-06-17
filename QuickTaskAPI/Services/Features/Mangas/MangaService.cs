using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Domain.Repositories;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Services.Features.Mangas;

public class MangaService
{
    private readonly IMangaRepository _mangaRepository;

    public MangaService(IMangaRepository mangaRepository)
    {
        _mangaRepository = mangaRepository;
    }

    public async Task<IEnumerable<MangaResponseDto>> GetAll()
    {
        var mangas = await _mangaRepository.GetAllAsync();
        return mangas.Select(MapToResponseDto);
    }

    public async Task<PaginatedResult<MangaResponseDto>> GetPaginated(int pageNumber = 1, int pageSize = 10)
    {
        var paginatedResult = await _mangaRepository.GetPaginatedAsync(pageNumber, pageSize);
        return new PaginatedResult<MangaResponseDto>(
            paginatedResult.Items.Select(MapToResponseDto),
            paginatedResult.TotalItems,
            paginatedResult.PageNumber,
            paginatedResult.PageSize
        );
    }

    public async Task<MangaResponseDto?> GetById(int id)
    {
        var manga = await _mangaRepository.GetByIdAsync(id);
        return manga != null ? MapToResponseDto(manga) : null;
    }

    public async Task<MangaResponseDto> Add(CreateMangaDto createDto)
    {
        var manga = new Manga
        {
            Title = createDto.Title,
            Author = createDto.Author,
            PublicationDate = createDto.PublicationDate,
            Volumes = createDto.Volumes,
            IsOngoing = createDto.IsOngoing,
            GenreId = createDto.GenreId
        };

        var newManga = await _mangaRepository.AddAsync(manga);
        return MapToResponseDto(newManga);
    }

    public async Task<bool> Update(int id, CreateMangaDto updateDto)
    {
        var existingManga = await _mangaRepository.GetByIdAsync(id);
        if (existingManga == null)
            return false;

        existingManga.Title = updateDto.Title;
        existingManga.Author = updateDto.Author;
        existingManga.PublicationDate = updateDto.PublicationDate;
        existingManga.Volumes = updateDto.Volumes;
        existingManga.IsOngoing = updateDto.IsOngoing;
        existingManga.GenreId = updateDto.GenreId;

        return await _mangaRepository.UpdateAsync(existingManga);
    }

    public async Task<bool> Delete(int id)
    {
        return await _mangaRepository.DeleteAsync(id);
    }

    private static MangaResponseDto MapToResponseDto(Manga manga)
    {
        return new MangaResponseDto
        {
            Id = manga.Id,
            Title = manga.Title,
            Author = manga.Author,
            PublicationDate = manga.PublicationDate,
            Volumes = manga.Volumes,
            IsOngoing = manga.IsOngoing,
            GenreId = manga.GenreId,
            Genre = manga.Genre != null ? new GenreResponseDto
            {
                Id = manga.Genre.Id,
                Name = manga.Genre.Name,
                Description = manga.Genre.Description
            } : null
        };
    }
} 