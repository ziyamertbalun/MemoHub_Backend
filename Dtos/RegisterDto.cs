using System.ComponentModel.DataAnnotations;

namespace MemoHubBackend.Dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [Required]
        [MinLength(6)]
        public string? ConfirmPassword { get; set; }

        [Required]
        public string? Username { get; set; }
    }
}
