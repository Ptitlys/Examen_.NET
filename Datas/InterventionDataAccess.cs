using Examen.Models;
using Examen.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas;

public class InterventionDataAccess : IInterventionDataAccess
{
    private readonly ApplicationDbContext _context;

    public InterventionDataAccess( ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Intervention>> GetAll()
    {
        return await _context.Interventions.ToListAsync();

    }

    public async Task<Intervention?> GetById(int id)
    {
        return await _context.Interventions.FindAsync(id);
    }
    
    public async Task<IEnumerable<Intervention>> GetByTechnicianId(string technicianId)
    {
        return await _context.Interventions
            .Include(i => i.Customer)
            .Include(i => i.ServiceType)
            .Include(i => i.Technicians)
            .Where(i => i.Technicians.Any(it => it.Id == technicianId))
            .ToListAsync();
    }

    public async Task<Intervention> Add(Intervention intervention, List<string> technicianIds)
    {
        _context.Interventions.Add(intervention);
        await _context.SaveChangesAsync();
    
        if (technicianIds.Any())
        {
            var technicians = await _context.Users
                .OfType<Technician>()
                .Where(t => technicianIds.Contains(t.Id))
                .ToListAsync();
            
            foreach (var technician in technicians)
            {
                intervention.Technicians.Add(technician);
            }
        
            await _context.SaveChangesAsync();
        }
    
        return intervention;
    }

    public async  Task Delete(int id)
    {
        var intervention = await _context.Interventions.FindAsync(id);

        if (intervention == null)
            throw new KeyNotFoundException($"Intervention with id {id} not found.");
        
        _context.Interventions.Remove(intervention);
        await _context.SaveChangesAsync();
            
    }
}