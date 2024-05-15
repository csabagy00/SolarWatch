using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SolarWatch.Data;

public class SolarWatchContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{
    public SolarWatchContext(DbContextOptions<SolarWatchContext> options) : base(options)
    {
    }
    
    public DbSet<City> Cities { get; set; }
    
    public DbSet<Sun> Suns { get; set; }
    
    public DbSet<LatLon> LatLon { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}   