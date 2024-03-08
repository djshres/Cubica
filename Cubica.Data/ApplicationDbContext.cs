using Cubica.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace Cubica.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Cubical> Cubicals { get; set; }
        public DbSet<CubicalBooking> CubicalBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cubical>().HasData(
                new Cubical
                {
                    Id = 1,
                    FloorNumber = 1,
                    CubicalName = "A101"
                }
            );
            modelBuilder.Entity<Cubical>().HasData(
                new Cubical
                {
                    Id = 2,
                    FloorNumber = 1,
                    CubicalName = "A102"
                }
            );
            modelBuilder.Entity<Cubical>().HasData(
                new Cubical
                {
                    Id = 3,
                    FloorNumber = 1,
                    CubicalName = "A103"
                }
            );
        }
    }
}
