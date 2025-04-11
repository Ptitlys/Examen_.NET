using Examen.Models.Entities;

namespace Examen.Datas;

public interface IServiceTypeDataAccess
{
    Task<IEnumerable<ServiceType>> GetAll();
    Task<ServiceType?> GetByName(string name);
    Task<ServiceType> Add(ServiceType serviceType);
    Task Delete(int id);
}