using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Dtos.Car
{
    public class CreateCarDto
    {
        [Required(ErrorMessage = "Car name cannot be null")]
        public string CarName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Car model cannot be null")]
        public string CarModel { get; set; } = string.Empty;

        [Required(ErrorMessage = "Car MaxSeatNumber cannot be null")]
        public int MaxSeatNumber { get; set; }
    }
}
