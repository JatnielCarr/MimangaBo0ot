using Microsoft.AspNetCore.Mvc;
using QuickTaskAPI.Domain.Models;
using QuickTaskAPI.Services.Features.Auth;

namespace QuickTaskAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            // Aquí deberías validar el usuario y contraseña con tu base de datos
            if (loginDto.Username == "admin" && loginDto.Password == "admin") // Ejemplo simple
            {
                var token = _jwtService.GenerateToken(loginDto.Username);
                return Ok(token);
            }
            return Unauthorized();
        }
    }
}