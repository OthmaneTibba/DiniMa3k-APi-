using DiniM3ak.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiniM3ak.Entity
{
    public class Trip
    {
        [Key]
        public Guid Id { get; set; }
        public double TripPrice { get; set; }
        public int TotalSeats { get; set; }
        public int RemainingSeats { get; set; }
        public Guid FromCityId { get; set; }
        [NotMapped]
        public City FromCity { get; set; } = null!;
        public Guid ToCityId { get; set; }
        [NotMapped]
        public City ToCity { get; set; } = null!;
        [NotMapped]
        public List<AppUser> Passangers { get; set; } = new();
        public string Status { get; set; } = TripStatus.OPEN.ToString();
        public DateTime TripDate { get; set; }
        public Guid OwnerId { get; set; }
        public AppUser Owner { get; set; } = null!;

        public Guid CardId { get; set; }
        [NotMapped]
        public Car Car { get; set; } = null!;
    }
}
