using Examen.Models;
using Examen.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas;

public class TechnicianDataAccess : ITechnicianDataAccess
{
    private readonly ApplicationDbContext _context;

    public TechnicianDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Technician>> GetAll()
    {
        return await _context.Technicians.ToListAsync();

    }

    public async Task<Technician?> GetById(string id)
    {
        var serviceType = await _context.Technicians.FindAsync(id);
        
        if (serviceType == null) 
            throw new KeyNotFoundException($"Service Type with id {id} not found.");
        
        return serviceType;
    }

    public async Task<bool> CheckIfTechniciansExist(IEnumerable<string> technicianIds)
    {
        var existingIds = await _context.Technicians
            .Where(t => technicianIds.Contains(t.Id))
            .Select(t => t.Id)
            .ToListAsync();

       return existingIds.Count() == technicianIds.Count();
    }

}