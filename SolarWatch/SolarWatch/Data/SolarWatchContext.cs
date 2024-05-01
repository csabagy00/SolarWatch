using Microsoft.EntityFrameworkCore;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Sun> Suns { get; set; }
    
    public DbSet<LatLon> LatLon { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        optionsBuilder.UseSqlServer(config["E:\\user-secrets\\connection"]);
    }
}   