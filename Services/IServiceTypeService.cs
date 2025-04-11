using Examen.Models;
using Examen.Models.Entities;

namespace Examen.Services;

public interface IServiceTypeService
{
    Task<IEnumerable<ServiceType>> GetAll();
    Task<ServiceType?> GetByName(string name);
    Task<ServiceType> Add(ServiceType serviceType);
    Task Delete(int id);
}