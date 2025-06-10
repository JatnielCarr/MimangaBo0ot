using Microsoft.AspNetCore.Mvc;
using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Services.Features.Mangas;

namespace QuickTaskAPI.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class MangaController : ControllerBase
{
    private readonly MangaService _mangaService;

    public MangaController(MangaService mangaService)
    {
        _mangaService = mangaService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var mangas = _mangaService.GetAll();
        return Ok(mangas);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById([FromRoute] int id)
    {
        var manga = _mangaService.GetById(id);
        if (manga == null)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado." });
        }
        return Ok(manga);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Manga manga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newManga = _mangaService.Add(manga);
        return CreatedAtAction(nameof(GetById), new { id = newManga.Id }, newManga);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update([FromRoute] int id, [FromBody] Manga mangaToUpdate)
    {
        if (id != mangaToUpdate.Id)
        {
            return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del cuerpo." });
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = _mangaService.Update(mangaToUpdate);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para actualizar." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var success = _mangaService.Delete(id);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para eliminar." });
        }
        return NoContent();
    }
} 