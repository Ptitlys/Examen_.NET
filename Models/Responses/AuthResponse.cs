
namespace Examen.Models.Responses;
public class AuthResponse
{
    public string? Token { get; set; }
    public DateTime Expiration { get; set; }
    public string? RefreshToken { get; set; }
}



