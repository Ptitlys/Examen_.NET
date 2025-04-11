using System.Text.Json.Serialization;

namespace Examen.Models.Entities;

public class Technician : User
{
    [JsonIgnore]
    public virtual ICollection<Intervention> Interventions { get; set; } = new List<Intervention>();}