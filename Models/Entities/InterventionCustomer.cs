using Microsoft.EntityFrameworkCore;

namespace Examen.Models.Entities;

[PrimaryKey(nameof(InterventionId), nameof(CustomerId))]
public class InterventionCustomer
{
    public int InterventionId { get; set; }
    public string CustomerId { get; set; }
    
    public virtual Intervention Intervention { get; set; }
    public virtual Customer Customer { get; set; }
}