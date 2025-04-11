using Examen.Models;
using Examen.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Examen.Datas;

public class CustomerDataAccess : ICustomerDataAccess
{
    private readonly ApplicationDbContext _context;

    public CustomerDataAccess(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Customer>> GetAll()
    {
        return await _context.Customers.ToListAsync();

    }

    public async Task<Customer?> GetById(string id)
    {
        var serviceType = await _context.Customers.FindAsync(id);
        
        if (serviceType == null) 
            throw new KeyNotFoundException($"Service Type with id {id} not found.");
        
        return serviceType;
    }
}