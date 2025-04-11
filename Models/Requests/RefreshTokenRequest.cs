using System.ComponentModel.DataAnnotations;

namespace Examen.Models.Requests;


    public class RefreshTokenRequest
    {
        [Required]
        public required string RefreshToken { get; set; }
    }
