using Examen.Models;
using Examen.Models.Entities;

namespace Examen.Datas;

public interface IRefreshTokenDataAccess
{
    public Task<RefreshToken?> GetByTokenAsync(string token);
    public Task<RefreshToken> AddAsync(RefreshToken refreshToken);
    public Task<RefreshToken> UpdateAsync(RefreshToken refreshToken);
}