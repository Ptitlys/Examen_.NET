using Examen.Models.Entities;

namespace Examen.Datas;

public interface ITechnicianDataAccess
{
    Task<IEnumerable<Technician>> GetAll();
    Task<Technician?> GetById(string id);
    Task<bool> CheckIfTechniciansExist(IEnumerable<string> technicianIds);
}