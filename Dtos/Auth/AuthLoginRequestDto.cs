using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Dtos.Auth
{
    public class AuthLoginRequestDto
    {
        [Required(ErrorMessage = "username is required")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; } = null!;
    }
}
