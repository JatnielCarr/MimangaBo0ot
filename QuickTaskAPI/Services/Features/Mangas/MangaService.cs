using QuickTaskAPI.Domain.Entities;

namespace QuickTaskAPI.Services.Features.Mangas;

public class MangaService
{
    private readonly List<Manga> _mangas;
    private int _nextId = 1;

    public MangaService()
    {
        _mangas = new List<Manga>();
        // Datos de ejemplo
        Add(new Manga 
        { 
            Title = "Naruto", 
            Author = "Masashi Kishimoto", 
            PublicationDate = new DateTime(1999, 9, 21), 
            Volumes = 72, 
            IsOngoing = false, 
            Genre = "Shonen" 
        });
        Add(new Manga 
        { 
            Title = "One Piece", 
            Author = "Eiichiro Oda", 
            PublicationDate = new DateTime(1997, 7, 22), 
            Volumes = 100, 
            IsOngoing = true, 
            Genre = "Shonen" 
        });
    }

    public IEnumerable<Manga> GetAll()
    {
        return _mangas;
    }

    public Manga? GetById(int id)
    {
        return _mangas.FirstOrDefault(manga => manga.Id == id);
    }

    public Manga Add(Manga manga)
    {
        manga.Id = _nextId++;
        _mangas.Add(manga);
        return manga;
    }

    public bool Update(Manga mangaToUpdate)
    {
        var existingManga = _mangas.FirstOrDefault(m => m.Id == mangaToUpdate.Id);
        if (existingManga != null)
        {
            existingManga.Title = mangaToUpdate.Title;
            existingManga.Author = mangaToUpdate.Author;
            existingManga.Genre = mangaToUpdate.Genre;
            existingManga.PublicationDate = mangaToUpdate.PublicationDate;
            existingManga.Volumes = mangaToUpdate.Volumes;
            existingManga.IsOngoing = mangaToUpdate.IsOngoing;
            return true;
        }
        return false;
    }

    public bool Delete(int id)
    {
        var mangaToRemove = _mangas.FirstOrDefault(manga => manga.Id == id);
        if (mangaToRemove != null)
        {
            _mangas.Remove(mangaToRemove);
            return true;
        }
        return false;
    }
} 