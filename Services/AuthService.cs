using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Examen.Datas;
using Examen.Exceptions;
using Examen.Models.Requests;
using Examen.Models.Entities;
using Examen.Models.Responses;

namespace Examen.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly IRefreshTokenDataAccess _refreshTokenDataAccess;
        private readonly RoleManager<Role> _roleManager;

        public AuthService(UserManager<User> userManager, IConfiguration config, ILogger<AuthService> logger, IRefreshTokenDataAccess refreshTokenDataAccess, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _logger = logger;
            _refreshTokenDataAccess = refreshTokenDataAccess;
        }

        public async Task<AuthResponse> Register(RegisterRequest register)
        {
            var user = new User { UserName = register.Email, Email = register.Email };
            
            if (register.Role == "Technician")
            {
                user = new Technician { UserName = register.Email, Email = register.Email};
            }
            else if (register.Role == "Customer")
            {
                user = new Customer { UserName = register.Email, Email = register.Email};
            }
            
            var result = await _userManager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                var errorMessages = result.Errors.Select(e => e.Description).ToArray();
                _logger.LogWarning("User creation failed with errors: {Errors}", errorMessages);
                throw new RegistrationFailedException();
            }
            
            if (await _roleManager.RoleExistsAsync(register.Role))
            {
                var addToRoleResult = await _userManager.AddToRoleAsync(user, register.Role);
                _logger.LogWarning("Failed to add user to role: {Role}", register.Role);
                if (!addToRoleResult.Succeeded)
                    throw new FailedToAssignRoleException();
            }
            
            return await GenerateTokenWithRefreshToken(user);
        }

        public async Task<AuthResponse> Login(LoginRequest login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
                throw new InvalidLoginException();

            return await GenerateTokenWithRefreshToken(user);
        }

        public async Task RevokeToken(string token)
        {
            var refreshToken = await _refreshTokenDataAccess.GetByTokenAsync(token);

            if (refreshToken == null )
                throw new KeyNotFoundException("Token introuvable");
            if (!refreshToken.IsActive)
                throw new ExpiredOrDeactivatedToken();

            refreshToken.RevokedDate = DateTime.UtcNow;
            await _refreshTokenDataAccess.UpdateAsync(refreshToken);
        }

        private async Task<AuthResponse> GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(int.Parse(_config["Jwt:ExpireMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new AuthResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires,
                RefreshToken = "" 
            };
        }

        private RefreshToken GenerateRefreshToken(string userId)
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            var token = Convert.ToBase64String(randomNumber);

            return new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow
            };
        }

        public async Task<AuthResponse> RefreshToken(string refreshToken)
        {
            var storedToken = await _refreshTokenDataAccess.GetByTokenAsync(refreshToken);

            if (storedToken == null)
                throw new KeyNotFoundException("Token introuvable");

            if (!storedToken.IsActive)
                throw new ExpiredOrDeactivatedToken();

            storedToken.RevokedDate = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken(storedToken.UserId);
            storedToken.ReplacedByToken = newRefreshToken.Token;

            await _refreshTokenDataAccess.UpdateAsync(storedToken);

            var user = await _userManager.FindByIdAsync(storedToken.UserId);

            if (user == null)
                throw new KeyNotFoundException("Utilisateur introuvable");

            var tokenResponse = await GenerateToken(user);

            tokenResponse.RefreshToken = newRefreshToken.Token;

            return tokenResponse;
        }

        private async Task<AuthResponse> GenerateTokenWithRefreshToken(User user)
        {
            var refreshToken = GenerateRefreshToken(user.Id);
            await _refreshTokenDataAccess.AddAsync(refreshToken);

            var tokenResponse = await GenerateToken(user);
            tokenResponse.RefreshToken = refreshToken.Token;

            return tokenResponse;
        }
    }
}
