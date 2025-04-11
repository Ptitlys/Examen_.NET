using Examen.Models.Entities;

namespace Examen.Datas;

public interface ICustomerDataAccess
{
    Task<IEnumerable<Customer>> GetAll();
    Task<Customer?> GetById(string id);
}