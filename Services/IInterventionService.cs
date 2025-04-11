using Examen.Models;
using Examen.Models.Entities;

namespace Examen.Services;

public interface IInterventionService
{
    Task<IEnumerable<Intervention>> GetAll();
    Task<Intervention?> GetById(int id);
    Task<Intervention> Add(Intervention intervention);
    Task Delete(int id);
}