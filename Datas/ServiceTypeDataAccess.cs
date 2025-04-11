using Examen.Models;
using Examen.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas;

public class ServiceTypeDataAccess : IServiceTypeDataAccess
{
    private readonly ApplicationDbContext _context;

    public ServiceTypeDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<ServiceType>> GetAll()
    {
        return await _context.ServiceTypes.ToListAsync();

    }

    public async Task<ServiceType?> GetByName(string name)
    {
        var serviceType = await _context.ServiceTypes
            .FirstOrDefaultAsync(s => s.Name == name);
        
        return serviceType;
    }

    public async Task<ServiceType> Add(ServiceType serviceType)
    {
        _context.ServiceTypes.Add(serviceType);
        await _context.SaveChangesAsync();
        return serviceType;
    }
    public async Task Delete(int id)
    {
        var serviceType = await _context.ServiceTypes.FindAsync(id);

        if (serviceType == null)
            throw new KeyNotFoundException($"ServiceType with id {id} not found.");
        
        _context.ServiceTypes.Remove(serviceType);
        await _context.SaveChangesAsync();
    }
}