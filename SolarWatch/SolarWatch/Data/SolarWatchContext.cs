using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SolarWatch.Data;

public class SolarWatchContext : DbContext
{
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
    
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Sun> Suns { get; set; }
    
    public DbSet<LatLon> LatLon { get; set; }


}   