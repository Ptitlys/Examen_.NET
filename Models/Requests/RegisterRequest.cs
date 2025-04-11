using System.ComponentModel.DataAnnotations;

namespace Examen.Models.Requests;

    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? FullName { get; set; }
        
        public string? Role { get; set; }
    }

