using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Sun> Suns { get; set; }
    
    public DbSet<LatLon> LatLons { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            "Server=localhost,1433;Database=WeatherApi;User Id=sa;Password=Admin12345;Encrypt=false;");
    }
}   