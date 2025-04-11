using Examen.Models.Entities;

namespace Examen.Datas;

public interface IInterventionDataAccess
{
    Task<IEnumerable<Intervention>> GetAll();
    Task<Intervention?> GetById(int id);
    Task<IEnumerable<Intervention>> GetByTechnicianId(string technicianId);
    Task<Intervention> Add(Intervention intervention, List<string> technicianIds);
    Task Delete(int id);
}