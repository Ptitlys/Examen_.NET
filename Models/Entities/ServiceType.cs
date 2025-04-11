using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Examen.Models.Entities;

[Index(nameof(Name), IsUnique = true)]
public class ServiceType
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; } = null!;}