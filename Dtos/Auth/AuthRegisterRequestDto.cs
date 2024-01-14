using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Dtos.Auth
{
    public class AuthRegisterRequestDto
    {
        [Required(ErrorMessage ="Full name is required")]
        public string Fullname { get; set; } = null!;
        [EmailAddress(ErrorMessage = "this is not a format of an email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = null!;
        [Required(ErrorMessage ="Profile Picture is required")]
        public IFormFile ProfilePicture { get; set; } = null!;

    }
}
