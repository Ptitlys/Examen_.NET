using Microsoft.EntityFrameworkCore;

namespace Examen.Models.Entities;

[PrimaryKey(nameof(InterventionId), nameof(TechnicianId))]
public class InterventionTechnician
{
    public int InterventionId { get; set; }
    public string TechnicianId { get; set; }
    
    public virtual Intervention Intervention { get; set; }
    public virtual Technician Technician { get; set; }
}