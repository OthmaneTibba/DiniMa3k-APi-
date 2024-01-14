using System.ComponentModel.DataAnnotations;

namespace DiniM3ak.Entity
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }
        public string CarName { get; set; } = string.Empty;
        public string CarModel { get; set; } = string.Empty;
        public int MaxSeatNumber { get; set; }
        public Guid UserId { get; set; }
        [Required]
        public AppUser User { get; set; } = null!;
    }
}
