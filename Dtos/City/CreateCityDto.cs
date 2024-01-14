using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Dtos.City
{
    public class CreateCityDto
    {
        [Required(ErrorMessage = "city name cannot be null")]
        public string CityName { get; set; } = null!;
    }
}
