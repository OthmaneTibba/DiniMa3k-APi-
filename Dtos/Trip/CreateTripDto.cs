using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Dtos.Trip
{
    public class CreateTripDto
    {
        [Required(ErrorMessage = "Price cannot be null")]
        public double TripPrice { get; set; }
        [Required(ErrorMessage = "TotalSeats cannot be null")]
        public int TotalSeats { get; set; }
        [Required(ErrorMessage = "FromCityId cannot be null")]
        public Guid FromCityId { get; set; }
        [Required(ErrorMessage = "ToCityId cannot be null")]
        public Guid ToCityId { get; set; }
        [Required(ErrorMessage = "TripDate cannot be null")]
        public DateTime TripDate { get; set; }
        [Required(ErrorMessage ="Car cannot be null")]
        public Guid CarId { get; set; }
    }
}
