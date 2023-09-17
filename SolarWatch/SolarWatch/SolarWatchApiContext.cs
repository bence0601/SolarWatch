using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using SolarWatch.Models;
using SolarWatch.Models.SunriseSunset;

namespace SolarWatch
{
    public class SolarWatchApiContext : DbContext
    {
        public DbSet<City> Cities { get; set; }
        public DbSet<SunriseSunsetResults> SunriseSunsetTimes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=localhost,1433;Database=SolarWatchApi;User Id=sa;Password=yourStrong(!)Password;Encrypt=True;TrustServerCertificate=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<City>()
                .HasOne(c => c.SunriseSunsetInfo) // City has one SunriseSunsetResults
                .WithOne(ss => ss.City) // SunriseSunsetResults has one City
                .HasForeignKey<SunriseSunsetResults>(ss => ss.CityId); // Foreign key

            modelBuilder.Entity<City>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<City>()
                .OwnsOne(c => c.Coordinates, cb =>
                {
                    cb.Property(co => co.Lat).HasColumnName("Coordinates_Lat");
                    cb.Property(co => co.Lon).HasColumnName("Coordinates_Lon");
                });

            modelBuilder.Entity<SunriseSunsetResults>()
                .HasIndex(ss => ss.CityId) // Törölje az egyedi indexet a CityId mezőről
                .IsUnique(false); // Az IsUnique false-ra állítása engedi a duplikátumokat
        }
    }
}
