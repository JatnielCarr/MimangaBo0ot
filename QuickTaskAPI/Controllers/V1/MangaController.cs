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
    public async Task<IActionResult> GetAll()
    {
        var mangas = await _mangaService.GetAll();
        return Ok(mangas);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var manga = await _mangaService.GetById(id);
        if (manga == null)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado." });
        }
        return Ok(manga);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] Manga manga)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newManga = await _mangaService.Add(manga);
        return CreatedAtAction(nameof(GetById), new { id = newManga.Id }, newManga);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] Manga mangaToUpdate)
    {
        if (id != mangaToUpdate.Id)
        {
            return BadRequest(new { Message = "El ID de la ruta no coincide con el ID del cuerpo." });
        }
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _mangaService.Update(mangaToUpdate);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para actualizar." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await _mangaService.Delete(id);
        if (!success)
        {
            return NotFound(new { Message = $"Manga con ID {id} no encontrado para eliminar." });
        }
        return NoContent();
    }
} 