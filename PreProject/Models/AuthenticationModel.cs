using System.ComponentModel.DataAnnotations;

namespace PreProject.Models.DTOs
{
    public class AuthenticationModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
