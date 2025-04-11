namespace Examen.Models.Entities;

public class Customer : User
{
    public virtual ICollection<InterventionTechnician> InterventionTechnicians { get; set; } = new List<InterventionTechnician>();

}