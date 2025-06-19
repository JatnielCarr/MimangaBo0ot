namespace QuickTaskAPI.Domain.Models
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
} 