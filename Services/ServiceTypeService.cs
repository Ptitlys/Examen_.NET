using System.Security.Claims;
using Examen.Datas;
using Examen.Models;
using Examen.Models.Entities;

namespace Examen.Services;

public class ServiceTypeService : IServiceTypeService
{
    private readonly IServiceTypeDataAccess _serviceTypeDataAccess;


    public ServiceTypeService(
      IServiceTypeDataAccess serviceTypeDataAccess)
    {
       _serviceTypeDataAccess = serviceTypeDataAccess;
    }
    
    public async Task<IEnumerable<ServiceType>> GetAll()
    {
        return await _serviceTypeDataAccess.GetAll();
    }

    public async Task<ServiceType?> GetByName(string name)
    {
        var serviceType = await _serviceTypeDataAccess.GetByName(name);
        if (serviceType == null) 
            throw new KeyNotFoundException($"Service Type with name {name} not found.");
        return serviceType;
    }

    public async Task<ServiceType> Add(ServiceType serviceType)
    {
        var existingServiceType = await _serviceTypeDataAccess.GetByName(serviceType.Name);
        if (existingServiceType != null)
            throw new InvalidOperationException($"Service Type with name {serviceType.Name} already exists.");
        
        return await _serviceTypeDataAccess.Add(serviceType);
    }

    public async Task Delete(int id)
    {
        await _serviceTypeDataAccess.Delete(id);
    }
}