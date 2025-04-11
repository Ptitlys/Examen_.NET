
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Examen.Models.Entities;

public class RefreshToken
{
    public int Id { get; set; }

    [Required]
    public required string Token { get; set; } 

    [Required]
    public required string UserId { get; set; }

    public required DateTime ExpiryDate { get; set; }  

    public required DateTime CreatedDate { get; set; }  

    public DateTime? RevokedDate { get; set; } 

    public string? ReplacedByToken { get; set; }  

    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;  

    public bool IsActive => RevokedDate == null && !IsExpired; 
}