using Microsoft.AspNetCore.Mvc;
using QuickTaskAPI.Domain.Entities;
using QuickTaskAPI.Services.Features.Genres;
using QuickTaskAPI.Domain.Models;

namespace QuickTaskAPI.Controllers.V1;

[ApiController]
[Route("api/v1/[controller]")]
public class GenreController : ControllerBase
{
    private readonly GenreService _genreService;

    public GenreController(GenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var genres = await _genreService.GetAll();
        return Ok(genres);
    }

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var paginatedResult = await _genreService.GetPaginated(pageNumber, pageSize);
        return Ok(paginatedResult);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var genre = await _genreService.GetById(id);
        if (genre == null)
        {
            return NotFound(new { Message = $"Género con ID {id} no encontrado." });
        }
        return Ok(genre);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateGenreDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newGenre = await _genreService.Add(createDto);
        return CreatedAtAction(nameof(GetById), new { id = newGenre.Id }, newGenre);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] CreateGenreDto updateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var success = await _genreService.Update(id, updateDto);
        if (!success)
        {
            return NotFound(new { Message = $"Género con ID {id} no encontrado para actualizar." });
        }
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await _genreService.Delete(id);
        if (!success)
        {
            return NotFound(new { Message = $"Género con ID {id} no encontrado para eliminar." });
        }
        return NoContent();
    }
} 