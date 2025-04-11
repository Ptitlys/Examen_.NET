using Examen.Models;
using Examen.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas;

public class RefreshTokenDataAccess : IRefreshTokenDataAccess
{
    private readonly ApplicationDbContext _context;
    
    public RefreshTokenDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
       return await _context.RefreshTokens
           .FirstOrDefaultAsync(t => t.Token == token);
    }

    public async Task<RefreshToken> AddAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
        
    }

    public async Task<RefreshToken> UpdateAsync(RefreshToken refreshToken)
    {
        _context.RefreshTokens.Update(refreshToken);
        await _context.SaveChangesAsync();
        return refreshToken;
    }
}