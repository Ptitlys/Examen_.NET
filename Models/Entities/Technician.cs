namespace Examen.Models.Entities;

public class Technician : User
{
    public virtual ICollection<InterventionTechnician> InterventionTechnicians { get; set; } = new List<InterventionTechnician>();
}