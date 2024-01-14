using DiniM3ak.Entity;
using Microsoft.EntityFrameworkCore;

namespace DiniM3ak.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>()
                .HasMany(c => c.Cars)
                .WithOne(c => c.User);

            modelBuilder.Entity<Trip>()
                .HasOne(c => c.FromCity);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.ToCity);

            modelBuilder.Entity<Trip>()
                .HasOne(t => t.Owner);

            modelBuilder.Entity<Trip>()
              .HasOne(t => t.Car);

            modelBuilder.Entity<Trip>()
                .HasMany(p => p.Passangers);
        }

        public DbSet<AppUser> Users { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Trip> Trips { get; set; }
    }
}
