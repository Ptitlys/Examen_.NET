
using Examen.Models.Requests;
using Examen.Models.Responses;

namespace Examen.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> Register(RegisterRequest register);
        Task<AuthResponse> Login(LoginRequest request);
        Task<AuthResponse> RefreshToken(string refreshToken);
        Task RevokeToken(string refreshToken);
    }
}
