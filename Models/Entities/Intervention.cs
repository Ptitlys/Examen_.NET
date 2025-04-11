
namespace Examen.Models.Entities;

public class Intervention
{
    public int Id { get; set; }
    public required virtual ICollection<Technician> Technicians { get; set; } = new List<Technician>();
    public required virtual Customer Customer { get; set; } = null!;
    public required virtual ServiceType ServiceType { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ScheduledFor { get; set; }
}